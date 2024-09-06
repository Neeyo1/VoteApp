using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PollsController(IUserRepository userRepository, IPollRepository pollRepository,
    IMapper mapper, IGroupRepository groupRepository) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<PollDto>> CreatePoll(PollCreateDto pollCreateDto, [FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var group = await groupRepository.GetGroupAsync(id);
        if (group == null) return Unauthorized();

        if (user.UserName != group.Owner) return Unauthorized();

        var poll = mapper.Map<Poll>(pollCreateDto);
        poll.GroupId = group.Id;
        
        pollRepository.AddPoll(poll);

        if (await pollRepository.Complete()) return Ok(mapper.Map<PollDto>(poll));
        return BadRequest("Failed to add poll");
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePoll([FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var poll = await pollRepository.GetPollAsync(id);
        if (poll == null) return Unauthorized();

        if (poll.Group.Owner != user.UserName) return Unauthorized();

        pollRepository.DeletePoll(poll);

        if (await pollRepository.Complete()) return NoContent();
        return BadRequest("Failed to delete poll");
    }

    [HttpPut]
    public async Task<ActionResult> EditPoll(PollCreateDto pollEditDto, [FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var poll = await pollRepository.GetPollAsync(id);
        if (poll == null) return Unauthorized();

        if (poll.Group.Owner != user.UserName) return Unauthorized();

        mapper.Map(pollEditDto, poll);

        if (await pollRepository.Complete()) return NoContent();
        return BadRequest("Failed to edit poll");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PollDto>>> GetPollsForUser()
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        var polls = await pollRepository.GetPollsForUserAsync(user.UserName);
        
        return Ok(polls);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<PollDto>>> GetPoll(int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null || user.UserName == null) return Unauthorized();

        var poll = await pollRepository.GetPollAsync(id);
        if (poll == null) return Unauthorized();

        var members = await groupRepository.GetGroupMembersAsync(poll.Group);
        foreach (var member in members)
        {
            if (member.Id == user.Id)
            {
                poll.PollOptions = await pollRepository.GetPollOptionsAsync(poll.Id);
                return Ok(mapper.Map<PollDto>(poll));
            } 
        }
        return Unauthorized();
    }

    [HttpPost("options")]
    public async Task<ActionResult<PollOptionDto>> CreatePollOption(PollOptionCreateDto pollOptionCreateDto, [FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var poll = await pollRepository.GetPollAsync(id);
        if (poll == null) return Unauthorized();

        if (user.UserName != poll.Group.Owner) return Unauthorized();

        var pollOption = mapper.Map<PollOption>(pollOptionCreateDto);
        pollOption.PollId = poll.Id;
        
        pollRepository.AddPollOption(pollOption);

        if (await pollRepository.Complete()) return Ok(mapper.Map<PollOptionDto>(pollOption));
        return BadRequest("Failed to add poll option");
    }

    [HttpDelete("options")]
    public async Task<ActionResult> DeletePollOption([FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var pollOption = await pollRepository.GetPollOptionAsync(id);
        if (pollOption == null) return Unauthorized();

        if (pollOption.Poll.Group.Owner != user.UserName) return Unauthorized();

        pollRepository.DeletePollOption(pollOption);

        if (await pollRepository.Complete()) return NoContent();
        return BadRequest("Failed to delete poll option");
    }

    [HttpPut("options")]
    public async Task<ActionResult> EditPollOption(PollOptionCreateDto pollOptionEditDto, [FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var pollOption = await pollRepository.GetPollOptionAsync(id);
        if (pollOption == null) return Unauthorized();

        if (pollOption.Poll.Group.Owner != user.UserName) return Unauthorized();

        mapper.Map(pollOptionEditDto, pollOption);

        if (await pollRepository.Complete()) return NoContent();
        return BadRequest("Failed to edit poll option");
    }

    [HttpPost("options/vote")]
    public async Task<ActionResult> Vote([FromQuery] int id)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if (user == null) return Unauthorized();

        var pollOption = await pollRepository.GetPollOptionAsync(id);
        if (pollOption == null) return Unauthorized();

        if (!await groupRepository.IsUserInGroup(user, pollOption.Poll.GroupId)) return Unauthorized();
        if(await pollRepository.HasUserVoted(user, pollOption.PollId))
            return BadRequest("You have already voted in this poll");

        pollRepository.AddVote(user, pollOption);

        if (await pollRepository.Complete()) return NoContent();
        return BadRequest("Failed to vote");
    }
}
