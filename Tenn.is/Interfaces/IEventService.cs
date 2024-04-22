using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IEventService
    {
        bool CreateEvent(Event evt);

        bool DeleteEvent(int id);

        bool EditEvent(Event evt, int id);

        List<Event> GetAllEvents();

        Event GetEventByNumber(int id);
    }
}
