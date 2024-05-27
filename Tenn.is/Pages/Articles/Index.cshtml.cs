using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Services;

namespace Tennis.Pages.Articles
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; } = "Initial Request";

        public bool IsLoggedIn = false;
        public bool IsAdmin = false;
        public string Username = string.Empty;

        public List<Article> AllArticles { get; set; }
        [BindProperty(SupportsGet = true)] public string Sorting { get; set; } = "Initial";

        [BindProperty(SupportsGet = true)] public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)] public bool IsChecked { get; set; }

        [BindProperty(SupportsGet = true)] public int Order_TimeStamp { get; set; }
        [BindProperty(SupportsGet = true)] public int Order_LastEdited { get; set; }
        [BindProperty(SupportsGet = true)] public int Order_Author { get; set; }


        public UserService _userService {  get; set; }

        private IArticleService _articleService;
        public IndexModel(IArticleService articleService)
        {
            _articleService = new ArticleService(true);
        }

        public void OnGet()
        {
            #region Determining how to display Articles:

            //if (Order_TimeStamp != 0)
            //{
            //    if (!SearchFilter.IsNullOrEmpty())
            //    {
            //        if (IsChecked) { DisplayContentSearchResults_ByTimeStamp();}
            //        else { DisplaySearchResults_ByTimeStamp(); }
            //    }
            //    else { DisplayAllArticles_ByTimeStamp(); }
            //}

            //else if (Order_LastEdited != 0)
            //{
            //    if (!SearchFilter.IsNullOrEmpty())
            //    {
            //        if (IsChecked) { DisplayContentSearchResults_ByLastEdited(); }
            //        else { DisplaySearchResults_ByLastEdited(); }
            //    }
            //    else { DisplayAllArticles_ByLastEdited(); }
            //}

            //else if (Order_Author != 0)
            //{
            //    if (!SearchFilter.IsNullOrEmpty())
            //    {
            //        if (IsChecked) { DisplayContentSearchResults_ByAuthor(); }
            //        else { DisplaySearchResults_ByAuthor(); }
            //    }
            //    else { DisplayAllArticles_ByAuthor(); }
            //}

            //else
            //{
            //    if (!SearchFilter.IsNullOrEmpty())
            //    {
            //        if (IsChecked) { DisplayContentSearchResults_ByTimeStamp(); }
            //        else { DisplaySearchResults_ByTimeStamp(); }
            //    }
            //    else { DisplayAllArticles_ByTimeStamp(); }
            //}
            #endregion

            if (_articleService.SortingByTimeStamp) {

            }
            else if (_articleService.SortingByLastEdited) {

            }
            else if (_articleService.SortingByAuthor) {

            }

            if (SearchFilter.IsNullOrEmpty())
            {
                DisplayAllArticles();
            }
            else
            {
                if (IsChecked)
                {
                    FilterBySearchPlusContent();
                }
                else
                {
                    FilterBySearch();
                }
            }
            #region User check (TODO)
            //    if (_userService.VerifyUser(HttpContext.Session.GetString("Username"), HttpContext.Session.GetString("Password")) != null)
            //    {
            //        IsLoggedIn = true;
            //        Username = HttpContext.Session.GetString("Username");

            //        if (_userService.AdminVerify(Username, HttpContext.Session.GetString("Password"))) 
            //        {
            //            IsAdmin = true;
            //        }
            //        else { IsAdmin = false; }
            //    } 
            //    else { IsLoggedIn = false; }
            #endregion
        }
        public void DisplayAllArticles()
        {
            AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.TimeStamp).ToList();
        }

        #region Display calls for each combination of search/contentcheck/orderby
        public void DisplayAllArticles_ByTimeStamp()
        {
            if (Order_TimeStamp == 2) { AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.TimeStamp).ToList(); }
            else { AllArticles = _articleService.GetAllArticles().OrderBy(a => a.TimeStamp).ToList(); }
        }

        public void DisplaySearchResults_ByTimeStamp()
        {
            if (Order_TimeStamp == 1) { AllArticles = _articleService.SearchArticlesDefault(SearchFilter).OrderBy(a => a.TimeStamp).ToList(); }
            else if (Order_TimeStamp == 2) { AllArticles = _articleService.SearchArticlesDefault(SearchFilter).OrderByDescending(a => a.TimeStamp).ToList(); }
        }

        public void DisplayContentSearchResults_ByTimeStamp()
        {
            if (Order_TimeStamp == 1) { AllArticles = _articleService.SearchArticlesContent(SearchFilter).OrderBy(a => a.TimeStamp).ToList(); }
            else if (Order_TimeStamp == 2) { AllArticles = _articleService.SearchArticlesContent(SearchFilter).OrderByDescending(a => a.TimeStamp).ToList(); }
        }

        public void DisplayAllArticles_ByLastEdited()
        {
            if (Order_LastEdited == 1) { AllArticles = _articleService.GetAllArticles().OrderBy(a => a.LastEdited).ToList(); }
            else if (Order_LastEdited == 2) { AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.LastEdited).ToList(); }
        }
        public void DisplaySearchResults_ByLastEdited()
        {
            if (Order_LastEdited == 1) { AllArticles = _articleService.SearchArticlesDefault(SearchFilter).OrderBy(a => a.LastEdited).ToList(); }
            else if (Order_LastEdited == 2) { AllArticles = _articleService.SearchArticlesDefault(SearchFilter).OrderByDescending(a => a.LastEdited).ToList(); }
        }
        public void DisplayContentSearchResults_ByLastEdited()
        {
            if (Order_LastEdited == 1) { AllArticles = _articleService.SearchArticlesContent(SearchFilter).OrderBy(a => a.LastEdited).ToList(); }
            else if (Order_LastEdited == 2) { AllArticles = _articleService.SearchArticlesContent(SearchFilter).OrderByDescending(a => a.LastEdited).ToList(); }
        }
        public void DisplayAllArticles_ByAuthor()
        {
            if (Order_Author == 1) { AllArticles = _articleService.GetAllArticles().OrderBy(a => a.AuthorID).ToList(); }
            else if (Order_Author == 2) { AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.AuthorID).ToList(); }
        }
        public void DisplaySearchResults_ByAuthor()
        {
            if (Order_Author == 1) { AllArticles = _articleService.SearchArticlesDefault(SearchFilter).OrderBy(a => a.AuthorID).ToList(); }
            else if (Order_Author == 2) { AllArticles = _articleService.SearchArticlesDefault(SearchFilter).OrderByDescending(a => a.AuthorID).ToList(); }
        }
        public void DisplayContentSearchResults_ByAuthor()
        {
            if (Order_Author == 1) { AllArticles = _articleService.SearchArticlesContent(SearchFilter).OrderBy(a => a.AuthorID).ToList(); }
            else if (Order_Author == 2) { AllArticles = _articleService.SearchArticlesContent(SearchFilter).OrderByDescending(a => a.AuthorID).ToList(); }
        }
        #endregion

        public void FilterBySearch()
        {
            AllArticles = _articleService.SearchArticlesDefault(SearchFilter);
        }
        public void FilterBySearchPlusContent()
        {
            AllArticles = _articleService.SearchArticlesContent(SearchFilter);
        }

        public void OrderArticles()
        {
            AllArticles.OrderByDescending(a => a.TimeStamp);
        }

        public IActionResult OnPostSorting()
        {

            return RedirectToPage("Index");
        }

        public IActionResult OnPostOprettet()
        {
            Order_LastEdited = 0;
            Order_Author = 0;
            if (Order_TimeStamp >= 2) {
                Order_TimeStamp = 0;
            }
            else {
                Order_TimeStamp++;
            }
            AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.TimeStamp).ToList();

            Message = $"Sorterer efter Oprettet{Order_TimeStamp}";
            return RedirectToPage("Index");
        }
        public IActionResult OnPostRedigeret()
        {
            Order_TimeStamp = 0;
            Order_Author = 0;
            if (Order_LastEdited >= 2) {
                Order_LastEdited = 0;
            }
            else {
                Order_LastEdited++;
            }
            AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.TimeStamp).ToList();

            Message = $"Sorterer efter Oprettet{Order_LastEdited}";
            return RedirectToPage("Index");
        }
        public IActionResult OnPostForfatter()
        {
            Order_TimeStamp = 0;
            Order_LastEdited = 0;
            if (Order_Author >= 2) {
                Order_Author = 0;
            }
            else {
                Order_Author++;
            }
            AllArticles = _articleService.GetAllArticles().OrderByDescending(a => a.TimeStamp).ToList();

            Message = $"Sorterer efter Oprettet{Order_Author}";
            return RedirectToPage("Index");
        }

        public IActionResult OnPostTime(int vala)
        {
            Order_TimeStamp = vala;
            if (Order_TimeStamp == 2) 
            { 
                Order_TimeStamp = 0;
                return RedirectToPage("Index");
            }
            else 
            {
                Order_TimeStamp++;
                return RedirectToPage("Index");
            }
        }
        public IActionResult OnPostEdit(int valb)
        {
            Order_LastEdited = valb;
            if (Order_LastEdited == 2)
            {
                Order_LastEdited = 0;
                return RedirectToPage("Index");
            }
            else
            {
                Order_LastEdited++;
                return RedirectToPage("Index");
            }
        }
        public IActionResult OnPostAuthor(int valc)
        {
            Order_Author = valc;
            if (Order_Author == 2)
            {
                Order_Author = 0;
                return RedirectToPage("Index");
            }
            else
            {
                Order_Author++;
                return RedirectToPage("Index");
            }
        }


        public IActionResult OnPost(int id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToPage("Index");
        }
        
    }
}
