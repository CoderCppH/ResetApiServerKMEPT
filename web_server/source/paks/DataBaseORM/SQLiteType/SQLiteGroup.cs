using System.Data.SQLite;

namespace web_server
{
    class SQLiteGroup
    {
        public static string @nameTable = "group_data";
        public static string @id = "@id";
        public static string @group_name = "@group_name";
        public static string @description = "@description";

        public int Id  {get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public static DataBaseConnection dataBase = new DataBaseConnection();

        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable} (id INTEGER PRIMARY KEY AUTOINCREMENT, group_name TEXT NOT NULL, description TEXT)";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, dataBase.GetConnection()))
                command.ExecuteNonQuery();
        }
    }
}