﻿using MySql.Data.MySqlClient;
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
    /// Lógica de interacción para Ventana_Cliente_Direccion.xaml
    /// </summary>
    public partial class Ventana_Cliente_Direccion : Window
    {
        public Ventana_Cliente_Direccion()
        {
            InitializeComponent();
        }

        string codigo_cliente = "";
        string ventana = "";

        public void TextBoxNombreCliente(string nombreCliente, string codigoCliente)
        {
            TB_NombreCliente.Text = nombreCliente;
            codigo_cliente = codigoCliente;
        }

        public void ventanaActual(string vent)
        {
            ventana = vent;
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            string cadenaConexion = "server=localhost;database=lynse;uid=root;pwd=\"\";";
            MySqlConnection conexionBBDD = new MySqlConnection(cadenaConexion);
            try
            {
                conexionBBDD.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO direccion_cliente (Codigo_Cliente, Direccion, CP, Provincia, Poblacion) VALUES ('" + codigo_cliente + "' , '" + TB_Direccion_Cliente.Text + "' , '" + TB_CP_Cliente.Text  + "' , '" +  TB_Provincia_Cliente.Text +"' , '" + TB_Poblacion_Cliente.Text +"');", conexionBBDD);

                cmd.ExecuteNonQuery();

                conexionBBDD.Close();
                if (ventana.Equals("Ventana_Cliente_Modificar_Direccion"))
                {
                    Ventana_Cliente_Modificar_Direccion.v.DatosDataGrid();
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

        private void TB_CP_Cliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 48 && ascci <= 57) e.Handled = false;

            else e.Handled = true;
        }

        private void TB_Provincia_Cliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;
        }

        private void TB_Poblacion_Cliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122)

                e.Handled = false;

            else e.Handled = true;
        }
    }
}
