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

                // Grid de Albaranes Ventas

                MySqlCommand cmd = new MySqlCommand("SELECT albaran_venta.Tipo, albaran_venta.Codigo, clientes.Nombre, clientes.Apellido1 Apellido, SUM(lineas_albaran_ventas.Precio) Importe, albaran_venta.Fecha " +
                    "FROM albaran_venta, clientes, lineas_albaran_ventas " +
                    "WHERE lineas_albaran_ventas.Codigo_Albaran_Venta = albaran_venta.Codigo AND clientes.Codigo = albaran_venta.Codigo_Cliente " +
                    "GROUP BY albaran_venta.Codigo"
                    , conexionBBDD);
                

                MySqlDataAdapter mySqlDataAdapter_AlbaranesVenta = new MySqlDataAdapter(cmd);
                DataSet dataSetAlbaranVenta = new DataSet();

                mySqlDataAdapter_AlbaranesVenta.Fill(dataSetAlbaranVenta);

                grid_AlbaranesVenta.ItemsSource = dataSetAlbaranVenta.Tables[0].DefaultView;

                // Grid de Facturas Ventas

                MySqlCommand cmd2 = new MySqlCommand("SELECT factura_venta.Tipo, factura_venta.Codigo, clientes.Nombre, clientes.Apellido1 Apellido, SUM(lineas_factura_venta.Precio) Importe, factura_venta.Fecha, factura_venta.Codigo_Albaran FROM factura_venta, clientes, lineas_factura_venta " +
                    "WHERE clientes.Codigo = factura_venta.Codigo_Cliente AND lineas_factura_venta.Codigo_Factura_Ventas = factura_venta.Codigo " +
                    "GROUP BY factura_venta.Codigo"
                    , conexionBBDD);
                

                MySqlDataAdapter mySqlDataAdapter_FacturasVentas = new MySqlDataAdapter(cmd2);
                DataSet dataSet_FacturasVentas = new DataSet();

                mySqlDataAdapter_FacturasVentas.Fill(dataSet_FacturasVentas);

                grid_FacturasVenta.ItemsSource = dataSet_FacturasVentas.Tables[0].DefaultView;

                // Circulo Presupuesto de venta
                /*
                 
                 Objetivo = 1000 €
                 Ventas = 500 €

                 100 % - 1000 €
                 x % --- 500 €
                 500 * 100(%) / 1000 (€) = 50 %

                lo que tenemos * 100% / objetivo = %
                 */

                MySqlDataReader reader = null;
                MySqlCommand consultaPresupuestoVenta = new MySqlCommand("SELECT SUM(lineas_factura_venta.Precio) FROM lineas_factura_venta", conexionBBDD);
                reader = consultaPresupuestoVenta.ExecuteReader();

                reader.Read();


                int valor = (Convert.ToInt32(reader.GetString(0)) * 100) / Convert.ToInt32(objetivo_Presupuesto_venta_anual.Content);

                grafica_Presupuesto_Venta.Value = valor;
                actual_presupuesto_venta_anual.Content = reader.GetString(0);
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
        
        public CartesianChart grafica1 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
