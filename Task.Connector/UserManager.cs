using Task.Integration.Data.DbCommon.DbModels;
using Task.Integration.Data.DbCommon;
using Task.Integration.Data.Models.Models;

namespace Task.Connector
{
    internal class UserManager
    {
        private readonly DataContext context;

        public UserManager(DataContext context)
        {
            this.context = context;
        }

        public User? GetUser(string login) => context.Users.FirstOrDefault(user => user.Login == login);

        public bool CheckUserExists(string login) => context.Users.Any(user => user.Login == login);

        public string GetPassword(string userLogin)
        {
            var password = context.Passwords.FirstOrDefault(x => x.UserId == userLogin);
            if (password != null)
                return password.Password;
            else return string.Empty;
        }

        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            List<UserProperty> properties = new();
            
            var user = GetUser(userLogin);
            if (user != null)
            {                             
                properties.Add(new UserProperty(PropertyConstants.lastName, user.LastName));
                properties.Add(new UserProperty(PropertyConstants.firstName, user.FirstName));
                properties.Add(new UserProperty(PropertyConstants.middleName, user.MiddleName));
                properties.Add(new UserProperty(PropertyConstants.telephoneNumber, user.TelephoneNumber));
                properties.Add(new UserProperty(PropertyConstants.isLead, user.IsLead.ToString()));
            }

            return properties;
        }       

        public IEnumerable<Property> GetProperties()
        {
            List<Property> properties = new()
            {
                new Property(PropertyConstants.lastName, "User's last name"),
                new Property(PropertyConstants.firstName, "User's first name"),
                new Property(PropertyConstants.middleName, "User's middle name"),
                new Property(PropertyConstants.telephoneNumber, "User's phone number"),
                new Property(PropertyConstants.isLead, "Whether the user is lead"),
                new Property(PropertyConstants.password, "User's password")
            };
            return properties;
        }

    }
}
