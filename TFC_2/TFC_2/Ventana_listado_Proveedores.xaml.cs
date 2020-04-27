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
    /// Lógica de interacción para Ventana_listado_Proveedores.xaml
    /// </summary>
    public partial class Ventana_listado_Proveedores : Window
    {
        public Ventana_listado_Proveedores()
        {
            InitializeComponent();
            DatosDataGridProveedores();

        }

        public void DatosDataGridProveedores()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT proveedores.Nombre_Empresa, proveedores.CIF, direccion_proveedores.Direccion, direccion_proveedores.Poblacion, email_proveedores.Email " +
                    "FROM proveedores, direccion_proveedores, email_proveedores, telefono_proveedor " +
                    "WHERE proveedores.Codigo = direccion_proveedores.Codigo_Proveedores " +
                    "AND email_proveedores.Codigo_Proveedores = proveedores.Codigo " +
                    "AND proveedores.Codigo = telefono_proveedor.Codigo_Proveedor " +
                    "GROUP BY proveedores.Codigo"
                    , conexionBBDD);


                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                DataGridProveedores.ItemsSource = dataSetClientes.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBuscarProveedor_Click(object sender, RoutedEventArgs e)
        {
            String where = "";

            if (!TBCodigo_Proveedor.Text.Equals(String.Empty))
            {
                where += " AND proveedores.Codigo = " + TBCodigo_Proveedor.Text;

            }
            if (!TBNombre_Proveedor.Text.Equals(String.Empty))
            {
                where += " AND proveedores.Nombre_Empresa LIKE '%" + TBNombre_Proveedor.Text + "%'";
            }
            if (!TBCIF_Proveedor.Text.Equals(String.Empty))
            {
                where += " AND proveedores.CIF = '" + TBCIF_Proveedor.Text + "'";

            }


            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT proveedores.Nombre_Empresa, proveedores.CIF, direccion_proveedores.Direccion, direccion_proveedores.Poblacion, email_proveedores.Email " +
                    "FROM proveedores, direccion_proveedores, email_proveedores, telefono_proveedor " +
                    "WHERE proveedores.Codigo = direccion_proveedores.Codigo_Proveedores " +
                    "AND email_proveedores.Codigo_Proveedores = proveedores.Codigo " + where + " " +
                    "AND proveedores.Codigo = telefono_proveedor.Codigo_Proveedor " +
                    "GROUP BY proveedores.Codigo"
                    , conexionBBDD);


                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                DataGridProveedores.ItemsSource = dataSetClientes.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAnadirProveedor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminarProveedor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnModificarCliente_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
