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
    
    public partial class MainWindow : Window
    {
        public OracleConnection oracleConnection;
        public Employee currentEmployee;
        public string currUserPos = "Администратор";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
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
                            currentEmployee.login = row["Login"].ToString();
                            currentEmployee.password = row["Password"].ToString();
                            currentEmployee.IDEmployee = row["IDEmployee"].ToString();
                        }
                    }

                    oracleConnection.Close();
                    currUserPos = loginText.Text;
                    if (currUserPos == currentEmployee.login)
                    {
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                        this.Close();
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
