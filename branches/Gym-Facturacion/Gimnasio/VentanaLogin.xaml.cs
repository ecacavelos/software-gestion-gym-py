using System;
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

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VentanaLogin.xaml
    /// </summary>
    public partial class VentanaLogin : Window
    {

        public static bool IsOpen { get; private set; }
        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

        public VentanaLogin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            textBoxUsuario.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {

            string esql = "SELECT value a FROM Admins as a WHERE (a.Nombre = '" + textBoxUsuario.Text + "') AND (a.Password = '" + textBoxContraseña.Text + "')";
            var adminsVar = database1Entities.CreateQuery<Admins>(esql);

            if (adminsVar.ToList().Count == 1)
            {
                this.DialogResult = true;
            }

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


    }
}
