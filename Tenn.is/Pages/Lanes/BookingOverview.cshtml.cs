using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Reflection;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingOverviewModel : PageModel
    {
        private ILaneService _laneService;
        private ILaneBookingService _laneBookingService;
        private DateTime _displayedMonth;


        public List<DateTime> DatesOfTheMonth { get; set; }
        public int FirstOfTheWeek { get; set; }
        public List<LaneBooking> Bookings { get; set; }
        public List<LaneBooking> UnfilteredBookings { get; set; }
        public List<Lane> Lanes { get; set; }
        public Calendar Calendar { get; set; }

        public SelectList SelectOptions { get; set; }

        public SelectList FromOptions { get; set; }

        public SelectList ToOptions { get; set; }
        [BindProperty(SupportsGet = true)]
        public int StartFilter {  get; set; }
        [BindProperty(SupportsGet = true)]
        public int EndFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool TennisFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool PadelFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool InsideFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool OutsideFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedMonth { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DateString { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SelectedDay { get {
                bool succeeded = DateTime.TryParse(DateString, out DateTime result);
                if (succeeded)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            } }

        public DateTime CurrentTime { get; set; }

        public string BookingError { get; set; }
        public BookingOverviewModel(ILaneService laneService, ILaneBookingService laneBookingService)
        {
            _laneService = laneService;
            Calendar = new GregorianCalendar();
            Dictionary<string, int> options = new Dictionary<string, int>()
            {
                {
                    "Januar", 1
                },
                                {
                    "Febuar", 2
                },
                                                {
                    "Marts", 3
                },
                                                                {
                    "April", 4
                },
                                                                                {
                    "Maj", 5
                },
                                                                                                {
                    "Juni", 6
                },
                                                                                                                {
                    "Juli", 7
                },
                                                                                                                                {
                    "August", 8
                },
                                                                                                                                                {
                    "September", 9
                },
                                                                                                                                                                {
                    "Oktober", 10
                },
                                                                                                                                                                                {
                    "November", 11
                },
                                                                                                                                                                                                {
                    "December", 12
                },
            };
            SelectOptions = new SelectList(options, "Value", "Key");
            Dictionary<string, int> hourOptions = new Dictionary<string, int>();
            for (int i = 8; i <= 22; i++)
            {
                hourOptions.Add($"{i}:00",i);
            }
            FromOptions = new SelectList(hourOptions, "Value", "Key");
            ToOptions = new SelectList(hourOptions, "Value", "Key");
            StartFilter = 8;
            EndFilter = 22;
            //TennisFilter = true;
            //PadelFilter = true;
            //InsideFilter = true;
            //OutsideFilter = true;
            _laneBookingService = laneBookingService;
        }
        public void OnGet()
        {
            Lanes = _laneService.GetAllLanes();
            if (SelectedMonth == 0)
            {
                _displayedMonth = DateTime.Now;
                SelectedMonth = _displayedMonth.Month;
            }
            else
            {
                _displayedMonth = new DateTime(DateTime.Now.Year, SelectedMonth, 1);
            }
            Bookings = _laneBookingService.GetAllLaneBookings<LaneBooking>().Where(b => !b.Cancelled).ToList();
            UnfilteredBookings = _laneBookingService.GetAllLaneBookings<LaneBooking>().Where(b => !b.Cancelled).ToList();
            FilterBookings();
            DatesOfTheMonth = new List<DateTime>();
            for(int i = 1; i <= Calendar.GetDaysInMonth(_displayedMonth.Year, _displayedMonth.Month); i++)
            {
                DatesOfTheMonth.Add(new DateTime(_displayedMonth.Year, _displayedMonth.Month, i));
            }
            FirstOfTheWeek = (int)Calendar.GetDayOfWeek(new DateTime(_displayedMonth.Year, _displayedMonth.Month, 1));

            CurrentTime = DateTime.Now;
        }

        public void OnGetFirstClick()
        {
            TennisFilter = true;
            PadelFilter = true;
            InsideFilter = true;
            OutsideFilter = true;
            OnGet();
        }

        public void OnGetBookingFailed()
        {
            BookingError = "Noget gik galt ved booking. Kontakt venligst support hvis dette problem opstår igen.";
            OnGetFirstClick();
        }

        public void FilterBookings()
        {
            Bookings = Bookings.Where(b => b.DateStart.Hour >= StartFilter && b.DateStart.Hour < EndFilter).ToList();
        }

        public bool CheckFilters(Lane currentLane)
        {
            return ((currentLane.PadelTennis && PadelFilter) || (!currentLane.PadelTennis && TennisFilter)) && ((currentLane.OutDoor && OutsideFilter) || (!currentLane.OutDoor && InsideFilter));
        }
    }
}
