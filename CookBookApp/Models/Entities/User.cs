using System;
using System.Collections.Generic;

namespace CookBookApp.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Receipts = new HashSet<Receipt>();
        }

        public User(string login, string password, string firstName, string lastName)
        {
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public int? Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual ICollection<Receipt> Receipts { get; set; }

        public string GetFullname() => FirstName + " " + LastName;
    }
}