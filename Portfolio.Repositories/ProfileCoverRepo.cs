using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class ProfileCoverRepo(PortfolioDbContext context) : IProfileCoverRepo, IAsyncDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task AddProfileCoverAsync(SaveRequestModel<PROFILE_COVER> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            await _context.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public  async Task<IList<PROFILE_COVER>> GetAllProfileCoversAsync()
        {
            return await _context.PROFILE_COVER.AsNoTracking().ToListAsync();
        }

        public async Task<PROFILE_COVER?> GetProfileCoverByIdAsync(int? id)
        {
            return await _context.PROFILE_COVER.FirstOrDefaultAsync(m => m.AUTO_ID == id);
        }

        public async Task RemoveProfileCoverAsync(int? id)
        {
            var cover = await _context.PROFILE_COVER.FindAsync(id);
            if (cover is not null)
            {
                _context.PROFILE_COVER.Remove(cover);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProfileCoverAsync(SaveRequestModel<PROFILE_COVER> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            _context.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
