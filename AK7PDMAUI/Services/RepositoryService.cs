using AK7PDMAUI.Managers;
using AK7PDMAUI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK7PDMAUI.Services
{
    public class RepositoryService
    {
        public MongoDBManager DBManager { get; set; }

        public User LoggedUser { get; set; }

        public bool IsAdmin
        {
            get => LoggedUser.IsAdmin;
        }

        public bool IsNotAdmin
        {
            get => !LoggedUser.IsAdmin;
        }

        public List<Book> Books { get; set; }

        public List<UserBook> UserBooks { get; set; }

        public List<Book> BorrowedBooks { get; set; }

        public Book SelectedBook { get; set; }

        public RepositoryService()
        {
            DBManager = new MongoDBManager
                ("mongodb+srv://utb:barbari@knihovna.cmf7vc6.mongodb.net/?retryWrites=true&w=majority");
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            List<Book> books = await DBManager.GetBooksAsync();
            Books = books;
            return books;
        }

        public async Task<List<Book>> GetBorrowedBooksAsync(List<UserBook> userBooks)
        {
            List<Book> borrowedBooks = await DBManager.GetBooksAsync(userBooks);
            BorrowedBooks = borrowedBooks;
            return borrowedBooks;
        }


        public async Task<Book> GetBookByIDAsync(Book book)
        {
            Book freshBook = await DBManager.GetBookByIDAsync(book);
            return freshBook;
        }



        public async Task<List<UserBook>> GetUserBooksAsync(User user)
        {
            List<UserBook> userBooks = await DBManager.GetUserBooksAsync(user);
            UserBooks = userBooks;
            return userBooks;
        }



        public async Task LoginAsync(string username, string password)
        {
            User user = await DBManager.LoginUserAsync(username, password);
            LoggedUser = user;
        }

        public async Task RegisterAsync(User user)
        {
            await DBManager.CreateUserAsync(user);
        }

        public async Task<bool> CheckIfUserExistsAsync(string login)
        {
            return await DBManager.CheckIfUserExists(login);
        }


        public async Task UpdateBookBorrowCountAsync(Book book)
        {
            await DBManager.UpdateBookBorrowCountAsync(book);
        }




        public async Task CreateUserBookAsync(Book book)
        {
            UserBook userBook = new()
            {
                BookId = book.Id,
                UserId = LoggedUser.Id,
                BorrowingDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(6),
                IsActive = true
            };
            await DBManager.CreateUserBookAsync(userBook);
            UserBooks.Add(userBook);
        }

        public async Task ReturnUserBookAsync(UserBook userBook, Book originalBook, UserBook originalUserBook)
        {
            await DBManager.ReturnUserBookAsync(userBook);
            BorrowedBooks.Remove(originalBook);
            UserBooks.Remove(originalUserBook);
        }

        #region admin

        public async Task CreateBookAsync(Book book)
        {
            await DBManager.CreateBookAsync(book);
        }

        public async Task DeleteBookAsync(Book book)
        {
            await DBManager.DeleteBookAsync(book);
            Books.Remove(book);
        }

        public async Task ReplaceBookAsync(Book book)
        {
            await DBManager.ReplaceBookAsync(book);
        }

        public async Task<List<Book>> SearchBooks(string searchText)
        {
            return await DBManager.SearchBooks(searchText);
        }

        #endregion
    }
}
