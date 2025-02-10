using System.Data.SQLite;

namespace web_server{
    class DataBaseConnection{
        internal SQLiteConnection Connection = new SQLiteConnection();
        internal string StringConnection = "";
        public DataBaseConnection(string db_name){
            StringConnection = $"Data Source = {db_name}";
            this.Connection = new SQLiteConnection(StringConnection);
        }
        
    }
}