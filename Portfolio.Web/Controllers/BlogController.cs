using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Common;
using Portfolio.Application.Services;
using Portfolio.Domain;
using Portfolio.Web.Common;
using Serilog;

namespace Portfolio.Web.Controllers;

public class BlogController(IBlogService blog, IWebHostEnvironment env) : BaseController
{
    private readonly IBlogService _blog = blog;
    private readonly IWebHostEnvironment _env = env;

    // Public: list of published posts
    public async Task<IActionResult> Index()
    {
        var showAll = User.Identity?.IsAuthenticated == true;
        var posts = await _blog.GetAllAsync(onlyPublished: !showAll);
        ViewData["rootPath"] = _env.WebRootPath;
        return View(posts);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var post = await _blog.GetByIdAsync(id);
        if (post == null) return NotFound();
        if (!post.IS_PUBLISHED && User.Identity?.IsAuthenticated != true) return NotFound();

        await _blog.TrackViewAsync(post.AUTO_ID);
        ViewData["rootPath"] = _env.WebRootPath;
        return View(post);
    }

    public async Task<IActionResult> Read(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return NotFound();
        var post = await _blog.GetBySlugAsync(slug);
        if (post == null) return NotFound();
        if (!post.IS_PUBLISHED && User.Identity?.IsAuthenticated != true) return NotFound();

        await _blog.TrackViewAsync(post.AUTO_ID);
        ViewData["rootPath"] = _env.WebRootPath;
        return View(nameof(Details), post);
    }

    // =========================================================
    //  Admin - post CRUD
    // =========================================================

    [Authorize]
    public async Task<IActionResult> Manage()
    {
        ViewData["rootPath"] = _env.WebRootPath;
        return View(await _blog.GetAllAsync(onlyPublished: false));
    }

    [Authorize]
    public IActionResult Create() => View(new BLOG_POSTS());

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BLOG_POSTS post)
    {
        try
        {
            var file = Request.Form.Files.FirstOrDefault();
            var saved = await Utility.SaveUploadedAsync(file, _env, "Images", "Blog", "Cover");
            if (saved != null) post.COVER_IMAGE = saved;

            var id = await _blog.CreateAsync(post, CurrentUserName);
            TempData[Constant.Success] = Constant.SuccessMessage;
            return RedirectToAction(nameof(Edit), new { id });
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return View(post);
    }

    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var post = await _blog.GetByIdAsync(id);
        if (post == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(post);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BLOG_POSTS post)
    {
        if (id != post.AUTO_ID) return NotFound();
        try
        {
            var existing = await _blog.GetByIdAsync(id, includeBlocks: false);
            post.CREATED_BY   = existing?.CREATED_BY;
            post.CREATED_DATE = existing?.CREATED_DATE ?? DateTime.Now;
            post.VIEW_COUNT   = existing?.VIEW_COUNT ?? 0;

            var file = Request.Form.Files.FirstOrDefault();
            var saved = await Utility.SaveUploadedAsync(file, _env, "Images", "Blog", "Cover");
            if (saved != null)
            {
                Utility.DeleteIfExists(existing?.COVER_IMAGE);
                post.COVER_IMAGE = saved;
            }
            else
            {
                post.COVER_IMAGE = existing?.COVER_IMAGE;
            }

            await _blog.UpdateAsync(post, CurrentUserName);
            TempData[Constant.Success] = Constant.SuccessMessage;
            return RedirectToAction(nameof(Edit), new { id });
        }
        catch (Exception ex) { LogAndFlash(ex); }
        ViewData["rootPath"] = _env.WebRootPath;
        return View(post);
    }

    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var post = await _blog.GetByIdAsync(id, includeBlocks: false);
        return post == null ? NotFound() : View(post);
    }

    [HttpPost, ActionName("Delete"), Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var post = await _blog.GetByIdAsync(id);
            if (post != null)
            {
                Utility.DeleteIfExists(post.COVER_IMAGE);
                foreach (var b in post.BLOCKS) Utility.DeleteIfExists(b.IMAGE_PATH);
            }
            await _blog.RemoveAsync(id);
            TempData[Constant.Success] = Constant.SuccessRmvMsg;
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Manage));
    }

    // =========================================================
    //  Admin - block CRUD
    // =========================================================

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBlock(BLOG_POST_BLOCKS block)
    {
        try
        {
            var file = Request.Form.Files.FirstOrDefault();
            var saved = await Utility.SaveUploadedAsync(file, _env, "Images", "Blog", "Blocks");
            if (saved != null) block.IMAGE_PATH = saved;

            await _blog.AddBlockAsync(block, CurrentUserName);
            TempData[Constant.Success] = Constant.SuccessMessage;
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Edit), new { id = block.BLOG_POST_ID });
    }

    [Authorize]
    public async Task<IActionResult> EditBlock(int? id)
    {
        var block = await _blog.GetBlockAsync(id);
        if (block == null) return NotFound();
        ViewData["rootPath"] = _env.WebRootPath;
        return View(block);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBlock(int id, BLOG_POST_BLOCKS block)
    {
        if (id != block.AUTO_ID) return NotFound();
        try
        {
            var existing = await _blog.GetBlockAsync(id);
            block.CREATED_BY   = existing?.CREATED_BY;
            block.CREATED_DATE = existing?.CREATED_DATE ?? DateTime.Now;

            var file = Request.Form.Files.FirstOrDefault();
            var saved = await Utility.SaveUploadedAsync(file, _env, "Images", "Blog", "Blocks");
            if (saved != null)
            {
                Utility.DeleteIfExists(existing?.IMAGE_PATH);
                block.IMAGE_PATH = saved;
            }
            else
            {
                block.IMAGE_PATH = existing?.IMAGE_PATH;
            }

            await _blog.UpdateBlockAsync(block, CurrentUserName);
            TempData[Constant.Success] = Constant.SuccessMessage;
            return RedirectToAction(nameof(Edit), new { id = block.BLOG_POST_ID });
        }
        catch (Exception ex) { LogAndFlash(ex); }
        ViewData["rootPath"] = _env.WebRootPath;
        return View(block);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBlock(int id, int postId)
    {
        try
        {
            var existing = await _blog.GetBlockAsync(id);
            Utility.DeleteIfExists(existing?.IMAGE_PATH);
            await _blog.RemoveBlockAsync(id);
            TempData[Constant.Success] = Constant.SuccessRmvMsg;
        }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Edit), new { id = postId });
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveBlock(int id, int postId, int direction)
    {
        try { await _blog.MoveBlockAsync(id, direction); }
        catch (Exception ex) { LogAndFlash(ex); }
        return RedirectToAction(nameof(Edit), new { id = postId });
    }

    private void LogAndFlash(Exception ex)
    {
        TempData[Constant.Error] = Constant.ErrorMessage;
        Log.Error(ex, "{Controller}.{Action} by {User}",
            ControllerContext.ActionDescriptor.ControllerName,
            ControllerContext.ActionDescriptor.ActionName,
            CurrentUserName);
    }
}
