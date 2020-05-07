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
    /// Lógica de interacción para Ventana_Busqueda.xaml
    /// </summary>
    public partial class Ventana_Busqueda : Window
    {
        string nombreVentana = "";
        public Ventana_Busqueda(String nombre)
        {
            InitializeComponent();
            nombreVentana = nombre;
            if (nombreVentana.Equals("Clientes"))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT clientes.Codigo, clientes.Nombre, CONCAT(clientes.Apellido1, ' ', clientes.Apellido2) AS Apellidos " +
                        "FROM clientes"
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
            }else if (nombreVentana.Equals("Empleados"))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT Codigo, Nombre, CONCAT(Apellido1, ' ', Apellido2) AS Apellidos " +
                        "FROM empleados"
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

        public Ventana_Busqueda()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            // CUANDO UN CLIENTE NO TIENE (EMAIL, TELEFONO O DIRECCION) FALLA PORQUE AL BUSCAR NO ENCUENTRA DATOSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS!
            if (dgDatos.SelectedIndex != -1)
            {
                int a = dgDatos.SelectedIndex;

                var codigo = (dgDatos.Items[a] as System.Data.DataRowView).Row.ItemArray[0];

                if (nombreVentana.Equals("Clientes"))
                {
                    string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                    MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                    try
                    {
                        conexionBBDD.Open();
                        
                        MySqlDataReader reader = null;
                        MySqlCommand consultaCliente = new MySqlCommand("SELECT * FROM clientes WHERE Codigo = " + codigo, conexionBBDD);
                        reader = consultaCliente.ExecuteReader();
                        reader.Read();

                        Ventana_Venta_Principal.v.txt_CodigoCliente.Text = reader.GetString(0);
                        Ventana_Venta_Principal.v.txt_NombreCliente.Text = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
                        Ventana_Venta_Principal.v.txt_DniCliente.Text = reader.GetString(4);
                        
                        reader.Close();
                        conexionBBDD.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    try
                    {
                        conexionBBDD.Open();
                        //-----Datos de Direccion
                        MySqlCommand consultaDireccion = new MySqlCommand("SELECT * FROM direccion_cliente WHERE Codigo_Cliente = " + codigo, conexionBBDD);
                        MySqlDataReader readerDirec = null;
                        readerDirec = consultaDireccion.ExecuteReader();
                        readerDirec.Read();
                        Ventana_Venta_Principal.v.txt_DireccionCliente.Text = readerDirec.GetString(2);
                        Ventana_Venta_Principal.v.txt_CPostalCiente.Text = readerDirec.GetString(3);
                        Ventana_Venta_Principal.v.txt_PoblacionCliente.Text = readerDirec.GetString(5);
                        Ventana_Venta_Principal.v.txt_ProvinciaCliente.Text = readerDirec.GetString(4);
                        Ventana_Venta_Principal.codigoDireccion = readerDirec.GetString(0);
                        
                        readerDirec.Close();
                        conexionBBDD.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    try
                    {
                        conexionBBDD.Open();
                        //-----Datos de Telefono
                        MySqlCommand consultaTelefono = new MySqlCommand("SELECT * FROM telefono_cliente WHERE Codigo_Cliente = " + codigo, conexionBBDD);
                        MySqlDataReader readerTelefono = null;
                        readerTelefono = consultaTelefono.ExecuteReader();
                        readerTelefono.Read();
                        Ventana_Venta_Principal.v.txt_Telefono.Text = readerTelefono.GetString(2);
                        Ventana_Venta_Principal.codigoTelefono = readerTelefono.GetString(0);
                        readerTelefono.Close();
                        conexionBBDD.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    try
                    {
                        conexionBBDD.Open();
                        //-----Datos de Email
                        MySqlCommand consultaEmail = new MySqlCommand("SELECT * FROM telefono_cliente WHERE Codigo_Cliente = " + codigo, conexionBBDD);
                        MySqlDataReader readerEmail = null;
                        readerEmail = consultaEmail.ExecuteReader();
                        readerEmail.Read();
                        Ventana_Venta_Principal.v.txt_Email.Text = readerEmail.GetString(2);
                        Ventana_Venta_Principal.codigoEmail = readerEmail.GetString(0);

                        readerEmail.Close();
                        conexionBBDD.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }


                    GuardarDatosPedido();

                    this.Close();

                }else if (nombreVentana.Equals("Empleados"))
                {
                    string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                    MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                    try
                    {
                        conexionBBDD.Open();

                        MySqlDataReader reader = null;
                        MySqlCommand consultaCliente = new MySqlCommand("SELECT * FROM empleados WHERE Codigo = " + codigo, conexionBBDD);
                        reader = consultaCliente.ExecuteReader();
                        reader.Read();

                        Ventana_Venta_Principal.v.txtVendedor.Text = Convert.ToString(codigo);
                        Ventana_Venta_Principal.v.lblNombreVendedor.Content = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);

                        reader.Close();
                        conexionBBDD.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    
                    this.Close();
                }
            }
        }

        public void GuardarDatosPedido()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO pedido_venta ( Codigo_Cliente, Codigo_Empleado, Tipo, date, Codigo_EmailCliente, Codigo_DireccionCliente, 	Codigo_TelefonoCliente) VALUES ( " + Ventana_Venta_Principal.v.txt_CodigoCliente.Text + ",'" +  "' , '"  + "', '" +  "', '" +  "');", conexionBBDD);
                cmd.ExecuteNonQuery();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            String where = "";

            if (!txtCodigo.Text.Equals(String.Empty))
            {
                if (where.Equals(""))
                {
                    where += " WHERE Codigo = " + txtCodigo.Text;
                }
                else
                {
                    where += " AND Codigo = " + txtCodigo.Text;
                }
                
            }
            if (!txtNombre.Text.Equals(String.Empty))
            {
                if (where.Equals(""))
                {
                    where += " WHERE Nombre LIKE '%" + txtNombre.Text + "%'";
                }
                else
                {
                    where += " AND Nombre LIKE '%" + txtNombre.Text + "%'";
                }
                
            }

            if (nombreVentana.Equals("Clientes"))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT clientes.Codigo, clientes.Nombre, CONCAT(clientes.Apellido1, ' ', clientes.Apellido2) AS Apellidos " +
                        "FROM clientes " + where 
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
            else if (nombreVentana.Equals("Empleados")){
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT Codigo, Nombre, CONCAT(Apellido1, ' ', Apellido2) AS Apellidos " +
                        "FROM empleados " + where
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
}
