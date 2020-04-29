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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para Ventana_listado_Clientes.xaml
    /// </summary>
    public partial class Ventana_listado_Clientes : Window
    {
        public static Ventana_listado_Clientes v;

        public Ventana_listado_Clientes()
        {
            InitializeComponent();
            Ventana_listado_Clientes.v = this;
            DataGridClientes();


        }

        public void DataGridClientes()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT clientes.Codigo, clientes.Nombre, CONCAT(clientes.Apellido1, ' ', clientes.Apellido2) AS Apellidos, clientes.DNI, direccion_cliente.Direccion, direccion_cliente.Provincia, direccion_cliente.Poblacion, direccion_cliente.CP, email_cliente.Email " +
                    "FROM clientes, direccion_cliente, email_cliente " +
                    "WHERE clientes.Codigo = direccion_cliente.Codigo_Cliente " +
                    "AND email_cliente.Codigo_Cliente = clientes.Codigo " +
                    "GROUP BY clientes.Codigo"
                    , conexionBBDD);


                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);
                
                dataGridClientes.ItemsSource = dataSetClientes.Tables[0].DefaultView;
                
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            String where = "";
            
            if (!TBCodigo_Cliente.Text.Equals(String.Empty))
            {
                where += " AND clientes.Codigo = " + TBCodigo_Cliente.Text;
                
            }
            if(!TBNombre_Cliente.Text.Equals(String.Empty))
            {
                where += " AND clientes.Nombre LIKE '%" + TBNombre_Cliente.Text + "%'";
            }
            if (!TBDNI_Cliente.Text.Equals(String.Empty))
            {
                where += " AND clientes.DNI = '" + TBDNI_Cliente.Text + "'";
                
            }

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT clientes.Codigo, clientes.Nombre, CONCAT(clientes.Apellido1, ' ', clientes.Apellido2) AS Apellidos, clientes.DNI, direccion_cliente.Direccion, direccion_cliente.Provincia, direccion_cliente.Poblacion, direccion_cliente.CP, email_cliente.Email " +
                    "FROM clientes, direccion_cliente, email_cliente " +
                    "WHERE clientes.Codigo = direccion_cliente.Codigo_Cliente " + where + " " +
                    "AND email_cliente.Codigo_Cliente = clientes.Codigo " +
                    "GROUP BY clientes.Codigo"
                    , conexionBBDD);


                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetClientes = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetClientes);

                dataGridClientes.ItemsSource = dataSetClientes.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnAnadirCliente_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Cliente_Anadir ventana_Cliente_Anadir = new Ventana_Cliente_Anadir();
            ventana_Cliente_Anadir.Show();
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Ayuda ventana_Ayuda = new Ventana_Ayuda();
            ventana_Ayuda.Show();
        }

        private void btnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridClientes.SelectedIndex != -1)
            {
                int a = dataGridClientes.SelectedIndex;

                var codigo = (dataGridClientes.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

                MessageBox.Show(codigo.ToString());

                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM clientes WHERE Codigo = " + codigo, conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();

                    DataGridClientes();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void ModificarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridClientes.SelectedIndex != -1)
            {
                int a = dataGridClientes.SelectedIndex;

                var codigo2 = (dataGridClientes.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

                Ventana_Cliente_Modificar ventana_Cliente_Modificar = new Ventana_Cliente_Modificar();
                ventana_Cliente_Modificar.codigoCliente(Convert.ToString(codigo2));
                ventana_Cliente_Modificar.Show();
            }
        }
    }
}
