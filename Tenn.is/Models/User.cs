namespace Tennis.Models
{
    public class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool Administrator { get; set; }

        public User(int id, string username, string firstname, string lastname, string email, string password, string phone, bool admin)
        {
            UserId = id;
            Username = username;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Password = password;
            Phone = phone;
            Administrator = admin;
        }
    }
}
