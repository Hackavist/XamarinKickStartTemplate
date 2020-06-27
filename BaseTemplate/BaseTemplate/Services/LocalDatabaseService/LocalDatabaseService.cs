using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BaseTemplate.Constants;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using Xamarin.Essentials;

namespace BaseTemplate.Services.LocalDatabaseService
{
    public class LocalDatabaseService : ILocalDatabaseService
    {

        #region Flags

        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache |
            // Encrypts The Database File
            SQLiteOpenFlags.ProtectionComplete;

        #endregion

        #region Statics

        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, AppConstants.DatabaseFileName);

        private static readonly Lazy<SQLiteAsyncConnection> LazyInitializer =
            new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(DatabasePath, Flags));

        private static SQLiteAsyncConnection SqlCon => LazyInitializer.Value;

        #endregion

        #region Methods

        public async Task CreateDatabaseTables(List<Type> tables)
        {
            foreach (Type item in tables)
                try
                {
                    await SqlCon.CreateTableAsync(item).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }

        public async Task CreateDatabaseTables(List<Type> tables, CreateFlags tableCreateFlags)
        {
            foreach (Type item in tables)
                try
                {
                    await SqlCon.CreateTableAsync(item, tableCreateFlags).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }

        public async Task<List<SQLiteConnection.ColumnInfo>> GetTableInfo(string tableName)
        {
            return await SqlCon.GetTableInfoAsync(tableName);
        }


        public async Task<T> GetOne<T>(Expression<Func<T, bool>> query) where T : class, new()
        {
            try
            {
                return await SqlCon.Table<T>().Where(query).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetAll<T>(Expression<Func<T, bool>> query) where T : new()
        {
            try
            {
                return await SqlCon.Table<T>().Where(query).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<T>> GetAll<T>() where T : class, new()
        {
            return await SqlCon.GetAllWithChildrenAsync<T>().ConfigureAwait(false);
        }

        public async Task<int> Insert(object item)
        {
            try
            {
                return await SqlCon.InsertAsync(item).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<int> InsertAll<T>(List<T> items) where T : new()
        {
            return await SqlCon.InsertAllAsync(items).ConfigureAwait(false);
        }


        public async Task<int> InsertOrReplaceOne<T>(object item) where T : new()
        {
            return await SqlCon.InsertOrReplaceAsync(item);
        }


        public async Task InsertOrReplaceAll<T>(List<T> items) where T : new()
        {
            await SqlCon.InsertOrReplaceAllWithChildrenAsync(items);
        }


        public async Task<int> Update(object item)
        {
            return await SqlCon.UpdateAsync(item).ConfigureAwait(false);
        }


        public async Task<int> UpdateAll<T>(List<T> items) where T : new()
        {
            return await SqlCon.UpdateAllAsync(items).ConfigureAwait(false);
        }


        public async Task<int> Delete(object item)
        {
            return await SqlCon.DeleteAsync(item).ConfigureAwait(false);
        }


        public async Task<int> DeleteAll<T>() where T : new()
        {
            return await SqlCon.DeleteAllAsync<T>().ConfigureAwait(false);
        }


        public async Task DeleteAll<T>(List<T> items) where T : new()
        {
            await SqlCon.DeleteAllAsync(items);
        }


        public async Task<IList<T>> QueryString<T>(string sql) where T : class, new()
        {
            return await SqlCon.QueryAsync<T>(sql);
        }


        public async Task<int> DropTableAsync<T>() where T : new()
        {
            return await SqlCon.DropTableAsync<T>();
        }


        public async Task<int> ExecuteAsync(string sqlQuery)
        {
            try
            {
                return await SqlCon.ExecuteAsync(sqlQuery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}