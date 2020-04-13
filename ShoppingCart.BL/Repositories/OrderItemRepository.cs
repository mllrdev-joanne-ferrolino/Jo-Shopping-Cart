﻿using Dapper;
using ShoppingCart.BL.Models;
using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Repositories
{
    internal class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        internal override string TableName => "OrderItem";
        public new IList<OrderItem> GetAll()
        {
            return base.GetAll();
        }

        public new OrderItem GetById(int id)
        {
            return base.GetById(id);
        }
        public new IList<OrderItem> GetByName(string name)
        {
            return base.GetByName(name);
        }

        public new bool Update(OrderItem orderItem)
        {
            return base.Update(orderItem);
        }

        public new bool Delete(int[] id)
        {
            return base.Delete(id);
        }

        public bool DeleteByOrderId(int[] id)
        {
            try
            {
                string sql = $"DELETE FROM {TableName} WHERE OrderId IN ({string.Join(", ", id)})";
                return _connection.Execute(sql) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

        public bool Insert(OrderItem orderItem)
        {
            try
            {
                var properties = orderItem.GetType().GetProperties().Where(e => e.Name.ToLower() != "id");
                var fields = string.Join(", ", properties.Select(e => e.Name));
                var values = string.Join(", ", properties.Select(e => $"@{e.Name}"));
                string sql = $"INSERT INTO {TableName} ({fields}) VALUES ({values})";
                return _connection.Execute(sql, orderItem) > 0;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace);
                return false;
            }
        }

    }
}
