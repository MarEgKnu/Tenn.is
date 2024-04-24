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
