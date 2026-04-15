using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Abstractions;

public interface IBlogRepo
{
    Task<IList<BLOG_POSTS>> GetAllPostsAsync(bool onlyPublished);
    Task<BLOG_POSTS?> GetPostByIdAsync(int? id, bool includeBlocks = true);
    Task<BLOG_POSTS?> GetPostBySlugAsync(string slug, bool includeBlocks = true);

    Task<int> AddPostAsync(SaveRequestModel<BLOG_POSTS> saveRequestModel);
    Task UpdatePostAsync(SaveRequestModel<BLOG_POSTS> saveRequestModel);
    Task RemovePostAsync(int? id);
    Task IncrementViewCountAsync(int id);

    Task<IList<BLOG_POST_BLOCKS>> GetBlocksByPostIdAsync(int postId);
    Task<BLOG_POST_BLOCKS?> GetBlockByIdAsync(int? id);
    Task AddBlockAsync(SaveRequestModel<BLOG_POST_BLOCKS> saveRequestModel);
    Task UpdateBlockAsync(SaveRequestModel<BLOG_POST_BLOCKS> saveRequestModel);
    Task RemoveBlockAsync(int? id);
    Task MoveBlockAsync(int id, int direction);
}
