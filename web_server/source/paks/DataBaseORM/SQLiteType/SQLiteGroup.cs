using System.Data.SQLite;
using Newtonsoft.Json;

namespace web_server
{
    class SQLiteGroup: OrmSqlTable
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
        public override bool IsNotNull(){
            if(GroupName != string.Empty)
                return true;
            return false;
        }
        public override bool Create()
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(this));
            string sqlCommand = $"INSERT INTO {nameTable} (group_name, description) VALUES ({@group_name}, {@description})";
            if(IsNotNull())
            {
                using(SQLiteCommand command = new SQLiteCommand(sqlCommand, dataBase.GetConnection()))
                {
                    command.Parameters.AddWithValue(@group_name, GroupName);
                    command.Parameters.AddWithValue(@description, Description);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            else
                return false;
        }
        public static List<SQLiteGroup> GetAllGroup(){
            List<SQLiteGroup> list_group = new List<SQLiteGroup>();
            string sqlCommand = $"SELECT * FROM {nameTable}";
            using(SQLiteDataReader read = new SQLiteCommand(sqlCommand, dataBase.GetConnection()).ExecuteReader())
                while(read.Read())
                    list_group.Add(new SQLiteGroup(){Id = read.GetInt32(0), GroupName = read.GetString(1), Description = read.GetString(2)});
            return list_group;
        }
    }
}