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
using Cinema.Connection;
using Cinema.Users;
using Cinema.Admin;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Cinema.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public static BindingList<Employee> allEmployeeList = new BindingList<Employee>();
        public static BindingList<Employee> sortEmployeeList = new BindingList<Employee>();
        public static BindingList<Film> allFilmList = new BindingList<Film>();
        public static BindingList<Film> sortFilmList = new BindingList<Film>();
        public static BindingList<string> FilmNameList = new BindingList<string>();
        public static BindingList<Seance> allSeanceList = new BindingList<Seance>();
        public static BindingList<Seance> sortSeanceList = new BindingList<Seance>();
        public static BindingList<string> SeanceTimeList = new BindingList<string>();
        public static BindingList<Hall> allHallList = new BindingList<Hall>();
        public static BindingList<string> HallNameList = new BindingList<string>();
        public static BindingList<Post> allPostList = new BindingList<Post>();
        public static BindingList<string> PostNameList = new BindingList<string>();
        public static BindingList<Genre> allGenreList = new BindingList<Genre>();
        public static BindingList<string> GenreNameList = new BindingList<string>();

        int userID;
        public AdminWindow()
        {            
            InitializeComponent();
         
            //seancceDateChoose.Text = Convert.ToString(DateTime.Today);
            filmStartDate.Text = Convert.ToString(DateTime.Today);
            filmEndDate.Text = Convert.ToString(DateTime.Today);
            //userID = ID;
            GetEmployee();
            employeeTable.ItemsSource = allEmployeeList;
            GetFilm();
            filmTable.ItemsSource = allFilmList;
            GetSeance();        
            allSeanceTime.ItemsSource = SeanceTimeList;
            seanceTable.ItemsSource = allSeanceList;
            GetHall();
            allSeanceHall.ItemsSource = HallNameList;
            GetPost();
            GetGenre();
           
        }

        public AdminWindow(int ID)
        {            
            InitializeComponent();
            //seancceDateChoose.Text = Convert.ToString(DateTime.Today);
            filmStartDate.Text = Convert.ToString(DateTime.Today);
            filmEndDate.Text = Convert.ToString(DateTime.Today);
            userID = ID;
            GetEmployee();
            employeeTable.ItemsSource = allEmployeeList;
            GetFilm();
            filmTable.ItemsSource = allFilmList;
            GetSeance();
            seanceTable.ItemsSource = allSeanceList;
            allSeanceTime.ItemsSource = SeanceTimeList;
            GetHall();
            allSeanceHall.ItemsSource = HallNameList;
            GetPost();
            GetGenre();

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Authorization autorization = new Authorization();            
            autorization.Show();
            this.Close();
        }

        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee(PostNameList);
            addEmployee.Owner = this;
            addEmployee.Show();
        }

        private void updateSeanceList_Click(object sender, RoutedEventArgs e)
        {
            GetHall();
            AddSeance addSeance = new AddSeance(HallNameList, FilmNameList, SeanceTimeList);
            addSeance.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetGenre();
            AddFilm addFilm = new AddFilm(GenreNameList);
            addFilm.Show();
        }

        private void printEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            GetEmployee();
            employeeTable.ItemsSource = allEmployeeList;
        }

        public void GetEmployee() {
            allEmployeeList.Clear();
            Employee cur_employee;
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter employeeList = new OracleParameter
                    {
                        ParameterName = "employeeList",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("getEmployeeList"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { employeeList });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row["name"].ToString();
                            string secondname = row["secondName"].ToString();
                            string surname = row["surname"].ToString();
                            string fio = surname + " " + name.Substring(0,1) + ". " + secondname.Substring(0, 1)+".";
                            string birthday = Convert.ToDateTime(row["birthday"]).ToString("D");
                            string startDay = Convert.ToDateTime(row["dateOfEnrollment"]).ToString("D");
                            string post = row["positionName"].ToString();
                            string phone = row["phoneNumber"].ToString();
                            string salary =(row["salary"]).ToString();

                            cur_employee = new Employee(fio, name, secondname, surname,birthday,startDay,phone,post,salary);
                            allEmployeeList.Add(cur_employee);
                        }
                    }
                }                
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetSeance()
        {
            allSeanceList.Clear();
            Seance cur_seance;
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("getSeanceList"))
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
                            string time = row["TimeSeance"].ToString();
                            string hall = row["hallName"].ToString();
                            string filmName = row["FilmName"].ToString(); 
                            Film film = new Film(filmName);
                            string capacity = row["HallCapacity"].ToString();
                            cur_seance = new Seance(date,time, hall, film, capacity);
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
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("getHallList"))
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

                            hall = new Hall(name,capacity);
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

        public static void GetPost()
        {
            Post post;
            allPostList.Clear();
            PostNameList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("getPostList"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { us_cur });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            string name = row["PositionName"].ToString();
                            string accessLevel = row["AccessLevel"].ToString();

                            post  = new Post(name, accessLevel);
                            allPostList.Add(post);
                            PostNameList.Add(name);

                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetGenre()
        {
            Genre genre;
            allGenreList.Clear();
            GenreNameList.Clear();
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter us_cur = new OracleParameter
                    {
                        ParameterName = "us_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("getGenreList"))
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
                            
                            genre = new Genre(name);
                            allGenreList.Add(genre);
                            GenreNameList.Add(name);

                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SortFilm()
        {            
            GetFilm();
            if (filmStartDate.SelectedDate <= filmEndDate.SelectedDate)
            {
                sortFilmList.Clear();
                foreach (var l in allFilmList)
                {
                    if (Convert.ToDateTime(l.StartreleaseDate) <= filmStartDate.SelectedDate && Convert.ToDateTime(l.EndreleaseDate) >= filmEndDate.SelectedDate)
                    {
                        sortFilmList.Add(l);
                    }
                }
                filmTable.ItemsSource = sortFilmList;

            }
            else if (filmStartDate.SelectedDate >= filmEndDate.SelectedDate)
            {
                MessageBox.Show("Неверный период!");
                filmStartDate.Text = Convert.ToString(DateTime.Today);
                filmEndDate.Text = Convert.ToString(DateTime.Today);
            }
            else 
            {
                filmStartDate.Text = Convert.ToString(DateTime.Today);
                filmEndDate.Text = Convert.ToString(DateTime.Today);
            }
        }

        public void GetFilm()
        {
            allFilmList.Clear();
            FilmNameList.Clear();
           
                Film cur_film;
                try
                {
                    using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                    {
                        connection.Open();
                        OracleParameter us_cur = new OracleParameter
                        {
                            ParameterName = "us_cur",
                            Direction = ParameterDirection.Output,
                            OracleDbType = OracleDbType.RefCursor
                        };

                        using (OracleCommand command = new OracleCommand("getFilmInfo"))
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddRange(new OracleParameter[] { us_cur });
                            var reader = command.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            foreach (DataRow row in dt.Rows)
                            {
                                string name = row["name"].ToString();
                                string genre = row["genre"].ToString();
                                string year = row["year"].ToString();
                                string duration = row["duration"].ToString();
                                string ageLimit = row["agelimit"].ToString();
                                string description = row["description"].ToString();
                                string startRelease = Convert.ToDateTime(row["startrelease"]).ToString("D");
                                string endRelease = Convert.ToDateTime(row["endrelease"]).ToString("D");
                                byte[] img = (byte[])(row["img"]);

                                cur_film = new Film(name, genre, year, duration, ageLimit, startRelease, endRelease, img, description);
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

        private void showAllFilms_Click(object sender, RoutedEventArgs e)
        {
            if (filmStartDate.SelectedDate == null || filmEndDate.SelectedDate == null)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                SortFilm();
            }
            
        }

        private void allSeanceTime_KeyDown(object sender, KeyEventArgs e)
        {
            allSeanceTime.IsDropDownOpen = true;        

        }

        private void allSeanceHall_KeyDown(object sender, KeyEventArgs e)
        {
            allSeanceHall.IsDropDownOpen = true;           
        }

        private void filmTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FilmWindow film = new FilmWindow(allFilmList[filmTable.SelectedIndex]);
            film.Show();
        }

        private void employeeTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GetPost();
            ChangeEmployee changeEmployee = new ChangeEmployee(allEmployeeList[employeeTable.SelectedIndex],PostNameList);
            changeEmployee.Show();
        }

       

        private void filmEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
                SortFilm();
            
        }

        private void filmStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {            
                SortFilm();
          
        }

        private void searchEmployee(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Search.Text == "")
                {
                    allEmployeeList.Clear();
                    GetEmployee();
                    employeeTable.ItemsSource = allEmployeeList;
                    Search.BorderBrush = Brushes.Red;
                    MessageBox.Show("Введите данные!");
                }
                else
                {
                    sortEmployeeList.Clear();                    
                    
                    if (Search.Text != "" && !Search.Text.ToLower().Contains("w") && !Search.Text.ToLower().Contains("^") && !Search.Text.ToLower().Contains("s") && !Search.Text.ToLower().Contains("d") && !Search.Text.Contains("["))
                    {
                        string s = Search.Text;
                        Regex regex = new Regex(@"^" + s + @"(\w)*", RegexOptions.IgnoreCase);
                        MatchCollection matches;
                        foreach (var p in allEmployeeList)
                        {
                            matches = regex.Matches(p.FIO);

                            foreach (Match match in matches)
                            {
                                sortEmployeeList.Add(p);
                                employeeTable.ItemsSource = sortEmployeeList;

                            }
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("Ничего не найдено!");
                    }
                    Search.BorderBrush = Brushes.Silver;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ничего не найдено!");
                Search.BorderBrush = Brushes.Silver;
                Search.Text = "";
            }
        }
                       
        private void showSeanceList_Click(object sender, RoutedEventArgs e)
        {
            GetSeance();
            sortSeanceList.Clear();
            foreach (var l in allSeanceList)
            {
                if (seancceDateChoose.SelectedDate != null && allSeanceTime.Text != "" && allSeanceHall.Text != "")
                {
                    if (Convert.ToDateTime(l.seanceDate) == seancceDateChoose.SelectedDate && l.seanceTime == allSeanceTime.Text && l.seanceHall == allSeanceHall.Text)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else if (seancceDateChoose.SelectedDate != null && allSeanceTime.Text != "" && allSeanceHall.Text == "")
                {
                    if (Convert.ToDateTime(l.seanceDate) == seancceDateChoose.SelectedDate && l.seanceTime == allSeanceTime.Text)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else if (seancceDateChoose.SelectedDate != null && allSeanceTime.Text == "" && allSeanceHall.Text != "")
                {
                    if (Convert.ToDateTime(l.seanceDate) == seancceDateChoose.SelectedDate && l.seanceHall == allSeanceHall.Text)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else if (seancceDateChoose.SelectedDate == null && allSeanceTime.Text != "" && allSeanceHall.Text != "")
                {
                    if (l.seanceTime == allSeanceTime.Text && l.seanceHall == allSeanceHall.Text)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else if (seancceDateChoose.SelectedDate != null && allSeanceTime.Text == "" && allSeanceHall.Text == "")
                {
                    if (Convert.ToDateTime(l.seanceDate) == seancceDateChoose.SelectedDate)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else if (seancceDateChoose.SelectedDate == null && allSeanceTime.Text != "" && allSeanceHall.Text == "")
                {
                    if (l.seanceTime == allSeanceTime.Text)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else if (seancceDateChoose.SelectedDate == null && allSeanceTime.Text == "" && allSeanceHall.Text != "")
                {
                    if (l.seanceHall == allSeanceHall.Text)
                    {
                        sortSeanceList.Add(l);
                    }
                    else
                    {

                    }
                }
                else {
                    sortSeanceList.Add(l);
                }

            }
            seanceTable.ItemsSource = sortSeanceList;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            GetEmployee();
            employeeTable.ItemsSource = allEmployeeList;
        }
    }
}
