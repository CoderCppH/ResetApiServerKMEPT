using System.Data;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace web_server
{
    class SQLiteAesKeyIv: OrmSqlTable
    {
        public static string @nameTable = "aes_key_iv";
        public static string @id = "@id";
        public static string @key = "@key";
        public static string @iv = "@iv";
        public static string @id_user = "@id_user";
        public int Id { get; set; }
        public byte[] Key { get; set; } = new byte[0];
        public byte[] IV { get; set; } = new byte[0];
        public int IdUser { get; set; }
        public static DataBaseConnection database = new DataBaseConnection();
        public static void CreateTable()
        {
            string sqlCommand = $"CREATE TABLE IF NOT EXISTS {@nameTable}(id INTEGER PRIMARY KEY AUTOINCREMENT, key BLOB NOT NULL, iv BLOB NOT NULL, id_user INTEGER NOT NULL, FOREIGN KEY (id_user) REFERENCES user_data(id))";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
                command.ExecuteNonQuery();
        }
        override public bool Create()
        {
            string sqlCommand = $"INSERT INTO {@nameTable} (key, iv, id_user ) VALUES ({@key}, {@iv}, {@id_user} ) ";
            using(SQLiteCommand command = new SQLiteCommand(sqlCommand, database.GetConnection()))
            {
                command.Parameters.AddWithValue(@key, Key);
                command.Parameters.AddWithValue(@iv, IV);
                command.Parameters.AddWithValue(@id_user, IdUser);
                int codeResult = command.ExecuteNonQuery();
                if(codeResult == 1)
                    return true;
            }
            return false;
        }
        public override bool IsNotNull()
        {
            if(Key != null && IV != null)
                return true;
            return false;
        }
        public static List<SQLiteAesKeyIv> GetAllAes()
        {
            List<SQLiteAesKeyIv> listAes = new List<SQLiteAesKeyIv>();
            string sqlCommand = $"SELECT * FROM {nameTable}";
            using(SQLiteDataReader read = new SQLiteCommand(sqlCommand, database.GetConnection()).ExecuteReader())
                while(read.Read())
                    listAes.Add(new SQLiteAesKeyIv(){Id = read.GetInt32(0), Key=(byte[])read["key"], IV=(byte[])read["iv"], IdUser = read.GetInt32(3)  });
            return listAes;
        }
        public static SQLiteAesKeyIv GetAesByIdUser(int id_user){
            foreach(var i_aes in GetAllAes())
                if(i_aes.IdUser == id_user)
                    return i_aes;
            return null;
        }
    }
}