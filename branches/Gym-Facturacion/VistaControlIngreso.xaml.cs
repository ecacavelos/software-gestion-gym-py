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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;


namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VistaControlIngreso.xaml
    /// </summary>
    public partial class VistaControlIngreso : Window
    {
        static DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        bool _Mensaje_activo = false;

        #region "Imported Functions"

        [DllImport("ParallelPort.dll")]
        private static extern void Out32(short PortAddress, short data);

        [DllImport("ParallelPort.dll")]
        private static extern short Inp32(short PortAddress);

        [DllImport("ParallelPort.dll")]
        private static extern Int16 ReadMemShort(short Address);

        #endregion

        bool _Activado = false;
        private int _PortAddress = 0;
        string ExceptionOccured;

        Configuration c2;
        private int _TiempoApertura = 0;
        private static int test = 0;
        public static bool IsOpen { get; private set; }

        public VistaControlIngreso()
        {
            ParallelPort();
            InitializeComponent();
            this.c2 = Configuration.Deserialize("config.xml");
            _TiempoApertura = this.c2.TiempoApertura;
            IsOpen = true;
        }

        public void ParallelPort()
        {
            InitializeComponent();

            int[] PortAddresses = Detect_LPT_Ports();
            if (PortAddresses.Length != 0)
            {
                _PortAddress = PortAddresses[0];
            }
            else
            {
                _PortAddress = 0;
                MessageBox.Show("No se detectó Puerto Paralelo en la computadora.\nNo se podrá utilizar el porton electrico.");
            }

        }
        private int[] Detect_LPT_Ports()
        {
            int Number_Of_LPT_Ports = 0;
            short[] portAddresses = new short[3];
            for (int i = 0; i < 3; i++)
            {
                portAddresses[i] = ReadMemShort(Convert.ToInt16(1032 + i * 2));
                if (portAddresses[i] != 0)
                {
                    Number_Of_LPT_Ports++;
                }
            }
            int[] PortAddresses = new int[Number_Of_LPT_Ports];
            for (int i = 0, j = 0; i < 3; i++)
            {
                if (portAddresses[i] != 0)
                {
                    PortAddresses[j] = Convert.ToInt32(portAddresses[i]);
                    j++;
                }
            }
            return PortAddresses;
        }


        #region "Data Bus"

        /// <summary>
        /// Gets the address of the Parallel Port's Data Bus.
        /// </summary>
        private short DataBusAddress
        {
            get
            {
                return Convert.ToInt16(_PortAddress);
            }
        }

        /// <summary>
        /// Read current status of the Data Bus. 
        /// </summary>
        /// <returns>
        /// Current status of all the Data Pins as an integer 
        /// whose bits give the current status of each Data Pin (D0 to D7) 
        /// with D0 being the LSB and D7 being the MSB.
        /// </returns>
        public int ReadFromDataBus()
        {
            return Convert.ToInt32(Inp32(DataBusAddress));
        }

        /// <summary>
        /// Writes data to the Data Bus.   
        /// </summary>
        /// <param name="Data">Data to be written</param>
        public void WriteToDataBus(int Data)
        {
            try
            {
                Out32(DataBusAddress, Convert.ToInt16(Data));
            }
            catch (Exception ex)
            {
                ExceptionOccured = "WriteToDataBus(int Data) called with Data = " + Data.ToString() + ". ERROR occured is ---> " + ex.Message;
            }
        }

        /// <summary>
        /// Make data at all the Data Pins HIGH.
        /// </summary>
        public void SetDataBus()
        {
            try
            {
                D0 = true;
            }
            catch (Exception ex)
            {
                ExceptionOccured = "SetDataBus() called. ERROR occured is ---> " + ex.Message;
            }
        }

        /// <summary>
        ///  Make data at all the Data Pins LOW.
        /// </summary>
        public void ResetDataBus()
        {
            try
            {
                D0 = false;
            }
            catch (Exception ex)
            {
                ExceptionOccured = "ResetDataBus() called. ERROR occured is ---> " + ex.Message;
            }
        }

        /// <summary>
        /// Gets or Sets the data at Pin D0 (Pin no. 2).
        /// </summary>
        public bool D0
        {
            get
            {
                return Convert.ToBoolean(Inp32(DataBusAddress) & 1);
            }
            set
            {
                try
                {
                    if (value)
                    {
                        Out32(DataBusAddress, Convert.ToInt16(Inp32(DataBusAddress) | 1));
                        _Activado = true;
                        //label1.Content = "D0 ON";
                    }
                    else
                    {
                        Out32(DataBusAddress, Convert.ToInt16(Inp32(DataBusAddress) & (~1)));
                        _Activado = false;
                        //label1.Content = "D0 OFF";
                    }
                }
                catch (Exception ex)
                {
                    ExceptionOccured = "D0 assigned = " + value.ToString() + ". ERROR occured is ---> " + ex.Message;
                }
            }
        }

        #endregion

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }


        public void dispatcherTimer_Tick(object sender, EventArgs e)//TIMER PARA EL INGRESO DEL USUARIO
        {
            D0 = false;
            this.label2_ResultadoIngreso.Content = "";
            this.labelNombre.Content = "";
            this.labelNombreCliente.Content = "";
            this.labelApellido.Content = "";
            this.labelApellidoCliente.Content = "";
            this.labelMensajeCuotaVencida.Content = "";
            this.centerlabel.Content = "";
            dispatcherTimer.Stop();
            //test += 1;
            this.textBox_Cedula.Text = "";
            label1.Visibility = System.Windows.Visibility.Visible;
            textBox_Cedula.Visibility = System.Windows.Visibility.Visible;
            //textBox_Cedula.Background = System.Windows.Media.Brushes.Transparent;
            //label2_ResultadoIngreso.Background = System.Windows.Media.Brushes.Transparent;
            image1.Source = null;
            if (this.IsActive)
            {
                textBox_Cedula.Focus();
            }
            _Mensaje_activo = false;
            //Console.WriteLine("Termina timer.");
        }

        private void textBoxNumeroCedula_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                ComprobarCedula();
            }
            else
            {
                // System.Console.WriteLine("otra tecla");
            }
        }

        private void window_ControlIngreso_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBox_Cedula.Focus();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        public void ComprobarCedula()
        {
            if (_Mensaje_activo == true)
            {
                // Si se está mostrando algún mensaje no hacemos nada, se debe esperar a que desaparezca
                return;
            }
            else
            {
                label1.Visibility = System.Windows.Visibility.Hidden;
                textBox_Cedula.Visibility = System.Windows.Visibility.Hidden;
            }

            // Hacer el control de pago de cuota. Si la fecha de vencimiento en la tabla pagos es menor al dia de hoy entonces habilitar

            Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

            //Se selecciona el cliente en cuestion
            string esql = "select value c from clientes as c where c.nro_cedula= '" + textBox_Cedula.Text + "\'";
            var clientesVar = database1Entities.CreateQuery<clientes>(esql);

            // Se controla que el cliente que se haya traido sea un cliente valido. 
            if (clientesVar.ToList().Count == 0)
            {
                //MessageBox.Show((Window)this, "No existe cliente con ese numero de cedula.");
                //Console.WriteLine("No existe cliente con ese numero de cedula.");

                _Mensaje_activo = true;
                this.centerlabel.Content = "No existe cliente con ese numero de cedula.";
                dispatcherTimer.Interval = new TimeSpan(0, 0, _TiempoApertura);
                dispatcherTimer.Start();
            }
            else
            {
                string fechaVencimientoQuery = "select value p from Pagos as p where p.fk_cliente=" + clientesVar.ToArray()[0].idCliente + " order by p.fecha_vencimiento desc limit 1";
                var fechaUltimoVencimientoResult = database1Entities.CreateQuery<Pagos>(fechaVencimientoQuery);

                // Se controla que tenga por lo menos un pago el cliente
                if (clientesVar.ToArray()[0].Pagos.ToList().Count >= 1)
                {
                    if (fechaUltimoVencimientoResult.ToArray()[0].fecha_vencimiento > System.DateTime.Today)
                    {

                        // Abrir el porton
                        try
                        {
                            this.labelMensajeCuotaVencida.Content = ""; // 
                            _Mensaje_activo = true;

                            // Seteamos  "Nombre: <NombreCliente>" 
                            this.labelNombre.Content = "Nombre:";
                            this.labelNombreCliente.Content = clientesVar.ToArray()[0].nombre.ToString();

                            // Seteamos  "Apellido: <ApellidoCliente>" 
                            this.labelApellido.Content = "Apellido:";
                            this.labelApellidoCliente.Content = clientesVar.ToArray()[0].apellido.ToString();

                            this.label2_ResultadoIngreso.Foreground = new SolidColorBrush(Colors.Green);
                            //this.label2_ResultadoIngreso.FontSize = 20;
                            TimeSpan cantDias = fechaUltimoVencimientoResult.ToArray()[0].fecha_vencimiento.Value - System.DateTime.Today;
                            this.label2_ResultadoIngreso.Content = "Ingreso Exitoso --> " + "Su cuota vence en: " + cantDias.Days + " días.";

                            if (clientesVar.ToArray()[0].hasfoto == true)
                            {
                                // Recuperamos la foto del cliente para mostrar
                                string pathfoto = String.Empty;
                                pathfoto = System.Windows.Forms.Application.ExecutablePath;
                                pathfoto = System.IO.Path.GetDirectoryName(pathfoto);

                                // Create source (BitmapImage.UriSource must be in a BeginInit/EndInit block)
                                BitmapImage myBitmapImage = new BitmapImage();
                                myBitmapImage.BeginInit();
                                myBitmapImage.UriSource = new Uri(pathfoto + @"\FotosClientes\" + clientesVar.ToArray()[0].idCliente.ToString() + ".jpg");
                                myBitmapImage.EndInit();

                                image1.Source = myBitmapImage;
                                myBitmapImage.UriSource = null;
                            }

                            //  DispatcherTimer setup
                            //Console.WriteLine("Inicia timer: " + _TiempoApertura.ToString());
                            dispatcherTimer.Interval = new TimeSpan(0, 0, _TiempoApertura);
                            D0 = true;  //Habilitar entrada.
                            dispatcherTimer.Start();

                        }
                        catch (Exception ex)
                        {
                            ExceptionOccured = "PD0_Click(object sender, EventArgs e) called. ERROR occured is ---> " + ex.Message;
                            //MessageBox.Show("Ocurrio un error al intentar abrir el porton, por favor contacte con los tecnicos");

                            _Mensaje_activo = true;
                            this.label2_ResultadoIngreso.Content = "Ocurrió un error al intentar abrir el porton.";
                            this.labelMensajeCuotaVencida.Content = "Por favor contacte con los tecnicos";
                            dispatcherTimer.Interval = new TimeSpan(0, 0, _TiempoApertura);
                            dispatcherTimer.Start();
                        }


                    }
                    else // CUOTA VENCIDA. NO ESTA HABILITADO PARA ENTRAR. 
                    {
                        _Mensaje_activo = true;

                        // Seteamos  "Nombre:<NombreCliente>" 
                        this.labelNombre.Content = "Nombre:";
                        this.labelNombreCliente.Content = clientesVar.ToArray()[0].nombre.ToString();

                        // Seteamos  "Apellido:<ApellidoCliente>" 
                        this.labelApellido.Content = "Apellido:";
                        this.labelApellidoCliente.Content = clientesVar.ToArray()[0].apellido.ToString();

                        this.label2_ResultadoIngreso.Foreground = new SolidColorBrush(Colors.Red);
                        //this.label2_ResultadoIngreso.FontSize = 20;
                        this.label2_ResultadoIngreso.Content = "Su cuota ha vencido el " + fechaUltimoVencimientoResult.ToArray()[0].fecha_vencimiento.Value.ToShortDateString() + "!.";
                        this.labelMensajeCuotaVencida.Content = "Por favor realice el pago para poder acceder.";

                        //  DispatcherTimer setup
                        //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        dispatcherTimer.Interval = new TimeSpan(0, 0, _TiempoApertura);
                        dispatcherTimer.Start();

                    }
                }
                else
                {//TODAVIA NO TIENE NI UN PAGO EL CLIENTE.
                    //MessageBox.Show("Todavia no tiene ninguna cuota cargada, por favor consulte con recepcion.");
                    _Mensaje_activo = true;
                    this.centerlabel.Content = "Todavía no tiene ninguna cuota cargada, por favor consulte con recepción.";
                    dispatcherTimer.Interval = new TimeSpan(0, 0, _TiempoApertura);
                    dispatcherTimer.Start();
                }

            }
            this.textBox_Cedula.Text = "";
        }

    }
}
