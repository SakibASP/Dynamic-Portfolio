using Portfolio.Models;

namespace Portfolio.Interfaces
{
    public interface IHomeRepo
    {
        /// <summary>
        /// Get the messages count which are not confirmed yet
        /// </summary>
        /// <returns></returns>
        Task<int?> GetUnreadMessagesCountAsync();
    }
}
