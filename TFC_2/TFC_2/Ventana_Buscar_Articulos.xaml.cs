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
    /// Lógica de interacción para Ventana_Buscar_Articulos.xaml
    /// </summary>
    public partial class Ventana_Buscar_Articulos : Window
    {
        public Ventana_Buscar_Articulos()
        {
            InitializeComponent();
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT articulos.Codigo, articulos.Nombre_Articulo, articulos.Precio_Venta, articulos.Descripcion, stock.Cantidad " +
                    "FROM articulos, stock " +
                    "WHERE articulos.Codigo = stock.Codigo_Articulo"
                    , conexionBBDD);

                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                dgDatos.ItemsSource = dataSetClientes.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (dgDatos.SelectedIndex != -1)
            {
                int a = dgDatos.SelectedIndex;
                var codigoArticulo = (dgDatos.Items[a] as System.Data.DataRowView).Row.ItemArray[0];
                var precio = (dgDatos.Items[a] as System.Data.DataRowView).Row.ItemArray[2];
                var codigoPedido = (dgDatos.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

                Ventana_Numeros_Pedido_Venta ventana_Numeros_Pedido_Venta = new Ventana_Numeros_Pedido_Venta();
                ventana_Numeros_Pedido_Venta.DatosCodigoArticulo(Convert.ToString(codigoArticulo), Convert.ToString(precio), Ventana_Venta_Principal.v.txt_CodigoPedido.Text);
                ventana_Numeros_Pedido_Venta.Show();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            string where = "";
            if (!txt_Codigo.Text.Equals(string.Empty))
            {
                where += " AND articulos.Codigo = " + txt_Codigo.Text;
            }
            if (!txt_Nombre.Text.Equals(string.Empty))
            {
                where += " AND articulos.Nombre_Articulo LIKE '%" + txt_Nombre.Text + "%'";
            }

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT articulos.Codigo, articulos.Nombre_Articulo, articulos.Precio_Venta, articulos.Descripcion, stock.Cantidad " +
                    "FROM articulos, stock " +
                    "WHERE articulos.Codigo = stock.Codigo_Articulo" + where
                    , conexionBBDD);

                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                dgDatos.ItemsSource = dataSetClientes.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
