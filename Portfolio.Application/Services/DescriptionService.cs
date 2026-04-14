using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public class DescriptionService(IDescriptionRepo repo) : IDescriptionService
{
    private readonly IDescriptionRepo _repo = repo;

    public Task<IList<DESCRIPTION>> GetAllAsync() => _repo.GetAllDescriptionsAsync();
    public Task<DESCRIPTION?> GetByIdAsync(int? id) => _repo.GetDescriptionByIdAsync(id);

    public Task CreateAsync(DESCRIPTION item, string? userName)
    {
        item.CREATED_BY = userName;
        item.CREATED_DATE = AppClock.BdNow;
        return _repo.AddDescriptionAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));
    }

    public Task UpdateAsync(DESCRIPTION item, string? userName)
    {
        item.MODIFIED_BY = userName;
        item.MODIFIED_DATE = AppClock.BdNow;
        return _repo.UpdateDescriptionAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));
    }

    public Task RemoveAsync(int? id) => _repo.RemoveDescriptionAsync(id);
}

public class DescriptionTypeService(IDescriptionTypeRepo repo) : IDescriptionTypeService
{
    private readonly IDescriptionTypeRepo _repo = repo;

    public Task<IList<DESCRIPTION_TYPE>> GetAllAsync() => _repo.GetAllDescriptionTypesAsync();
    public Task<DESCRIPTION_TYPE?> GetByIdAsync(int? id) => _repo.GetDescriptionTypeByIdAsync(id);

    public Task CreateAsync(DESCRIPTION_TYPE item, string? userName) =>
        _repo.AddDescriptionTypeAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(DESCRIPTION_TYPE item, string? userName) =>
        _repo.UpdateDescriptionTypeAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveDescriptionTypeAsync(id);
}
