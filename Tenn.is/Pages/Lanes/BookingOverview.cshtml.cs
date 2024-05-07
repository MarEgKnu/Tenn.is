using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using Tennis.Helpers;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingOverviewModel : PageModel
    {
        private ILaneService _laneService;
        private DateTime _displayedMonth;


        public List<DateTime> DatesOfTheMonth { get; set; }
        public int FirstOfTheWeek { get; set; }
        public List<LaneBooking> Bookings { get; set; }
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
        public BookingOverviewModel(ILaneService laneService)
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
            Dictionary<string, int> hourOptions = new Dictionary<string, int>()
            {
                {
                    "8:00",8
                },
                {
                    "9:00",9
                },
                {
                    "10:00",10
                },
                {
                    "11:00",11
                },
                {
                    "12:00",12
                },
                {
                    "13:00",13
                },
                {
                    "14:00",14
                },
                {
                    "15:00",15
                },
                {
                    "16:00",16
                },
                {
                    "17:00",17
                },
                {
                    "18:00",18
                },
                {
                    "19:00",19
                },
                {
                    "20:00",20
                },
                {
                    "21:00",21
                },
                {
                    "22:00",22
                }
            };
            FromOptions = new SelectList(hourOptions,"Value","Key");
            ToOptions = new SelectList(hourOptions,"Value","Key");
            StartFilter = 8;
            EndFilter = 22;
            TennisFilter = true;
            PadelFilter = true;
            InsideFilter = true;
            OutsideFilter = true;
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
            Bookings = new List<LaneBooking>();
            FilterBookings();
            DatesOfTheMonth = new List<DateTime>();
            for(int i = 1; i <= Calendar.GetDaysInMonth(_displayedMonth.Year, _displayedMonth.Month); i++)
            {
                DatesOfTheMonth.Add(new DateTime(_displayedMonth.Year, _displayedMonth.Month, i));
            }
            FirstOfTheWeek = (int)Calendar.GetDayOfWeek(new DateTime(_displayedMonth.Year, _displayedMonth.Month, 1));
        }

        public void FilterBookings()
        {
            List<Predicate<LaneBooking>> conditions = new List<Predicate<LaneBooking>>();
            conditions.Add(b => b.DateStart.Hour >= StartFilter && b.DateStart.Hour < EndFilter);
            if (!TennisFilter)
            {
                conditions.Add(b => Lanes.Where(l => l.Id == b.LaneNumber && l.PadelTennis).Count() < 0);
            }
            if (!PadelFilter)
            {
                conditions.Add(b => Lanes.Where(l => l.Id == b.LaneNumber && !l.PadelTennis).Count() < 0);
            }
            if (!InsideFilter)
            {
                conditions.Add(b => Lanes.Where(l => l.Id == b.LaneNumber && !l.OutDoor).Count() < 0);
            }
            if (!OutsideFilter)
            {
                conditions.Add(b => Lanes.Where(l => l.Id == b.LaneNumber && l.OutDoor).Count() < 0);
            }
            Bookings = FilterHelpers.GetItemsOnConditions(conditions, Bookings);
        }
    }
}
