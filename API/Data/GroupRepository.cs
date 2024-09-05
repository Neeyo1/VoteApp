using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class GroupRepository(DataContext context, IMapper mapper) : IGroupRepository
{
    public void AddGroup(Group group)
    {
        context.Groups.Add(group);
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void DeleteGroup(Group group)
    {
        context.Groups.Remove(group);
    }

    public async Task<Group?> GetGroupAsync(int id)
    {
        return await context.Groups
            .Include(x => x.Members)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<GroupDto>> GetGroupsForUserAsync(string username)
    {
        return await context.UserGroups
            .Where(x => x.User.UserName == username)
            .Select(x => x.Group)
            .ProjectTo<GroupDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<MemberDto>> GetGroupMembersAsync(Group group)
    {
        return await context.UserGroups
            .Where(x => x.Group == group)
            .Select(x => x.User)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public void AddUserToGroup(AppUser user, Group group)
    {
        var userGroup = new UserGroup
        {
            User = user,
            UserId = user.Id,
            Group = group,
            GroupId = group.Id
        };
        context.UserGroups.Add(userGroup);
    }

    public void RemoveUserFromGroup(UserGroup userGroup)
    {
        context.UserGroups.Remove(userGroup);
    }

    public async Task<bool> IsUserInGroup(AppUser user, int groupId)
    {
        //return group.Members.FirstOrDefault(x => x.User.KnownAs == user.KnownAs) != null;
        return await context.Groups
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == groupId) != null;
    }

    public async Task<UserGroup?> GetUserGroup(AppUser user, Group group)
    {
        return await context.UserGroups
            .Where(x => x.User == user)
            .Where(x => x.Group == group)
            .FirstOrDefaultAsync();
    }
}
