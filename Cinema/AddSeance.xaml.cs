using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для AddSeance.xaml
    /// </summary>
    public partial class AddSeance : Window
    {
        BindingList<string> HallNameList;
        BindingList<string> FilmNameList;
        BindingList<string> SeanceTimeList;
        public AddSeance(BindingList<string> nameHallList, BindingList<string> nameFilmList, BindingList<string> seanceTimeList)
        {
            InitializeComponent();
            HallNameList = nameHallList;
            seanceHall.ItemsSource = HallNameList;
            FilmNameList = nameFilmList;
            seanceFilm.ItemsSource = FilmNameList;
            SeanceTimeList = seanceTimeList;
            seanceTime.ItemsSource = SeanceTimeList;
        }
    }
}
