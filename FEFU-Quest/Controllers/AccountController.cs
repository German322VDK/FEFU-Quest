using FEFU_Quest.Domain.Identity;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.Infrastructure.Methods;
using FEFU_Quest.Models.Static;
using FEFU_Quest.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FEFU_Quest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserDTO> _userManager;
        private readonly SignInManager<UserDTO> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUser _user;
        private readonly IEmailConfirm _emailConfirm;
        private readonly IUniverGroup _univerGroup;

        public AccountController(UserManager<UserDTO> userManager, SignInManager<UserDTO> signInManager,
            ILogger<AccountController> logger, IUser user, IEmailConfirm emailConfirm, IUniverGroup univerGroup)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _user = user;
            _emailConfirm = emailConfirm;
            _univerGroup = univerGroup;
        }

        ////меняем пароль
        //_userManager.ChangePasswordAsync(user, "", "");
        ////сбрасываем пароль, генерация - await _userManager.GeneratePasswordResetTokenAsync(user) - токен
        //await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), Model.Password);


        #region Register

        [AllowAnonymous]
        public IActionResult RegisterStart()
        {
            _logger.LogInformation("Кто-то пытается зарегестрироваться!");
            
            return View(new RegisterStartViewModel
            {

            });
        }

        [AllowAnonymous]
        public IActionResult Register(RegisterStartViewModel model)
        {
            _logger.LogInformation($"Тип с почтой {model.Email} пытается зарегестрироваться!");

            return View(new RegisterUserViewModel
            {
                Email = model.Email
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (Model is null || string.IsNullOrEmpty(Model.Email) || _emailConfirm.Get(Model.Email) is null)
            {
                _logger.LogWarning("Какой то конченный хотел меня обмануть");

                return RedirectToAction("RegisterStart", "Account");
            }

            var userNameIsExist = CheckUserName(Model.UserName);

            var emailIsExist = CheckEmailName(Model.Email);

            if (userNameIsExist)
            {
                _logger.LogWarning($"Тип с почтой {Model.Email} пытается забрать существующий {nameof(Model.UserName)}: {Model.UserName}");

                ModelState["UserName"].Errors.Add(new Exception("Логин обязателен и не должен использоваться другими"));
                ModelState["UserName"].ValidationState = ModelValidationState.Invalid;
            }

            if (emailIsExist)
            {
                _logger.LogWarning($"Тип с ником {Model.UserName} пытается забрать существующую почту {Model.Email}");

                return RedirectToAction("RegisterStart", "Account");
            }


            if (!ModelState.IsValid)
                return View(Model);

            _logger.LogInformation("Регистрация пользователя {0}", Model.UserName);

            using (_logger.BeginScope($"Регистрация пользователя {Model.UserName}"))
            {
                var arr = ImageMethods.GetByteArrFromFile("wwwroot/img/anon.jpg");

                var group = _univerGroup.GetName(Model.UniverGroup);

                if (group is null)
                    _univerGroup.Add(Model.UniverGroup);

                group = _univerGroup.GetName(Model.UniverGroup);

                var user = new UserDTO
                {
                    UserName = Model.UserName,
                    FirstName = Model.FirstName,
                    SecondName = Model.SecondName,
                    Patronymic = Model.Patr,
                    Status = "",
                    Email = Model.Email,
                    Photo = arr,
                    Dormitory = Model.Dormitory,
                    UniverGroup = group,
                };
                
                var registration_result = await _userManager.CreateAsync(user, Model.Password);

                if (registration_result.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} успешно зарегестрирован", Model.UserName);

                    await _userManager.AddToRoleAsync(user, UserStatus.User.ToString());

                    _logger.LogInformation("Пользователь {0} наделён ролью {1}", Model.UserName, UserStatus.User);

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }

                _logger.LogWarning("В процессе регистрации пользователя {0} возникли ошибки :( {1}",
                    Model.UserName, string.Join(",", registration_result.Errors.Select(e => e.Description)));

                foreach (var errors in registration_result.Errors)
                {
                    ModelState.AddModelError("", errors.Description);
                }
            }

            return View(Model);
        }

        public bool CheckUserName(string username) =>
            _user.Get(username) is not null ? true : false;

        public bool CheckEmailName(string email) =>
            _user.GetAll().FirstOrDefault(us => us.Email == email) is not null ? true : false;


        #endregion

        #region Login

        [AllowAnonymous]
        public IActionResult Log() =>
            View();

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl) =>
            View(new LoginViewModel { ReturnUrl = ReturnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var user = _user.GetByEmail(Model.Email);

            if(user is null)
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль");

                return View(Model);
            }

            _logger.LogInformation("Вход пользователя {0}", user.UserName);

            var login_result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                Model.Password,
                true,
#if DEBUG
                false
#else
                true
#endif
                );

            if (login_result.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} успешно зашёл", user.UserName);

                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            _logger.LogWarning("В процессе входа пользователя {0} возникли ошибки :( ",
                user.UserName);

            ModelState.AddModelError("", "Неверное имя пользователя или пароль");

            return View(Model);
        }

        #endregion

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RestorePasswordStart() =>
            View(new RestorePasswordStartViewModel { });

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestorePasswordStart(RestorePasswordStartViewModel model)
        {
            if(model is null || string.IsNullOrEmpty(model.Email))
                return RedirectToAction("RestorePasswordStart", "Account");

            var curUser = _user.GetByEmail(model.Email);

            if(curUser is null)
                return RedirectToAction("RestorePasswordStart", "Account");

            string token = await _userManager.GeneratePasswordResetTokenAsync(curUser);

            string callbackUrl = Url.Action("RestorePassword", "Account", new { email = model.Email, code = token }, protocol: HttpContext.Request.Scheme);

            var resultSending = await SendMailMethods.SendEmailAsync(Emails.MAIN_EMAIL, Emails.MAIN_NAME, Emails.MAIN_PASS, model.Email, "Востановление пароля",
            $"<p>Для восстановления <b>пароля</b> перейдите по ссылке: <a href='{callbackUrl}'>Ссылка</a>.</p>");

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RestorePassword(string email, string code = null)
        {
            return code == null ? RedirectToAction("RestorePasswordStart", "Account") : View(new RestorePasswordViewModel { GeneratedToken = code, Email = email });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestorePassword(RestorePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var curUser = _user.GetByEmail(model.Email);

            var result = await _userManager.ResetPasswordAsync(curUser, model.GeneratedToken, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return View(model);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation("Пользователь вышел");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            var userName = User.Identity.Name;

            if (userName is null || _user.Get(userName) is null)
            {
                _logger.LogWarning("Опять эти куки пытаются не существующего пользователя куда-то отправить");
            }

            _logger.LogWarning($"Тип {userName} хотел попасть на {ReturnUrl}");

            return View();
        }
    }
}
