using Portfolio.Models;
using Portfolio.Utils;

namespace Portfolio.Interfaces
{
    public interface IContactsRepo
    {
        /// <summary>
        /// Get All Contacts
        /// </summary>
        /// <returns></returns>
        Task<IList<CONTACTS>> GetAllContactsAsync();

        /// <summary>
        /// Get All Contacts which are not seen [confirmed] yet
        /// </summary>
        /// <returns></returns>
        Task<IList<CONTACTS>> GetAllPendingContactsAsync();

        /// <summary>
        /// Get Contact By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CONTACTS?> GetContactByIdAsync(int? id);

        /// <summary>
        /// Add new Contact
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task ConfirmContactAsync(SaveRequestModel<CONTACTS> saveRequestModel);

        /// <summary>
        ///  Update Contact
        /// </summary>
        /// <param name="saveRequestModel"></param>
        /// <returns></returns>
        Task UpdateContactAsync(SaveRequestModel<CONTACTS> saveRequestModel);

        /// <summary>
        ///  Remove Contact from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveContactAsync(int? id);
    }
}
