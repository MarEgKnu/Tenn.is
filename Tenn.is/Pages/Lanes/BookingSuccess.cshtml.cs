using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingSuccessModel : PageModel
    {

        public string BookingDate { get; set; }
        public string Partner { get; set; }
        public string Lane { get; set; }

        public void OnGet(string lane, string mate, string date)
        {
            BookingDate = date;
            Partner = mate;
            Lane = lane;
        }
    }
}
