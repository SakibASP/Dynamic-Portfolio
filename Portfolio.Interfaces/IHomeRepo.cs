using Portfolio.ViewModels;

namespace Portfolio.Interfaces;

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
