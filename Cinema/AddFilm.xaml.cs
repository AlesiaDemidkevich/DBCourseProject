﻿using System;
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
    /// Логика взаимодействия для AddFilm.xaml
    /// </summary>
    public partial class AddFilm : Window
    {
        BindingList<string> GenreNameList;
        public AddFilm(BindingList<string> genreNameList)
        {
            InitializeComponent();
            GenreNameList = genreNameList;
            filmGenre.ItemsSource = GenreNameList;
        }
    }
}
