using MySql.Data.MySqlClient;
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

namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para Ventana_Cliente_Email.xaml
    /// </summary>
    public partial class Ventana_Cliente_Email : Window
    {
        string codigo_cliente = "";
        string ventana = "";
        public Ventana_Cliente_Email()
        {
            InitializeComponent();
        }

        public void TextBoxNombreCliente(string nombreCliente, string codigoCliente)
        {
            TB_NombreCliente.Text = nombreCliente;
            codigo_cliente = codigoCliente;
        }
        

        public void ventanaActual(string nombreVentana)
        {
            ventana = nombreVentana;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO email_cliente (Codigo_Cliente, Email) VALUES ('" + codigo_cliente + "' , '" + TB_Email_Cliente.Text + "');", conexionBBDD);

                cmd.ExecuteNonQuery();

                conexionBBDD.Close();
                if (ventana.Equals("Ventana_Cliente_Modificar_Email"))
                {
                    Ventana_Cliente_Modificar_Email.v.DatosDataGrid();
                }
                else
                {
                    Ventana_listado_Clientes.v.DataGridClientes();
                }
                
                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
