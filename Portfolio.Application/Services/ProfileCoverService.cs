using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IProfileCoverService
{
    Task<IList<PROFILE_COVER>> GetAllAsync();
    Task<PROFILE_COVER?> GetByIdAsync(int? id);
    Task CreateAsync(PROFILE_COVER item, string? userName);
    Task UpdateAsync(PROFILE_COVER item, string? userName);
    Task RemoveAsync(int? id);
}

public class ProfileCoverService(IProfileCoverRepo repo) : IProfileCoverService
{
    private readonly IProfileCoverRepo _repo = repo;

    public Task<IList<PROFILE_COVER>> GetAllAsync() => _repo.GetAllProfileCoversAsync();
    public Task<PROFILE_COVER?> GetByIdAsync(int? id) => _repo.GetProfileCoverByIdAsync(id);

    public Task CreateAsync(PROFILE_COVER item, string? userName) =>
        _repo.AddProfileCoverAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(PROFILE_COVER item, string? userName) =>
        _repo.UpdateProfileCoverAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveProfileCoverAsync(id);
}
