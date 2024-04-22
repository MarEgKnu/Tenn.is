using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IEventService
    {
        bool CreateEvent(Event event);

        bool DeleteEvent(int id);

        bool EditEvent(Event event, int id);

        List<Event> GetAllEvents();

        Event GetEventByNumber(int id);
    }
}
