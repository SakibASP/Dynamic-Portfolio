using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions;
using Portfolio.Application.Common;
using Portfolio.Domain;
using Portfolio.Infrastructure.Persistence;

namespace Portfolio.Infrastructure;

public class BlogRepo(PortfolioDbContext context) : IBlogRepo
{
    private readonly PortfolioDbContext _context = context;

    public async Task<IList<BLOG_POSTS>> GetAllPostsAsync(bool onlyPublished)
    {
        var q = _context.BLOG_POSTS.AsNoTracking();
        if (onlyPublished) q = q.Where(p => p.IS_PUBLISHED);
        return await q
            .OrderByDescending(p => p.PUBLISHED_DATE ?? p.CREATED_DATE)
            .ToListAsync();
    }

    public async Task<BLOG_POSTS?> GetPostByIdAsync(int? id, bool includeBlocks = true)
    {
        var post = await _context.BLOG_POSTS.FirstOrDefaultAsync(p => p.AUTO_ID == id);
        if (post != null && includeBlocks)
            post.BLOCKS = await GetBlocksByPostIdAsync(post.AUTO_ID);
        return post;
    }

    public async Task<BLOG_POSTS?> GetPostBySlugAsync(string slug, bool includeBlocks = true)
    {
        var post = await _context.BLOG_POSTS.FirstOrDefaultAsync(p => p.SLUG == slug);
        if (post != null && includeBlocks)
            post.BLOCKS = await GetBlocksByPostIdAsync(post.AUTO_ID);
        return post;
    }

    public async Task<int> AddPostAsync(SaveRequestModel<BLOG_POSTS> saveRequestModel)
    {
        ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
        var item = saveRequestModel.Item;
        item.CREATED_BY   = saveRequestModel.UserName;
        item.CREATED_DATE = saveRequestModel.BdCurrentTime;
        if (item.IS_PUBLISHED && item.PUBLISHED_DATE == null)
            item.PUBLISHED_DATE = saveRequestModel.BdCurrentTime;
        await _context.BLOG_POSTS.AddAsync(item);
        await _context.SaveChangesAsync();
        return item.AUTO_ID;
    }

    public async Task UpdatePostAsync(SaveRequestModel<BLOG_POSTS> saveRequestModel)
    {
        ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
        var item = saveRequestModel.Item;
        item.UPDATED_BY   = saveRequestModel.UserName;
        item.UPDATED_DATE = saveRequestModel.BdCurrentTime;
        if (item.IS_PUBLISHED && item.PUBLISHED_DATE == null)
            item.PUBLISHED_DATE = saveRequestModel.BdCurrentTime;
        _context.BLOG_POSTS.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemovePostAsync(int? id)
    {
        var post = await _context.BLOG_POSTS.FindAsync(id);
        if (post is null) return;
        var blocks = _context.BLOG_POST_BLOCKS.Where(b => b.BLOG_POST_ID == post.AUTO_ID);
        _context.BLOG_POST_BLOCKS.RemoveRange(blocks);
        _context.BLOG_POSTS.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task IncrementViewCountAsync(int id)
    {
        await _context.BLOG_POSTS
            .Where(p => p.AUTO_ID == id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.VIEW_COUNT, p => p.VIEW_COUNT + 1));
    }

    public async Task<IList<BLOG_POST_BLOCKS>> GetBlocksByPostIdAsync(int postId)
    {
        return await _context.BLOG_POST_BLOCKS.AsNoTracking()
            .Where(b => b.BLOG_POST_ID == postId)
            .OrderBy(b => b.SORT_ORDER).ThenBy(b => b.AUTO_ID)
            .ToListAsync();
    }

    public async Task<BLOG_POST_BLOCKS?> GetBlockByIdAsync(int? id)
        => await _context.BLOG_POST_BLOCKS.FirstOrDefaultAsync(b => b.AUTO_ID == id);

    public async Task AddBlockAsync(SaveRequestModel<BLOG_POST_BLOCKS> saveRequestModel)
    {
        ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
        var item = saveRequestModel.Item;
        item.CREATED_BY   = saveRequestModel.UserName;
        item.CREATED_DATE = saveRequestModel.BdCurrentTime;
        if (item.SORT_ORDER == 0)
        {
            var max = await _context.BLOG_POST_BLOCKS
                .Where(b => b.BLOG_POST_ID == item.BLOG_POST_ID)
                .Select(b => (int?)b.SORT_ORDER).MaxAsync() ?? 0;
            item.SORT_ORDER = max + 10;
        }
        await _context.BLOG_POST_BLOCKS.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBlockAsync(SaveRequestModel<BLOG_POST_BLOCKS> saveRequestModel)
    {
        ArgumentNullException.ThrowIfNull(saveRequestModel.Item);
        var item = saveRequestModel.Item;
        item.UPDATED_BY   = saveRequestModel.UserName;
        item.UPDATED_DATE = saveRequestModel.BdCurrentTime;
        _context.BLOG_POST_BLOCKS.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveBlockAsync(int? id)
    {
        var block = await _context.BLOG_POST_BLOCKS.FindAsync(id);
        if (block is null) return;
        _context.BLOG_POST_BLOCKS.Remove(block);
        await _context.SaveChangesAsync();
    }

    public async Task MoveBlockAsync(int id, int direction)
    {
        var block = await _context.BLOG_POST_BLOCKS.FindAsync(id);
        if (block is null) return;

        var siblings = await _context.BLOG_POST_BLOCKS
            .Where(b => b.BLOG_POST_ID == block.BLOG_POST_ID)
            .OrderBy(b => b.SORT_ORDER).ThenBy(b => b.AUTO_ID)
            .ToListAsync();

        var idx = siblings.FindIndex(b => b.AUTO_ID == id);
        var swapIdx = idx + direction;
        if (idx < 0 || swapIdx < 0 || swapIdx >= siblings.Count) return;

        (siblings[idx].SORT_ORDER, siblings[swapIdx].SORT_ORDER) =
            (siblings[swapIdx].SORT_ORDER, siblings[idx].SORT_ORDER);

        await _context.SaveChangesAsync();
    }
}
