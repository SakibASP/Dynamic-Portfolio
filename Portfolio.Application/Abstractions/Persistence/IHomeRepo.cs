using Portfolio.Application.DTOs;

namespace Portfolio.Application.Abstractions;

public interface IHomeRepo
{
    /// <summary>
    /// Get the messages count which are not confirmed yet
    /// </summary>
    /// <returns></returns>
    Task<int?> GetUnreadMessagesCountAsync();

    /// <summary>
    /// Saves visitor location information
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> SaveLocationAsync(LocationRequest request);
}
