using Task.Integration.Data.DbCommon.DbModels;
using Task.Integration.Data.DbCommon;
using Task.Integration.Data.Models.Models;

namespace Task.Connector
{
    internal class UserManager
    {
        private readonly DataContext context;

        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string MiddleName { get; set; }
        private string TelephoneNumber { get; set; }
        private bool IsLead { get; set; }

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

        public string? GetPropertyValue(UserToCreate userToCreate, string propertyName) => userToCreate.Properties.FirstOrDefault(x => x.Name == propertyName)?.Value;
          
        public void SetProperties(UserToCreate userToCreate)
        {
            string? firstName = GetPropertyValue(userToCreate, PropertyConstants.firstName);
            FirstName = firstName ?? string.Empty;

            string? lastName = GetPropertyValue(userToCreate, PropertyConstants.lastName);
            LastName = lastName ?? string.Empty;

            string? middleName = GetPropertyValue(userToCreate, PropertyConstants.middleName);
            MiddleName = middleName ?? string.Empty;

            string? telephoneNumber = GetPropertyValue(userToCreate, PropertyConstants.telephoneNumber);
            TelephoneNumber = telephoneNumber ?? string.Empty;

            string? isLead = GetPropertyValue(userToCreate, PropertyConstants.isLead);
            IsLead = Convert.ToBoolean(isLead);
        }

        public void CreateUser(UserToCreate user)
        {
            SetProperties(user);

            User newUser = new()
            {
                Login = user.Login,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                TelephoneNumber = TelephoneNumber,
                IsLead = IsLead
            };
            context.Users.Add(newUser);

            Sequrity sequrity = new()
            {
                UserId = user.Login,
                Password = user.HashPassword
            };
            context.Passwords.Add(sequrity);
            context.SaveChanges();
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
