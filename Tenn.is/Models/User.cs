﻿using System.ComponentModel.DataAnnotations;

namespace Tennis.Models
{
    public class User
    {
        public int UserId { get; set; }
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
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Udfyld password")]
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

        public User()
        {
            
        }
    }
}
