using Dapper;
using Dapper.Contrib.Extensions;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories
{
    internal abstract class BaseRepository<T> where T: class
    {
        internal abstract string TableName { get; }
        internal SqlConnection _connection;
        internal static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal BaseRepository()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbShoppingCart"].ConnectionString);
            _connection.Open();
        }

        internal IList<T> GetAll()
        {
            try
            {
                string sql = $"SELECT * FROM {TableName}";
                return _connection.Query<T>(sql).AsList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return null;
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

        internal bool Insert(T entity)
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var fields = string.Join(", ", properties.Select(e => e.Name));
                var values = string.Join(", ", properties.Select(e => $"@{e.Name}"));
                string sql = $"INSERT INTO {TableName} ({fields}) VALUES ({values})";
                _connection.Execute(sql, entity);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }

        }
        internal bool Update(T entity)
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(e => e.Name.ToLower() != "id" );
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
    }
}
