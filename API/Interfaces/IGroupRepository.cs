using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IGroupRepository
{
    void AddGroup(Group group);
    void DeleteGroup(Group group);
    Task<Group?> GetGroupAsync(int id);
    Task<IEnumerable<GroupDto>> GetGroupsForUserAsync(string username);
    Task<List<MemberDto>> GetGroupMembersAsync(Group group);
    Task<bool> Complete();
    void AddUserToGroup(AppUser user, Group group);
    void RemoveUserFromGroup(UserGroup userGroup);
    Task<bool> IsUserInGroup(AppUser user, int groupId);
    Task<UserGroup?> GetUserGroup(AppUser user, Group group);
}
