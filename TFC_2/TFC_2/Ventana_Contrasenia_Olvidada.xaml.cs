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

using MySql.Data.MySqlClient;
namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para Ventana_Contrasenia_Olvidada.xaml
    /// </summary>
    public partial class Ventana_Contrasenia_Olvidada : Window
    {
        int contador = 1;
        int contadorAsunto = 1;
        public Ventana_Contrasenia_Olvidada()
        {
            InitializeComponent();
        }

        private void txtNombreUser_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (contador == 1)
            {
                txtNombreUser.Text = "";
                txtNombreUser.Foreground = Brushes.Black;
                contador++;
            }
        }

        private void txtAsunto_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (contadorAsunto == 1)
            {
                txtAsunto.Text = "";
                txtAsunto.Foreground = Brushes.Black;
                contadorAsunto++;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (contador != 1 && !txtNombreUser.Text.Equals(""))
            {
                Boolean encontrado = false;
                string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
                MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
                try
                {
                    conexionBBDD.Open();

                    MySqlDataReader reader = null;
                    MySqlCommand cmd = new MySqlCommand("SELECT Email FROM email_empleado", conexionBBDD);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.GetString(0).Equals(txtNombreUser.Text))
                        {
                            encontrado = true;
                        }
                    }

                    conexionBBDD.Close();
                    if (encontrado)
                    {
                        System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
                        mmsg.To.Add("alberto.anabel2020@gmail.com");
                        mmsg.Subject = "CAMBIO DE CONTRASEÑA " + txtNombreUser.Text;
                        mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

                        if (!txtAsunto.Text.Equals("ESCRIBA UNA BREVE DESCRIPCIÓN SI ASÍ LO DESEA")){
                            mmsg.Body = txtAsunto.Text;
                            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                        }
                        
                        mmsg.From = new System.Net.Mail.MailAddress("lynse.company@gmail.com");

                        System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
                        cliente.Credentials = new System.Net.NetworkCredential("lynse.company@gmail.com", "lynse2020");

                        cliente.Port = 587;
                        cliente.EnableSsl = true;
                        cliente.Host = "smtp.gmail.com";

                        try
                        {
                            cliente.Send(mmsg);
                            MessageBox.Show("CORREO ENVIADO");
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("ERROR AL ENVIAR EL EMAIL");
                        }
                    }
                    else
                    {
                        txtNombreUser.Background = Brushes.LightCoral;
                        lblError.Content = "EL CORREO ELECTRONICO NO COINCIDE CON NINGUNO REGISTRADO";
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                txtNombreUser.Background = Brushes.LightCoral;
                lblError.Content = "OBLIGATORIO RELLENAR EL CAMPO DE DIRECCIÓN DE CORREO ELECTRÓNICO";
            }
        }
    }
}
