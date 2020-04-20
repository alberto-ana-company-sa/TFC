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
            GraficaPresupuestoCompraAnual();
            GraficaPedidosTotalAnual();
            GraficaFacturasTotalAnual();
            ValorTotalFacturadoLabelVerde();
            ValorTotalPedidosNoFacturasLabelAmarillo();
            ValorTotalAlbaranesPendientesDeCobroLabelAzul();
            ValorTotalNumeroPedidosFacturadosLabelNaranja();



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
                reader.Close();





                // -------------------------------------------------------------------------------------------------------------------- GRAFICA DE BARRAS

                MySqlDataReader leerFacturas = null;
                MySqlCommand consultaGrafica = new MySqlCommand("SELECT MONTH(factura_venta.Fecha) Mes, COUNT(*), YEAR(factura_venta.Fecha) anio FROM factura_venta GROUP BY Mes", conexionBBDD);
                leerFacturas = consultaGrafica.ExecuteReader();
                int contador = 0;
                int anio = 0;
                int temp = 0;
                
                
                while (leerFacturas.Read())
                {
                    temp = Convert.ToInt32(leerFacturas.GetString(2));

                    if (contador == 0)
                    {
                        SeriesCollection = new SeriesCollection
                        {
                            new ColumnSeries
                            {
                                Title = leerFacturas.GetString(2),
                            Values = new ChartValues<double> {0}
                            }
                        };
                        for (int i = 1; i <= Convert.ToInt32(leerFacturas.GetString(0)) ;i++)
                        {
                            
                            if (i == Convert.ToInt32(leerFacturas.GetString(0)))
                            {
                                SeriesCollection[0].Values.Add(Convert.ToDouble(leerFacturas.GetString(1)));
                            }
                            else
                            {
                                SeriesCollection[0].Values.Add(0d);
                            }
                        }
                        contador++;
                        anio = Convert.ToInt32(leerFacturas.GetString(2));
                    }
                    else if(anio != temp){
                        SeriesCollection.Add(new ColumnSeries
                        {
                            Title = leerFacturas.GetString(2),
                            Values = new ChartValues<double> {  }
                        });
                        anio = Convert.ToInt32(leerFacturas.GetString(2));
                        for (int i = 1; i <= Convert.ToInt32(leerFacturas.GetString(0)); i++)
                        {

                            if (i == Convert.ToInt32(leerFacturas.GetString(0)))
                            {
                                SeriesCollection[contador-1].Values.Add(Convert.ToDouble(leerFacturas.GetString(1)));
                            }
                            else
                            {
                                SeriesCollection[contador-1].Values.Add(0d);
                            }
                        }
                        contador++;
                    }
                    else
                    {
                        for (int i = 1; i <= Convert.ToInt32(leerFacturas.GetString(0)); i++)
                        {

                            if (i == Convert.ToInt32(leerFacturas.GetString(0)))
                            {
                                SeriesCollection[contador-1].Values.Add(Convert.ToDouble(leerFacturas.GetString(1)));
                            }
                            else
                            {
                                SeriesCollection[contador-1].Values.Add(0d);
                            }
                        }
                    }
                    
                }

                
                Labels = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                Formatter = value => value.ToString("N");

                DataContext = this;

                leerFacturas.Close();

                //also adding values updates and animates the chart automatically
               
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        
        public CartesianChart grafica1 { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }



        public  void GraficaPresupuestoCompraAnual()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                 conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaPresupuestoCompra = new MySqlCommand("SELECT SUM(lineas_factura_compras.Precio) FROM lineas_factura_compras", conexionBBDD);
                reader = consultaPresupuestoCompra.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0) )
                {
                    int valor = (Convert.ToInt32(reader.GetString(0)) * 100) / Convert.ToInt32(objetivo_PresupuestoCompra_anual.Content);
                    grafica_Presupuesto_Compra.Value = valor;
                    actual_PresupuestoCompra.Content = reader.GetString(0);
                }
               else 
                {
                    grafica_Presupuesto_Compra.Value = 0;
                } 
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void GraficaPedidosTotalAnual()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaPedidosTotalAnual = new MySqlCommand("SELECT Codigo FROM pedido_venta", conexionBBDD);
                reader = consultaPedidosTotalAnual.ExecuteReader();
                int contador = 0;
              
                while (reader.Read())
                {
                    contador++;
                }
                int valor = (contador * 100) / Convert.ToInt32(objetivo_PedidosTotales.Content);
                grafica_PedidosTotales.Value = valor;
                lblActual_PedidosTotales.Content = contador;
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void GraficaFacturasTotalAnual()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaFacturaTotalAnual = new MySqlCommand("SELECT Codigo FROM factura_venta", conexionBBDD);
                reader = consultaFacturaTotalAnual.ExecuteReader();
                int contador = 0;

                while (reader.Read())
                {
                    contador++;
                }
                int valor = (contador * 100) / Convert.ToInt32(objetivo_Facturas.Content);
                grafica_Facturas.Value = valor;
                lblActual_Facturas.Content = contador;
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void ValorTotalFacturadoLabelVerde()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaPedidosTotalAnual = new MySqlCommand("SELECT SUM(lineas_factura_venta.Precio) FROM lineas_factura_venta", conexionBBDD);
                reader = consultaPedidosTotalAnual.ExecuteReader();
                reader.Read();

                LblVerdeTotalFactura.Content = reader.GetString(0) + "€";
                
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ValorTotalPedidosNoFacturasLabelAmarillo()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaAlbaranesNoFacturados = new MySqlCommand("SELECT COUNT(albaran_venta.Traspaso_factura) FROM albaran_venta Where albaran_venta.Traspaso_factura = 0 ", conexionBBDD);
                reader = consultaAlbaranesNoFacturados.ExecuteReader();
                reader.Read();

                LblAmarilloTotalAlbaranesNoFacturados.Content = reader.GetString(0);

                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    
        public void ValorTotalAlbaranesPendientesDeCobroLabelAzul()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaAlbaranesNoFacturados = new MySqlCommand("SELECT SUM(lineas_albaran_ventas.Precio)  " +
                    "FROM lineas_albaran_ventas, albaran_venta" +
                    " WHERE albaran_venta.Traspaso_Factura = 0 " +
                    "AND albaran_venta.Codigo = lineas_albaran_ventas.Codigo_Albaran_Venta", conexionBBDD);
                reader = consultaAlbaranesNoFacturados.ExecuteReader();
                reader.Read();

                LblAzulAlbaranesPendienteDeCobro.Content = reader.GetString(0) + " €";

                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public void ValorTotalNumeroPedidosFacturadosLabelNaranja()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaAlbaranesNoFacturados = new MySqlCommand("SELECT COUNT(factura_venta.Codigo) FROM factura_venta", conexionBBDD);
                reader = consultaAlbaranesNoFacturados.ExecuteReader();
                reader.Read();

                LblNaranjaNumeroTotalDeFacturas.Content = reader.GetString(0);

                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
