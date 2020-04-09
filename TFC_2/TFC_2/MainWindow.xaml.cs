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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TFC_2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }


        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Boolean encontrado = false;
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();

                MySqlDataReader reader = null;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuario", conexionBBDD);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    if (reader.GetString(2).Equals(txtUser.Text) && reader.GetString(3).Equals(txtPassword.Password))
                    {
                        encontrado = true;
                    }
                }
                
                conexionBBDD.Close();
                if (encontrado)
                {
                    Ventana_Home ventana_Home = new Ventana_Home();
                    ventana_Home.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El nombre de usuario y/o contraseña que has introducido no pertenece a ninguna cuenta. \nComprueba tu nombre y contraseña de usuario y vuelve a intentarlo.", "ERROR", MessageBoxButton.OK);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
