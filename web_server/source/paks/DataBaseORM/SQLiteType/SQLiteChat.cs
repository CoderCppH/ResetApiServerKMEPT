using System.Data.SQLite;

namespace web_server
{
    class SQLiteChat : OrmSqlTable
    {
        public static string @nameTable = "chat_data";
        public static string @id = "@id";
        public static string @id_user1 = "@id_user1";
        public static string @id_user2 = "@id_user2";
        public int Id { get; set; }
        public int IdUser1 { get; set; }
        public int IdUser2 { get; set; }
        public static DataBaseConnection dataBase = new DataBaseConnection();
        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable} (id INTEGER PRIMARY KEY AUTOINCREMENT, id_user1 INTEGER NOT NULL, id_user2 INTEGER NOT NULL, FOREIGN KEY (id_user1) REFERENCES user_data(id), FOREIGN KEY (id_user2) REFERENCES user_data(id))";
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