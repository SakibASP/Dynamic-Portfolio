using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IDescriptionService
{
    Task<IList<DESCRIPTION>> GetAllAsync();
    Task<DESCRIPTION?> GetByIdAsync(int? id);
    Task CreateAsync(DESCRIPTION item, string? userName);
    Task UpdateAsync(DESCRIPTION item, string? userName);
    Task RemoveAsync(int? id);
}

public interface IDescriptionTypeService
{
    Task<IList<DESCRIPTION_TYPE>> GetAllAsync();
    Task<DESCRIPTION_TYPE?> GetByIdAsync(int? id);
    Task CreateAsync(DESCRIPTION_TYPE item, string? userName);
    Task UpdateAsync(DESCRIPTION_TYPE item, string? userName);
    Task RemoveAsync(int? id);
}
