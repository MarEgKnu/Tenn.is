using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingOverviewModel : PageModel
    {
        private DateTime _now;
        public List<DateTime> DatesOfTheMonth { get; set; }
        public int FirstOfTheWeek { get; set; }
        public List<LaneBooking> Bookings { get; set; }
        public Calendar Calendar { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? chosendate { get; set; }
        public BookingOverviewModel()
        {
            Calendar = new GregorianCalendar();
        }
        public void OnGet(int chosendate)
        {
            if (chosendate == null)
            {
                this.chosendate = null;
            }
            else
            {
                this.chosendate = new DateTime((int)chosendate);
            }

            _now = DateTime.Now;
            Bookings = new List<LaneBooking>();
            DatesOfTheMonth = new List<DateTime>();
            for(int i = 1; i <= Calendar.GetDaysInMonth(_now.Year, _now.Month); i++)
            {
                DatesOfTheMonth.Add(new DateTime(_now.Year, _now.Month, i));
            }
            FirstOfTheWeek = (int)Calendar.GetDayOfWeek(new DateTime(_now.Year, _now.Month, 1));
        }
    }
}
