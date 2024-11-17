using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Repositories
{
    public class DescriptionTypeRepo(PortfolioDbContext context) : IDescriptionTypeRepo
    {
        private readonly PortfolioDbContext _context = context;
        public async Task AddDescriptionTypeAsync(SaveRequestModel<DESCRIPTION_TYPE> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            await _context.DESCRIPTION_TYPE.AddAsync(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<DESCRIPTION_TYPE>> GetAllDescriptionTypesAsync()
        {
            return await _context.DESCRIPTION_TYPE.ToListAsync();
        }

        public async Task<DESCRIPTION_TYPE?> GetDescriptionTypeByIdAsync(int? id)
        {
            return await _context.DESCRIPTION_TYPE.FirstOrDefaultAsync(x=>x.AUTO_ID == id);
        }

        public async Task RemoveDescriptionTypeAsync(int? id)
        {
            var _type = await _context.DESCRIPTION_TYPE.FindAsync(id);
            if(_type is not null)
            {
                _context.DESCRIPTION_TYPE.Remove(_type);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDescriptionTypeAsync(SaveRequestModel<DESCRIPTION_TYPE> saveRequestModel)
        {
            ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
            _context.DESCRIPTION_TYPE.Update(saveRequestModel.Item);
            await _context.SaveChangesAsync();
        }
    }
}
