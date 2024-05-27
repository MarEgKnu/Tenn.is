using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private IArticleService _articleService;
    private IEventService _eventService;
    public List<Event> Events { get; set; }
    public Article NewestArticle { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IEventService eventService, IArticleService articleService)
    {
        _logger = logger;
        _articleService = articleService;
        _eventService = eventService;
    }

    public void OnGet()
    {
        Events = _eventService.GetAllEvents().Where(x => x.EventState == Helpers.RelativeTime.Future).OrderBy(x => x.EventTime.StartTime).Take(5).ToList();
        NewestArticle = _articleService.GetAllArticles().OrderBy(a => a.TimeStamp).FirstOrDefault();
        if (NewestArticle == null)
        {
            NewestArticle = new Article();
        }
    }
}
