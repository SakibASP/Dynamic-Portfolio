using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Repositories.Data;
using Portfolio.Utils;
using Portfolio.ViewModels;

namespace Portfolio.Repositories
{
    public class ProfileRepo(PortfolioDbContext context) : IProfileRepo, IAsyncDisposable
    {
        private readonly PortfolioDbContext _context = context;
        public async Task<IList<CONTACTS>> GetContactsAsync() => await _context.CONTACTS.AsNoTracking().ToListAsync();
        public async Task<IList<DESCRIPTION>> GetDescriptionsAsync() => await _context.DESCRIPTION.AsNoTracking().ToListAsync();
        public async Task<IList<EDUCATION>> GetEducationsAsync() => await _context.EDUCATION.AsNoTracking().ToListAsync();
        public async Task<IList<EXPERIENCE>> GetExperiencesAsync() => await _context.EXPERIENCE.OrderByDescending(x => x.SORT_ORDER).AsNoTracking().ToListAsync();
        public async Task<IList<MY_PROFILE>> GetProflieAsync() => await _context.MY_PROFILE.AsNoTracking().ToListAsync();
        public async Task<IList<PROJECTS>> GetProjectsAsync() => await _context.PROJECTS.AsNoTracking().ToListAsync();
        public async Task<MY_PROFILE?> GetSingleProflieAsync() => await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync();
        public async Task<MY_PROFILE?> GetSingleProflieByIdAsync(int? id) => await _context.MY_PROFILE.AsNoTracking().FirstOrDefaultAsync(x=>x.AUTO_ID ==id);
        public async Task<IList<MY_SKILLS>> GetSkillsAsync() => await _context.MY_SKILLS.AsNoTracking().ToListAsync();

        public async Task AddContactInfoAsync(SaveRequestModel<CONTACTS> saveRequest)
        {
            ArgumentNullException.ThrowIfNull(saveRequest.Item);
            saveRequest.Item.CREATED_DATE = saveRequest.BdCurrentTime;
            await _context.CONTACTS.AddAsync(saveRequest.Item);
            await _context.SaveChangesAsync();
        }

        public async Task AddProfileAsync(SaveRequestModel<MY_PROFILE> saveRequest)
        {
            ArgumentNullException.ThrowIfNull(saveRequest.Item);
            await _context.MY_PROFILE.AddAsync(saveRequest.Item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(SaveRequestModel<MY_PROFILE> saveRequest)
        {
            ArgumentNullException.ThrowIfNull(saveRequest.Item);
            _context.MY_PROFILE.Update(saveRequest.Item);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<VisitorsViewModel>> GetVisitorsAsync(SqlParameter[] parameters)
        {
            var visitors = await _context.Database.SqlQueryRaw<VisitorsViewModel>(Constant.udspGetVisitors, parameters).ToListAsync();
            return visitors;
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
