using Cinema.Connection;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
        private byte[] imageCode;
        BindingList<string> GenreNameList;
        public AddFilm(BindingList<string> genreNameList)
        {
            InitializeComponent();
            GenreNameList = genreNameList;
            filmGenre.ItemsSource = GenreNameList;
        }

        private void filmImgAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (ofd.ShowDialog() != true) return;
            FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] ic = br.ReadBytes((Int32)fs.Length);
            imageCode = ic;

            MemoryStream strIm = new MemoryStream(imageCode);
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.StreamSource = strIm;
            myBitmapImage.DecodePixelWidth = 225;
            myBitmapImage.DecodePixelHeight = 160;
            myBitmapImage.EndInit();
            filmImg.Source = myBitmapImage;
        }

        public void addNewFilm() {
            try
            {
                using (OracleConnection connection = new OracleConnection(OracleDatabaseConnection.connection))
                {
                    connection.Open();
                    OracleParameter name_in = new OracleParameter
                    {
                        ParameterName = "Name_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = filmName.Text
                    };
                    OracleParameter genre_in = new OracleParameter
                    {
                        ParameterName = "Genre_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value =  filmGenre.Text
                    };
                    OracleParameter year_in = new OracleParameter
                    {
                        ParameterName = "Year_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = filmYear.Text
                    };
                    OracleParameter duration_in = new OracleParameter
                    {
                        ParameterName = "Duration_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = filmDuration.Text
                    };
                    OracleParameter limit_in = new OracleParameter
                    {
                        ParameterName = "AgeLimit_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int32,
                        Value = Convert.ToInt32(filmAgeLimit.Text)
                    };
                    OracleParameter start_in = new OracleParameter
                    {
                        ParameterName = "Start_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = filmStart.SelectedDate
                    };
                    OracleParameter end_in = new OracleParameter
                    {
                        ParameterName = "End_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Date,
                        Value = filmEnd.SelectedDate
                    };
                    OracleParameter description_in = new OracleParameter
                    {
                        ParameterName = "Description_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Varchar2,
                        Value = filmDescription.Text
                    };
                    OracleParameter img_in = new OracleParameter
                    {
                        ParameterName = "Img_in",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Blob,
                        Value = imageCode
                    };
                    using (OracleCommand command = new OracleCommand("addFilm"))
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(new OracleParameter[] {name_in, year_in, genre_in, duration_in, limit_in, description_in, start_in, end_in,img_in});
                        command.ExecuteNonQuery();
                        MessageBox.Show("Фильм добавлен!");
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void filmAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (filmName.Text == null || filmGenre.Text == null || filmYear.Text == null || filmDuration.Text == null || filmAgeLimit.Text == null
                                                 || filmStart.SelectedDate == null || filmEnd.SelectedDate == null || filmDescription.Text == null )
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    addNewFilm();
                    this.Owner.Activate();
                    this.Close();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
