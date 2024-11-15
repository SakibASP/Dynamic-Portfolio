using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Web.Common;
using Portfolio.Utils;
using Portfolio.Interfaces;
using Serilog;
using Microsoft.Data.SqlClient;
using System.Data;
using X.PagedList;

namespace Portfolio.Web.Controllers
{
    public class MY_PROFILEController(IWebHostEnvironment webHostEnvironment, IProfileRepo profile,SendEmail sendEmail) : BaseController
    {
        private readonly IProfileRepo _profile = profile;
        private readonly SendEmail _sendEmail = sendEmail;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: MY_PROFILE
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return View(await _profile.GetProflie());
        }

        public async Task<IActionResult> About()
        {
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            ViewData["SKILLS"] = await _profile.GetSkills();
            ViewData["PROFILES"] = await _profile.GetSingleProflie();
            return View();
        }

        public async Task<IActionResult> Resume()
        {
            ViewData["PROFILES"] = await _profile.GetSingleProflie();
            ViewData["EDUCATIONS"] = await _profile.GetEducations();
            ViewData["EXPERIENCEs"] = await _profile.GetExperiences();
            ViewData["DESCRIPTIONs"] = await _profile.GetDescriptions();
            return View();
        }

        public async Task<IActionResult> Projects()
        {
            ViewData["PROFILES"] = await _profile.GetSingleProflie();
            ViewData["PROJECTS"] = await _profile.GetProjects();
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            ViewData["PROFILES"] = await _profile.GetSingleProflie();
            return View();
        }

        public async Task<IActionResult> Visitors(string searchString, int? page, DateTime? startDate, DateTime? endDate)
        {
            ViewBag.SearchString = searchString;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd"); //"yyyy-MM-dd"
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
            var IsAuthenticated = User.Identity?.IsAuthenticated;
            ViewData["IsAuthenticated"] = IsAuthenticated ?? false;
            int pageSize = 10;
            int pageNumber = page ?? 1;
            SqlParameter param1 = new("@PageNumber", SqlDbType.Int)
            {
                Value = (object)pageNumber ?? DBNull.Value
            };
            SqlParameter param2 = new("@PageSize", SqlDbType.Int)
            {
                Value = (object)pageSize ?? DBNull.Value
            };
            SqlParameter param3 = new("@StartDate", SqlDbType.DateTime)
            {
                Value = (object)startDate! ?? DBNull.Value
            };
            SqlParameter param4 = new("@EndDate", SqlDbType.DateTime)
            {
                Value = (object)endDate! ?? DBNull.Value
            };
            SqlParameter param5 = new("@SearchString", SqlDbType.NVarChar)
            {
                Value = (object)searchString! ?? DBNull.Value
            };

            var @params = new[] { param1, param2, param3, param4, param5 };
            var visitors = await _profile.GetVisitors(@params) ?? [];
            var totalRows = visitors.FirstOrDefault()?.TotalRows ?? 0;
            ViewData["TotalRecords"] = totalRows;
            return View(await visitors.ToPagedListAsync(pageNumber, pageSize, totalRows));
        }

        [HttpPost]
        public async Task<JsonResult> Contact(CONTACTS objContact)
        {
            try
            {
                //sending email if it is not null
                if (!string.IsNullOrEmpty(objContact.EMAIL))
                {
                    try
                    {
                        const string subject = "Thank You for Reaching Out!";
                        string htmlMessage = $@"<h5>Hello {objContact.NAME},</h5>
                            <p>Thank you for reaching out! I’ve received your message and will get back to you as soon as possible.</p>
                            <p>Warm regards,<br><strong>Md. Sakibur Rahman</strong></p>";
                        await _sendEmail.SendEmailAsync(objContact.EMAIL, subject, htmlMessage);
                    }
                    catch(Exception ex) 
                    {
                        Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... ");
                        return Json(data: new { message = "Not a valid email", status = false });
                    }
                }

                var saveParameter = GenerateParameter.SingleModel(objContact, string.Empty, BdCurrentTime);
                await _profile.AddContactInfoAsync(saveParameter);
                return Json(data: new { message = "Message Sent Successfully", status = true });
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"I am from {ControllerContext.ActionDescriptor.ControllerName} {ControllerContext.ActionDescriptor.MethodInfo.Name}... ");
                return Json(data: new { message = "Something went wrong", status = false });
            }
        }

        [Authorize]
        //GET: MY_PROFILE/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MY_PROFILE mY_PROFILE)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var files = Request.Form.Files.FirstOrDefault();
                    if (files != null)
                    {
                        const string rootFolder = @"Images\About";
                        string? directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, rootFolder);
                        // Check if the directory exists; if not, create it
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        //Time in seconds
                        string formattedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var fileName = formattedDateTime + "_" + files.FileName;
                        var uploadPath = Path.Combine(directoryPath, fileName);

                        //saving the file
                        await Utility.SaveFileAsync(uploadPath, files);
                        mY_PROFILE.PROFILE_IMAGE = uploadPath;
                    }

                    var saveRequest = GenerateParameter.SingleModel(mY_PROFILE, string.Empty, BdCurrentTime);
                    await _profile.AddProfileAsync(saveRequest);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                //DO SOMTHING
            }
            return View(mY_PROFILE);
        }

        // GET: MY_PROFILE/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var mY_PROFILE = await _profile.GetSingleProflieById(id);
            if (mY_PROFILE == null)
            {
                return NotFound();
            }
            ViewData["rootPath"] = _webHostEnvironment.WebRootPath;
            return View(mY_PROFILE);
        }

        // POST: MY_PROFILE/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,MY_PROFILE mY_PROFILE)
        {
            if (id != mY_PROFILE.AUTO_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = Request.Form.Files.FirstOrDefault();
                    if (files is not null)
                    {
                        const string rootFolder = @"Images\About";
                        string? directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, rootFolder);
                        // Check if the directory exists; if not, create it
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        //Time in seconds
                        string formattedDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var fileName = formattedDateTime + "_" + files.FileName;
                        var uploadPath = Path.Combine(directoryPath, fileName);
                        //delete old picture
                        if (System.IO.File.Exists(mY_PROFILE?.PROFILE_IMAGE))
                            System.IO.File.Delete(mY_PROFILE.PROFILE_IMAGE);
                        //saving the file
                        await Utility.SaveFileAsync(uploadPath, files);
                        mY_PROFILE!.PROFILE_IMAGE = uploadPath;
                    }

                    var saveRequest = GenerateParameter.SingleModel(mY_PROFILE, string.Empty, BdCurrentTime);
                    await _profile.UpdateProfileAsync(saveRequest);
                }
                catch
                {
                    // Something went wrong
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mY_PROFILE);
        }
    }
}
