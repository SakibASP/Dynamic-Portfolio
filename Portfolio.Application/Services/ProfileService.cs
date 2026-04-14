using Microsoft.Data.SqlClient;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Application.DTOs;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public class ProfileService(IProfileRepo repo) : IProfileService
{
    private readonly IProfileRepo _repo = repo;

    public Task<IList<MY_PROFILE>> GetAllAsync() => _repo.GetProflieAsync();
    public Task<MY_PROFILE?> GetSingleAsync() => _repo.GetSingleProflieAsync();
    public Task<MY_PROFILE?> GetByIdAsync(int? id) => _repo.GetSingleProflieByIdAsync(id);
    public Task<IList<MY_SKILLS>> GetSkillsAsync() => _repo.GetSkillsAsync();
    public Task<IList<EDUCATION>> GetEducationsAsync() => _repo.GetEducationsAsync();
    public Task<IList<EXPERIENCE>> GetExperiencesAsync() => _repo.GetExperiencesAsync();
    public Task<IList<DESCRIPTION>> GetDescriptionsAsync() => _repo.GetDescriptionsAsync();
    public Task<IList<PROJECTS>> GetProjectsAsync() => _repo.GetProjectsAsync();

    public Task CreateAsync(MY_PROFILE profile, string? userName) =>
        _repo.AddProfileAsync(GenerateParameter.SingleModel(profile, userName, AppClock.BdNow));

    public Task UpdateAsync(MY_PROFILE profile, string? userName) =>
        _repo.UpdateProfileAsync(GenerateParameter.SingleModel(profile, userName, AppClock.BdNow));

    public Task SendContactAsync(CONTACTS contact, string? userName) =>
        _repo.AddContactInfoAsync(GenerateParameter.SingleModel(contact, userName, AppClock.BdNow));

    public Task<IList<VisitorsViewModel>> GetVisitorsAsync(SqlParameter[] parameters) =>
        _repo.GetVisitorsAsync(parameters);
}
