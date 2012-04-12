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
using System.Reflection;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VentanaPrincipal.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {

        Window winClientes = new Window();
        Window winIngresoManual = new Window();
        Window winVistaTiposCuotas = new Window();
        Window winVistaControlIngreso = new Window();

        public VentanaPrincipal()
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

        private void abrirVentanaClientes(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;


            // Create an instance of the window named
            // by the current button.
            if (VistaClientes.IsOpen){
                this.winClientes.Activate();
                return;
            } 
            else
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winClientes = (Window)assembly.CreateInstance("Gimnasio.VistaClientes");
                //winClientes.Owner = this;
                //this.winClientes.
                // Show the window.
                this.winClientes.Show();
                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            if (VistaControlIngreso.IsOpen)
            {
                this.winVistaControlIngreso.Activate();
                return;
            }
            else
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaControlIngreso = (Window)assembly.CreateInstance("Gimnasio.VistaControlIngreso");
                this.winVistaControlIngreso.Show();

            }
        }

       

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)e.OriginalSource;

            if (VistaIngresoManual.IsOpen)
            {
                this.winIngresoManual.Activate();
                return;
            }
            else
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winIngresoManual = (Window)assembly.CreateInstance("Gimnasio.VistaIngresoManual");
                this.winIngresoManual.Show();
            }
        }

        private void clickBtnIngresarPago(object sender, RoutedEventArgs e)
        {
            // Get the current button.
            Button cmd = (Button)e.OriginalSource;

            // Create an instance of the window named
            // by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance("Gimnasio.VistaIngresoDeCuota");
            //win.Owner = this;

            // BLOQUEA LAS OTRAS VENTANAS !
            win.Show();
        }

        private void SalirMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.winClientes.Close();
            this.winVistaControlIngreso.Close();
            this.winIngresoManual.Close();
            this.winVistaTiposCuotas.Close();
            this.Close();
        }

        private void VerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (VistaClientes.IsOpen)
            {
                this.winClientes.Activate();
                return;
            }
            else
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winClientes = (Window)assembly.CreateInstance("Gimnasio.VistaClientes");
                winClientes.Owner = this;
                //this.winClientes.
                // Show the window.
                this.winClientes.Show();

            }
        }

        private void menuItem_CuotasEditar(object sender, RoutedEventArgs e)
        {
            if (VistaTiposCuotas.IsOpen)
            {
                this.winVistaTiposCuotas.Activate();
                return;
            }
            else
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaTiposCuotas = (Window)assembly.CreateInstance("Gimnasio.VistaTiposCuotas");
                this.winVistaTiposCuotas.Show();
            }
        }

        private void click_AboutGymAdmin(object sender, RoutedEventArgs e)
        {
            AboutGymAdmin windowAbout = new AboutGymAdmin();
            windowAbout.Show();

        }

        private void click_ConsultarPagos(object sender, RoutedEventArgs e)
        {
            // Create an instance of the window named
            // by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance("Gimnasio.ConsultarPagosCliente");
            // Show the window.
            win.Show();

        }

        
        
        private void MenuItem_Click_Configuracion(object sender, RoutedEventArgs e)
        {
            // Create an instance of the window named
            // by the current button.
            
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                Window win = (Window)assembly.CreateInstance("Gimnasio.VistaConfiguracion");
                // Show the window.
                win.Show();
        }

     

        private void Window_Closed(object sender, EventArgs e)
        {
            this.winClientes.Close();
            this.winVistaControlIngreso.Close();
            this.winIngresoManual.Close();
            this.winVistaTiposCuotas.Close();
        }

    }
}
