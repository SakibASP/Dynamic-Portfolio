using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces;

public interface IExperienceRepo
{
    /// <summary>
    /// Get All Experience
    /// </summary>
    /// <returns></returns>
    Task<IList<EXPERIENCE>> GetAllExperiencesAsync();

    /// <summary>
    /// Get Experience By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EXPERIENCE?> GetExperienceByIdAsync(int? id);

    /// <summary>
    /// Add new Experience
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task AddExperienceAsync(SaveRequestModel<EXPERIENCE> saveRequestModel);

    /// <summary>
    ///  Update Experience
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task UpdateExperienceAsync(SaveRequestModel<EXPERIENCE> saveRequestModel);

    /// <summary>
    ///  Remove Experience from database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveExperienceAsync(int? id);
}
