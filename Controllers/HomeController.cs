using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Task4.Areas.Identity.Data;
using Task4.IRepositoty;
using Task4.Models;
using System.Web;

namespace Task4.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(IUserRepository userRepository,
                              IHttpContextAccessor httpContext,
                              SignInManager<AppUser> signInManager)
        {
            _userRepository = userRepository;
            _httpContext = httpContext;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser[] users = await _userRepository.GetAllUsers();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Block(AppUser[] users)
        {
            AddTempData("block", _userRepository.BlockUsers(users));
            var userId = GetCurrentUserId();
            var user = users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user != null && user.CheckBox)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Unblock(AppUser[] users)
        {
            AddTempData("unblock", _userRepository.UnblockUsers(users));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AppUser[] users)
        {
            AddTempData("delet", _userRepository.DeleteMany(users));
            var userId = GetCurrentUserId();
            var user = users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user != null && user.CheckBox)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index");
        }

        private void AddTempData(string action, int count)
        {
            if (count > 0)
            {
                TempData["success"] =
                    count == 1
                        ? $"1 account was successfuly {action}ed!"
                        : $"{count} accounts were successfuly {action}ed!";
            }
            else
            {
                TempData["error"] = $"No account was {action}ed!";
            }
        }

        private string GetCurrentUserId()
        {
            return _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}