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
    /// Lógica de interacción para Ventana_Proveedores_Modificar_Email.xaml
    /// </summary>
    public partial class Ventana_Proveedores_Modificar_Email : Window
    {
        public static Ventana_Proveedores_Modificar_Email v;
        public string codigo = "";
        string nombre = "";

        public Ventana_Proveedores_Modificar_Email()
        {
            InitializeComponent();
            Ventana_Proveedores_Modificar_Email.v = this;
        }

        public void DatosProveedores(String cod)
        {
            codigo = cod;

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaProducto = new MySqlCommand("SELECT * FROM proveedores WHERE Codigo = " + codigo, conexionBBDD);
                reader = consultaProducto.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                    nombre = reader.GetString(1);
                    lblNombre.Content = reader.GetString(1);
                }
                reader.Close();
                conexionBBDD.Close();
                DatosDataGrid();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void DatosDataGrid()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT Codigo, Email " +
                    "FROM email_proveedores " +
                    "WHERE Codigo_Proveedores = " + codigo,
                    conexionBBDD);
                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                DatosDataGridEmail.ItemsSource = dataSetClientes.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEliminarDireccion_Click(object sender, RoutedEventArgs e)
        {
            if (DatosDataGridEmail.SelectedIndex != -1)
            {
                int a = DatosDataGridEmail.SelectedIndex;

                var codigoDataGrid = (DatosDataGridEmail.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM email_proveedores WHERE Codigo = " + codigoDataGrid, conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();

                    DatosDataGrid();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnAnadirDireccion_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Proveedores_Email ventana_Proveedores_Email = new Ventana_Proveedores_Email();
            ventana_Proveedores_Email.InformacionProveedor(nombre, codigo);
            ventana_Proveedores_Email.ventanaActual("Ventana_Proveedores_Modificar_Email");
            ventana_Proveedores_Email.Show();
        }
    }
}
