using System;
namespace AGSIdentity.Models.DataModels
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public User()
        {

        }

        // this is for converting ASP.NET Identity user to AGS User model
        public User(ApplicationUser user)
        {
            if (user != null)
            {
                this.Id = user.Id;
                this.Username = user.UserName;
                this.Email = user.Email;
            }
            
        }

        // this is for converting Active directory user to AGS User model
        // To be done...
    }
}
