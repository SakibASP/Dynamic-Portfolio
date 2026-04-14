using Microsoft.Data.SqlClient;
using Portfolio.Application.DTOs;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

/// <summary>
/// Profile use cases. Wraps the persistence abstraction and carries all cross-cutting
/// concerns (audit timestamps via <see cref="Common.GenerateParameter"/>) so controllers
/// can stay thin.
/// </summary>
public interface IProfileService
{
    Task<IList<MY_PROFILE>> GetAllAsync();
    Task<MY_PROFILE?> GetSingleAsync();
    Task<MY_PROFILE?> GetByIdAsync(int? id);
    Task<IList<MY_SKILLS>> GetSkillsAsync();
    Task<IList<EDUCATION>> GetEducationsAsync();
    Task<IList<EXPERIENCE>> GetExperiencesAsync();
    Task<IList<DESCRIPTION>> GetDescriptionsAsync();
    Task<IList<PROJECTS>> GetProjectsAsync();
    Task CreateAsync(MY_PROFILE profile, string? userName);
    Task UpdateAsync(MY_PROFILE profile, string? userName);
    Task SendContactAsync(CONTACTS contact, string? userName);
    Task<IList<VisitorsViewModel>> GetVisitorsAsync(SqlParameter[] parameters);
}
