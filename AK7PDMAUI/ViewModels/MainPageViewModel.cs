using AK7PDMAUI.Models;
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

        public MainPageViewModel(RepositoryService repository)
        {
            Repository = repository;
            RefreshUserBooksCommand = new Command(async () => await RefreshUserBooks());
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
    }
}
