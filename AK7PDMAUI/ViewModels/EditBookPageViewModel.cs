using AK7PDMAUI.Models;
using AK7PDMAUI.Resources.Resx;
using AK7PDMAUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AK7PDMAUI.ViewModels
{
    public class EditBookPageViewModel : BaseViewModel
    {
        public RepositoryService Repository { get; set; }

        private string _bookTitle;
        public string BookTitle
        {
            get => _bookTitle;
            set => SetProperty(ref _bookTitle, value);
        }

        private int _bookRelease;
        public int BookRelease
        {
            get => _bookRelease;
            set => SetProperty(ref _bookRelease, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private int _availableLicense;
        public int AvailableLicense
        {
            get => _availableLicense;
            set => SetProperty(ref _availableLicense, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private int _pages;
        public int Pages
        {
            get => _pages;
            set => SetProperty(ref _pages, value);
        }

        public EditBookPageViewModel(RepositoryService repository)
        {
            Repository = repository;
            
            RefreshEditedBookCommand = new Command(() => RefreshEditedBook());
            EditBookCommand = new Command(async () => await EditBook());
        }

        private void RefreshEditedBook()
        {
            //do EditBookPage by se v jinem pripade ani nemelo vstupovat, ale pro jistotu
            if (Repository.SelectedBook is not null)
            {
                Book selected = Repository.SelectedBook;
                BookTitle = selected.Title;
                BookRelease = selected.Year;
                FirstName = selected.Author.FirstName;
                LastName = selected.Author.LastName;
                AvailableLicense = selected.TotalCount;
                ImageUrl = selected.ImgUrl;
                Pages = selected.Pages;
            }
            else
            {
                Shell.Current.GoToAsync("..");
            }
        }

        private async Task EditBook()
        {
            try
            {
                IsBusy = true;
                if (string.IsNullOrWhiteSpace(BookTitle) ||
                    string.IsNullOrWhiteSpace(FirstName) ||
                    string.IsNullOrWhiteSpace(ImageUrl) ||
                    BookRelease <= 0 ||
                    AvailableLicense < 0)
                {
                    IsBusy = false;
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, "Je třeba vyplnit všechna pole!", AppResources.OK);
                }
                else
                {
                    Book book = new Book()
                    {
                        Id = Repository.SelectedBook.Id,
                        BorrowedCount = Repository.SelectedBook.BorrowedCount,
                        Title = BookTitle,
                        Year = BookRelease,
                        Author = new Author()
                        {
                            FirstName = FirstName,
                            LastName = LastName,
                        },
                        ImgUrl = ImageUrl,
                        TotalCount = AvailableLicense,
                        Pages = Pages
                    };
                    await Repository.ReplaceBookAsync(book);
                    IsBusy = false;
                    await Shell.Current.DisplayAlert("Oznámení", "Uložení knihy proběhlo v pořádku.", AppResources.OK);
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                await Shell.Current.DisplayAlert(AppResources.Error, "Při ukládání knihy došlo k chybě.", AppResources.OK);
            }
        }

        public ICommand EditBookCommand { get; set; }
        public ICommand RefreshEditedBookCommand { get; set; }
    }
}
