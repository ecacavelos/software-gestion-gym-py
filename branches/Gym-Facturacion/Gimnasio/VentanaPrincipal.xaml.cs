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
        private Configuration c2;

        private Window winVistaConfiguracion = new Window();
        private Window winVistaTiposCuotas = new Window();
        private Window winClientes = new Window();
        private Window winIngresarPago = new Window();
        private Window winConsultarPagos = new Window();
        private Window winIngresoManual = new Window();
        private Window winVistaControlIngreso = new Window();
        private Window winVistaFacturas = new Window();
        private Window winVistaConfiguracionFacturas = new Window();
        private Window winReportePagos = new Window();
        private Window winReporteIngresos = new Window();
        private Window winAdministrarAdmins = new Window();

        private RawStuff.InputDevice id;
        private int NumberOfKeyboards;
        private Message message = new Message();

        private bool xmlinvalido = false;

        public VentanaPrincipal()
        {
            /* Se intenta leer el archivo de Configuración, antes de mostrar las ventanas.
             * Si el archivo 'config.xml' no existe o no posee la sintaxis correcta, 
             * se arrojan y manejan las excepciones correspondientes. */
            try
            {
                this.c2 = Configuration.Deserialize("config.xml");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("No se encontró el archivo de configuración.\nPor favor ingrese a la ventana de Configuración para recuperar las opciones.", "Archivo de Configuración");
                xmlinvalido = true;
            }
            catch (System.InvalidOperationException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("Existe un error con el archivo de configuración.\nPor favor ingrese a la ventana de Configuración para recuperar las opciones.", "Archivo de Configuración");
                xmlinvalido = true;
            }
        }

        private System.Data.Objects.ObjectQuery<clientes> GetclientesQuery(Database1Entities database1Entities)
        {
            // Auto generated code.
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = database1Entities.clientes;

            // Returns an ObjectQuery.
            return clientesQuery;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        #region "Funciones para el Toolbar de Botones de la Ventana Principal"

        private void abrirVentana_Clientes(object sender, RoutedEventArgs e)
        {
            if (VistaClientes.IsOpen)   // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winClientes.Activate();    // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winClientes = (Window)assembly.CreateInstance("Gimnasio.VistaClientes");
                this.winClientes.Show();
            }
        }

        private void abrirVentana_IngresarPago(object sender, RoutedEventArgs e)
        {
            if (VistaIngresoDeCuota.IsOpen)     // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winIngresarPago.Activate();    // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winIngresarPago = (Window)assembly.CreateInstance("Gimnasio.VistaIngresoDeCuota");
                this.winIngresarPago.Show();
            }
        }

        private void abrirVentana_ControlIngreso(object sender, RoutedEventArgs e)
        {
            if (VistaControlIngreso.IsOpen) // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winVistaControlIngreso.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaControlIngreso = (Window)assembly.CreateInstance("Gimnasio.VistaControlIngreso");
                this.winVistaControlIngreso.Show();
            }
        }

        private void abrirVentana_IngresoManual(object sender, RoutedEventArgs e)
        {
            if (VistaIngresoManual.IsOpen) // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winIngresoManual.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winIngresoManual = (Window)assembly.CreateInstance("Gimnasio.VistaIngresoManual");
                this.winIngresoManual.Show();
            }
        }

        #endregion "Funciones para el Toolbar de Botones de la Ventana Principal"

        #region "Funciones para los Menús de la Ventana Principal"

        private void menuItem_Configuracion(object sender, RoutedEventArgs e)
        {
            if (VistaConfiguracion.IsOpen) // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winVistaConfiguracion.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                // Si el archivo de configuración es inválido, es borrado para generar uno nuevo.
                if (xmlinvalido == true)
                {
                    System.IO.File.Delete("config.xml");
                    xmlinvalido = false;
                }
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaConfiguracion = (Window)assembly.CreateInstance("Gimnasio.VistaConfiguracion");
                this.winVistaConfiguracion.Show();
            }
        }

        private void menuItem_CuotasEditar(object sender, RoutedEventArgs e)
        {
            if (VistaTiposCuotas.IsOpen)    // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winVistaTiposCuotas.Activate();    // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaTiposCuotas = (Window)assembly.CreateInstance("Gimnasio.VistaTiposCuotas");
                this.winVistaTiposCuotas.Show();
            }
        }

        private void menuItem_Salir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        private void menuItem_VerClientes(object sender, RoutedEventArgs e)
        {
            if (VistaClientes.IsOpen) // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winClientes.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winClientes = (Window)assembly.CreateInstance("Gimnasio.VistaClientes");
                this.winClientes.Show();
            }
        }

        private void menuItem_ConsultarPagos(object sender, RoutedEventArgs e)
        {
            if (VistaConsultarPagos.IsOpen) // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winConsultarPagos.Activate();  // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winConsultarPagos = (Window)assembly.CreateInstance("Gimnasio.VistaConsultarPagos");
                this.winConsultarPagos.Show();
            }
        }

        private void menuItem_FacturasVer(object sender, RoutedEventArgs e)
        {
            if (VistaFacturas.IsOpen)   // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winVistaFacturas.Activate();   // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaFacturas = (Window)assembly.CreateInstance("Gimnasio.VistaFacturas");
                this.winVistaFacturas.Show();
            }
        }

        private void menuItem_FacturasConfiguracion(object sender, RoutedEventArgs e)
        {
            if (VistaConfiguracionFacturas.IsOpen)  // Si que una instancia de esta ventana está abierta.
            {
                this.winVistaConfiguracionFacturas.Activate();  // Si está abierta activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaConfiguracionFacturas = (Window)assembly.CreateInstance("Gimnasio.VistaConfiguracionFacturas");
                this.winVistaConfiguracionFacturas.Show();
            }
        }

        private void menuItem_AboutGymAdmin(object sender, RoutedEventArgs e)
        {
            AboutGymAdmin windowAbout = new AboutGymAdmin();
            windowAbout.Show();
        }

        private void menuItem_ReportePagos(object sender, RoutedEventArgs e)
        {
            if (VistaReportePagos.IsOpen)   // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winReportePagos.Activate();    // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin();
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.winReportePagos = (Window)assembly.CreateInstance("Gimnasio.VistaReportePagos");
                    this.winReportePagos.Show();
                }
                else
                {
                    //System.Console.WriteLine("Se canceló el Login.");
                }
            }
        }

        private void menuItem_ReporteIngresos(object sender, RoutedEventArgs e)
        {
            if (VistaReporteIngresos.IsOpen)    // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winReporteIngresos.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin();
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.winReporteIngresos = (Window)assembly.CreateInstance("Gimnasio.VistaReporteIngresos");
                    this.winReporteIngresos.Show();
                }
                else
                {
                    //System.Console.WriteLine("Se canceló el Login.");
                }
            }
        }

        private void menuItem_administrarAdmins(object sender, RoutedEventArgs e)
        {
            if (VistaAdministrarAdmins.IsOpen)  // Se controla que una instancia de esta ventana no esté abierta.
            {
                this.winAdministrarAdmins.Activate();   // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin(true);
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.winAdministrarAdmins = (Window)assembly.CreateInstance("Gimnasio.VistaAdministrarAdmins");
                    this.winAdministrarAdmins.Show();
                }
                else
                {
                    //System.Console.WriteLine("Se canceló el Login.");
                }
            }
        }

        #endregion "Funciones para los Menús de la Ventana Principal"

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

            if (VistaConfiguracion.IsOpen)
            {
                VistaConfiguracion ventana000 = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is VistaConfiguracion) as VistaConfiguracion;
                System.Windows.Controls.Label label_ID = ventana000.label4;
                System.Windows.Controls.TextBox textbox_ID = ventana000.textBox1;
                System.Windows.Controls.Button boton_aplicar = ventana000.button_AceptarConfiguracion;
                if (textbox_ID.IsKeyboardFocused)
                {
                    textbox_ID.Text = e.Keyboard.vKey;
                    label_ID.Content = e.Keyboard.deviceHandle.ToString();
                    boton_aplicar.IsEnabled = true;
                }
            }

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
            if (e.Keyboard.vKey == "NumLock")
            {
                teclaObtenida = -1;
            }

            this.c2 = Configuration.Deserialize("config.xml");

            //Console.WriteLine("Main ID: " + this.c2.MainDeviceID + ", Pressed ID: " + e.Keyboard.deviceHandle.ToString());
            if (e.Keyboard.deviceHandle.ToString().Equals(this.c2.MainDeviceID))
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
                                if (teclaObtenida != (-1))
                                {
                                    destino.Text += teclaObtenida.ToString();
                                }
                            }
                            if (e.Keyboard.vKey == "Back")
                            {
                                if (destino.Text.Length >= 1)
                                {
                                    destino.Text = destino.Text.Substring(0, destino.Text.Length - 1);
                                }
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

                // Note: Depending on your application you may or may not want to set the handled param.

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

            if (xmlinvalido == true)
            {
                System.IO.File.Delete("config.xml");
                xmlinvalido = false;

                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.winVistaConfiguracion = (Window)assembly.CreateInstance("Gimnasio.VistaConfiguracion");
                this.winVistaConfiguracion.Show();
            }
        }

        private void StartWndProcHandler()
        {
            IntPtr hwnd = IntPtr.Zero;
            Window myWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is VentanaPrincipal) as VentanaPrincipal;

            try
            {
                hwnd = new WindowInteropHelper(myWin).Handle;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
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
                System.Windows.MessageBox.Show("No se detectó un Teclado Auxiliar.\nNo podrá utilizarlo para ingresar números de Cédula.", "Teclado Auxiliar");
            }
            id.KeyPressed += new RawStuff.InputDevice.DeviceEventHandler(_KeyPressed);
        }

        #endregion "Funciones para la captura y manejo de teclas"
    }
}