using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class EducationRepo(PortfolioDbContext context) : IEducationRepo, IAsyncDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task AddEducationAsync(SaveRequestModel<EDUCATION> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            if (string.IsNullOrEmpty(saveRequestModel.Item.TO_DATE))
                saveRequestModel.Item.TO_DATE = "Present";

            await _context.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<EDUCATION>> GetAllEducationsAsync()
        {
            return await _context.EDUCATION.ToListAsync();
        }

        public async Task<EDUCATION?> GetEducationByIdAsync(int? id)
        {
            return await _context.EDUCATION.FirstOrDefaultAsync(x => x.AUTO_ID == id);
        }

        public async Task RemoveEducationAsync(int? id)
        {
            var education = await _context.EDUCATION.FindAsync(id);
            if (education is not null)
            {
                _context.EDUCATION.Remove(education);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateEducationAsync(SaveRequestModel<EDUCATION> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            if (string.IsNullOrEmpty(saveRequestModel.Item.TO_DATE))
                saveRequestModel.Item.TO_DATE = "Present";

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
