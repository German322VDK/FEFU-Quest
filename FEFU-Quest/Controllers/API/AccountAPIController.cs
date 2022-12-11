using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.Infrastructure.Methods;
using FEFU_Quest.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FEFU_Quest.Controllers.API
{
    [Route("api/account")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IEmailConfirm _emailConfirm;
        private readonly ILogger<AccountAPIController> _logger;

        public AccountAPIController(IUser user, IEmailConfirm emailConfirm, ILogger<AccountAPIController> logger)
        {
            _user = user;
            _logger = logger;
            _emailConfirm = emailConfirm;
        }

        [HttpGet("givehash")]
        public async Task<bool> GiveHash(string email)
        {
            var mail = email ?? "";
            _logger.LogInformation($"Тип с почтой {mail} хочет получить хэшкод");

            var emails = _user.GetAll().Select(el => el.Email);

            if (email is null || emails.Contains(email) || !SendMailMethods.CheckEmail(email))
            {
                _logger.LogWarning($"Тип с почтой {email} не смог получить хэшкод");

                return false;
            }


            var randString = StringGenerationMethods.Generate(6, engLow: false, EngUp: true, numbers: true);

            var result = _emailConfirm.Set(email, randString);

            if (!result)
            {
                _logger.LogWarning($"Типу с почтой {email} не получилось создать хэшкод");

                return false;
            }

            _logger.LogInformation($"Типу с почтой {email} был создан хэшкод");

            _logger.LogInformation($"Типу с почтой {email} пытаемся отправить хэшкод на почту");

            var resultSending = await SendMailMethods.SendEmailAsync(Emails.MAIN_EMAIL, Emails.MAIN_NAME, Emails.MAIN_PASS, email, "Подтверждение почты",
                $"Код для регистрации <b>{randString}</b>. Никому кроме нас его не сообщайте)");

            if (!resultSending)
            {
                _logger.LogWarning($"Типу с почтой {email} не был отправлен хэшкод на почту, произошёл краш");

                return false;
            }

            _logger.LogInformation($"Типу с почтой {email} был отправлен хэшкод на почту");

            return true;
        }

        [HttpGet("confirm")]
        public bool Confirm(string email, string hash)
        {
            _logger.LogInformation($"Тип с почтой {email} хочет подтвердить хэшкод");

            var ec = _emailConfirm.Get(email);

            if (ec is null)
            {
                _logger.LogWarning($"Для типа с почтой {email} нет хэшкода, видимо его почта не зарегестрирована в системе");

                return false;
            }


            if (ec.Hash != hash)
            {
                _logger.LogWarning($"Тип с почтой {email} ввёл не правильный хэшкод");

                return false;
            }

            _logger.LogInformation($"Тип с почтой {email} ввёл правильный хэшкод");

            return true;
        }
    }
}
