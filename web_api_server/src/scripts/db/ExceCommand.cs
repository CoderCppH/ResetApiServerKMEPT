using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using Dapper;
using Microsoft.VisualBasic;

namespace Orm {
    class ExceCommand: IDisposable {
        private string? dbPath = "database.db";
        private Connect? connect;
        public ExceCommand() {
            connect = new Connect(dbPath);
            connect?.Open();
        }

        public void Dispose()
        {
            connect?.Close();
        }
        public bool CreateTable(string? tableName, string? args1) {
            if (connect?.GetConnection().Execute($"CREATE TABLE IF NOT EXISTS {tableName} ({args1})") > 0)
                return true;
            return false;
        }
        public bool Insert(string? tableName, string? args1, string? args2, object? data) {
            try{
                if (connect?.GetConnection().Execute($"INSERT INTO {tableName} ({args1}) VALUES ({args2})", data) > 0)
                    return true;
            }
            catch {} 
            return false;
        }
        public List<T>?SelectFrom<T>(string? tableName) {
            return connect?.GetConnection().Query<T>($"SELECT * FROM {tableName}").ToList();
        }
        public bool Update(string? tableName, string? args1, string? args2, object? data) {
            if (connect?.GetConnection().Execute($"UPDATE {tableName} SET {args1} WHERE {args2}", data) > 0) 
                return true;
            return false;
        }
        public bool Drop(string? tableName, string argq1, object data) {
            if (connect?.GetConnection().Execute($"DELETE FROM {tableName} WHERE {argq1}", data) > 0)
                return true;
            return false;
        }
    }
}