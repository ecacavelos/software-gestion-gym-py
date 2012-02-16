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

        public VistaConfiguracion()
        {
            InitializeComponent();
            this.c2 = Configuration.Deserialize("config.xml");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            //TO DO: Testeo de si exsite el archivo de configuracion. 
            this.label_SegundosApertura.Content = c2.TiempoApertura.ToString();
        }

     
        

        private void slider_TiempoAperturaPorton_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsInitialized == true) 
            {
                if (this.c2.TiempoApertura !=(int)this.slider_TiempoAperturaPorton.Value)
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
            this.c2.TiempoApertura = (int)this.slider_TiempoAperturaPorton.Value;
            Configuration.Serialize("config.xml", this.c2);
            this.button_AceptarConfiguracion.IsEnabled = false;
        }
    }
}
