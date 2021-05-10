using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cinema.Connection;
using Cinema.Users;
using Cinema.Admin;

namespace Cinema
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class Authorization : Window
    {
        public OracleConnection oracleConnection;
        public Employee currentEmployee;
        public string currUserPos = "Администратор";
        public Authorization()
        {
            InitializeComponent();
            loginText.ToolTip = "Введите имя пользователя";
            passwordText.ToolTip = "Пароль должен содержать 16 символов";
            showPassword.ToolTip = "Показать пароль";
        }

        public void checkUser()
        {          
            currentEmployee = new Employee();
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    oracleConnection.Open();

                    OracleParameter login = new OracleParameter
                    {
                        ParameterName = "in_login",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = loginText.Text
                    };
                    OracleParameter password = new OracleParameter
                    {
                        ParameterName = "in_password",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = passwordText.Password
                    };
                    OracleParameter user = new OracleParameter
                    {
                        ParameterName = "user_cur",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor
                    };

                    using (OracleCommand command = new OracleCommand("findUser"))
                    {
                        command.Connection = oracleConnection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { login, password, user });
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow row in dt.Rows)
                        {
                            currentEmployee.Login = row["Login"].ToString();
                            currentEmployee.Password = row["Password"].ToString();
                            currentEmployee.IDEmployee = Convert.ToInt32(row["e_ID"]);
                            currentEmployee.Name = row["Name"].ToString();
                            currentEmployee.Secondname = row["SecondName"].ToString();
                            currentEmployee.Surname = row["Surname"].ToString();
                            currentEmployee.FIO = currentEmployee.Surname + " " + currentEmployee.Name.Substring(0, 1) + ". " + currentEmployee.Secondname.Substring(0, 1) + ".";
                        }
                    }

                    oracleConnection.Close();
                    currUserPos = loginText.Text;
                    if (currUserPos == currentEmployee.Login)
                    {
                        if (currUserPos == "Администратор")
                        {
                            AdminWindow adminWindow = new AdminWindow(currentEmployee.IDEmployee,currentEmployee.FIO);
                            adminWindow.Show();
                            this.Close();
                        }
                        else {
                            BookingWindow bookingWindow = new BookingWindow(currentEmployee.IDEmployee,currentEmployee.FIO);
                            bookingWindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователя не существует");
                        loginText.Text = "";
                        passwordText.Password = "";
                        passwordText2.Text = "";
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginText.Text != "" && passwordText.Password != "")
            {
                checkUser();
            }
            else if (loginText.Text == "" && passwordText.Password != "")
            {
                PopupText.Content = "Заполните поле";
                loginText.BorderBrush = Brushes.Red;
            }
            else if (loginText.Text != "" && passwordText.Password == "")
            {
                PopupText1.Content = "Заполните поле";
                passwordText.BorderBrush = Brushes.Red;
            }
            else {
                PopupText.Content = "Заполните поле";
                loginText.BorderBrush = Brushes.Red;
                PopupText1.Content = "Заполните поле";
                passwordText.BorderBrush = Brushes.Red;
            }
        }

        private void showPassword_Click(object sender, RoutedEventArgs e)
        {
            var showPassword = sender as CheckBox;
            if (showPassword.IsChecked.Value)
            {
                passwordText2.Text = passwordText.Password;
                passwordText2.Visibility = Visibility.Visible;
                passwordText.Visibility = Visibility.Hidden;
            }
            else
            {
                passwordText.Password = passwordText2.Text;
                passwordText2.Visibility = Visibility.Hidden;
                passwordText.Visibility = Visibility.Visible;
            }
        }

        private void loginText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (loginText.Text == "")
            {
                loginText.BorderBrush = Brushes.Red;
                PopupText.Content = "Заполните поле";
            }
            else
            {
                loginText.BorderBrush = Brushes.Silver;
                PopupText.Content = "";
            }
        }

        private void passwordText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordText.Password == "")
            {
                passwordText.BorderBrush = Brushes.Red;
                PopupText1.Content = "Заполните поле";
            }
            else
            {
                passwordText.BorderBrush = Brushes.Silver;
                PopupText1.Content = "";
            }
        }

        private void passwordText_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordText.BorderBrush = Brushes.Silver;
            PopupText1.Content = "";
        }

        private void loginText_GotFocus(object sender, RoutedEventArgs e)
        {
            loginText.BorderBrush = Brushes.Silver;
            PopupText.Content = "";
        }
    }
}
