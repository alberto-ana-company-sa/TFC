using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para Ventana_Listado_Articulos.xaml
    /// </summary>
    public partial class Ventana_Listado_Articulos : Window
    {
        public Ventana_Listado_Articulos()
        {
            InitializeComponent();
            DatosDataGridProductos();
        }

        public void DatosDataGridProductos()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT	articulos.Codigo, articulos.Nombre_Articulo, articulos.Descripcion, articulos.Marca, articulos.Precio_Venta, stock.Cantidad, articulos.Baja" +
                    " FROM articulos, stock" +
                    " WHERE articulos.Codigo = stock.Codigo_Articulo" +
                    " AND articulos.Baja = 0", conexionBBDD);


                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetProducto = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetProducto);

                DataGridProductos.ItemsSource = dataSetProducto.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CheckDadoBaja_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckDadoBaja.IsChecked == true)
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT	articulos.Codigo, articulos.Nombre_Articulo, articulos.Descripcion, articulos.Marca, articulos.Precio_Venta, stock.Cantidad, articulos.Baja" +
                        " FROM articulos, stock" +
                        " WHERE articulos.Codigo = stock.Codigo_Articulo", conexionBBDD);


                    MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                    DataSet dataSetProducto = new DataSet();

                    mySqlDataAdapter_Clientes.Fill(dataSetProducto);

                    DataGridProductos.ItemsSource = dataSetProducto.Tables[0].DefaultView;

                    conexionBBDD.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            
        }

        private void CheckDadoBaja_Unchecked(object sender, RoutedEventArgs e)
        {
            DatosDataGridProductos();
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Ayuda ventanaAyuda = new Ventana_Ayuda();
            String texto = "En este apartado el usuario podrá ver una lista de todos \n"
                          + "los campos, además de poder filtrar a su gusto, \n" 
                          +"y prodra modificar los articulos.";
            ventanaAyuda.cambiarTexto(texto);
            ventanaAyuda.Show();
        }

        private void btnModificarProducto_Click(object sender, RoutedEventArgs e)
        {
            int a = DataGridProductos.SelectedIndex;

            var codigo = (DataGridProductos.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

            Ventana_Listado_Articulos_Modificacion ventanaModificacionArticulos = new Ventana_Listado_Articulos_Modificacion();
            ventanaModificacionArticulos.Show();
        }
    }
}
