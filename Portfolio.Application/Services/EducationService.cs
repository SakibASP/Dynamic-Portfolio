using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IEducationService
{
    Task<IList<EDUCATION>> GetAllAsync();
    Task<EDUCATION?> GetByIdAsync(int? id);
    Task CreateAsync(EDUCATION item, string? userName);
    Task UpdateAsync(EDUCATION item, string? userName);
    Task RemoveAsync(int? id);
}

public class EducationService(IEducationRepo repo) : IEducationService
{
    private readonly IEducationRepo _repo = repo;

    public Task<IList<EDUCATION>> GetAllAsync() => _repo.GetAllEducationsAsync();
    public Task<EDUCATION?> GetByIdAsync(int? id) => _repo.GetEducationByIdAsync(id);

    public Task CreateAsync(EDUCATION item, string? userName) =>
        _repo.AddEducationAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(EDUCATION item, string? userName) =>
        _repo.UpdateEducationAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveEducationAsync(id);
}
