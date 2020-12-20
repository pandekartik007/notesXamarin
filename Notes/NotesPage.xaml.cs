using System;
using Xamarin.Forms;
using Notes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Notes
{
    public class StringToColorConverter : IValueConverter
    {

        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return Color.Green;
            return Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public partial class NotesPage : ContentPage
    {
        List<Note> list;
        public NotesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            list = await App.Database.GetNotesAsync();
            listView.ItemsSource = await App.Database.GetNotesAsync();
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteEntryPage
            {
                BindingContext = new Note()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new NoteEntryPage
                {
                    BindingContext = e.SelectedItem as Note
                });
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listView.BeginRefresh();
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                listView.ItemsSource = list;
            else
                listView.ItemsSource = list.FindAll(i => i.Text.Contains(e.NewTextValue));    //.Where(i => i.Name.Contains(e.NewTextValue));

            listView.EndRefresh();
        }
        
    }
}
