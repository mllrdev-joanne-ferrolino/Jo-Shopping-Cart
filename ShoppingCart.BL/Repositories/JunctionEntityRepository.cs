using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ShoppingCart.BL.Repositories
{
    internal abstract class JunctionEntityRepository<T> : BaseRepository<T> where T:class
    {
       
        public bool Insert(T entity)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var properties = entity.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                    var fields = string.Join(", ", properties.Select(e => e.Name));
                    var values = string.Join(", ", properties.Select(e => $"@{e.Name}"));
                    string sql = $"INSERT INTO {TableName} ({fields}) VALUES ({values})";
                    var result = _connection.Execute(sql, entity) > 0;
                    scope.Complete();
                    return result;
                    
                }
               
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        
        }

        
    }
}
