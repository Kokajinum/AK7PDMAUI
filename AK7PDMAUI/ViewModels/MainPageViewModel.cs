using AK7PDMAUI.Models;
using AK7PDMAUI.Resources.Resx;
using AK7PDMAUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AK7PDMAUI.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public RepositoryService Repository { get; set; }

        private ObservableCollection<UserBook> _userBooks;
        public ObservableCollection<UserBook> UserBooks
        {
            get => _userBooks;
            set => SetProperty(ref _userBooks, value);
        }

        private ObservableCollection<Book> _borrowedBooks;
        public ObservableCollection<Book> BorrowedBooks
        {
            get => _borrowedBooks;
            set => SetProperty(ref _borrowedBooks, value);
        }

        private bool _noContent;
        public bool NoContent
        {
            get => _noContent;
            set => SetProperty(ref _noContent, value);
        }

        public MainPageViewModel(RepositoryService repository)
        {
            Repository = repository;
            RefreshUserBooksCommand = new Command(async () => await RefreshUserBooks());
            ReturnBookCommand = new Command<Book>(async (book) => await ReturnBook(book));
        }

        private async Task ReturnBook(Book book)
        {
            try
            {
                IsBusy = true;
                UserBook selectedUserBook = UserBooks.Where(x => x.BookId == book.Id).First();
                UserBook originalUserBook = UserBooks.Where(x => x.BookId == book.Id).First();
                Book originalBook = BorrowedBooks.Where(x => x.Id == originalUserBook.BookId).First();
                selectedUserBook.IsActive = false;
                await Repository.ReturnUserBookAsync(selectedUserBook, originalBook, originalUserBook);
                UserBooks.Remove(originalUserBook);
                BorrowedBooks.Remove(originalBook);
                IsBusy = false;
                await Shell.Current.DisplayAlert("Oznámení", "Vrácení knihy proběhlo úspěšně.", AppResources.OK);
            }
            catch (Exception)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert(AppResources.Error, "Při vrácení knihy došlo k chybě.", AppResources.OK);
            }
        }

        private async Task RefreshUserBooks()
        {
            try
            {
                IsBusy = true;
                List<UserBook> userBooks = await Repository.GetUserBooksAsync(Repository.LoggedUser);
                UserBooks = new(userBooks);
                List<Book> borrowedBooks = await Repository.GetBorrowedBooksAsync(userBooks);
                BorrowedBooks = new(borrowedBooks);

            }
            catch(Exception)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand RefreshUserBooksCommand { get; set; }
        public ICommand ReturnBookCommand { get; set; }
    }
}
