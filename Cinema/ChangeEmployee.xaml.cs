using Cinema.Admin;
using Cinema.Connection;
using Cinema.Users;
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
    /// Логика взаимодействия для ChangeEmployee.xaml
    /// </summary>
    public partial class ChangeEmployee : Window
    {
        Employee employee;
        BindingList<string> PostNameList;
        public ChangeEmployee(Employee employee, BindingList<string> postList)
        {
            InitializeComponent();
            this.employee = employee;
            PostNameList = postList;
            employeePost.ItemsSource = PostNameList;
            employeeSurname.Text = employee.Surname;
            employeeName.Text = employee.Name;
            employeeSecondName.Text = employee.Secondname;
            employeeBirthday.Text = employee.Birthday;
            employeeStart.Text = employee.DateOfEnrollment;
            employeePost.Text = employee.Post;
            employeePhoneNumber.Text = employee.PhoneNumber;
            employeeSalary.Text = employee.Salary;          
        }

        private void employeePost_KeyDown(object sender, KeyEventArgs e)
        {
            employeePost.IsDropDownOpen = true;
        }

        private void employeeAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (employeeSurname.Text == null || employeeName.Text == null || employeeSecondName.Text == null || employeeBirthday.SelectedDate == null || employeeStart.SelectedDate == null
                                                 || employeePost.Text == null || employeePhoneNumber.Text == null || employeeSalary.Text == null)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    saveChanges();
                    this.Owner.Activate();
                    this.Close();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void saveChanges() 
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter ID_in = new OracleParameter
                    {
                        ParameterName = "ID_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = employee.ID
                    };
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
                    using (OracleCommand command = new OracleCommand("updateEmployee"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] { ID_in,surname_in, name_in, secondName_in, birthday_in, dateOfEnrollment_in, post_in, phoneNumber_in, salary_in});
                        command.ExecuteNonQuery();
                        MessageBox.Show("Информация о сотруднике изменена!");
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
