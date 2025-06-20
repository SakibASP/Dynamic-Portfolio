﻿using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class ExperienceRepo(PortfolioDbContext context) : IExperienceRepo, IAsyncDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task AddExperienceAsync(SaveRequestModel<EXPERIENCE> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            if (string.IsNullOrEmpty(saveRequestModel.Item.TO_DATE))
                saveRequestModel.Item.TO_DATE = "Present";

            await _context.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<EXPERIENCE>> GetAllExperiencesAsync()
        {
            return await _context.EXPERIENCE.OrderByDescending(x => x.SORT_ORDER).AsNoTracking().ToListAsync();
        }

        public async Task<EXPERIENCE?> GetExperienceByIdAsync(int? id)
        {
            return await _context.EXPERIENCE.FirstOrDefaultAsync(x => x.AUTO_ID == id);
        }

        public async Task RemoveExperienceAsync(int? id)
        {
            var experience = await _context.EXPERIENCE.FindAsync(id);
            if (experience is not null)
            {
                _context.EXPERIENCE.Remove(experience);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateExperienceAsync(SaveRequestModel<EXPERIENCE> saveRequestModel)
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
