using Portfolio.Application.DTOs;
using Portfolio.Domain;

namespace Portfolio.Application.Abstractions;

public interface IHomeRepo
{
    Task<int?> GetUnreadMessagesCountAsync();
    Task<bool> SaveLocationAsync(LocationRequest request);

    Task<MY_PROFILE?> GetProfileAsync();
    Task<EXPERIENCE?> GetCurrentExperienceAsync();
    Task<IList<EXPERIENCE>> GetCompaniesWithLogosAsync();
    Task<IList<BLOG_POSTS>> GetFeaturedBlogPostsAsync(int take);
}
