using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces
{
    public interface ISkillsRepo
    {
        /// <summary>
        /// Get All Skills
        /// </summary>
        /// <returns></returns>
        Task<IList<MY_SKILLS>> GetAllSkillsAsync();

        /// <summary>
        /// Get Skill By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MY_SKILLS?> GetSkillByIdAsync(int? id);

        /// <summary>
        /// Add new Skill
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task AddSkillAsync(SaveRequestModel<MY_SKILLS> saveRequestModel);

        /// <summary>
        ///  Update Skill
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task UpdateSkillAsync(SaveRequestModel<MY_SKILLS> saveRequestModel);

        /// <summary>
        ///  Remove Skill from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveSkillAsync(int? id);
    }
}
