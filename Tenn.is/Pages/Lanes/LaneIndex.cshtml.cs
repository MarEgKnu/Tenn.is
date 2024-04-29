using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class LaneIndexModel : PageModel
    {
        ILaneService Service;
        public List<Lane> LaneList { get; set; }

        public LaneIndexModel(ILaneService service)
        {
            Service = service;
        }

        public void OnGet()
        {
            LaneList = Service.GetAllLanes();
        }


    }
}
