using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class GroupsController(IUserRepository userRepository, IGroupRepository groupRepository,
    IMapper mapper) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<GroupDto>> CreateGroup(GroupCreateDto groupCreateDto)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        var group = new Group
        {
            Name = groupCreateDto.Name,
            Owner = user.UserName,
        };

        groupRepository.AddGroup(group);
        groupRepository.AddUserToGroup(user, group);

        if (await groupRepository.Complete()) return Ok(mapper.Map<GroupDto>(group));

        return BadRequest("Failed to save message");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditGroup(int id, GroupCreateDto groupEditDto)
    {
        var group = await groupRepository.GetGroupAsync(id);
        if (group == null) return Unauthorized();

        mapper.Map(groupEditDto, group);

        if (await groupRepository.Complete()) return NoContent();
        return BadRequest("Failed to update group");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGroup(int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        var group = await groupRepository.GetGroupAsync(id);
        if (group == null) return Unauthorized();

        if (user.UserName != group.Owner) return Unauthorized();

        groupRepository.DeleteGroup(group);

        if (await groupRepository.Complete()) return Ok();

        return BadRequest("Problem occured while deleting group");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        var groups = await groupRepository.GetGroupsForUserAsync(user.UserName);
        
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetGroup(int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        var group = await groupRepository.GetGroupAsync(id);
        if (group == null) return Unauthorized();

        if (!groupRepository.IsUserInGroup(user, group))
            return Unauthorized();

        return Ok(mapper.Map<GroupDto>(group));
    }

    [HttpPost("members")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers([FromQuery]int id)
    {
        var group = await groupRepository.GetGroupAsync(id);
        if (group == null) return Unauthorized();

        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        if (!groupRepository.IsUserInGroup(user, group))
            return Unauthorized();

        return await groupRepository.GetGroupMembersAsync(group);
    }

    [HttpPost("members/edit")]
    public async Task<ActionResult> EditMembers([FromQuery]int id, string username)
    {
        var group = await groupRepository.GetGroupAsync(id);
        if (group == null) return Unauthorized();

        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        if (!groupRepository.IsUserInGroup(user, group))
            return Unauthorized();

        if (user.UserName != group.Owner) return Unauthorized();

        var userToEdit = await userRepository.GetUserByUsernameAsync(username.ToLower());
        if (userToEdit == null || userToEdit.UserName == null) return BadRequest("User does not exist");
        if (userToEdit == user) return BadRequest("As owner, you cannot remove yourself from this group");

        if(groupRepository.IsUserInGroup(userToEdit, group))
        {
            var userGroup = await groupRepository.GetUserGroup(userToEdit, group);
            if (userGroup == null) return BadRequest("Something went wrong while removing user from group");
            
            groupRepository.RemoveUserFromGroup(userGroup);
            if (await groupRepository.Complete()) return NoContent();
        }
        else
        {
            groupRepository.AddUserToGroup(userToEdit, group);
            if (await groupRepository.Complete()) return NoContent();
        }
        return BadRequest("Failed to edit members");
    }
}
