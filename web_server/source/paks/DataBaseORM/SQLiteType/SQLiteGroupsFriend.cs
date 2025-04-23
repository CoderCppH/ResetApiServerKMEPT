using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_server.source.paks.DataBaseORM.SQLiteType
{
    internal class SQLiteGroupsFriend : OrmSqlTable
    {
        //name
        public static string @nameTable = "groups_friend";
        public static string @id = "@id";
        public static string @id_user_1 = "@id_user_1";
        public static string @id_user_2 = "@id_user_2";
        //value
        public int Id { get; set; }
        public int IdUser1 { get; set; }
        public int IdUser2 { get; set; }

        public static DataBaseConnection database = new DataBaseConnection();
        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable} ( id INTEGER PRIMARY KEY AUTOINCREMENT, id_user_1 INTEGER NOT NULL, id_user_2 INTEGER NOT NULL, FOREIGN KEY(\"id_user_1\") REFERENCES \"{SQLiteUser.nameTable}\"(\"id\") , FOREIGN KEY(\"id_user_2\") REFERENCES \"{SQLiteUser.nameTable}\"(\"id\"))";
            using (SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
                command.ExecuteNonQuery();
        }
        public override bool Create()
        {

            string sqlCommand = $"INSERT INTO {@nameTable} (id_user_1, id_user_2) VALUES ({id_user_1}, {id_user_2})";
            using (SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
            {
                command.Parameters.AddWithValue(id_user_1, IdUser1);
                command.Parameters.AddWithValue(id_user_2, IdUser2);
                int codeResult = command.ExecuteNonQuery();
                if (codeResult == 1)
                    return true;
            }

            return false;
        }
        public static List<p_gorups_friends> GetAllFriends()
        {
            List<p_gorups_friends> users = new List<p_gorups_friends>();
            string sqlCommand = $"SELECT * FROM {@nameTable}";
            using (SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
            {
                var read = command.ExecuteReader();
                while (read.Read())
                    users.Add(new p_gorups_friends() { id = read.GetInt32(0), id_user_1 = read.GetInt32(1), id_user_2 = read.GetInt32(2)});
            }
            return users;
        }
        public override bool IsNotNull()
        {
            if (IdUser1 != 0 && IdUser2 != 0)
                return true;
            return false;
        }
        public static List<p_user_data> GetAllReturnAllMyFriendsById(int my_id)
        {
            List<p_user_data> list = new List<p_user_data>();
            var all_suf = GetAllFriends();
            foreach (var item in all_suf) {
                if (item.id_user_1 == my_id)
                {
                    var t_user = SQLiteUser.GetByIdOrEmailUser(item.id_user_2, "");
                    list.Add(new p_user_data() { id = t_user.Id, email = t_user.Email, fullanme = t_user.FullName });
                }
                else if (item.id_user_2 == my_id) 
                {
                    var t_user = SQLiteUser.GetByIdOrEmailUser(item.id_user_1, "");
                    list.Add(new p_user_data() { id = t_user.Id, email = t_user.Email, fullanme = t_user.FullName });
                }          
            }
            return list;
        }
       
    }
}
