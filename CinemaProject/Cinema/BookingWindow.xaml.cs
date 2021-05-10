using Cinema.Connection;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Image = System.Drawing.Image;

namespace Cinema
{
    /// <summary>
    /// Логика взаимодействия для BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        public static BindingList<Ticket> allTicketList = new BindingList<Ticket>();
        public static BindingList<Ticket> sortTicketList = new BindingList<Ticket>();
        public static BindingList<Film> allFilmList = new BindingList<Film>();
        public static BindingList<Film> sortFilmList = new BindingList<Film>();
        public static BindingList<string> FilmNameList = new BindingList<string>();
        public static BindingList<Seance> allSeanceList = new BindingList<Seance>();
        public static BindingList<Seance> sortSeanceList = new BindingList<Seance>();
        public static BindingList<string> SeanceTimeList = new BindingList<string>();
        public static BindingList<Hall> allHallList = new BindingList<Hall>();
        public static BindingList<string> HallNameList = new BindingList<string>();
        public static BindingList<int> RawsList = new BindingList<int>();
        public static BindingList<int> PlaceList = new BindingList<int>();
        int userID;
        public BookingWindow()
        {
            InitializeComponent();
            GetTicketInfo();
            ticketTable.ItemsSource = allTicketList;
            GetFilm();
            ticketFilmList.ItemsSource = FilmNameList;
            GetSeance();
            ticketTimeList.ItemsSource = SeanceTimeList;
            GetHall();
            ticketHallList.ItemsSource = HallNameList;
            ticketEmployeeEnter.Content = "ФИО";
        }
        public BookingWindow(int ID, string FIO)
        {
            InitializeComponent();
            userID = ID;
            fio.Content = FIO;
            ticketEmployeeEnter.Content = FIO;
            GetTicketInfo();
            ticketTable.ItemsSource = allTicketList;
            GetFilm();
            ticketFilmList.ItemsSource = FilmNameList;
            GetSeance();
            ticketTimeList.ItemsSource = SeanceTimeList;
            GetHall();
            ticketHallList.ItemsSource = HallNameList;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Authorization autorization = new Authorization();
            autorization.Show();
            this.Close();
        }

        public void GetSeance()
        {
            allSeanceList.Clear();
            Seance cur_seance;
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getSeanceList"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string date = Convert.ToDateTime(row["DateSeance"]).ToString("D");
                            DateTime date2 = Convert.ToDateTime(row["DateSeance"]);
                            string time = row["TimeSeance"].ToString();
                            string hall = row["hallName"].ToString();
                            string filmName = row["FilmName"].ToString();
                            int seanceID = Convert.ToInt32(row["ID"]);
                            Film film = new Film(filmName);
                            string capacity = row["HallCapacity"].ToString();
                            cur_seance = new Seance(seanceID, date, date2, time, hall, film, capacity);
                            allSeanceList.Add(cur_seance);

                            if (SeanceTimeList.Contains(time))
                            {

                            }
                            else
                            {
                                SeanceTimeList.Add(time);
                            }
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetHall()
        {
            Hall hall;
            allHallList.Clear();
            HallNameList.Clear();

            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getHallList"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row["Name"].ToString();
                            string capacity = row["Capacity"].ToString();

                            hall = new Hall(name, capacity);
                            allHallList.Add(hall);
                            HallNameList.Add(name);

                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetFilm()
        {
            allFilmList.Clear();
            FilmNameList.Clear();

            Film cur_film;
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getFilmInfo"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            int id = Convert.ToInt32(row["ID"]);
                            string name = row["Name"].ToString();
                            string genre = row["genre"].ToString();
                            string year = row["year"].ToString();
                            string duration = row["duration"].ToString();
                            string ageLimit = row["agelimit"].ToString();
                            string description = row["description"].ToString();
                            string startRelease = Convert.ToDateTime(row["startrelease"]).ToString("D");
                            string endRelease = Convert.ToDateTime(row["endrelease"]).ToString("D");
                            byte[] img = (byte[])(row["img"]);

                            cur_film = new Film(id, name, genre, year, duration, ageLimit, startRelease, endRelease, img, description);
                            allFilmList.Add(cur_film);
                            FilmNameList.Add(name);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addTicketButton_Click(object sender, RoutedEventArgs e)
        {
            SellTicket();
            GetTicketInfo();
            ticketTable.ItemsSource = allTicketList;
            PrintTicketInfo();

            ticketDateChoose.Text = "";
            ticketTimeList.Text = "";
            ticketHallList.Text = "";
            ticketFilmList.Text = "";
            Raw.Text = "";
            Place.Text = "";
        }

        public void GetTicketInfo()
        {
            allTicketList.Clear();

            Ticket cur_ticket;
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getTicketInfo"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            int id = Convert.ToInt32(row["ID"]);
                            int number = Convert.ToInt32(row["ticketNumber"]);
                            int cost = Convert.ToInt32(row["Cost"]);
                            string dateSeance = Convert.ToDateTime(row["DateSeance"]).ToString("D");
                            string timeSeance = row["TimeSeance"].ToString();
                            string hallPlace = row["Hall"].ToString();
                            int rowPlace = Convert.ToInt32(row["RawNumber"]);
                            int placeNumber = Convert.ToInt32(row["Place"]);
                            string typePlace = (row["typePlace"]).ToString();
                            string filmName = (row["FilmName"]).ToString();
                            string filmDuration = (row["FilmDuration"]).ToString();
                            string filmLimit = (row["FilmLimit"]).ToString();

                            Hall hall = new Hall(hallPlace);
                            Film film = new Film(filmName, filmDuration, filmLimit);
                            Place place = new Place(rowPlace, placeNumber, hall, typePlace, cost);
                            Seance seance = new Seance(dateSeance, timeSeance, film);


                            cur_ticket = new Ticket(id, number, seance, place, fio.Content.ToString());
                            allTicketList.Add(cur_ticket);
                            ;
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SellTicket()
        {

            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter date_in = new OracleParameter
                    {
                        ParameterName = "Date_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = ticketDateChoose.SelectedDate
                    };
                    OracleParameter time_in = new OracleParameter
                    {
                        ParameterName = "Time_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketTimeList.SelectedItem.ToString()
                    };
                    OracleParameter hall_in = new OracleParameter
                    {
                        ParameterName = "Hall_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketHallList.SelectedItem.ToString()
                    };
                    OracleParameter film_in = new OracleParameter
                    {
                        ParameterName = "Film_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketFilmList.SelectedItem.ToString()
                    };
                    OracleParameter row_in = new OracleParameter
                    {
                        ParameterName = "Row_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(Raw.SelectedItem)
                    };
                    OracleParameter place_in = new OracleParameter
                    {
                        ParameterName = "Place_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(Place.SelectedItem)
                    };
                    OracleParameter emp_in = new OracleParameter
                    {
                        ParameterName = "Employee_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(userID)
                    };
                    using (OracleCommand command = new OracleCommand("admin.sellTicket"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { date_in, time_in, hall_in, film_in, row_in, place_in, emp_in });
                        command.ExecuteNonQuery();
                        MessageBox.Show("Билет продан успешно!");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void PrintTicketInfo()
        {
            Ticket curr_ticket = allTicketList[allTicketList.Count-1];
            ticketNumberEnter.Content = curr_ticket.Number.ToString();
            ticketCostEnter.Content = curr_ticket.Cost.ToString();
            ticketDateEnter.Content = Convert.ToDateTime(curr_ticket.seance.seanceDate).ToString("D");
            ticketTimeEnter.Content = curr_ticket.seance.seanceTime;
            ticketHallEnter.Content = curr_ticket.place.Hall.Name.ToString();
            ticketRawEnter.Content = curr_ticket.place.Raw.ToString();
            ticketPlaceEnter.Content = curr_ticket.place.Number.ToString();
            ticketTypeEnter.Content = curr_ticket.place.Type.ToString();
            ticketFilmEnter.Content = curr_ticket.seance.seanceFilm.Name.ToString();
            ticketDurationEnter.Content = curr_ticket.seance.seanceFilm.Duration.Substring(0, curr_ticket.seance.seanceFilm.Duration.IndexOf('.')) + " ч " + curr_ticket.seance.seanceFilm.Duration.Substring(curr_ticket.seance.seanceFilm.Duration.IndexOf('.') + 1, 2) + " мин"; ;
            ticketLimitEnter.Content = curr_ticket.seance.seanceFilm.AgeLimit + " лет";
            ticketEmployeeEnter.Content = fio.Content;
        }

        public void GetFilmWithDate()
        {
            FilmNameList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter Date_in = new OracleParameter
                    {
                        ParameterName = "Date_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = ticketDateChoose.SelectedDate
                    };
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getFilmWithDate"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { Date_in, us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row["Name"].ToString();
                            FilmNameList.Add(name);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetFilmWithTime()
        {
            FilmNameList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter Date_in = new OracleParameter
                    {
                        ParameterName = "Date_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = ticketDateChoose.SelectedDate
                    };
                    OracleParameter Time_in = new OracleParameter
                    {
                        ParameterName = "Time_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketTimeList.SelectedItem.ToString()
                    };
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getFilmWithTime"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { Date_in, Time_in, us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row["Name"].ToString();
                            FilmNameList.Add(name);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetFilmWithHall()
        {
            FilmNameList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter Date_in = new OracleParameter
                    {
                        ParameterName = "Date_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = ticketDateChoose.SelectedDate
                    };
                    OracleParameter Time_in = new OracleParameter
                    {
                        ParameterName = "Time_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketTimeList.SelectedItem.ToString()
                    };
                    OracleParameter Hall_in = new OracleParameter
                    {
                        ParameterName = "Hall_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketHallList.SelectedItem.ToString()
                    };
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getFilmWithHall"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { Date_in, Time_in, Hall_in, us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row["Name"].ToString();
                            FilmNameList.Add(name);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetFilmWithParameter()
        {
            try
            {
                if (ticketDateChoose.SelectedDate == null && ticketTimeList.SelectedItem == null && ticketHallList.SelectedItem == null)
                {
                    GetFilm();
                    ticketFilmList.ItemsSource = FilmNameList;
                }
                else if (ticketDateChoose.SelectedDate != null && ticketTimeList.SelectedItem == null && ticketHallList.SelectedItem == null)
                {
                    GetFilmWithDate();
                    ticketFilmList.ItemsSource = FilmNameList;
                }
                else if (ticketDateChoose.SelectedDate != null && ticketTimeList.SelectedItem != null && ticketHallList.SelectedItem == null)
                {
                    GetFilmWithTime();
                    ticketFilmList.ItemsSource = FilmNameList;
                }
                else if (ticketDateChoose.SelectedDate != null && ticketTimeList.SelectedItem != null && ticketHallList.SelectedItem != null)
                {
                    GetFilmWithHall();
                    ticketFilmList.ItemsSource = FilmNameList;
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ticketHallList_LostFocus(object sender, RoutedEventArgs e)
        {
            GetFilmWithParameter();

        }

        private void ticketTimeList_LostFocus(object sender, RoutedEventArgs e)
        {
            GetFilmWithParameter();
        }

        private void ticketDateChoose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            GetFilmWithParameter();
        }

        public void GetRaws()
        {
            RawsList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();

                    OracleParameter Hall_in = new OracleParameter
                    {
                        ParameterName = "Hall_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketHallList.SelectedItem.ToString()
                    };
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getRaws"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { Hall_in, us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            int id = Convert.ToInt32(row["RowNumber"]);
                            RawsList.Add(id);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ticketHallList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ticketHallList.SelectedItem != null)
            {
                GetRaws();
                Raw.ItemsSource = RawsList;
            }
        }

        public void GetPlace()
        {
            PlaceList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();

                    OracleParameter Hall_in = new OracleParameter
                    {
                        ParameterName = "Hall_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = ticketHallList.SelectedItem.ToString()
                    };
                    OracleParameter Row_in = new OracleParameter
                    {
                        ParameterName = "Row_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(Raw.SelectedItem)
                    };
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("admin.getPlace"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { Hall_in, Row_in, us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            int id = Convert.ToInt32(row["Place"]);
                            PlaceList.Add(id);
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Raw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Raw.SelectedItem != null)
            {
                GetPlace();
                Place.ItemsSource = PlaceList;
            }
        }

        private void deleteTicket_Click(object sender, RoutedEventArgs e)
        {
            DeleteCurrentTicket();
            GetTicketInfo();
            ticketTable.ItemsSource = allTicketList;
        }

        public void DeleteCurrentTicket()
        {
            int index = allTicketList.IndexOf((Ticket)ticketTable.SelectedItem);
            Ticket cur_ticket = allTicketList[index];
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection_meneger))
                {
                    connection.Open();
                    OracleParameter number_in = new OracleParameter
                    {
                        ParameterName = "Number_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = cur_ticket.Number
                    };
                    OracleParameter date_in = new OracleParameter
                    {
                        ParameterName = "Date_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = Convert.ToDateTime(cur_ticket.seance.seanceDate)
                    };
                    OracleParameter time_in = new OracleParameter
                    {
                        ParameterName = "Time_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = cur_ticket.seance.seanceTime
                    };
                    OracleParameter hall_in = new OracleParameter
                    {
                        ParameterName = "Hall_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = cur_ticket.place.Hall.Name
                    };
                    OracleParameter film_in = new OracleParameter
                    {
                        ParameterName = "Film_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = cur_ticket.seance.seanceFilm.Name
                    };
                    OracleParameter row_in = new OracleParameter
                    {
                        ParameterName = "Row_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = cur_ticket.place.Raw
                    };
                    OracleParameter place_in = new OracleParameter
                    {
                        ParameterName = "Place_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = cur_ticket.place.Number
                    };
                    using (OracleCommand command = new OracleCommand("admin.deleteTicket"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { number_in, date_in, time_in, hall_in,film_in,row_in,place_in });
                        command.ExecuteNonQuery();
                        MessageBox.Show("Билет отменен!");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Search.Text == "")
                {
                    allTicketList.Clear();
                    GetTicketInfo();
                    ticketTable.ItemsSource = allTicketList;                    
                    MessageBox.Show("Введите данные!");
                }
                else
                {
                    sortTicketList.Clear();

                    if (Search.Text != "" && !Search.Text.ToLower().Contains("w") && !Search.Text.ToLower().Contains("^") && !Search.Text.ToLower().Contains("s") && !Search.Text.ToLower().Contains("d") && !Search.Text.Contains("["))
                    {
                        string s = Search.Text;
                        Regex regex = new Regex(@"^" + s + @"(\w)*", RegexOptions.IgnoreCase);
                        MatchCollection matches;
                        foreach (var p in allTicketList)
                        {
                            matches = regex.Matches(p.Number.ToString());

                            foreach (Match match in matches)
                            {
                                sortTicketList.Add(p);
                                ticketTable.ItemsSource = sortTicketList;

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ничего не найдено!");
                    }
                   
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ничего не найдено!");                
                Search.Text = "";
            }
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                Bitmap printscreen = new Bitmap(414, 463);
                Graphics graphics = Graphics.FromImage(printscreen as Image);
                graphics.CopyFromScreen(1070, 302, 0, 0, printscreen.Size);
                printscreen.Save("D:\\Alesya\\3course\\2sem\\БД\\Курсовая работа\\Screen\\screen.png",System.Drawing.Imaging.ImageFormat.Png);
                using (MailMessage mess = new MailMessage())
                {
                    SmtpClient client = new SmtpClient("smtp.mail.ru", Convert.ToInt32(587))
                    {
                        Credentials = new NetworkCredential("alesya.demidkevich@mail.ru", "nevergiveup-98765"),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    mess.From = new MailAddress("alesya.demidkevich@mail.ru");
                    mess.To.Add(new MailAddress("alesia.demidkevich@gmail.com"));
                    mess.Subject = "Cinema";
                    mess.IsBodyHtml = true;
                    mess.SubjectEncoding = Encoding.UTF8;
                    mess.Body = $"<html><head></head><body><p>Билет на сеанс</br></p></body>";

                    try
                    {
                        mess.Attachments.Add(new Attachment("D:\\Alesya\\3course\\2sem\\БД\\Курсовая работа\\Screen\\screen.png"));
                    }
                    catch { }

                    client.Send(mess);
                    mess.Dispose();
                    client.Dispose();
                    MessageBox.Show("Отправлено");

                    ticketNumberEnter.Content = "";
                    ticketCostEnter.Content = "";
                    ticketDateEnter.Content = "";
                    ticketTimeEnter.Content = "";
                    ticketHallEnter.Content = "";
                    ticketRawEnter.Content = "";
                    ticketPlaceEnter.Content = "";
                    ticketTypeEnter.Content = "";
                    ticketFilmEnter.Content = "";
                    ticketDurationEnter.Content = ""; 
                    ticketLimitEnter.Content = "";
                    ticketEmployeeEnter.Content = "";

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error");
            }
        }
    }
    
}