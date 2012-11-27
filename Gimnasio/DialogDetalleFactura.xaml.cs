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
    /// Interaction logic for DialogDetalleFactura.xaml
    /// </summary>
    public partial class DialogDetalleFactura : Window
    {
        public DialogDetalleFactura(Facturas factura, Pagos[] pagos)
        {
            InitializeComponent();

            // Asignamos los Pagos de la Factura en cuestión al DataGrid para ser mostrados.
            dataGridFacturaDetalle.ItemsSource = pagos;

            // Mostramos los detalles relativos a la Factura.
            labelCliente.Content = factura.clientes.nombre + " " + factura.clientes.apellido;
            labelRUC.Content = factura.RUC_Pagador.ToString();
            labelNroFactura.Content = factura.Nro_Factura.ToString();
            labelFechaEmision.Content = factura.Fecha_Emision.ToString();
            labelMontoTotal.Content = "Monto Total: " + ((int)(factura.Monto_Total)).ToString("#,##0");

            // Indicamos si la Factura está anulada.
            if(factura.Anulada){
                labelAnulada.Visibility = Visibility.Visible;
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
