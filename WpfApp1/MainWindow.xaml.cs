using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1;
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private List<string> comboBox2 = [];
    private List<Book> books;
    int s = 0;
    public List<string> ComboBox1 { get; set; } = ["Author", "Themes", "Category"];
    public List<string> ComboBox2 { get => comboBox2; set { comboBox2 = value; OnPropertyChanged(); } }
    public List<Book> Books { get => books; set { books = value; OnPropertyChanged(); } }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox2 = new();
        using LibraryContext db = new LibraryContext();
        var cb = sender as ComboBox;
        if (cb?.SelectedItem.ToString() is "Author")
        {
            s = 1;
            ComboBox2 = (from a in db.Authors
                         select a.FirstName + ' ' + a.LastName).ToList();
        }
        else if (cb?.SelectedItem.ToString() is "Themes")
        {
            s = 2;
            ComboBox2 = (from t in db.Themes
                         select t.Name).ToList();
        }
        else
        {
            s = 3;
            ComboBox2 = (from c in db.Categories
                         select c.Name).ToList();
        }
    }

    private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        Books = new();
        using LibraryContext db = new LibraryContext();
        var cb = sender as ComboBox;
        if (cb is null || cb.SelectedItem is null) return;
        if (s == 1)
        {
            var aut = db.Authors.FirstOrDefault(a => a.FirstName + ' ' + a.LastName == cb.SelectedItem);
            if (aut is null) return;
            Books = db.Books.Where(b => b.IdAuthor == aut.Id).ToList();
        }
        else if (s == 2)
        {
            var theme = db.Themes.FirstOrDefault(t => t.Name == cb.SelectedItem);
            if (theme is null) return;
            Books=db.Books.Where(b=>b.IdThemes==theme.Id).ToList();
        }

        else
        {
            var category=db.Categories.FirstOrDefault(c=>c.Name==cb.SelectedItem);
            if (category is null) return;
            Books=db.Books.Where(b=>b.IdCategory==category.Id).ToList();
        }
    }
}