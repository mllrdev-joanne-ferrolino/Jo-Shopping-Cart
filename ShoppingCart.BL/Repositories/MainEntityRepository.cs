﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}