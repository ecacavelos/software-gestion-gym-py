﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

     

        private System.Data.Objects.ObjectQuery<clientes> GetclientesQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = database1Entities.clientes;
            // Returns an ObjectQuery.
            return clientesQuery;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void abrirVentanaClientes(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            // Create an instance of the window named
            // by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance("Gimnasio.Window2");
            // Show the window.
            win.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            // Create an instance of the window named
            // by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance("Gimnasio.Window3");
            //win.Owner = this;

            // Show the window.
            win.Show();
                
            System.Console.WriteLine("abrir la ventana de control");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            // Create an instance of the window named
            // by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance("Gimnasio.Window4");
            //win.Owner = this;

            // BLOQUEA LAS OTRAS VENTANAS !
            win.ShowDialog();
                

        }
    }
}
