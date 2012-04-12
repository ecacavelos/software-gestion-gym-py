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

using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VentanaPrincipal.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        Configuration c2;
        //private bool _Keypad_usb = false;

        Window winClientes = new Window();
        Window winIngresoManual = new Window();
        Window winVistaTiposCuotas = new Window();
        Window winVistaControlIngreso = new Window();
        Window winVistaConfiguracion = new Window();

        RawStuff.InputDevice id;
        int NumberOfKeyboards;
        Message message = new Message();

        public VentanaPrincipal()
        {
            //InitializeComponent();
            Activate();
            this.c2 = Configuration.Deserialize("config.xml");
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

        #region "Funciones para la captura y manejo de teclas"
        // Esto es necesario para permitir el ingreso de numeros de cedula mediante el keypad USB

        private void _KeyPressed(object sender, RawStuff.InputDevice.KeyControlEventArgs e)
        {
            int teclaObtenida = -1;
            //Console.WriteLine("Presionaste una Tecla.");

            string[] tokens = e.Keyboard.Name.Split(';');
            string token = tokens[1];

            //Console.WriteLine(e.Keyboard.deviceHandle.ToString());
            //Console.WriteLine(e.Keyboard.deviceType);
            //Console.WriteLine(e.Keyboard.deviceName);
            //Console.WriteLine(e.Keyboard.key.ToString());
            //Console.WriteLine(NumberOfKeyboards.ToString());

            //Console.WriteLine(e.Keyboard.vKey);
            //Console.WriteLine(token);

            if (e.Keyboard.vKey == "NumPad0")
            {
                teclaObtenida = 0;
            }
            if (e.Keyboard.vKey == "NumPad1")
            {
                teclaObtenida = 1;
            }
            if (e.Keyboard.vKey == "NumPad2")
            {
                teclaObtenida = 2;
            }
            if (e.Keyboard.vKey == "NumPad3")
            {
                teclaObtenida = 3;
            }
            if (e.Keyboard.vKey == "NumPad4")
            {
                teclaObtenida = 4;
            }
            if (e.Keyboard.vKey == "NumPad5")
            {
                teclaObtenida = 5;
            }
            if (e.Keyboard.vKey == "NumPad6")
            {
                teclaObtenida = 6;
            }
            if (e.Keyboard.vKey == "NumPad7")
            {
                teclaObtenida = 7;
            }
            if (e.Keyboard.vKey == "NumPad8")
            {
                teclaObtenida = 8;
            }
            if (e.Keyboard.vKey == "NumPad9")
            {
                teclaObtenida = 9;
            }

            if (token.Equals("Teclado PS/2 estándar"))
            {
                //Console.WriteLine("Teclado Principal.");
                this.c2.Keypad_usb = false;
                Configuration.Serialize("config.xml", this.c2);
            }
            else
            {
                this.c2.Keypad_usb = true;
                Configuration.Serialize("config.xml", this.c2);
                //Console.WriteLine("Keypad USB.");
                if (VistaControlIngreso.IsOpen)
                {
                    VistaControlIngreso ventana001 = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is VistaControlIngreso) as VistaControlIngreso;
                    System.Windows.Controls.TextBox destino = ventana001.textBox_Cedula;
                    if (!destino.IsKeyboardFocused)
                    {
                        if (e.Keyboard.vKey == "Return")
                        {
                            ventana001.ComprobarCedula();
                        }
                        else
                        {
                            if (destino.GetLineLength(0) < 8 && e.Keyboard.vKey != "Back")
                            {
                                destino.Text += teclaObtenida.ToString();
                            }
                            if (e.Keyboard.vKey == "Back")
                            {
                                destino.Text = destino.Text.Substring(0, destino.Text.Length - 1);
                            }
                        }
                    }
                }

            }

        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            if (id != null)
            {
                // I could have done one of two things here.
                // 1. Use a Message as it was used before.
                // 2. Changes the ProcessMessage method to handle all of these parameters(more work).
                //    I opted for the easy way.

                //Note: Depending on your application you may or may not want to set the handled param.

                message.HWnd = hwnd;
                message.Msg = msg;
                message.LParam = lParam;
                message.WParam = wParam;

                id.ProcessMessage(message);
            }
            return IntPtr.Zero;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            // I am new to WPF and I don't know where else to call this function.
            // It has to be called after the window is created or the handle won't
            // exist yet and the function will throw an exception.
            StartWndProcHandler();

            base.OnSourceInitialized(e);
        }

        void StartWndProcHandler()
        {
            IntPtr hwnd = IntPtr.Zero;
            Window myWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is VentanaPrincipal) as VentanaPrincipal;

            try
            {
                hwnd = new WindowInteropHelper(myWin).Handle;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //Get the Hwnd source   
            HwndSource source = HwndSource.FromHwnd(hwnd);
            //Win32 queue sink
            source.AddHook(new HwndSourceHook(WndProc));

            id = new RawStuff.InputDevice(source.Handle);
            NumberOfKeyboards = id.EnumerateDevices();
            Console.WriteLine("Teclados: " + NumberOfKeyboards.ToString());
            if (NumberOfKeyboards == 1)
            {
                System.Windows.MessageBox.Show("No se detectó un Teclado Auxiliar.\nNo podrá utilizarlo para ingresar números de Cédula.");
            }
            id.KeyPressed += new RawStuff.InputDevice.DeviceEventHandler(_KeyPressed);
        }
        #endregion

    }
}
