namespace Task.Connector
{
    public static class Utils
    {
        public static string GetProvider(string connectionString)
        {
            if (connectionString.Contains("SqlServer"))
                return "MSSQL";
            else if (connectionString.Contains("PostgreSQL"))
                return "POSTGRE";
            else
                return string.Empty;
        }

        public static string GetValidConnectionString(string connectionString)
        {
            connectionString = connectionString.Split("'")[1];
            return connectionString;
        }

    }
}
