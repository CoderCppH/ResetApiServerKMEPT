using System.Data.SQLite;

namespace web_server{
    class DataBaseConnection{
        internal SQLiteConnection Connection = new SQLiteConnection();
        internal string StringConnection = "";
        public DataBaseConnection(){}
        public DataBaseConnection(string db_name){
            StringConnection = $"Data Source = {db_name}";
            this.Connection = new SQLiteConnection(StringConnection);
        }
        public void OpenConnection (){
            if(Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
        }
        public void CloseConnection(){
            if(Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
        }
        public SQLiteConnection GetConnection()=> Connection;
    }
}