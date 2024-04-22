using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Services
{
    public class TennisLobbyService : Connection, ITennisLobbyService
    {
        public TennisLobbyService()
        {
            connectionString = Secret.ConnectionString;
        }
        public TennisLobbyService(bool test)
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
        public bool CreateTennisLobby(TennisLobby tennisLobby)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTennisLobby(int id)
        {
            throw new NotImplementedException();
        }

        public bool EditTennisLobby(TennisLobby tennisLobby, int id)
        {
            throw new NotImplementedException();
        }

        public List<TennisLobby> GetAllTennisLobbies()
        {
            throw new NotImplementedException();
        }

        public TennisLobby GetTennisLobbyById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
