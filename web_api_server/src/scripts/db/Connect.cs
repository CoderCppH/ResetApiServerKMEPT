using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
namespace Orm {
    class Connect {
        private SQLiteConnection connection;
        public Connect(string? dbPath) {
            if(!File.Exists(dbPath)) {
                SQLiteConnection.CreateFile(dbPath);
            }
            connection = new SQLiteConnection($"Data Source={dbPath}; Version=3");
        }
        public void Open() { if(connection.State == System.Data.ConnectionState.Closed)  connection.Open(); }
        public void Close() { if(connection.State == System.Data.ConnectionState.Open) connection.Close(); }
        public SQLiteConnection GetConnection() => connection;
    }
}