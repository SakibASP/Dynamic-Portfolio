using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces
{
    public interface IProfileRepo
    {
        /// <summary>
        /// Getting all profiles
        /// </summary>
        /// <returns></returns>
        Task<IList<MY_PROFILE>> GetProflie();

        /// <summary>
        /// Get a single profile from the profile list
        /// </summary>
        /// <returns></returns>
        Task<MY_PROFILE?> GetSingleProflie();

        /// <summary>
        ///  Get a single profile using it's single id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MY_PROFILE?> GetSingleProflieById(int? id);

        /// <summary>
        /// Getting all skills
        /// </summary>
        /// <returns></returns>
        Task<IList<MY_SKILLS>> GetSkills();

        /// <summary>
        /// Getting all educations
        /// </summary>
        /// <returns></returns>
        Task<IList<EDUCATION>> GetEducations();

        /// <summary>
        /// Getting all experiences
        /// </summary>
        /// <returns></returns>
        Task<IList<EXPERIENCE>> GetExperiences();

        /// <summary>
        /// Getting all descriptions
        /// </summary>
        /// <returns></returns>
        Task<IList<DESCRIPTION>> GetDescriptions();

        /// <summary>
        /// Getting all of my projects list
        /// </summary>
        /// <returns></returns>
        Task<IList<PROJECTS>> GetProjects();

        /// <summary>
        /// Getting all contacts
        /// </summary>
        /// <returns></returns>
        Task<IList<CONTACTS>> GetContacts();

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
    }
}
