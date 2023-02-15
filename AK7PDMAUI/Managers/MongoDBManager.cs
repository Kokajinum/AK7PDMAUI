using AK7PDMAUI.Extensions;
using AK7PDMAUI.Models;
using AK7PDMAUI.Resources.Resx;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AK7PDMAUI.Managers
{
    public class MongoDBManager
    {
        public MongoClient MongoClient { get; set; }

        public IMongoDatabase CurrentDatabase { get; set; }

        public IMongoCollection<User> UserCollection { get; set; }

        public IMongoCollection<Book> BookCollection { get; set; }

        public IMongoCollection<UserBook> UserBookCollection { get; set; }

        public MongoDBManager(string connectionString)
        {
            try
            {
                MongoClient = new MongoClient(connectionString);
                CurrentDatabase = MongoClient.GetDatabase("DB-Knihovna");
                UserCollection = CurrentDatabase.GetCollection<User>("User");
                BookCollection = CurrentDatabase.GetCollection<Book>("Book");
                UserBookCollection = CurrentDatabase.GetCollection<UserBook>("UserBook");

                var indexKeysDefinition = Builders<Book>.IndexKeys.Text(x => x.Title);
                var createIndexModel = new CreateIndexModel<Book>(indexKeysDefinition);
                BookCollection.Indexes.CreateOne(createIndexModel);

                //List<string> databases = MongoClient.ListDatabaseNames().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfUserExists(string login)
        {
            return await UserCollection.Find(x => x.Login == login).CountDocumentsAsync() != 0;
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                if (UserCollection.Find(x => x.Login == user.Login).CountDocuments() != 0)
                {
                    throw new Exception("Účet se zadaným uživatelským jménem již existuje!");
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await UserCollection.InsertOneAsync(user);
            }
            catch (Exception e)
            {
                throw new Exception("Účet se nepodařilo vytvořit.", e);
            }
        }

        public async Task<User> LoginUserAsync(string login, string password)
        {
            try
            {
                User user = await UserCollection.Find(x => x.Login == login).FirstAsync();
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
                throw new Exception(AppResources.WrongUsernamePassword);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<UserBook>> GetUserBooksAsync(User user)
        {
            List<UserBook> userBooks = new List<UserBook>();
            try
            {
                userBooks = await UserBookCollection.Find(x => x.UserId == user.Id && x.IsActive).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return userBooks;
        }

        public async Task CreateUserBookAsync(UserBook userBook)
        {
            try
            {
                await UserBookCollection.InsertOneAsync(userBook);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ReturnUserBookAsync(UserBook userBook)
        {
            try
            {
                var filter = Builders<UserBook>.Filter.Eq("_id", userBook.Id);
                var update = Builders<UserBook>.Update.Set("active", userBook.IsActive);
                await UserBookCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteUserBookAsync(UserBook userBook)
        {
            try
            {
                await UserBookCollection.DeleteOneAsync(x => x.Id == userBook.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Book>> GetBooksAsync(int limit = 50)
        {
            List<Book> books = new List<Book>();
            try
            {
                books = await BookCollection
                    .Find(new BsonDocument())
                    .Limit(50)
                    .SortByDescending(x => x.BorrowedCount)
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return books;
        }

        public async Task<List<Book>> GetBooksAsync(List<UserBook> userBooks)
        {
            try
            {
                List<Book> books = new();
                books = BookCollection
                    .AsQueryable()
                    .Where(x => userBooks.Any(y => x.Id == y.BookId && y.IsActive))
                    .ToList();
                return books;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Book>> SearchBooks(string searchText)
        {
            try
            {
                List<Book> books = new();
                searchText = searchText.ToLower();
                //searchText = RemoveDiacritics(searchText);
                //books = BookCollection
                //    .AsQueryable()
                //    .WhereText(searchText)
                //    .Take(10)
                //    .ToList();
                books = BookCollection
                     .AsQueryable()
                     .Where(x => x.Title.ToLower().Contains(searchText))
                     .ToList();
                return books;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //zdroj https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }


        public async Task CreateBookAsync(Book book)
        {
            try
            {
                await BookCollection.InsertOneAsync(book);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBookAsync(Book book)
        {
            try
            {
                await BookCollection.DeleteOneAsync(x => x.Id == book.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ReplaceBookAsync(Book book)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", book.Id);
                await BookCollection.ReplaceOneAsync(filter, book);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateBookBorrowCountAsync(Book book)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", book.Id);
                var update = Builders<Book>.Update.Set("borrowed_count", book.BorrowedCount);
                await BookCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book> GetBookByIDAsync(Book book)
        {
            try
            {
                var filter = Builders<Book>.Filter.Eq("_id", book.Id);
                var books = await BookCollection.FindAsync(filter);
                return books.First();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
