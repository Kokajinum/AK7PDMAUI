using AK7PDMAUI.Models;
using AK7PDMAUI.Resources.Resx;
using MongoDB.Bson;
using MongoDB.Driver;

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

                //List<string> databases = MongoClient.ListDatabaseNames().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                await UserCollection.InsertOneAsync(user);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            return false;
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
                userBooks = await UserBookCollection.Find(x => x.UserId == user.Id.ToString()).ToListAsync();

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

        public async Task<List<Book>> GetBooksAsync()
        {
            List<Book> books = new List<Book>();
            try
            {
                books = await BookCollection.Find(new BsonDocument()).ToListAsync();
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
                //books = await BookCollection.Find
                //    (x => userBooks.Find(y => x.Id.ToString() == y.BookId) != null).ToListAsync();
                books = BookCollection.AsQueryable()
                    .Where(x => userBooks.Any(y => x.Id.ToString() == y.BookId && y.IsActive)).ToList();
                return books;
            }
            catch(Exception)
            {
                throw;
            }
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
            catch(Exception)
            {
                throw;
            }
        }

    }
}
