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
using Syncfusion.UI.Xaml.ProgressBar;

using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System.Data;

namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para Ventana_Home.xaml
    /// </summary>
    public partial class Ventana_Home : Window
    {
        public Ventana_Home()
        {
            InitializeComponent();

            
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                
                conexionBBDD.Open();

                //MySqlDataReader reader = null;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM empleados", conexionBBDD);
                //reader = cmd.ExecuteReader();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);

                grid_AlbaranesVenta_Copy1.ItemsSource = ds.Tables[0].DefaultView;

                conexionBBDD.Close();
                
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            // -------------------------------------------------------------------------------------------------------------------- GRAFICA DE BARRAS
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            DataContext = this;

        }
        // -------------------------------------------------------------------------------------------------------------------- GRAFICA DE BARRAS

            /*
        public void gird_Prueba()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();

                MySqlDataReader reader = null;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuario", conexionBBDD);
                reader = cmd.ExecuteReader();


                grid_AlbaranesVenta_Copy1.ItemsSource = reader.ToString();

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        */

        public CartesianChart grafica1 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
