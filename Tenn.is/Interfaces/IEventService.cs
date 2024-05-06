using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface IEventService
    {
        // should prehaps be changed to static?
        public event Action<Event> OnCancelling;
        public event Action<Event> OnCreate;
        public event Action<Event> OnDelete;
        public event Action<Event> OnEdit;

        bool CreateEvent(Event evt);

        bool CreateEventNoRequirements(Event evt);

        bool DeleteEvent(int id);

        bool EditEvent(Event evt, int id);

        List<Event> GetAllEvents();

        Event GetEventByNumber(int id);
    }
}
