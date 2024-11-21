using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class ProjectRepo(PortfolioDbContext context) : IProjectRepo, IDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task AddProjectAsync(SaveRequestModel<PROJECTS> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            await _context.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<PROJECTS>> GetAllProjectsAsync()
        {
            return await _context.PROJECTS.ToListAsync();
        }

        public async Task<IList<DESCRIPTION>> GetDescriptionByProjectIdAsync(int? id)
        {
            return await _context.DESCRIPTION
                .Where(x => x.PROJECT_ID == id)
                .OrderBy(x => x.SORT_ORDER)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PROJECTS?> GetProjectByIdAsync(int? id)
        {
            return await _context.PROJECTS.FirstOrDefaultAsync(m => m.AUTO_ID == id);
        }

        public async Task RemoveProjectAsync(int? id)
        {
            var project = await _context.PROJECTS.FindAsync(id);
            if (project is not null)
            {
                _context.PROJECTS.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProjectAsync(SaveRequestModel<PROJECTS> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            _context.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
