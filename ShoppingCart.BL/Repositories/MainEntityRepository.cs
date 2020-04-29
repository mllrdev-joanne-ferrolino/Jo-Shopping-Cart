using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShoppingCart.BL.Repositories
{
    internal abstract class MainEntityRepository<T> : BaseRepository<T> where T:class
    {
        public int GetId(int id)
        {
            try
            {
                string sql = $"SELECT {id} FROM {TableName}";
                return _connection.Execute(sql);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }
        }

        internal T GetById(int id)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE Id = {id}";
                return _connection.QueryFirstOrDefault<T>(sql);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }

        }
        internal IList<T> GetByName(string name)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE Name = '{name}'";
                return _connection.Query<T>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
            }
        }

        public int Insert(T entity)
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var fields = string.Join(", ", properties.Select(e => e.Name));
                var values = string.Join(", ", properties.Select(e => $"@{e.Name}"));
                string sql = $"INSERT INTO {TableName} ({fields}) VALUES ({values}); SELECT @@IDENTITY";
                return _connection.ExecuteScalar<int>(sql, entity);
               
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return 0;
            }

        }
        internal bool Update(T entity)
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var values = string.Join(", ", properties.Select(e => $"{e.Name} = @{e.Name}"));
                string sql = $"UPDATE {TableName} SET {values} WHERE Id = @Id";
                return _connection.Execute(sql, entity) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }

        }

        internal bool Delete(int[] id)
        {
            try
            {
                string sql = $"DELETE FROM {TableName} WHERE Id IN ({string.Join(", ", id)})";
                return _connection.Execute(sql) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }

        }

        internal IList<T> Search(List<string> condition) 
        {
            try
            {
                //List<string> condition = new List<string>();
                //var properties = obj.GetType().GetProperties();

                //foreach (var property in properties)
                //{
                //    var value = property.GetValue(obj);

                //    if (value is int)
                //    {
                //        if ((int)value > 0)
                //        {
                //            condition.Add($"{property.Name} = {value}");
                //        }

                //    }
                //    else if (value is string)
                //    {
                //        if (!string.IsNullOrWhiteSpace((string)value))
                //        {
                //            condition.Add($"{property.Name} = '{value}'");
                //        }

                //    } 
                //    else if (value is float)
                //    {
                //        if ((float)value > 0.0f)
                //        {
                //            condition.Add($"{property.Name} = {value}");
                //        }

                //    }

                //}

                string sql = $"SELECT * FROM {TableName} WHERE {string.Join(" AND ", condition.ToArray())}";
                return _connection.Query<T>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }
            
        }

    }
}
