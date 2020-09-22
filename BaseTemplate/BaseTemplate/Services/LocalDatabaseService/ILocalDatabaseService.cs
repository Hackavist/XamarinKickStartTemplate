using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace BaseTemplate.Services.LocalDatabaseService
{
    public interface ILocalDatabaseService
    {
        Task CreateDatabaseTables(List<Type> tables);
        Task CreateDatabaseTables(List<Type> tables, CreateFlags tableCreateFlags);
        Task<List<T>> GetAll<T>() where T : class, new();
        Task<int> Insert(object item);
        Task<int> InsertAll<T>(List<T> items) where T : new();
        Task<int> Update(object item);
        Task<int> UpdateAll<T>(List<T> items) where T : new();
        Task<int> Delete(object obj);
        Task<int> DeleteAll<T>() where T : new();
        Task<int> DropTableAsync<T>() where T : new();
        Task<IList<T>> QueryString<T>(string sql) where T : class, new();
        Task<List<SQLiteConnection.ColumnInfo>> GetTableInfo(string tableName);
        Task<List<T>> GetAll<T>(Expression<Func<T, bool>> query) where T : new();
        Task<T> GetOne<T>(Expression<Func<T, bool>> query) where T : class, new();
        Task InsertOrReplaceAll<T>(List<T> items) where T : new();
        Task<int> InsertOrReplaceOne<T>(object item) where T : new();
        Task DeleteAll<T>(List<T> items) where T : new();
        Task<int> ExecuteAsync(string sqlQuery);
    }
}