using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class LaneService : Connection, ILaneService
    {
        public LaneService()
        {
            connectionString = Secret.ConnectionString;
        }
        public LaneService(bool test)
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
        public bool CreateLane(Lane lane)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLane(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditLane(Lane lane, int id)
        {
            throw new NotImplementedException();
        }

        public List<Lane> GetAllLanes()
        {
            throw new NotImplementedException();
        }

        public Lane GetLaneByNumber(int id)
        {
            throw new NotImplementedException();
        }
    }
}
