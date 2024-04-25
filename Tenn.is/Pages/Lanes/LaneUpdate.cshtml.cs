using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class LaneUpdateModel : PageModel
    {
        ILaneService Service;
        [BindProperty]
        public Lane LaneToUpdate { get; set; }
        [BindProperty]
        public int Id { get; set; }
        public void OnGet(ILaneService service, int id)
        {
            Service = service;
            LaneToUpdate = Service.GetLaneByNumber(id);
            Id = id;
        }

        public IActionResult OnPostUpdate()
        {
            Service.EditLane(LaneToUpdate, Id);
            return RedirectToPage("LaneIndex");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("LaneIndex");
        }

    }
}
