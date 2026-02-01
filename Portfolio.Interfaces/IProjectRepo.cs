using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces;

public interface IProjectRepo
{
    /// <summary>
    /// Get All Projects
    /// </summary>
    /// <returns></returns>
    Task<IList<PROJECTS>> GetAllProjectsAsync();

    /// <summary>
    /// Get Project By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PROJECTS?> GetProjectByIdAsync(int? id);

    /// <summary>
    /// Get All descript using project id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<DESCRIPTION>> GetDescriptionByProjectIdAsync(int? id);

    /// <summary>
    /// Add new Project
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task AddProjectAsync(SaveRequestModel<PROJECTS> saveRequestModel);

    /// <summary>
    ///  Update Project
    /// </summary>
    /// <param name="saveRequestModel"></param>
    /// <returns></returns>
    Task UpdateProjectAsync(SaveRequestModel<PROJECTS> saveRequestModel);

    /// <summary>
    ///  Remove Project from database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemoveProjectAsync(int? id);
}
