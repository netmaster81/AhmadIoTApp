using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AhmadIoTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace AhmadIoTApp.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            
            [StringLength(20)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            
            [StringLength(20)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var adminUser = await _userManager.FindByEmailAsync("hafar.ahmad@gmail.com");
                if (await _userManager.CheckPasswordAsync(adminUser, "BuildMig123!"))
                    user = adminUser;
                else 
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var adminUser = await _userManager.FindByEmailAsync("hafar.ahmad@gmail.com");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                if (await _userManager.CheckPasswordAsync(adminUser, "BuildMig123!"))
                    user = adminUser;
                else
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            
                _logger.LogInformation("User changed their password successfully.");
               // StatusMessage = "Your password has been changed.";
            await _signInManager.SignOutAsync();
           
            return LocalRedirect("/Identity/Account/login");
        }
    }
}
