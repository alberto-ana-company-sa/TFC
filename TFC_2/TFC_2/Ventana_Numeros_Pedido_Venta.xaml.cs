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
    /// Lógica de interacción para Ventana_Numeros_Pedido_Venta.xaml
    /// </summary>
    public partial class Ventana_Numeros_Pedido_Venta : Window
    {
        public Ventana_Numeros_Pedido_Venta()
        {
            InitializeComponent();
        }
        string precio = "";
        string codigo_Pedido = "";
        string codigoArticulo = "";

        public void DatosCodigoArticulo(string codigo, string p, string codigoPedido)
        {
            txtCodigoArticulo.Text = codigo;
            codigoArticulo = codigo;
            txtCodigoPedido.Text = codigoPedido + " €";
            codigo_Pedido = codigoPedido;
            txtPrecio.Text = p;
            precio = p;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!txtIVA.Equals(string.Empty) && !txtCantidad.Equals(string.Empty))
            {
                GuardarDatosLineasPedido();

                RefrescarDataGridLineasPedido();
                
                this.Close();
            }
        }

        public void GuardarDatosLineasPedido()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO lineas_pedido_venta (Codigo_Pedido_Venta, Codigo_Articulo, Tipo_IVA, Cantidad, Precio) VALUES ( " +
                    codigo_Pedido + ", " + codigoArticulo + "," + txtIVA.Text + " , " + txtCantidad.Text + ", " + Convert.ToDouble(precio) + ");", conexionBBDD);
                cmd.ExecuteNonQuery();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RefrescarDataGridLineasPedido()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT lineas_pedido_venta.Codigo, lineas_pedido_venta.Codigo_Articulo, articulos.Nombre_Articulo, articulos.Descripcion, lineas_pedido_venta.Tipo_IVA, lineas_pedido_venta.Cantidad, lineas_pedido_venta.Precio" +
                    " FROM lineas_pedido_venta, articulos " +
                    "WHERE lineas_pedido_venta.Codigo_Articulo = articulos.Codigo " +
                    "AND lineas_pedido_venta.Codigo_Pedido_Venta = " + codigo_Pedido
                    , conexionBBDD);

                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetPedidos = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetPedidos);

                Ventana_Venta_Principal.v.dgDatos.ItemsSource = dataSetPedidos.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
