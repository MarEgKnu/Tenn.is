using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class LaneDeleteModel : PageModel
    {
        ILaneService Service;
        [BindProperty]
        public Lane DelLane { get; set; }

        public LaneDeleteModel(ILaneService service)
        {
            Service = service;
        }

        public IActionResult OnGet(int Id)
        {
            DelLane = Service.GetLaneByNumber(Id);
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            Service.DeleteLane(DelLane.Id);
            return RedirectToPage("LaneIndex");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("LaneIndex");
        }
    }
}
