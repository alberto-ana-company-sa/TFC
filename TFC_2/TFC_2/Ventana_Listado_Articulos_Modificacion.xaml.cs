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
    /// Lógica de interacción para Ventana_Listado_Articulos_Modificacion.xaml
    /// </summary>
    public partial class Ventana_Listado_Articulos_Modificacion : Window
    {

        public Ventana_Listado_Articulos_Modificacion()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        String codigo = "";

        public void datosSelccionadosProducto(String codigo)
        {
           this.codigo = codigo;
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaProducto = new MySqlCommand("SELECT * FROM articulos WHERE Codigo = " + codigo, conexionBBDD);
                reader = consultaProducto.ExecuteReader();
                reader.Read();
                if (!reader.IsDBNull(0))
                {
                    TB_NombreProducto.Text = reader.GetString(1);
                    TBDescripcion.Text = reader.GetString(4);
                    TBPrecio.Text = reader.GetString(3);
                    TB_Marca.Text = reader.GetString(5);
                }
                else
                {
                   
                }
                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            //string query = "UPDATE articulos SET Nombre_Articulo = '" + TB_NombreProducto.Text + "',Descripcion='" + TBDescripcion.Text + "', Precio_Venta=" + Convert.ToDecimal(TBPrecio.Text) + ",Marca='" + TB_Marca.Text + "' WHERE Codigo = " + codigo;
            string query = "UPDATE articulos SET Nombre_Articulo = '" + TB_NombreProducto.Text + "', Descripcion='" + TBDescripcion.Text + "', Precio_Venta=" + Convert.ToDouble(TBPrecio.Text) + ",Marca='" + TB_Marca.Text + "' WHERE Codigo = " + codigo;
            if (!TB_NombreProducto.Text.Equals(String.Empty))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    MySqlCommand myCommand = new MySqlCommand(query, conexionBBDD);
                    myCommand.Connection.Open();
                    myCommand.ExecuteNonQuery();
                    conexionBBDD.Close();
                    Ventana_Listado_Articulos.v.DatosDataGridProductos();
                    lbl_ERROR.Foreground = Brushes.Green;
                    lbl_ERROR.Content = "PRODUCTO MODIFICADO CORRECTAMENTE";
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                lbl_ERROR.Foreground = Brushes.Red;
                lbl_ERROR.Content = "OBLIGATORIO QUE EL NOMBRE DEL PRODUCTO ESTÉ RELLENO";
            }
        }
    }
}
