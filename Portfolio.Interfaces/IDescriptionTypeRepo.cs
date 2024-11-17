using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces
{
    public interface IDescriptionTypeRepo
    {
        /// <summary>
        /// Get All Description Types
        /// </summary>
        /// <returns></returns>
        Task<IList<DESCRIPTION_TYPE>> GetAllDescriptionTypesAsync();

        /// <summary>
        /// Get Description Type By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DESCRIPTION_TYPE?> GetDescriptionTypeByIdAsync(int? id);

        /// <summary>
        /// Add new description type
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task AddDescriptionTypeAsync(SaveRequestModel<DESCRIPTION_TYPE> saveRequestModel);

        /// <summary>
        ///  Update description type
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task UpdateDescriptionTypeAsync(SaveRequestModel<DESCRIPTION_TYPE> saveRequestModel);

        /// <summary>
        ///  Remove description type from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveDescriptionTypeAsync(int? id);
    }
}
