using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IExperienceService
{
    Task<IList<EXPERIENCE>> GetAllAsync();
    Task<EXPERIENCE?> GetByIdAsync(int? id);
    Task CreateAsync(EXPERIENCE item, string? userName);
    Task UpdateAsync(EXPERIENCE item, string? userName);
    Task RemoveAsync(int? id);
}

public class ExperienceService(IExperienceRepo repo) : IExperienceService
{
    private readonly IExperienceRepo _repo = repo;

    public Task<IList<EXPERIENCE>> GetAllAsync() => _repo.GetAllExperiencesAsync();
    public Task<EXPERIENCE?> GetByIdAsync(int? id) => _repo.GetExperienceByIdAsync(id);

    public Task CreateAsync(EXPERIENCE item, string? userName) =>
        _repo.AddExperienceAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(EXPERIENCE item, string? userName) =>
        _repo.UpdateExperienceAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveExperienceAsync(id);
}
