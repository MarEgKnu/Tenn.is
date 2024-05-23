using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private ILaneBookingService _laneBookingService;
    public UserLaneBooking StuffToCancel {  get; set; }
    public List<UserLaneBooking> LaneBooking { get; set; }

    public IndexModel(ILogger<IndexModel> logger, ILaneBookingService laneBookingService)
    {
        _logger = logger;
        _laneBookingService = laneBookingService;
    }

    public void OnGet()
    {
        LaneBooking = _laneBookingService.GetAllLaneBookings<UserLaneBooking>();
        StuffToCancel = LaneBooking.Find(u => !u.Cancelled);
    }
}
