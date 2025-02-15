using System.Data.SQLite;

namespace web_server
{
    class SQLiteMessanger: OrmSqlTable
    {
        public static string @nameTable = "messanger_data";
        public static string @id = "@id";
        public static string @message = "@message";
        public static string @time_send = "@time_send";
        public static string @id_sender_user = "@id_sender_user";
        public static string @id_group = "@id_group";

        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string TimeSend { get; set; } = string.Empty;
        public int IdSenderUser { get; set; }
        public int IdGroup { get; set; }
        public static DataBaseConnection dataBase = new DataBaseConnection();

        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable} (id INTEGER PRIMARY KEY AUTOINCREMENT, message TEXT NOT NULL, time_send TEXT NOT NULL, id_sender_user INTEGER NOT NULL, id_group INTEGER NOT NULL, FOREIGN KEY (id_group) REFERENCES group_data(id) )";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, dataBase.GetConnection()))
                command.ExecuteNonQuery();
        }
        public override bool Create()
        {
            throw new NotImplementedException();
        }
        public override bool IsNotNull()
        {
            throw new NotImplementedException();
        }
    }
}