using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces
{
    public interface IProfileCoverRepo
    {
        /// <summary>
        /// Get All ProfileCovers
        /// </summary>
        /// <returns></returns>
        Task<IList<PROFILE_COVER>> GetAllProfileCoversAsync();

        /// <summary>
        /// Get ProfileCover By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PROFILE_COVER?> GetProfileCoverByIdAsync(int? id);

        /// <summary>
        /// Add new ProfileCover
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task AddProfileCoverAsync(SaveRequestModel<PROFILE_COVER> saveRequestModel);

        /// <summary>
        ///  Update ProfileCover
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task UpdateProfileCoverAsync(SaveRequestModel<PROFILE_COVER> saveRequestModel);

        /// <summary>
        ///  Remove ProfileCover from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveProfileCoverAsync(int? id);
    }
}
