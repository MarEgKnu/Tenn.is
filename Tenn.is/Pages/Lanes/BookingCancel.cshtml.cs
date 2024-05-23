using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Data.SqlClient;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages.Lanes
{
    public class BookingCancelModel : PageModel
    {
        private ILaneBookingService _bookingService;
        private IUserService _userService;
        public User CurrentUser { get; set; }
        public User Booker {  get; set; }
        public User Mate { get; set; }
        public UserLaneBooking CurrentBooking { get; set; }

        public TrainingTeam TrainingTeam { get; set; }

        bool IsTraining { get; set; }

        public TrainingLaneBooking CurrentBookingTraining { get; set; }
        public string Message { get; set; }

        public BookingCancelModel(ILaneBookingService bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }
        public IActionResult OnGet(int bookingid)
        {
            
            try
            {
                
                CurrentUser = _userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password"));
                CurrentBooking = _bookingService.GetUserLaneBookingById(bookingid);
                if (CurrentBooking == null)
                {
                    IsTraining = true;
                    CurrentBookingTraining = _bookingService.GetTrainingLaneBookingById(bookingid);
                    if (CurrentBookingTraining == null)
                    {
                        ViewData["ErrorMessage"] = "Fejl, bookingen eksistere ikke";
                        return Page();
                    }
                    TrainingTeam = CurrentBookingTraining.trainingTeam;
                    if (!CurrentUser.Administrator && !TrainingTeam.IsTrainer(CurrentUser))
                    {
                        return RedirectToPage("/Users/Login", "Redirect", new { message = "Du skal være logget ind som administrator eller træner for at kunne aflyse denne booking" });
                    }                 
                   
                }
                else
                {
                    
                    IsTraining = false;                   
                    if (!(CurrentUser.Administrator) && (CurrentBooking.UserID != CurrentUser.UserId && CurrentBooking.MateID != CurrentUser.UserId))
                    {
                        return RedirectToPage("/Users/Login", "Redirect", new { message = "Du kan ikke aflyse bookinger bookinger du ikke selv er en del af" });
                    }
                    Booker = _userService.GetUserById(CurrentBooking.UserID);
                    Mate = _userService.GetUserById(CurrentBooking.MateID);
                }
            }
            catch (SqlException sqlExp)
            {
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }

            return Page();
        }

        public IActionResult OnPostDelete(int bookingid)
        {
            try
            {
                if (!_bookingService.CancelLaneBonking(bookingid))
                {
                    Message = "Bookingen kunne ikke aflyses. Prøv at navigere til denne side igen";
                    return OnGet(bookingid);
                }
                else
                {

                    return RedirectToPage("/Users/Index");
                }
            }
            catch (SqlException sqlExp)
            {
                ViewData["ErrorMessage"] = "Databasefejl: " + sqlExp.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Generel fejl: " + ex.Message;
                return Page();
            }
        }
        public IActionResult OnPostCancel()
        {
            return RedirectToPage("/Users/Index");
        }
    }
}
