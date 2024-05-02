using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingOverviewModel : PageModel
    {

        public List<LaneBooking> Bookings { get; set; }
        public void OnGet()
        {
        }
    }
}
