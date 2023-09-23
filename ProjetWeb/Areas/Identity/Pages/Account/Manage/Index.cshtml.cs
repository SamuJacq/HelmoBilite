// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjetWeb.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ProjetWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// [Phone]
            [Display(Name = "Nom")]
            public string Name { get; set; }

            [Display(Name = "Prénom")]
            public string Prenom { get; set; }

            [Display(Name = "Matricule")]
            public string Matricule { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "date d'anniversaire")]
            public DateTime Birthday { get; set; }
            
            [Display(Name = "Photo")]
            public IFormFile Photo { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            if (User.IsInRole("Customer"))
            {
                Input = new InputModel
                {
                    Name = user.Name,
                    Email = user.Email
                };
            }else {
                var name = user.Name.Split(' ');
                Input = new InputModel
                {
                    Name = name[0],
                    Prenom = name[1],
                    Matricule = user.Matricule,
                    Email = user.Email,
                    Birthday = (DateTime)((user.Birthday == null) ? DateTime.Today : user.Birthday)
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            string extension = Input.Photo.FileName.Substring(Input.Photo.FileName.Length - 3, 3);
            if (!extension.Equals("jpg") || !extension.Equals("png") || !extension.Equals("gif")) {
                return NotFound($"extension de l'image incorrect, il doit être en jpg, png ou gif.");
            }
            using (var fileStream = new FileStream(Path.Combine("wwwroot/img/photo/", Input.Photo.FileName), FileMode.Create))
            {
                await Input.Photo.CopyToAsync(fileStream);
            }
            user.Name = Input.Name + ' ' + Input.Prenom;
            user.Matricule = Input.Matricule;
            user.Email = Input.Email;
            user.Birthday = Input.Birthday;
            user.Photo = "wwwroot/img/photo/" + Input.Photo.FileName;

            try
            {
                await _userManager.SetUserNameAsync(user, Input.Email);
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "modification enregistrer";
            return RedirectToPage();
        }
    }
}
