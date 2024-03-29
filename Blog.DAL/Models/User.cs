﻿namespace Blog.DAL.Models
{
    public class User
    {
        public ICollection<UserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new List<UserRole>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
