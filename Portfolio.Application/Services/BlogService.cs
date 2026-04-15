using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;

namespace Portfolio.Application.Services;

public interface IBlogService
{
    Task<IList<BLOG_POSTS>> GetAllAsync(bool onlyPublished);
    Task<BLOG_POSTS?> GetByIdAsync(int? id, bool includeBlocks = true);
    Task<BLOG_POSTS?> GetBySlugAsync(string slug, bool includeBlocks = true);
    Task<int> CreateAsync(BLOG_POSTS item, string? userName);
    Task UpdateAsync(BLOG_POSTS item, string? userName);
    Task RemoveAsync(int? id);
    Task TrackViewAsync(int id);

    Task<BLOG_POST_BLOCKS?> GetBlockAsync(int? id);
    Task AddBlockAsync(BLOG_POST_BLOCKS block, string? userName);
    Task UpdateBlockAsync(BLOG_POST_BLOCKS block, string? userName);
    Task RemoveBlockAsync(int? id);
    Task MoveBlockAsync(int id, int direction);
}

public class BlogService(IBlogRepo repo) : IBlogService
{
    private readonly IBlogRepo _repo = repo;

    public Task<IList<BLOG_POSTS>> GetAllAsync(bool onlyPublished) => _repo.GetAllPostsAsync(onlyPublished);
    public Task<BLOG_POSTS?> GetByIdAsync(int? id, bool includeBlocks = true) => _repo.GetPostByIdAsync(id, includeBlocks);
    public Task<BLOG_POSTS?> GetBySlugAsync(string slug, bool includeBlocks = true) => _repo.GetPostBySlugAsync(slug, includeBlocks);

    public Task<int> CreateAsync(BLOG_POSTS item, string? userName) =>
        _repo.AddPostAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task UpdateAsync(BLOG_POSTS item, string? userName) =>
        _repo.UpdatePostAsync(GenerateParameter.SingleModel(item, userName, AppClock.BdNow));

    public Task RemoveAsync(int? id) => _repo.RemovePostAsync(id);
    public Task TrackViewAsync(int id) => _repo.IncrementViewCountAsync(id);

    public Task<BLOG_POST_BLOCKS?> GetBlockAsync(int? id) => _repo.GetBlockByIdAsync(id);

    public Task AddBlockAsync(BLOG_POST_BLOCKS block, string? userName) =>
        _repo.AddBlockAsync(GenerateParameter.SingleModel(block, userName, AppClock.BdNow));

    public Task UpdateBlockAsync(BLOG_POST_BLOCKS block, string? userName) =>
        _repo.UpdateBlockAsync(GenerateParameter.SingleModel(block, userName, AppClock.BdNow));

    public Task RemoveBlockAsync(int? id) => _repo.RemoveBlockAsync(id);
    public Task MoveBlockAsync(int id, int direction) => _repo.MoveBlockAsync(id, direction);
}
