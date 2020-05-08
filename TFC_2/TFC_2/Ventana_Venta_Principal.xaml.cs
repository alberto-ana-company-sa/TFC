using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
    /// Lógica de interacción para Ventana_Venta_Principal.xaml
    /// </summary>
    public partial class Ventana_Venta_Principal : Window
    {
        public static Ventana_Venta_Principal v;

        string primerCodigo = "";
        string ultimoCodigo = "";
        Boolean entra = false;

        public Ventana_Venta_Principal()
        {
            InitializeComponent();
            Ventana_Venta_Principal.v = this;

            dateFecha.SelectedDate = System.DateTime.Now;
            cb_TipoPedido.SelectedIndex = 0;

            NombreVendedor(txtVendedor.Text);
            
        }

        public void NombreVendedor(string codigoPedido)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlDataReader reader = null;
                MySqlCommand consultaPresupuestoCompra = new MySqlCommand("SELECT * FROM empleados WHERE Codigo = " + codigoPedido, conexionBBDD); ;
                reader = consultaPresupuestoCompra.ExecuteReader();

                reader.Read();
                lblNombreVendedor.Content = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);

                reader.Close();
                conexionBBDD.Close();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static string codigoDireccion = "";
        public static string codigoTelefono = "";
        public static string codigoEmail = "";
       

        private void txt_CodigoPedido_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string num = "";
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlDataReader reader = null;
                    MySqlCommand consultaPresupuestoCompra = new MySqlCommand("SELECT * FROM pedido_venta", conexionBBDD);
                    reader = consultaPresupuestoCompra.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        num = reader.GetString(0);
                    }
                    if (num != "" && num != null)
                    {
                        int codigoPedido = Convert.ToInt32(num) + 1;
                        txt_CodigoPedido.Text = Convert.ToString(codigoPedido);
                    }
                    else
                    {
                        txt_CodigoPedido.Text = "1";
                    }
                    reader.Close();
                    conexionBBDD.Close();
                    btnIrParaAlante.IsEnabled = true;
                    btnIrParaAtras.IsEnabled = true;
                    btnBuscarCodigoCliente.IsEnabled = true;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }
        }

        private void btnBuscarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Busqueda ventana_Busqueda = new Ventana_Busqueda("Empleados");
            ventana_Busqueda.Show();
        }

        private void btnBuscarCodigoCliente_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Busqueda ventana_Busqueda = new Ventana_Busqueda("Clientes");
            ventana_Busqueda.Show();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            //UPDATE DE COMENTARIO EN LA TABLA DE PEDIDOS
            GuardarComentarios();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //UPDATE DE COMENTARIO EN LA TABLA DE PEDIDOS
            GuardarComentarios();
        }

        private void btnAnadirArticulo_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Buscar_Articulos ventana_Buscar_Articulos = new Ventana_Buscar_Articulos();
            ventana_Buscar_Articulos.Show();
        }

        private void btnBorrarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (dgDatos.SelectedIndex != -1)
            {
                int a = dgDatos.SelectedIndex;
                var codigo = (dgDatos.Items[a] as System.Data.DataRowView).Row.ItemArray[0];
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM lineas_pedido_venta WHERE Codigo = " + codigo, conexionBBDD);

                    cmd.ExecuteNonQuery();

                    conexionBBDD.Close();

                    RefrescarDataGrid();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                //Si no ha seleccionado ninguna fila del dataGrid elimina PEDIDO

                MessageBoxResult resultado = MessageBox.Show("¿DESEA ELIMINAR EL PEDIDO COMPLETO?", "¡CUIDADO!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
                switch (resultado)
                {
                    case MessageBoxResult.Yes:
                        EliminarPedidoEntero();
                        break;
                }
                
            }
        }
        
        public void RefrescarDataGrid()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT lineas_pedido_venta.Codigo, lineas_pedido_venta.Codigo_Articulo, articulos.Nombre_Articulo, articulos.Descripcion, lineas_pedido_venta.Tipo_IVA, lineas_pedido_venta.Cantidad, lineas_pedido_venta.Precio" +
                    " FROM lineas_pedido_venta, articulos " +
                    "WHERE lineas_pedido_venta.Codigo_Articulo = articulos.Codigo " +
                    "AND lineas_pedido_venta.Codigo_Pedido_Venta = " + txt_CodigoPedido.Text
                    , conexionBBDD);

                MySqlDataAdapter mySqlDataAdapter_Clientes = new MySqlDataAdapter(cmd);
                DataSet dataSetPedidos = new DataSet();

                mySqlDataAdapter_Clientes.Fill(dataSetPedidos);

                dgDatos.ItemsSource = dataSetPedidos.Tables[0].DefaultView;

                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void EliminarPedidoEntero()
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM pedido_venta WHERE Codigo = " + txt_CodigoPedido.Text, conexionBBDD);

                cmd.ExecuteNonQuery();

                conexionBBDD.Close();

                Ventana_Venta_Principal ventana_Venta = new Ventana_Venta_Principal();
                ventana_Venta.Show();
                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void GuardarComentarios()
        {
            string query = "UPDATE pedido_venta SET Comentario = '" + txtComentario.Text + "' WHERE Codigo = " + txt_CodigoPedido.Text;
            if (!txtComentario.Text.Equals(String.Empty))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conexionBBDD);
                    cmd.ExecuteNonQuery();
                    conexionBBDD.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnNuevoPedido_Click(object sender, RoutedEventArgs e)
        {
            string query = "UPDATE pedido_venta SET Comentario = '" + txtComentario.Text + "' WHERE Codigo = " + txt_CodigoPedido.Text;
            if (!txtComentario.Text.Equals(String.Empty) && !txt_CodigoPedido.Text.Equals(String.Empty))
            {
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conexionBBDD);
                    cmd.ExecuteNonQuery();
                    conexionBBDD.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            Ventana_Venta_Principal ventana_Venta = new Ventana_Venta_Principal();
            ventana_Venta.Show();
            this.Close();
        }

        public void PrincipioFin()
        {

            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                int contador = 0;
                MySqlDataReader reader = null;
                MySqlCommand consultaCliente = new MySqlCommand("SELECT Codigo FROM pedido_venta", conexionBBDD);
                reader = consultaCliente.ExecuteReader();
                while (reader.Read())
                {
                    if (contador == 0)
                    {
                        primerCodigo = reader.GetString(0);
                    }
                    else
                    {
                        ultimoCodigo = reader.GetString(0);
                    }
                    contador++;
                }

                reader.Close();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnIrParaAtras_Click(object sender, RoutedEventArgs e)
        {
            txt_CodigoPedido.IsEnabled = false;
            if (!entra)
            {
                PrincipioFin();
                entra = true;
            }
            if (!txt_CodigoPedido.Text.Equals(primerCodigo))
            {
                int numeroCodigo = Convert.ToInt32(txt_CodigoPedido.Text) - 1;
                RecargarDatos(Convert.ToString(numeroCodigo));
            }

        }

        private void btnIrParaAlante_Click(object sender, RoutedEventArgs e)
        {
            txt_CodigoPedido.IsEnabled = false;
            if (!entra)
            {
                PrincipioFin();
                entra = true;
            }
            if (Convert.ToInt32(txt_CodigoPedido.Text) < Convert.ToInt32(ultimoCodigo))
            {
                int numeroCodigo = Convert.ToInt32(txt_CodigoPedido.Text) + 1;
                RecargarDatos(Convert.ToString(numeroCodigo));
            }
        }

        public void RecargarDatos(string codigoPedido)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();

                MySqlDataReader reader = null;
                MySqlCommand consultaCliente = new MySqlCommand("SELECT * FROM pedido_venta WHERE Codigo = " + codigoPedido, conexionBBDD);
                reader = consultaCliente.ExecuteReader();
                reader.Read();

                txt_CodigoPedido.Text = codigoPedido;
                dateFecha.SelectedDate = Convert.ToDateTime(reader.GetString(4));
                cb_TipoPedido.SelectedItem = reader.GetString(3);
                txtVendedor.Text = reader.GetString(2);
                txtComentario.Text = reader.GetString(5);

                NombreVendedor(txtVendedor.Text);
                RecargarClientes(reader.GetString(2)); ;
                RecargarDireccion(reader.GetString(8));
                RecargarTelefono(reader.GetString(9));
                RecargarEmail(reader.GetString(7));
                reader.Close();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RecargarClientes(string codigo)
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

                txt_CodigoCliente.Text = reader.GetString(0);
                txt_NombreCliente.Text = reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3);
                txt_DniCliente.Text = reader.GetString(4);

                reader.Close();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RecargarDireccion(string codigo)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                //-----Datos de Direccion
                MySqlCommand consultaDireccion = new MySqlCommand("SELECT * FROM direccion_cliente WHERE Codigo = " + codigo, conexionBBDD);
                MySqlDataReader readerDirec = null;
                readerDirec = consultaDireccion.ExecuteReader();
                readerDirec.Read();
                txt_DireccionCliente.Text = readerDirec.GetString(2);
                txt_CPostalCiente.Text = readerDirec.GetString(3);
                txt_PoblacionCliente.Text = readerDirec.GetString(5);
                txt_ProvinciaCliente.Text = readerDirec.GetString(4);

                readerDirec.Close();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RecargarEmail(string codigo)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                //-----Datos de Email
                MySqlCommand consultaEmail = new MySqlCommand("SELECT * FROM telefono_cliente WHERE Codigo = " + codigo, conexionBBDD);
                MySqlDataReader readerEmail = null;
                readerEmail = consultaEmail.ExecuteReader();
                readerEmail.Read();
                txt_Email.Text = readerEmail.GetString(2);

                readerEmail.Close();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RecargarTelefono(string codigo)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                //-----Datos de Telefono
                MySqlCommand consultaTelefono = new MySqlCommand("SELECT * FROM telefono_cliente WHERE Codigo = " + codigo, conexionBBDD);
                MySqlDataReader readerTelefono = null;
                readerTelefono = consultaTelefono.ExecuteReader();
                readerTelefono.Read();
                txt_Telefono.Text = readerTelefono.GetString(2);
                readerTelefono.Close();
                conexionBBDD.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnPDF_Click(object sender, RoutedEventArgs e)
        {/*
            using (PdfDocument document = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for a page
                PdfGraphics graphics = page.Graphics;

                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                //Draw the text
                graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

                //Save the document
                document.Save("Output.pdf");
            }
            */
        }

        public void CrearExcel()
        {
            
        }

        private void btnPrimeroGuardado_Click(object sender, RoutedEventArgs e)
        {
            txt_CodigoPedido.IsEnabled = false;
            btnIrParaAlante.IsEnabled = true;
            btnBuscarCodigoCliente.IsEnabled = false;
            btnIrParaAtras.IsEnabled = true;
            if (!entra)
            {
                PrincipioFin();
                entra = true;
            }
            RecargarDatos(Convert.ToString(primerCodigo));
        }

        private void btnUltimoGuardado_Click(object sender, RoutedEventArgs e)
        {
            btnBuscarCodigoCliente.IsEnabled = false;
            txt_CodigoPedido.IsEnabled = false;
            btnIrParaAlante.IsEnabled = true;
            btnIrParaAtras.IsEnabled = true;
            if (!entra)
            {
                PrincipioFin();
                entra = true;
            }
            RecargarDatos(Convert.ToString(ultimoCodigo));
        }
    }
}
