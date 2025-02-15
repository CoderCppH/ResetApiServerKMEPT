using System.Data.SQLite;

namespace web_server
{
    class SQLiteGroupMembers :OrmSqlTable
    {
         public static string @nameTable = "group_member_data";
        public static string @id = "@id";
        public static string @group_name = "@group_name";
        public static string @description = "@description";

        public int Id  {get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public static DataBaseConnection dataBase = new DataBaseConnection();

        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable} (id INTEGER PRIMARY KEY AUTOINCREMENT, role TEXT CHECK(role IN ('admin', 'member', 'guest')) NOT NULL, id_group INTEGER NOT NULL, id_user INTEGER NOT NULL, FOREIGN KEY (id_group) REFERENCES group_data(id), FOREIGN KEY (id_user) REFERENCES user_data(id) )";
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