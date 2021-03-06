﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<IdentityUser> _singInManager;
		private readonly UserManager<IdentityUser> _userManager;

		public AuthController(
				SignInManager<IdentityUser> signInManager,
				UserManager<IdentityUser> userManager
			)
		{
			_singInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel vm)
		{
			// check model is valid 
			if (!ModelState.IsValid) 
			{
				return View(vm);
			}
			var result = await _singInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

			if (result.Succeeded)
			{
				return Redirect(vm.ReturnUrl);
			}
			else if (result.IsLockedOut)
			{

			}

			return View(new LoginViewModel { Username = vm.Username, ReturnUrl = vm.ReturnUrl});
		}

		[HttpGet]
		public IActionResult Register(string returnUrl)
		{
			return View(new RegisterViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel vm)
		{
			// check model is valid 
			if (!ModelState.IsValid)
			{
				return View(vm);
			}

			var user = new IdentityUser(vm.Username);
			var result = await _userManager.CreateAsync(user, vm.Password);

			if (result.Succeeded)
			{
				await _singInManager.SignInAsync(user, false);

				return Redirect(vm.ReturnUrl);
			}


			return View(new RegisterViewModel { Username = vm.Username, ReturnUrl = vm.ReturnUrl });
		}
	}
}