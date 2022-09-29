using MySql.Data.MySqlClient;
using System;

namespace kftest
{
    public class MySqlDatabase : IDisposable
    {
        public MySqlConnection Connection;

        public MySqlDatabase(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);

            this.Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
