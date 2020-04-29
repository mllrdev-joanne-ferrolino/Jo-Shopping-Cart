using ShoppingCart.BL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.BL.Managers
{
    public abstract class MainEntityManager<T> where T: class
    {
        public abstract IMainEntityRepository<T> Repository { get; }

        public T GetById(int id)
        {
            return Repository.GetById(id);
        }

        public IList<T> GetByName(string name)
        {
            return Repository.GetByName(name);
        }

        public int Insert(T entity)
        {
            return Repository.Insert(entity);
        }

        public bool Update(T entity)
        {
            return Repository.Update(entity);
        }

        public bool Delete(int[] id)
        {
            return Repository.Delete(id);
        }

        public int GetId(int id) 
        {
            return Repository.GetId(id);
        }

        public IList<T> Search(List<string> conditions) 
        {
            return Repository.Search(conditions);
        }

        public List<string> CreateConditions(T obj) 
        {
            List<string> conditions = new List<string>();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);

                if (value is int)
                {
                    if ((int)value > 0)
                    {
                        conditions.Add($"{property.Name} = {value}");
                    }

                }
                else if (value is string)
                {
                    if (!string.IsNullOrWhiteSpace((string)value))
                    {
                        conditions.Add($"{property.Name} = '{value}'");
                    }

                }
                else if (value is float)
                {
                    if ((float)value > 0.0f)
                    {
                        conditions.Add($"{property.Name} = {value}");
                    }

                }

            }

            return conditions;
        }
    }
}
