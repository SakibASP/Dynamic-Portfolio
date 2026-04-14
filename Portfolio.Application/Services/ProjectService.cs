using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IProjectService
{
    Task<IList<PROJECTS>> GetAllAsync();
    Task<PROJECTS?> GetByIdAsync(int? id);
    Task<IList<DESCRIPTION>> GetDescriptionsByProjectIdAsync(int? id);
    Task CreateAsync(PROJECTS item, string? userName);
    Task UpdateAsync(PROJECTS item, string? userName);
    Task RemoveAsync(int? id);
}

public class ProjectService(IProjectRepo repo) : IProjectService
{
    private readonly IProjectRepo _repo = repo;

    public Task<IList<PROJECTS>> GetAllAsync() => _repo.GetAllProjectsAsync();
    public Task<PROJECTS?> GetByIdAsync(int? id) => _repo.GetProjectByIdAsync(id);
    public Task<IList<DESCRIPTION>> GetDescriptionsByProjectIdAsync(int? id) => _repo.GetDescriptionByProjectIdAsync(id);

    public Task CreateAsync(PROJECTS item, string? userName) =>
        _repo.AddProjectAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(PROJECTS item, string? userName) =>
        _repo.UpdateProjectAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveProjectAsync(id);
}
