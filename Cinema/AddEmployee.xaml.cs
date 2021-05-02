using Cinema.Admin;
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
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        BindingList<string> PostNameList;
        public AddEmployee(BindingList<string> postList)
        {
            InitializeComponent();
            PostNameList = postList;
            employeePost.ItemsSource = PostNameList;
            employeeBirthday.DisplayDateEnd = DateTime.Now;
            employeeStart.DisplayDateEnd = DateTime.Now;
        }

        private void employeeAddButton_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                if (employeeSurname.Text == null || employeeName.Text == null || employeeSecondName.Text == null || employeeBirthday.SelectedDate == null || employeeStart.SelectedDate == null
                                                 || employeePost.Text == null || employeePhoneNumber.Text == null || employeeSalary.Text == null) {
                    MessageBox.Show("Заполните все поля!");
                }
                else 
                {

                    addEmployee();
                    this.Owner.Activate();
                    this.Close();  
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void checkPost() {
            foreach (var l in AdminWindow.allPostList) 
            {

                if (employeePost.Text == l.postName)
                {
                    if (l.postAccessLevel == "1" || l.postAccessLevel == "2")
                    {
                        employeeLogin.IsReadOnly = false;
                        employeePassword.IsReadOnly = false;
                        employeeLogin.Text = null;
                        employeePassword.Text = null;
                        employeeLogin.Background = new SolidColorBrush(Colors.White);
                        employeePassword.Background = new SolidColorBrush(Colors.White);
                    }
                    else {
                        employeeLogin.IsReadOnly = true;
                        employeeLogin.Background = new SolidColorBrush(Colors.Silver);
                        employeePassword.IsReadOnly = true;
                        employeePassword.Background = new SolidColorBrush(Colors.Silver);
                        employeeLogin.Text = null;
                        employeePassword.Text = null;
                    }
                }                
            }
        }

        public void addEmployee() {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();                    
                    OracleParameter surname_in = new OracleParameter
                    {
                        ParameterName = "Surname_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeeSurname.Text
                    };
                    OracleParameter name_in = new OracleParameter
                    {
                        ParameterName = "Name_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeeName.Text
                    };
                    OracleParameter secondName_in = new OracleParameter
                    {
                        ParameterName = "SecondName_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeeSecondName.Text
                    };
                    OracleParameter birthday_in = new OracleParameter
                    {
                        ParameterName = "Birthday_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = employeeBirthday.SelectedDate
                    };
                    OracleParameter dateOfEnrollment_in = new OracleParameter
                    {
                        ParameterName = "DateOfEnrollment_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = employeeStart.SelectedDate
                    };
                    OracleParameter post_in = new OracleParameter
                    {
                        ParameterName = "Post_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeePost.Text
                    };
                    OracleParameter phoneNumber_in = new OracleParameter
                    {
                        ParameterName = "PhoneNumber_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeePhoneNumber.Text
                    };
                    OracleParameter salary_in = new OracleParameter
                    {
                        ParameterName = "Salary_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Decimal,
                        Value = Convert.ToDecimal(employeeSalary.Text)
                    };
                    OracleParameter login_in = new OracleParameter
                    {
                        ParameterName = "Login_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeeLogin.Text
                    };
                    OracleParameter password_in = new OracleParameter
                    {
                        ParameterName = "Password_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = employeePassword.Text
                    };
                    using (OracleCommand command = new OracleCommand("addEmployeeAndUser"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { surname_in,name_in,secondName_in,birthday_in,dateOfEnrollment_in,post_in, phoneNumber_in, salary_in, login_in, password_in});
                        command.ExecuteNonQuery();
                        MessageBox.Show("Сотрудник добавлен!");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void employeePost_GotFocus(object sender, RoutedEventArgs e)
        {
            checkPost();
        }
    }
}
