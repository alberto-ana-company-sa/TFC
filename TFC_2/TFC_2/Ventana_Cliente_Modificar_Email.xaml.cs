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
    /// Lógica de interacción para Ventana_Cliente_Modificar_Email.xaml
    /// </summary>
    public partial class Ventana_Cliente_Modificar_Email : Window
    {
        public static Ventana_Cliente_Modificar_Email v;
        public string codigo = "";
        string nombre = "";

        public Ventana_Cliente_Modificar_Email()
        {
            InitializeComponent();
            Ventana_Cliente_Modificar_Email.v = this;
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
                    lblNombre.Content = reader.GetString(1)  + " " + reader.GetString(2) + " " + reader.GetString(3);
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
                    "FROM email_cliente " +
                    "WHERE Codigo_Cliente = " + codigo,
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

        private void btnAnadirEmail_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Cliente_Email ventana_Cliente_Email = new Ventana_Cliente_Email();
            ventana_Cliente_Email.TextBoxNombreCliente(nombre, codigo);
            ventana_Cliente_Email.ventanaActual("Ventana_Cliente_Modificar_Email");
            ventana_Cliente_Email.Show();
        }

        private void btnEliminarEmail_Click(object sender, RoutedEventArgs e)
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
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM email_cliente WHERE Codigo = " + codigoDataGrid, conexionBBDD);

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

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
