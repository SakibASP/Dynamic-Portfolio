using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces;

public interface IDescriptionRepo
{
    /// <summary>
    /// Get All Description
    /// </summary>
    /// <returns></returns>
    Task<IList<DESCRIPTION>> GetAllDescriptionsAsync();

    /// <summary>
    /// Get Description By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DESCRIPTION?> GetDescriptionByIdAsync(int? id);

    /// <summary>
    /// Add new description type
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task AddDescriptionAsync(SaveRequestModel<DESCRIPTION> saveRequestModel);

    /// <summary>
    ///  Update description
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task UpdateDescriptionAsync(SaveRequestModel<DESCRIPTION> saveRequestModel);

    /// <summary>
    ///  Remove description from database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveDescriptionAsync(int? id);
}
