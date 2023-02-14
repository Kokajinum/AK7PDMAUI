using AK7PDMAUI.Models;
using AK7PDMAUI.Resources.Resx;
using AK7PDMAUI.Services;
using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AK7PDMAUI.ViewModels
{
    public class CatalogPageViewModel : BaseViewModel
    {
        public RepositoryService Repository { get; set; }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get => _books;
            set { SetProperty(ref _books, value); }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set { SetProperty(ref _isRunning, value); }
        }


        public CatalogPageViewModel(RepositoryService repository)
        {
            Repository = repository;
            RefreshBooksCommand = new Command(async () => await RefreshBooks());
            ShowCreateBookCommand = new Command(async () => await ShowCreateBook());
            BorrowBookCommand = new Command<Book>(async (book) => await BorrowBook(book));
            DeleteBookCommand = new Command<Book>(async (book) => await DeleteBook(book));
        }

        private async Task DeleteBook(Book book)
        {
            try
            {
                IsBusy = true;
                Book freshBook = await Repository.GetBookByIDAsync(book);
                if (freshBook.BorrowedCount != 0)
                {
                    await Shell.Current.DisplayAlert
                        (AppResources.Error, "Nelze odstranit knihu, kterou má někdo vypůjčenou!", AppResources.OK);
                    IsBusy = false;
                    return;
                }
                await Repository.DeleteBookAsync(freshBook);
                Books.Remove(book);
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert
                        (AppResources.Error, "Odstranění knihy se nepodařilo.", AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task BorrowBook(Book book)
        {
            try
            {
                IsBusy = true;
                if (Repository.UserBooks.Find
                    (x => x.BookId == book.Id.ToString() && 
                    x.UserId == Repository.LoggedUser.Id.ToString()) != null)
                {
                    await Shell.Current.DisplayAlert
                        (AppResources.Error, "Knihu již máte vypůjčenou!", AppResources.OK);
                    return;
                }
                if (Repository.UserBooks.Count >= 6)
                {
                    await Shell.Current.DisplayAlert
                        (AppResources.Error, "Máte vypůjčené maximální množství knih!", AppResources.OK);
                }
                Book freshBook = await Repository.GetBookByIDAsync(book);
                if (freshBook.TotalCount > 0)
                {
                    freshBook.BorrowedCount += 1;
                    await Repository.UpdateBookBorrowCountAsync(freshBook);
                    await Repository.CreateUserBookAsync(freshBook);
                    await Shell.Current.DisplayAlert("Oznámení", "Přidání knihy proběhlo v pořádku.", AppResources.OK);
                }
                else
                {
                    await Shell.Current.DisplayAlert
                        (AppResources.Error, "Bylo dosaženo maximálního počtu vypůjčených licencí.", AppResources.OK);
                }
                
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert
                        (AppResources.Error, "Vypůjčení se nezdařilo!", AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ShowCreateBook()
        {
            try
            {
                await Shell.Current.GoToAsync("/CreateBookPage");
            }
            catch (Exception)
            {
                IsBusy = false;
            }
        }

        private async Task RefreshBooks()
        {
            try
            {
                IsBusy = true;
                List<Book> books = await Repository.GetBooksAsync();
                Books = new(books);
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
            }

        }

        public ICommand RefreshBooksCommand { get; set; }
        public ICommand ShowCreateBookCommand { get; set; }
        public ICommand BorrowBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
    }
}
