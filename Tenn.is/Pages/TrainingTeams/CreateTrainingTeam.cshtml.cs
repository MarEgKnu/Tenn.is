using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.TrainingTeams
{
    public class CreateTrainingTeamModel : PageModel
    {
        [BindProperty]
        public TrainingTeam Team { get; set; }
        public SelectList DayOptions { get; set; }

        private ITrainingTeamService _teamService;

        public CreateTrainingTeamModel(ITrainingTeamService teamService)
        {
            _teamService = teamService;
            Dictionary<DayOfWeek, string> dayOptionsDictionary = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "Mandag"},
                { DayOfWeek.Tuesday, "Tirsdag"},
                { DayOfWeek.Wednesday, "Onsdag"},
                { DayOfWeek.Thursday, "Torsdag"},
                { DayOfWeek.Friday, "Fredag"},
                { DayOfWeek.Saturday, "Lørdag"},
                { DayOfWeek.Sunday, "Søndag"}
            };
            DayOptions = new SelectList(dayOptionsDictionary, "Key", "Value");
            Team = new TrainingTeam(0,null,null,null,null,new WeeklyTimeBetween(new TimeOnly(6,0), new TimeOnly(7,0), DayOfWeek.Monday), 0);
        }
        public void OnGet()
        {
        }
    }
}
