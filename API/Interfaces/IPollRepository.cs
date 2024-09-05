using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IPollRepository
{
    void AddPoll(Poll poll);
    void DeletePoll(Poll poll);
    Task<Poll?> GetPollAsync(int id);
    Task<IEnumerable<PollDto>> GetPollsForUserAsync(string username);
    void AddPollOption(PollOption pollOption);
    void DeletePollOption(PollOption pollOption);
    Task<PollOption?> GetPollOptionAsync(int id);
    Task<bool> Complete();
}
