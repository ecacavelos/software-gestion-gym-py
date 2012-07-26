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
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine(ex);
                //System.Windows.MessageBox.Show("Se restablecieron las opciones a sus valores por defecto.\nPor favor vuelva a colocar los valores deseados.");
                this.c2 = new Gimnasio.Configuration();
                this.c2.TiempoApertura = 5;
                this.c2.MainDeviceID = "";
                Configuration.Serialize("config.xml", this.c2);
            }            
            Activate();
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


        //cerrar la ventana. 
        private void button_CancelarConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //escribir en el archivo de configuracion.
        private void button_AceptarConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            result = System.Windows.Forms.MessageBox.Show("Ha seleccionado " + this.label4.Content.ToString() + " como el teclado principal.", "Confirmar Configuración", System.Windows.Forms.MessageBoxButtons.OK);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.c2.TiempoApertura = (int)this.slider_TiempoAperturaPorton.Value;
                this.c2.MainDeviceID = this.label4.Content.ToString();
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

    }
}
