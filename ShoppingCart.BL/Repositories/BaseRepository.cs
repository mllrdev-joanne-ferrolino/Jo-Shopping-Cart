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
        internal abstract string ColumnIdName { get; }
        internal SqlConnection _connection;
        internal static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal BaseRepository()
        {
            try
            {
                _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dbShoppingCart"].ConnectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {

                _log.Error(ex.StackTrace);
            }
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

        internal bool Delete(int[] id)
        {
            try
            {
                string sql = $"DELETE FROM {TableName} WHERE {ColumnIdName} IN ({string.Join(", ", id)})";
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
