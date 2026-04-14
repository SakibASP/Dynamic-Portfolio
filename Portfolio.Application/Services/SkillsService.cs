using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface ISkillsService
{
    Task<IList<MY_SKILLS>> GetAllAsync();
    Task<MY_SKILLS?> GetByIdAsync(int? id);
    Task CreateAsync(MY_SKILLS item, string? userName);
    Task UpdateAsync(MY_SKILLS item, string? userName);
    Task RemoveAsync(int? id);
}

public class SkillsService(ISkillsRepo repo) : ISkillsService
{
    private readonly ISkillsRepo _repo = repo;

    public Task<IList<MY_SKILLS>> GetAllAsync() => _repo.GetAllSkillsAsync();
    public Task<MY_SKILLS?> GetByIdAsync(int? id) => _repo.GetSkillByIdAsync(id);

    public Task CreateAsync(MY_SKILLS item, string? userName) =>
        _repo.AddSkillAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(MY_SKILLS item, string? userName) =>
        _repo.UpdateSkillAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveSkillAsync(id);
}
