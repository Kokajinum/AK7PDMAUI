using AK7PDMAUI.Managers;
using AK7PDMAUI.Models;

namespace AK7PDMAUI.ViewModels
{
    public class GlobalViewModel
    {
        public MongoDBManager DBManager { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public User LoggedUser { get; set; }
        public bool IsAdmin { get; set; }

        public List<Book> Books { get; set; }

        public GlobalViewModel()
        {
            DBManager = new MongoDBManager("mongodb+srv://utb:barbari@knihovna.cmf7vc6.mongodb.net/?retryWrites=true&w=majority");
        }

        public async Task LoginAsync(string username, string password)
        {
            User user = await DBManager.LoginUserAsync(username, password);
            LoggedUser = user;
            IsAdmin = user.IsAdmin;
        }

        public async Task<List<Book>> DownloadBooks()
        {
            List<Book> books = await DBManager.GetBooksAsync();
            return books;
        }

    }

    
}
