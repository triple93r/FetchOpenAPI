using GetAPIData;
using GetAPIData.Entities;
using GetAPIData.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GetAPIData.Controllers
{
    public class SaveApiController : Controller
    {
        private readonly ILogger<SaveApiController> _logger;
        private readonly ApplicationDbContext _context;
        

        public SaveApiController(ILogger<SaveApiController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSaveApi()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";                
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveApiData(string geturl)
        {
            try
            {
                if (string.IsNullOrEmpty(geturl))
                {
                    ViewBag.ErrorMessage = "URL is required";
                    return View("GetSaveApi");
                }

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(geturl);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        dynamic logoutList = JsonConvert.DeserializeObject(data);

                        List<UserComments> actionmaster4 = JsonConvert.DeserializeObject<List<UserComments>>(logoutList.ToString());

                        UserComments mh = new UserComments();
                        foreach (UserComments log in actionmaster4)
                        {
                            int w = await _context.Database.ExecuteSqlRawAsync("insert into dbo.UserComments(postId, id, name, email, body) values({0},{1},{2},{3},{4})", log.postId, log.id, log.name, log.email, log.body);
                        }
                        //var g = await _context.UserComments.FromSqlRaw("select * from UserComments").ToListAsync();

                        return View("GetSaveApi");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Failed to fetch data from URL. Status code: {response.StatusCode}";
                        return View("GetSaveApi");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> GetSaveApiDispl(string geturl)
        {
            try
            {
                var g = await _context.UserComments.FromSqlRaw("select * from UserComments").ToListAsync();
                return View(g);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetEditApi(int id)
        {
            var c = _context.UserComments.Where(x => x.slno == id).FirstOrDefault();
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> GetEditApi(UserComments ucom)
        {
            var ucm = _context.UserComments.Where(x => x.slno == ucom.slno).FirstOrDefault();
            ucm.id = ucom.id;
            ucm.name = ucom.name;
            ucm.body = ucom.body;
            ucm.email = ucom.email;
            ucm.postId = ucom.postId;
            await _context.SaveChangesAsync();
            return RedirectToAction("GetSaveApiDispl");
        }

        public IActionResult GetEditApi2(int id)
        {
            return View();
        }

    }
}
