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
        public void OnGet(ILaneService service, int Id)
        {
            Service = service;
            DelLane = Service.GetLaneByNumber(Id);
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
