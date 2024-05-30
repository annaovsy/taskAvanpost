using Task.Integration.Data.DbCommon;
using Task.Integration.Data.Models;
using Task.Integration.Data.Models.Models;

namespace Task.Connector
{
    public class ConnectorDb : IConnector
    {
        private DbContextFactory dbContextFactory;
        private string provider;
        private DataContext dbContext;    
        private UserManager userManager;

        public ConnectorDb() { }

        public void StartUp(string connectionString)
        {
            provider = Utils.GetProvider(connectionString);
            if (String.IsNullOrEmpty(provider))
                Logger.Error("Provider not found");
            connectionString = Utils.GetValidConnectionString(connectionString);
            dbContextFactory = new DbContextFactory(connectionString);            
            dbContext = dbContextFactory.GetContext(provider);
            userManager = new UserManager(dbContext);           
        }

        public void CreateUser(UserToCreate user)
        {       
            throw new NotImplementedException();
        }

        public IEnumerable<Property> GetAllProperties()
        {
            Logger.Debug($"Method: GetAllProperties");
            return userManager.GetProperties();           
        }

        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            Logger.Debug($"Method: GetUserProperties");
            if (IsUserExists(userLogin))
            {
                var userProperties = userManager.GetUserProperties(userLogin);
                Logger.Debug($"Properties found for user with login {userLogin}");
                return userProperties;
            }
            
            return Enumerable.Empty<UserProperty>();
        }

        public bool IsUserExists(string userLogin)
        {
            Logger.Debug($"Method: IsUserExists");
            if(userManager.CheckUserExists(userLogin))
            {
                Logger.Debug($"Found user with login {userLogin}");
                return true;
            }  
            Logger.Warn($"User with login {userLogin} not found");
            return false;            
        }

        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            throw new NotImplementedException();
        }

        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetUserPermissions(string userLogin)
        {
            throw new NotImplementedException();
        }

        public ILogger Logger { get; set; }
    }
}