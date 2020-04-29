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
    /// Lógica de interacción para Ventana_Cliente_Modificar_Direccion.xaml
    /// </summary>
    public partial class Ventana_Cliente_Modificar_Direccion : Window
    {
        public static Ventana_Cliente_Modificar_Direccion v;
        public string codigo = "";
        string nombre = "";
        public Ventana_Cliente_Modificar_Direccion()
        {
            InitializeComponent();
            Ventana_Cliente_Modificar_Direccion.v = this;
        }

        public void DatosCliente(String cod)
        {
            codigo = cod;

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaProducto = new MySqlCommand("SELECT * FROM clientes WHERE Codigo = " + codigo, conexionBBDD);
                reader = consultaProducto.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                    nombre = reader.GetString(1);
                    lblNombre.Content = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
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
                MySqlCommand cmd = new MySqlCommand("SELECT Codigo, Direccion, CP, Poblacion, Provincia " +
                    "FROM direccion_cliente " +
                    "WHERE Codigo_Cliente = " + codigo,
                    conexionBBDD);
                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                DatosDataGridDireccion.ItemsSource = dataSetClientes.Tables[0].DefaultView;

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

        private void btnAnadirDireccion_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Cliente_Direccion ventana_Cliente_Direccion = new Ventana_Cliente_Direccion();
            ventana_Cliente_Direccion.TextBoxNombreCliente(nombre, codigo);
            ventana_Cliente_Direccion.ventanaActual("Ventana_Cliente_Modificar_Direccion");
            ventana_Cliente_Direccion.Show();
        }

        private void btnEliminarDireccion_Click(object sender, RoutedEventArgs e)
        {
            if (DatosDataGridDireccion.SelectedIndex != -1)
            {
                int a = DatosDataGridDireccion.SelectedIndex;

                var codigoDataGrid = (DatosDataGridDireccion.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM direccion_cliente WHERE Codigo = " + codigoDataGrid, conexionBBDD);

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
    }
}
