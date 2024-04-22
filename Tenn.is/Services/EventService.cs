using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class EventService : Connection, IEventService
    {
        public EventService()
        {
            connectionString = Secret.ConnectionString;
        }
        public EventService(bool test)
        {
            if (test) 
            {
                connectionString = Secret.ConnectionStringTest;
            }
            else
            {
                connectionString = Secret.ConnectionString;
            }

        }
        public bool CreateEvent(Event evt)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEvent(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditEvent(Event evt, int id)
        {
            throw new NotImplementedException();
        }

        public List<Event> GetAllEvents()
        {
            throw new NotImplementedException();
        }

        public Event GetEventByNumber(int id)
        {
            throw new NotImplementedException();
        }
    }
}
