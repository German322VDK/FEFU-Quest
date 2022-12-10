using FEFU_Quest.Domain.Identity;
using FEFU_Quest.Infrastructure.Methods;
using FEFU_Quest.SQlite.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FEFU_Quest.Data
{
    public class FEFU_QuestDbInitializer
    {
        private readonly FEFU_QuestDBSQlite _db;
        private readonly ILogger<FEFU_QuestDbInitializer> _logger;
        private readonly UserManager<UserDTO> _userManager;
        private readonly RoleManager<RoleDTO> _roleManager;

        private const string adminName =  "Admin";

        public FEFU_QuestDbInitializer(FEFU_QuestDBSQlite db, ILogger<FEFU_QuestDbInitializer> logger,
            UserManager<UserDTO> userManager, RoleManager<RoleDTO> roleManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();

            using (_logger.BeginScope("Инициализация бд"))
            {
                _logger.LogInformation("Инициализация базы данных...");
            }

            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Выполнение миграций...");

                db.Migrate();

                _logger.LogInformation("Выполнение миграций выполнено успешно");
            }
            else
            {
                _logger.LogInformation("База данных находится в актуальной версии ({0:0.0###} c)",
                    timer.Elapsed.TotalSeconds);
            }

            try
            {
                InitialIdentitiesAsync().Wait();

                //InitialGropsAsync().Wait();

                //InitialChatsAsync().Wait();

                //InitialUserGroupFix();
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Ошибка при выполнении инициализации БД :(");
                throw;
            }

            _logger.LogInformation("Инициализация БД выполнена успешно {0}",
                timer.Elapsed.TotalSeconds);
        }

        private async Task InitialIdentitiesAsync()
        {
            var timer = Stopwatch.StartNew();

            _logger.LogInformation("Инициализация системы Identity...");

            await CheckRole(UserStatus.Admin.ToString());
            await CheckRole(UserStatus.User.ToString());
            await CheckRole(UserStatus.Banned.ToString());

            if (await _userManager.FindByNameAsync(adminName) is null)
            {
                _logger.LogInformation("Отсутствует учётная запись Админа");

                var admin = new UserDTO
                {
                    FirstName = "Создатель",
                    SecondName = "Сайта",
                    UserName = adminName,
                    Patronymic = "",
                    Photo = ImageMethods.GetByteArrFromFile("wwwroot/img/pepegod.jpg"),
                    Status = "Обидно что frontent на js, когда мог быть на c#",
                    Dormitory = "8.1",
                    Email = "germean322@gmail.com",
                    UniverGroup = new UniverGroup
                    {
                        GroupName = "М9122-01.04.02пи"
                    }
                };

                var creation_result = await _userManager.CreateAsync(admin, "FEFU_Quest_Admin");

                if (creation_result.Succeeded)
                {
                    _logger.LogInformation("Учётная запись Бога создана успешно.");

                    await _userManager.AddToRoleAsync(admin, UserStatus.Admin.ToString());

                    _logger.LogInformation($"Учётная запись Бога наделена ролью {adminName}");
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);

                    throw new InvalidOperationException($"Ошибка при создании учётной записи " +
                        $"Бога:( ({string.Join(",", errors)})");
                }
            }

            _logger.LogInformation($"Инициализация системы Identity завершена успешно за " +
                $"{timer.Elapsed.Seconds:0.0##}с");
        }

        private async Task CheckRole(string RoleName)
        {
            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                _logger.LogInformation($"Роль {RoleName} отсуствует. Создаю...");

                await _roleManager.CreateAsync(new RoleDTO
                {
                    Name = RoleName,
                    Description = GetDescription(RoleName)
                });

                _logger.LogInformation($"Роль {RoleName} создана успешно");
            }
        }

        private string GetDescription(string role)
        {
            string desc;

            switch (role)
            {
                case "Admin":
                    desc = "Может почти всё";
                    break;
                case "User":
                    desc = "Может что все";
                    break;
                default:
                    desc = "Неизвестно";
                    break;
            }

            return desc;
        }
    }
}
