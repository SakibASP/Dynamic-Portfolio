using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Portfolio.Interfaces;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.ViewModels;
using Portfolio.Web.Data;

namespace Portfolio.Web.Repository
{
    public class ProfileRepo(ApplicationDbContext context) : IProfileRepo
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<IList<CONTACTS>> GetContacts() => await _context.CONTACTS.ToListAsync();

        public async Task<IList<DESCRIPTION>> GetDescriptions() => await _context.DESCRIPTION.ToListAsync();

        public async Task<IList<EDUCATION>> GetEducations() => await _context.EDUCATION.ToListAsync();

        public async Task<IList<EXPERIENCE>> GetExperiences() => await _context.EXPERIENCE.ToListAsync();

        public async Task<IList<MY_PROFILE>> GetProflie() => await _context.MY_PROFILE.ToListAsync();

        public async Task<IList<PROJECTS>> GetProjects() => await _context.PROJECTS.ToListAsync();

        public async Task<MY_PROFILE?> GetSingleProflie() => await _context.MY_PROFILE.FirstOrDefaultAsync();

        public async Task<MY_PROFILE?> GetSingleProflieById(int? id) => await _context.MY_PROFILE.FirstOrDefaultAsync(x=>x.AUTO_ID ==id);

        public async Task<IList<MY_SKILLS>> GetSkills() => await _context.MY_SKILLS.ToListAsync();

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

        public async Task<IList<VisitorsViewModel>> GetVisitors(SqlParameter[] parameters)
        {
            var visitors = await _context.Database.SqlQueryRaw<VisitorsViewModel>(Constant.udspGetVisitors, parameters).ToListAsync();
            return visitors;
        }
    }
}
