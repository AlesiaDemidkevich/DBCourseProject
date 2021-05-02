using Cinema.Users;
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
            employeeBirhday.Text = employee.Birthday;
            employeeStart.Text = employee.DateOfEnrollment;
            employeePost.Text = employee.Post;
            employeePhoneNumber.Text = employee.PhoneNumber;
            employeeSalary.Text = employee.Salary;
            employeeLogin.Text = employee.Login;
            employeePassword.Text = employee.Password;
        }

        private void employeePost_KeyDown(object sender, KeyEventArgs e)
        {
            employeePost.IsDropDownOpen = true;
        }
    }
}
