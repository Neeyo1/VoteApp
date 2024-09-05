using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PollRepository(DataContext context, IMapper mapper) : IPollRepository
{
    public void AddPoll(Poll poll)
    {
        context.Polls.Add(poll);
    }

    public void AddPollOption(PollOption pollOption)
    {
        context.PollOptions.Add(pollOption);
    }

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void DeletePoll(Poll poll)
    {
        context.Polls.Remove(poll);
    }

    public void DeletePollOption(PollOption pollOption)
    {
        context.PollOptions.Remove(pollOption);
    }

    public async Task<Poll?> GetPollAsync(int id)
    {
        return await context.Polls
            .Include(x => x.Group)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PollOption?> GetPollOptionAsync(int id)
    {
        return await context.PollOptions.FindAsync(id);
    }

    public async Task<IEnumerable<PollDto>> GetPollsForUserAsync(string username)
    {
        return await context.Polls
            .Where(x => x.Group.Members.Any(x => x.User.UserName == username))
            .ProjectTo<PollDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
