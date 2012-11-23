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
    /// Interaction logic for VistaConfiguracionFacturas.xaml
    /// </summary>
    public partial class VistaConfiguracionFacturas : Window
    {
        Configuration c2;
        public static bool IsOpen { get; private set; }

        public VistaConfiguracionFacturas()
        {
            this.c2 = Configuration.Deserialize("config.xml");
            InitializeComponent();

            // Leemos los datos de Coordenadas del Archivo de Configuración e inicializamos los cuadros de texto.

            textBox_yFecha.Text = this.c2.CoordenadasImpresion.yFecha.ToString();
            textBox_xFechaDia.Text = this.c2.CoordenadasImpresion.xFechaDia.ToString();
            textBox_xFechaMes.Text = this.c2.CoordenadasImpresion.xFechaMes.ToString();
            textBox_xFechaAño.Text = this.c2.CoordenadasImpresion.xFechaAño.ToString();

            textBox_xContadoCredito.Text = this.c2.CoordenadasImpresion.xContadoCredito.ToString();

            textBox_xNombre.Text = this.c2.CoordenadasImpresion.xNombre.ToString();
            textBox_yNombre.Text = this.c2.CoordenadasImpresion.yNombre.ToString();
            textBox_xRUC.Text = this.c2.CoordenadasImpresion.xRUC.ToString();
            textBox_yRUC.Text = this.c2.CoordenadasImpresion.yRUC.ToString();

            textBox_yItem.Text = this.c2.CoordenadasImpresion.yItem.ToString();
            textBox_xItemCant.Text = this.c2.CoordenadasImpresion.xItemCant.ToString();
            textBox_xItemConcepto.Text = this.c2.CoordenadasImpresion.xItemConcepto.ToString();
            textBox_xItemIVAExentas.Text = this.c2.CoordenadasImpresion.xItemIVAExentas.ToString();
            textBox_xItemIVA05.Text = this.c2.CoordenadasImpresion.xItemIVA05.ToString();
            textBox_xItemIVA10.Text = this.c2.CoordenadasImpresion.xItemIVA10.ToString();

            textBox_ySubTotal.Text = this.c2.CoordenadasImpresion.ySubTotal.ToString();
            textBox_xSubTotalIVAExentas.Text = this.c2.CoordenadasImpresion.xSubTotalIVAExentas.ToString();
            textBox_xSubTotalIVA05.Text = this.c2.CoordenadasImpresion.xSubTotalIVA05.ToString();
            textBox_xSubTotalIVA10.Text = this.c2.CoordenadasImpresion.xSubTotalIVA10.ToString();

            textBox_xTotalPagar.Text = this.c2.CoordenadasImpresion.xTotalPagar.ToString();
            textBox_xTotalEnLetras.Text = this.c2.CoordenadasImpresion.xTotalEnLetras.ToString();
            textBox_yTotalEnLetras.Text = this.c2.CoordenadasImpresion.yTotalEnLetras.ToString();

            textBox_yTotal.Text = this.c2.CoordenadasImpresion.yTotal.ToString();
            textBox_xTotalIVA05.Text = this.c2.CoordenadasImpresion.xTotalIVA05.ToString();
            textBox_xTotalIVA10.Text = this.c2.CoordenadasImpresion.xTotalIVA10.ToString();
            textBox_xTotalIVAGeneral.Text = this.c2.CoordenadasImpresion.xTotalIVAGeneral.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void buttonAceptar_Click(object sender, RoutedEventArgs e)
        {
            // Creamos una estructura temporal para leer los datos introducidos en los cuadros de texto.
            Gimnasio.Configuration.ImpresionCoords myCoordsStruct = new Gimnasio.Configuration.ImpresionCoords();

            int tempEntero = 0;
            // Cargamos esta estructura con los datos introducidos en los cuadros de texto,
            // de modo a escribir en el archivo de configuración.            
            Int32.TryParse(textBox_yFecha.Text, out tempEntero);
            myCoordsStruct.yFecha = tempEntero;
            Int32.TryParse(textBox_xFechaDia.Text, out tempEntero);
            myCoordsStruct.xFechaDia = tempEntero;
            Int32.TryParse(textBox_xFechaMes.Text, out tempEntero);
            myCoordsStruct.xFechaMes = tempEntero;
            Int32.TryParse(textBox_xFechaAño.Text, out tempEntero);
            myCoordsStruct.xFechaAño = tempEntero;
            Int32.TryParse(textBox_xContadoCredito.Text, out tempEntero);
            myCoordsStruct.xContadoCredito = tempEntero;
            Int32.TryParse(textBox_xNombre.Text, out tempEntero);
            myCoordsStruct.xNombre = tempEntero;
            Int32.TryParse(textBox_yNombre.Text, out tempEntero);
            myCoordsStruct.yNombre = tempEntero;
            Int32.TryParse(textBox_xRUC.Text, out tempEntero);
            myCoordsStruct.xRUC = tempEntero;
            Int32.TryParse(textBox_yRUC.Text, out tempEntero);
            myCoordsStruct.yRUC = tempEntero;
            Int32.TryParse(textBox_yItem.Text, out tempEntero);
            myCoordsStruct.yItem = tempEntero;
            Int32.TryParse(textBox_xItemCant.Text, out tempEntero);
            myCoordsStruct.xItemCant = tempEntero;
            Int32.TryParse(textBox_xItemConcepto.Text, out tempEntero);
            myCoordsStruct.xItemConcepto = tempEntero;
            Int32.TryParse(textBox_xItemIVAExentas.Text, out tempEntero);
            myCoordsStruct.xItemIVAExentas = tempEntero;
            Int32.TryParse(textBox_xItemIVA05.Text, out tempEntero);
            myCoordsStruct.xItemIVA05 = tempEntero;
            Int32.TryParse(textBox_xItemIVA10.Text, out tempEntero);
            myCoordsStruct.xItemIVA10 = tempEntero;
            Int32.TryParse(textBox_ySubTotal.Text, out tempEntero);
            myCoordsStruct.ySubTotal = tempEntero;
            Int32.TryParse(textBox_xSubTotalIVAExentas.Text, out tempEntero);
            myCoordsStruct.xSubTotalIVAExentas = tempEntero;
            Int32.TryParse(textBox_xSubTotalIVA05.Text, out tempEntero);
            myCoordsStruct.xSubTotalIVA05 = tempEntero;
            Int32.TryParse(textBox_xSubTotalIVA10.Text, out tempEntero);
            myCoordsStruct.xSubTotalIVA10 = tempEntero;
            Int32.TryParse(textBox_xTotalPagar.Text, out tempEntero);
            myCoordsStruct.xTotalPagar = tempEntero;
            Int32.TryParse(textBox_xTotalEnLetras.Text, out tempEntero);
            myCoordsStruct.xTotalEnLetras = tempEntero;
            Int32.TryParse(textBox_yTotalEnLetras.Text, out tempEntero);
            myCoordsStruct.yTotalEnLetras = tempEntero;
            Int32.TryParse(textBox_yTotal.Text, out tempEntero);
            myCoordsStruct.yTotal = tempEntero;
            Int32.TryParse(textBox_xTotalIVA05.Text, out tempEntero);
            myCoordsStruct.xTotalIVA05 = tempEntero;
            Int32.TryParse(textBox_xTotalIVA10.Text, out tempEntero);
            myCoordsStruct.xTotalIVA10 = tempEntero;
            Int32.TryParse(textBox_xTotalIVAGeneral.Text, out tempEntero);
            myCoordsStruct.xTotalIVAGeneral = tempEntero;

            this.c2.CoordenadasImpresion = myCoordsStruct;

            Configuration.Serialize("config.xml", this.c2);
            this.buttonAceptar.IsEnabled = false;
        }

        // Cerrar la ventana si se hace click en el botón 'Cancelar'.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
