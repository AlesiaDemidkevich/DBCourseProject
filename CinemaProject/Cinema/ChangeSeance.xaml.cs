using Cinema.Connection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Логика взаимодействия для ChangeSeance.xaml
    /// </summary>
    public partial class ChangeSeance : Window
    {
        BindingList<string> HallNameList;
        BindingList<string> FilmNameList;
        BindingList<string> SeanceTimeList;
        Seance seance;
        public ChangeSeance(Seance seance,BindingList<string> nameHallList, BindingList<string> nameFilmList, BindingList<string> seanceTimeList)
        {
            InitializeComponent();
            this.seance = seance;
            HallNameList = nameHallList;
            seanceHall.ItemsSource = HallNameList;
            FilmNameList = nameFilmList;
            seanceFilm.ItemsSource = FilmNameList;
            SeanceTimeList = seanceTimeList;
            seanceTime.ItemsSource = SeanceTimeList;
            seanceDate.Text = seance.seanceDate;
            seanceTime.SelectedItem = seance.seanceTime;
            seanceHall.SelectedItem = seance.seanceHall;
            seanceFilm.SelectedItem = seance.seanceFilm.Name;
            
        }

        private void seanceSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (seanceDate.SelectedDate == null || seanceTime.Text == null || seanceHall.Text == null || seanceFilm.Text == null)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    changeSeance();
                    this.Owner.Activate();
                    this.Close();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void changeSeance()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter id_in = new OracleParameter
                    {
                        ParameterName = "ID_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = seance.seanceID
                    };
                    OracleParameter date_in = new OracleParameter
                    {
                        ParameterName = "Date_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = seanceDate.SelectedDate
                    };
                    OracleParameter time_in = new OracleParameter
                    {
                        ParameterName = "Time_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = seanceTime.Text
                    };
                    OracleParameter hall_in = new OracleParameter
                    {
                        ParameterName = "Hall_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = seanceHall.Text
                    };
                    OracleParameter film_in = new OracleParameter
                    {
                        ParameterName = "Film_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = seanceFilm.Text
                    };
                    using (OracleCommand command = new OracleCommand("updateSeance"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { id_in, date_in, time_in, hall_in, film_in });
                        command.ExecuteNonQuery();
                        MessageBox.Show("Сеанс изменен!");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
