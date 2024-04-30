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
        /// <summary>
        /// Returns a list of filtered events based on the conditions from the database
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        List<Event> GetEventsOnConditions(List<Predicate<Event>> conditions);
        /// <summary>
        /// Returns a list of filtered events based on the conditions from the events parameter
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        List<Event> GetEventsOnConditions(List<Predicate<Event>> conditions, List<Event> events);
    }
}
