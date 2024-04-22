using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ILaneService
    {
        bool CreateLane(Lane lane);

        bool DeleteLane(int id);

        bool EditLane(Lane lane, int id);

        List<Lane> GetAllLanes();

        Lane GetLaneByNumber(int id);
    }
}
