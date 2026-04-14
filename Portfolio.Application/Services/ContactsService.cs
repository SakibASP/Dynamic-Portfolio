using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public class ContactsService(IContactsRepo repo) : IContactsService
{
    private readonly IContactsRepo _repo = repo;

    public Task<IList<CONTACTS>> GetAllAsync() => _repo.GetAllContactsAsync();
    public Task<IList<CONTACTS>> GetPendingAsync() => _repo.GetAllPendingContactsAsync();
    public Task<CONTACTS?> GetByIdAsync(int? id) => _repo.GetContactByIdAsync(id);

    public Task ConfirmAsync(CONTACTS contact, string? userName) =>
        _repo.ConfirmContactAsync(GenerateParameter.SingleModel(contact, userName, AppClock.BdNow));

    public Task UpdateAsync(CONTACTS contact, string? userName) =>
        _repo.UpdateContactAsync(GenerateParameter.SingleModel(contact, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemoveContactAsync(id);
}
