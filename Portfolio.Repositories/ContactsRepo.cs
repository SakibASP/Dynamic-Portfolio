using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class ContactsRepo(PortfolioDbContext context) : IContactsRepo, IDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task ConfirmContactAsync(SaveRequestModel<CONTACTS> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            saveRequestModel.Item.IsConfirmed = 1;
            saveRequestModel.Item.MODIFIED_DATE = saveRequestModel.BdCurrentTime;
            _context.CONTACTS.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<CONTACTS>> GetAllContactsAsync()
        {
            return await _context.CONTACTS.ToListAsync();
        }

        public async Task<IList<CONTACTS>> GetAllPendingContactsAsync()
        {
            return await _context.CONTACTS.Where(x=>x.IsConfirmed == null).ToListAsync();
        }

        public async Task<CONTACTS?> GetContactByIdAsync(int? id)
        {
            return await _context.CONTACTS.FirstOrDefaultAsync(m => m.AUTO_ID == id);
        }

        public async Task RemoveContactAsync(int? id)
        {
            var contact = await _context.CONTACTS.FindAsync(id);
            if (contact is not null)
            {
                _context.CONTACTS.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateContactAsync(SaveRequestModel<CONTACTS> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            _context.CONTACTS.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
