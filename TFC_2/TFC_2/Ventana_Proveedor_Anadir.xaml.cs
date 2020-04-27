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
    /// Lógica de interacción para Ventana_Proveedor_Anadir.xaml
    /// </summary>
    public partial class Ventana_Proveedor_Anadir : Window
    {
        public Ventana_Proveedor_Anadir()
        {
            InitializeComponent();
        }
        Boolean btn_Telefono = false;
        Boolean btn_Email = false;
        Boolean btn_Direccion = false;
        Boolean btn_Guardar = false;


        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!TB_NombreEmpresa.Text.Equals(String.Empty) && !TB_CIF_Proveedor.Text.Equals(String.Empty))
            {
                btn_Guardar = true;

                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO proveedores ( Nombre_Empresa, CIF) VALUES ('" + TB_NombreEmpresa.Text + "' , '" + TB_CIF_Proveedor.Text + "');", conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();
                    btnDireccion.IsEnabled = true;
                    btnTelefono.IsEnabled = true;
                    btnEmail.IsEnabled = true;
                    Ventana_listado_Proveedores.v.DatosDataGridProveedores();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (btn_Direccion == true && btn_Email == true && btn_Telefono == true)
            {
                this.Close();
            }
            else if (!btn_Guardar)
            {
                this.Close();
            }
            else
            {
                lbl_ERROR.Content = "OBLIGATORIO AGREGAR TELEFONO, DIRECCION Y EMAIL";
            }
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            btn_Email = true;
            string codigo = BuscarCodigo();
            Ventana_Proveedores_Email ventana_Proveedores_Email = new Ventana_Proveedores_Email();
            ventana_Proveedores_Email.InformacionProveedor(TB_NombreEmpresa.Text, codigo);
            ventana_Proveedores_Email.Show();
        }

        private void btnTelefono_Click(object sender, RoutedEventArgs e)
        {
            btn_Telefono = true;
            string codigo = BuscarCodigo();
            Ventana_Proveedores_Telefono ventana_Proveedores_Telefono = new Ventana_Proveedores_Telefono();
            ventana_Proveedores_Telefono.InformacionProveedor(TB_NombreEmpresa.Text, codigo);
            ventana_Proveedores_Telefono.Show();
        }

        private void btnDireccion_Click(object sender, RoutedEventArgs e)
        {
            btn_Direccion = true;
            string codigo = BuscarCodigo();
            Ventana_Proveedores_Direccion ventana_Proveedores_Direccion = new Ventana_Proveedores_Direccion();
            ventana_Proveedores_Direccion.InformacionProveedor(TB_NombreEmpresa.Text, codigo);
            ventana_Proveedores_Direccion.Show();
        }

        private void TB_CIF_Proveedor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57) e.Handled = false;

            else e.Handled = true;
        }

        public String BuscarCodigo()
        {
            String codigo = "";
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaCodigo = new MySqlCommand("SELECT Codigo FROM proveedores WHERE Nombre_Empresa ='" + TB_NombreEmpresa.Text + "' AND CIF ='" + TB_CIF_Proveedor.Text + "' "
                    , conexionBBDD);
                reader = consultaCodigo.ExecuteReader();


                while (reader.Read())
                {
                    codigo = reader.GetString(0);
                }

                reader.Close();
                conexionBBDD.Close();
                return codigo;
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return "";
        }
    }
}
