using Microsoft.Data.SqlClient;
using Portfolio.Models;
using Portfolio.Utils;
using Portfolio.ViewModels;

namespace Portfolio.Interfaces
{
    public interface IProfileRepo
    {
        /// <summary>
        /// Getting all profiles
        /// </summary>
        /// <returns></returns>
        Task<IList<MY_PROFILE>> GetProflieAsync();

        /// <summary>
        /// Get a single profile from the profile list
        /// </summary>
        /// <returns></returns>
        Task<MY_PROFILE?> GetSingleProflieAsync();

        /// <summary>
        ///  Get a single profile using it's single id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MY_PROFILE?> GetSingleProflieByIdAsync(int? id);

        /// <summary>
        /// Getting all skills
        /// </summary>
        /// <returns></returns>
        Task<IList<MY_SKILLS>> GetSkillsAsync();

        /// <summary>
        /// Getting all educations
        /// </summary>
        /// <returns></returns>
        Task<IList<EDUCATION>> GetEducationsAsync();

        /// <summary>
        /// Getting all experiences
        /// </summary>
        /// <returns></returns>
        Task<IList<EXPERIENCE>> GetExperiencesAsync();

        /// <summary>
        /// Getting all descriptions
        /// </summary>
        /// <returns></returns>
        Task<IList<DESCRIPTION>> GetDescriptionsAsync();

        /// <summary>
        /// Getting all of my projects list
        /// </summary>
        /// <returns></returns>
        Task<IList<PROJECTS>> GetProjectsAsync();

        /// <summary>
        /// Getting all contacts
        /// </summary>
        /// <returns></returns>
        Task<IList<CONTACTS>> GetContactsAsync();

        /// <summary>
        /// Addning contact info while someone send me message
        /// </summary>
        /// <param name="saveRequest"></param>
        /// <returns></returns>
        Task AddContactInfoAsync(SaveRequestModel<CONTACTS> saveRequest);

        /// <summary>
        /// Adding new profiles
        /// </summary>
        /// <param name="saveRequest"></param>
        /// <returns></returns>
        Task AddProfileAsync(SaveRequestModel<MY_PROFILE> saveRequest);

        /// <summary>
        /// Updating existing profile
        /// </summary>
        /// <param name="saveRequest"></param>
        /// <returns></returns>
        Task UpdateProfileAsync(SaveRequestModel<MY_PROFILE> saveRequest);

        /// <summary>
        /// Get Visitors from Database
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IList<VisitorsViewModel>> GetVisitorsAsync(SqlParameter[] parameters);
    }
}
