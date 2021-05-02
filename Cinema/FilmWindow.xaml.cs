using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cinema
{
    /// <summary>
    /// Логика взаимодействия для FilmWindow.xaml
    /// </summary>
    public partial class FilmWindow : Window
    {        
        Film film;
        public FilmWindow(Film film)
        {            
            InitializeComponent();
            this.film = film;
            name.Text = film.Name;
            year.Text = film.Year;
            genre.Text = film.Genre;
            duration.Text = film.Duration.Substring(0, film.Duration.IndexOf('.')) + " ч " + film.Duration.Substring(film.Duration.IndexOf('.') + 1, 2) + " мин";
            ageLimit.Text = film.AgeLimit + " лет";
            description.Text = film.Description;

            MemoryStream strIm = new MemoryStream(film.Imagine);
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.StreamSource = strIm;
            myBitmapImage.DecodePixelWidth = 270;
            myBitmapImage.DecodePixelHeight = 250;
            myBitmapImage.EndInit();
            img.Source = myBitmapImage;
        }
    }
}
