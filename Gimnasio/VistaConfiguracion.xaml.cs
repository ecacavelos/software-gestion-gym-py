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
    /// Interaction logic for VistaConfiguracion.xaml
    /// </summary>
    public partial class VistaConfiguracion : Window
    {
        Configuration c2;
        public static bool IsOpen { get; private set; }

        public VistaConfiguracion()
        {
            InitializeComponent();
            try
            {
                this.c2 = Configuration.Deserialize("config.xml");
                this.label4.Content = this.c2.MainDeviceID;
                this.ip1.Text = this.c2.ip1;
                this.ip2.Text = this.c2.ip2;
                this.ip3.Text = this.c2.ip3;
                this.ip4.Text = this.c2.ip4;
                this.txtPuerto.Text = this.c2.puerto;
                if (Marcador.conected)
                {
                    label8.Content = "Conectado";
                    label8.Foreground = new SolidColorBrush(Colors.Green);
                    connectionBtn.Content = "Desconectar";
                }
                else
                {
                    label8.Content = "Desconectado";
                    label8.Foreground = new SolidColorBrush(Colors.Red);
                    connectionBtn.Content = "Conectar";
                }

            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Console.WriteLine(ex.Message);
                //System.Windows.MessageBox.Show("Se restablecieron las opciones a sus valores por defecto.\nPor favor vuelva a colocar los valores deseados.");
                this.c2 = new Gimnasio.Configuration();
                this.c2.TiempoApertura = 5;
                this.c2.MainDeviceID = "";
                Configuration.Serialize("config.xml", this.c2);
            }
            Activate();
        }
        //VARIABLES DEL MARCADOR
        //El valor identifica cuando el dispositivo esta conectado
        private bool bIsConnected = false;
        //Create Standalone SDK class dynamicly.
        public zkemkeeper.CZKEM axCZKEM1 = Marcador.NewZkInstance();

        private bool nonNumberEntered = false;

        private void onBlur(object sender, RoutedEventArgs e)
        {
            ip2.Focus();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            IsOpen = true;
            //TO DO: Testeo de si exsite el archivo de configuracion. 
            this.label_SegundosApertura.Content = c2.TiempoApertura.ToString();
        }

        private void slider_TiempoAperturaPorton_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsInitialized == true)
            {
                if (this.c2.TiempoApertura != (int)this.slider_TiempoAperturaPorton.Value)
                {
                    this.button_AceptarConfiguracion.IsEnabled = true;
                }
                else
                {
                    this.button_AceptarConfiguracion.IsEnabled = false;
                }
            }
        }

        // Cerrar la ventana. 
        private void button_CancelarConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Escribir en el archivo de configuracion.
        private void button_AceptarConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            result = System.Windows.Forms.MessageBox.Show("Ha seleccionado " + this.label4.Content.ToString() + " como el teclado principal.", "Confirmar Configuración", System.Windows.Forms.MessageBoxButtons.OK);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.c2.TiempoApertura = (int)this.slider_TiempoAperturaPorton.Value;
                this.c2.MainDeviceID = this.label4.Content.ToString();
                this.c2.ip1 = this.ip1.Text;
                this.c2.ip2 = this.ip2.Text;
                this.c2.ip3 = this.ip3.Text;
                this.c2.ip4 = this.ip4.Text;
                this.c2.puerto = this.txtPuerto.Text;
                Configuration.Serialize("config.xml", this.c2);
                this.button_AceptarConfiguracion.IsEnabled = false;
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #region "Funciones relativas al Keypad USB"
        // Funciones para evitar que el keypad USB afecte los controles de esta ventana.
        private void button_AceptarConfiguracion_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Funciones relativas al marcador
        private void txtPuerto_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            result = System.Windows.Forms.MessageBox.Show("txtPuerto_KeyDown", "Alerta", System.Windows.Forms.MessageBoxButtons.OK);
        }
        private void connectionBtn_Click(object sender, RoutedEventArgs e)
        {
            String msg = "";
            System.Windows.Forms.DialogResult result;
            if (ip1.Text.Equals("") || ip2.Text.Equals("") || ip3.Text.Equals("") || ip4.Text.Equals(""))
            {
                if (txtPuerto.Text.Equals(""))
                {
                    msg = "La IP y el puerto no pueden contener valores vacíos";
                }
                else
                {
                    msg = "La IP no puede contener valores vacíos";
                }
                result = System.Windows.Forms.MessageBox.Show(msg, "Alerta", System.Windows.Forms.MessageBoxButtons.OK);
            }
            else
            {
                if (txtPuerto.Text.Equals(""))
                {
                    msg = "El puerto no puede contener valores vacíos";
                    result = System.Windows.Forms.MessageBox.Show(msg, "Alerta", System.Windows.Forms.MessageBoxButtons.OK);
                }
                else
                {
                    String ip = ip1.Text + "." + ip2.Text + "." + ip3.Text + "." + ip4.Text;
                    String puerto = txtPuerto.Text;
                    int idwErrorCode = 0;

                    //Cursor = Cursors.Wait;
                    //Si el marcador esta desconectado
                    if (!Marcador.conected)
                    {
                        //Conecta el marcador
                        bIsConnected = Marcador.connectMarcador(ip, puerto);
                        if (!bIsConnected)
                        {
                            axCZKEM1.GetLastError(ref idwErrorCode);
                            MessageBox.Show("Imposible conectar el dispositivo. Verifique la configuración de la red", "Error");
                        }
                    }
                    else
                    {
                        //Si no esta conectado 
                        //Desconecta el marcador
                        Marcador.desconectar();
                        label8.Foreground = new SolidColorBrush(Colors.Red);
                        bIsConnected = Marcador.conected;
                    }

                    if (bIsConnected)
                    {
                        label8.Content = "Conectado";
                        label8.Foreground = new SolidColorBrush(Colors.Green);
                        connectionBtn.Content = "Desconectar";
                        Marcador.conected = true;
                        this.button_AceptarConfiguracion.IsEnabled = true;
                    }
                    else
                    {
                        label8.Content = "Desconectado";
                        new SolidColorBrush(Colors.Red);
                        connectionBtn.Content = "Conectar";
                        Marcador.conected = false;
                    }
                    //Cursor = Cursors.None;
                }

            }
        }
        #endregion

    }
}
