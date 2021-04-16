using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WorkWithDb.Areas.Identity.Data;
using WorkWithDb.Models;

namespace WorkWithDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<WorkWithDbUser> _userManager;
        private readonly SignInManager<WorkWithDbUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<WorkWithDbUser> userManager, SignInManager<WorkWithDbUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public async Task<ActionResult> Block(string[] state)
        {

            foreach (var id in state)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.Status = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.SetLockoutEndDateAsync(user, new DateTime(9999, 12, 30));
                    WorkWithDbUser iuser = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (iuser == user)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("index");
                    }
                }
                await _signInManager.RefreshSignInAsync(user);
            }
            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<ActionResult> Unblock(string[] state)
        {
            foreach (var id in state)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.Status = false;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                }
                await _signInManager.RefreshSignInAsync(user);
            }
            return RedirectToAction("index");
        }
        [Authorize]
        public async Task<ActionResult> Delete(string[] state)
        {
            foreach (var id in state)
            {
                var user = await _userManager.FindByIdAsync(id);
                WorkWithDbUser iuser = await _userManager.FindByNameAsync(User.Identity.Name);

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    if (iuser == user)
                    {
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("index");
                    }
                }
            }
            return RedirectToAction("index");
        }
    }
}
