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
        Window winVistaConfiguracion = new Window();

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

        private void abrirVentanaClientes(object sender, RoutedEventArgs e) // Se controla que una instancia de esta ventana no este abierta.
        {
            if (VistaClientes.IsOpen)// Se controla que una instancia de esta ventana no este abierta. 
            {
                this.winClientes.Activate();// Si esta abierta entonces activar, mandar al frente
                return;
            }
            else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winClientes = (Window)assembly.CreateInstance("Gimnasio.VistaClientes");
                this.winClientes.Show();
            }
        }

        private void clickBtnIngresarPago(object sender, RoutedEventArgs e)
        {
            
            // Create an instance of the window named
            // by the current button.
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Window win = (Window)assembly.CreateInstance("Gimnasio.VistaIngresoDeCuota");
            //win.Owner = this;

            
            win.Show();
        }

        private void SalirMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //CERRAR LA APLICACION ENTERA. 
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
            if (VistaConfiguracion.IsOpen)// Se controla que una instancia de esta ventana no este abierta. 
            {
                this.winVistaConfiguracion.Activate();// Si esta abierta entonces activar, mandar al frente
                return;
            }
            else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaConfiguracion = (Window)assembly.CreateInstance("Gimnasio.VistaConfiguracion");
                this.winVistaConfiguracion.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); //CERRAR LA APLICACION ENTERA. 
            
        }

        private void abrirVentanaControlIngreso(object sender, RoutedEventArgs e)
        {
            
            if (VistaControlIngreso.IsOpen)// Se controla que una instancia de esta ventana no este abierta. 
            {
                this.winVistaControlIngreso.Activate();// Si esta abierta entonces activar, mandar al frente
                return;
            }
            else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaControlIngreso = (Window)assembly.CreateInstance("Gimnasio.VistaControlIngreso");
                this.winVistaControlIngreso.Show();

            }
        }

        private void abrirVentanaIngresoManual(object sender, RoutedEventArgs e)
        {

            if (VistaIngresoManual.IsOpen) // Se controla que una instancia de esta ventana no este abierta. 
            {
                this.winIngresoManual.Activate(); // Si esta abierta entonces activar, mandar al frente
                return;
            }
            else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winIngresoManual = (Window)assembly.CreateInstance("Gimnasio.VistaIngresoManual");
                this.winIngresoManual.Show();
            }
        }

        private void VerClientesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (VistaClientes.IsOpen)// Se controla que una instancia de esta ventana no este abierta. 
            {
                this.winClientes.Activate();// Si esta abierta entonces activar, mandar al frente
                return;
            }
            else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winClientes = (Window)assembly.CreateInstance("Gimnasio.VistaClientes");
                this.winClientes.Show();
            }
        }
    }
}
