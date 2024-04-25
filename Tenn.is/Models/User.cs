using System.ComponentModel.DataAnnotations;

namespace Tennis.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Dit password kan ikke være tomt")]
        public string Password { get; set; }
        [Display(Name = "Brugernavn")]
        [Required(ErrorMessage = "Udfyld brugernavn")]
        public string Username { get; set; }
        [Display(Name = "Fornavn")]
        [Required(ErrorMessage = "Udfyld fornavn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternavn")]
        [Required(ErrorMessage = "Udfyld efternavn")]
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }
        public bool Administrator { get; set; }

        public bool RandomPassword { get; set; }

        public User(int id, string username, string firstname, string lastname, string email, string password, string phone, bool admin, bool randompassword)
        {
            UserId = id;
            Username = username;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Password = password;
            Phone = phone;
            Administrator = admin;
            RandomPassword = randompassword;
        }

        public User(string password)
        {
            Password = password;
            RandomPassword = true;
        }

        public User()
        {

        }
    }
}
