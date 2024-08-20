using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADizErp
{
    public class LocalDbService
    {
        private const string DB_NAME = "app_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;
        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<UserProfileImage>();
        }

        public async Task<UserProfileImage> GetByUsername(string username)
        {
            return await _connection.Table<UserProfileImage>()
                .Where(x => x.UserName == username)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task Create(UserProfileImage profileImage)
        {
            await _connection.InsertAsync(profileImage);
        }

        public async Task Update(UserProfileImage profileImage)
        {
            await _connection.UpdateAsync(profileImage);
        }

        public async Task Delete(UserProfileImage profileImage)
        {
            await _connection.DeleteAsync(profileImage);
        }

    }
}
