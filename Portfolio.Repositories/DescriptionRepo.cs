using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class DescriptionRepo(PortfolioDbContext context) : IDescriptionRepo, IDisposable
    {
        private readonly PortfolioDbContext _context = context;

        public async Task AddDescriptionAsync(SaveRequestModel<DESCRIPTION> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            saveRequestModel.Item.CREATED_BY = saveRequestModel.UserName;
            saveRequestModel.Item.CREATED_DATE = saveRequestModel.BdCurrentTime;
            await _context.DESCRIPTION.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<DESCRIPTION>> GetAllDescriptionsAsync()
        {
            return await _context.DESCRIPTION
                .OrderBy(d => d.EXPERIENCE_ID == null) // Experience with IDs comes first
                .ThenBy(d => d.PROJECT_ID)
                .ThenBy(d => d.SORT_ORDER)            // Sort by SORT_ORDER
                .ToListAsync();
        }

        public async Task<DESCRIPTION?> GetDescriptionByIdAsync(int? id)
        {
            return await _context.DESCRIPTION.FirstOrDefaultAsync(x => x.AUTO_ID == id);
        }

        public async Task RemoveDescriptionAsync(int? id)
        {
            var description = await _context.DESCRIPTION.FindAsync(id);
            if (description is not null)
            {
                _context.DESCRIPTION.Remove(description);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDescriptionAsync(SaveRequestModel<DESCRIPTION> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            saveRequestModel.Item.MODIFIED_BY = saveRequestModel.UserName;
            saveRequestModel.Item.MODIFIED_DATE = saveRequestModel.BdCurrentTime;
            _context.DESCRIPTION.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
