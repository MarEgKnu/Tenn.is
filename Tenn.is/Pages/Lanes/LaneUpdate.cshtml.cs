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

        public LaneUpdateModel(ILaneService service)
        {
            Service = service;
        }

        public IActionResult OnGet(int id)
        {            
            LaneToUpdate = Service.GetLaneByNumber(id);
            Id = id;
            return Page();
        }

        public IActionResult OnPostUpdate(int id)
        {
            Service.EditLane(LaneToUpdate, id);
            return RedirectToPage("LaneIndex");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("LaneIndex");
        }

    }
}
