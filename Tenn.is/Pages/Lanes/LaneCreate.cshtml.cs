using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class LaneCreateModel : PageModel
    {
        ILaneService Service;
        [BindProperty]
        public Lane NewLane{ get; set; }

        public LaneCreateModel(ILaneService service)
        {
            Service = service;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCreate() 
        {
            Service.CreateLane(NewLane);
            return RedirectToPage("LaneIndex");
        }
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("LaneIndex");
        }
    }
}
