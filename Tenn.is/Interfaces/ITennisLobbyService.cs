using Tennis.Models;

namespace Tennis.Interfaces
{
    public interface ITennisLobbyService
    {
        bool CreateTennisLobby(TennisLobby tennisLobby);

        bool DeleteTennisLobby(int id);

        bool EditTennisLobby(TennisLobby tennisLobby, int id);

        List<TennisLobby> GetAllTennisLobbies();

        TennisLobby GetTennisLobbyById(int id);
    }
}
