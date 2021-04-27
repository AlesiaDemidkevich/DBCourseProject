using System;
using System.Collections.Generic;
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

namespace Cinema.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        int userID;
        public AdminWindow()
        {
            InitializeComponent();
        }

        public AdminWindow(int ID)
        {
            InitializeComponent();
            userID = ID;

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Authorization autorization = new Authorization();            
            autorization.Show();
            this.Close();
        }

        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.Show();
        }

        private void updateSeanceList_Click(object sender, RoutedEventArgs e)
        {
            AddSeance addSeance = new AddSeance();
            addSeance.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddFilm addFilm = new AddFilm();
            addFilm.Show();
        }
    }
}
