using Portfolio.Domain;
using Portfolio.Application.Common;

namespace Portfolio.Application.Abstractions;

public interface IEducationRepo
{
    /// <summary>
    /// Get All Education
    /// </summary>
    /// <returns></returns>
    Task<IList<EDUCATION>> GetAllEducationsAsync();

    /// <summary>
    /// Get Education By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EDUCATION?> GetEducationByIdAsync(int? id);

    /// <summary>
    /// Add new Education
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task AddEducationAsync(SaveRequestModel<EDUCATION> saveRequestModel);

    /// <summary>
    ///  Update Education
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task UpdateEducationAsync(SaveRequestModel<EDUCATION> saveRequestModel);

    /// <summary>
    ///  Remove Education from database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveEducationAsync(int? id);
}
