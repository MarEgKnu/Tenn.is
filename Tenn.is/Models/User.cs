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
        [Required(ErrorMessage = "Udfyld email")]
        public string Email { get; set; }
        [Display(Name = "Telefon nr")]
        [Required(ErrorMessage = "Udfyld telefon nr")]
        public string Phone { get; set; }
        public bool Administrator { get; set; }

        public bool RandomPassword { get; set; }

        public string NameDispayForAdmin { get
            {
                return $"{Username} : {FirstName} {LastName}";
            } }
        public string NameDispayForUser
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        public bool IsUtilityUser { get
            {
                return UserId < 1;
            } }

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
        public User(int id)
        {
            UserId = id;
        }
        public User()
        {

        }
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) 
            {
                return false;
            }
            User user = obj as User;
            return user.UserId == UserId &&
                   user.Username == Username &&
                   user.FirstName == FirstName &&
                   user.LastName == LastName &&
                   user.Email == Email &&
                   user.Password == Password &&
                   user.Phone == Phone &&
                   user.Administrator == Administrator &&
                   user.RandomPassword == RandomPassword;
        }
    }
}
