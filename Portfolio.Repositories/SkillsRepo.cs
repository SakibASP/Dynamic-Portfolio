using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class SkillsRepo(PortfolioDbContext context) : ISkillsRepo, IDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task AddSkillAsync(SaveRequestModel<MY_SKILLS> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            await _context.MY_SKILLS.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<MY_SKILLS>> GetAllSkillsAsync()
        {
            return await _context.MY_SKILLS.ToListAsync();
        }

        public async Task<MY_SKILLS?> GetSkillByIdAsync(int? id)
        {
            return await _context.MY_SKILLS.FirstOrDefaultAsync(x => x.AUTO_ID == id);
        }

        public async Task RemoveSkillAsync(int? id)
        {
            var skill = await _context.MY_SKILLS.FindAsync(id);
            if (skill is not null)
            {
                _context.MY_SKILLS.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateSkillAsync(SaveRequestModel<MY_SKILLS> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            _context.MY_SKILLS.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
