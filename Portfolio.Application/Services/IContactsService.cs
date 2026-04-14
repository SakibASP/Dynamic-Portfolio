using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IContactsService
{
    Task<IList<CONTACTS>> GetAllAsync();
    Task<IList<CONTACTS>> GetPendingAsync();
    Task<CONTACTS?> GetByIdAsync(int? id);
    Task ConfirmAsync(CONTACTS contact, string? userName);
    Task UpdateAsync(CONTACTS contact, string? userName);
    Task RemoveAsync(int? id);
}
