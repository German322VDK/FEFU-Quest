using FEFU_Quest.Domain.Quest;
using FEFU_Quest.Infrastructure.Interfaces;
using FEFU_Quest.Models;
using FEFU_Quest.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FEFU_Quest.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IFefuQuest _fefuQuest;

        public EventsController(IFefuQuest fefuQuest)
        {
            _fefuQuest = fefuQuest;
        }

        public IActionResult Index()
        {
            var lol = DateTime.Now.DayOfWeek;

            return View();
        }

        public IActionResult Dormitory()
        {
            return View();
        }

        public IActionResult Fefu(FefuViewModel model)
        {
            if(model == null || string.IsNullOrEmpty(model.Week))
            {
                var lol = (int)DateTime.Now.DayOfWeek;

                Calendar calendar = new CultureInfo("ru-RU").Calendar;

                calendar.GetDayOfWeek(DateTime.Now);

                var mon = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

                var sun = DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek).AddHours(23).AddMinutes(59);

                var data = new FefuViewModel
                {
                    Week = $"{mon.Day}-{sun.Day}.{mon.Month}.{mon.Year}/{(int)DateTime.Today.DayOfWeek}",
                };

                var quests = _fefuQuest.Get(mon, sun);

                for (int i = 0; i < data.Quests.Length; i++)
                {
                    data.Quests[i] = new List<FefuQuestModel>();
                }

                foreach (var quest in quests)
                {
                    int dayOfWeek = (int)quest.DateTimeStart.DayOfWeek;
                    data.Quests[dayOfWeek - 1].Add((FefuQuestModel)quest);
                }

                return View(data);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult FefuQuest(int id)
        {
            var quest = (FefuQuestModel)_fefuQuest.GetById(id);

            return View(quest);
        }


        public IActionResult FefuQuestAdd(FefuQuest model)
        {
             
            if (model is null || string.IsNullOrEmpty(model.Name))
                return View(model);

            _fefuQuest.Add(model);

            return RedirectToAction("Fefu", "Events");
        }
    }
}
