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

using System.Globalization;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VistaConsultarPagos.xaml
    /// </summary>
    public partial class VistaConsultarPagos : Window
    {
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        Gimnasio.Database1Entities database1Entities2 = new Gimnasio.Database1Entities();
        public static bool IsOpen { get; private set; }

        private System.Data.Objects.DataClasses.EntityCollection<Pagos> pagosCliente;

        DataGridRow[] PagosRow001 = new DataGridRow[99999];

        System.Data.Objects.ObjectQuery<clientes> clientesVar;
        private int cantidadPagosSeleccionados = 0;
        private List<Pagos> arrayPagosSeleccionados = new List<Pagos>();

        public VistaConsultarPagos()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectQuery<Pagos> GetPagosQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.Pagos> pagosQuery = database1Entities.Pagos;
            // To explicitly load data, you may need to add Include methods like below:
            // pagosQuery = pagosQuery.Include("Pagos.clientes").
            // For more information, please see http://go.microsoft.com/fwlink/?LinkId=157380
            // Returns an ObjectQuery.
            return pagosQuery;
        }

        #region "Funciones Manejadoras de Carga y Descarga de la Ventana"
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            buttonFacturasMultiples.IsEnabled = false;
            // Load data into Pagos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource pagosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pagosViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Pagos> pagosQuery = this.GetPagosQuery(database1Entities);
            pagosViewSource.Source = pagosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            textBoxNroCedula.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
        #endregion

        private void textBoxNroCedula_KeyDown(object sender, KeyEventArgs e)
        {
            // se presiono ENTER
            if (e.Key.ToString() == "Return")
            {
                string esql = "SELECT value c FROM clientes as c WHERE c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                clientesVar = database1Entities.CreateQuery<clientes>(esql);

                //Chequear si existe un cliente con ese nro. de Cedula. 
                if (clientesVar.ToList().Count == 0)// NO HAY NADA. 
                {
                    // NO hacer nada. 
                }
                else //HAY ALGO. OJO: Puede que haya mas de un cliente, porque en la base de datos no esta el nro. de cedula como unico. 
                {
                    if (clientesVar.ToList().Count == 1)// Solo se selecciono un cliente, esta correcto
                    {
                        this.textBoxNombre.Text = clientesVar.ToArray()[0].nombre;
                        this.textBoxApellido.Text = clientesVar.ToArray()[0].apellido;
                        this.pagosCliente = clientesVar.ToArray()[0].Pagos;
                        this.dataGridPagos.ItemsSource = clientesVar.ToArray()[0].Pagos;
                    }
                }
            }
            else
            {
                //System.Console.WriteLine("se presiono cualquier otra tecla que no es Return");
            }

        }

        // Lógica para manejar el borrado de Pagos.
        private void unLoadingRow_Pagos(object sender, DataGridRowEventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            string esql = "SELECT value c FROM clientes as c WHERE c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
            var clientesVar = database1Entities2.CreateQuery<clientes>(esql);

            if (this.pagosCliente.Count < clientesVar.ToArray()[0].Pagos.Count)
            {
                result = System.Windows.Forms.MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNoCancel);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // Ver los controles necesarios. 
                    database1Entities.SaveChanges();
                }
                else
                {
                    if (result == System.Windows.Forms.DialogResult.No)
                    {   // No hacer nada y poner de vuelta los elementos.
                        database1Entities = new Gimnasio.Database1Entities();
                        string esql2 = "SELECT value c FROM clientes as c WHERE c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                        var clientesVar2 = database1Entities.CreateQuery<clientes>(esql2);
                        this.pagosCliente = clientesVar2.ToArray()[0].Pagos;
                        this.dataGridPagos.ItemsSource = clientesVar2.ToArray()[0].Pagos;
                    }
                }
            }
        }

        #region "Funciones relativas a la Facturación de Pagos en esta Ventana"

        private void PrintFactura(object sender, RoutedEventArgs e)
        {
            Pagos pago = ((FrameworkElement)sender).DataContext as Pagos;

            Pagos[] pagosSeleccionados = new Pagos[1];
            pagosSeleccionados[0] = pago;

            Facturacion.DatosFactura(pagosSeleccionados, pago.clientes);

            // Se actualiza el Datagrid, para que se refleje que ya se facturó el pago.
            database1Entities = new Gimnasio.Database1Entities();
            string esql3 = "SELECT value c FROM clientes as c WHERE c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
            var clientesVar3 = database1Entities.CreateQuery<clientes>(esql3);
            this.dataGridPagos.ItemsSource = clientesVar3.ToArray()[0].Pagos;
        }

        private void checkBoxAddToFactura_Checked(object sender, RoutedEventArgs e)
        {
            cantidadPagosSeleccionados++;
            if (cantidadPagosSeleccionados <= 3)
            {
                Pagos pago = ((FrameworkElement)sender).DataContext as Pagos;
                arrayPagosSeleccionados.Add(pago);

                buttonColumn.Visibility = Visibility.Hidden;
                buttonFacturasMultiples.IsEnabled = true;
            }
            else
            {
                ((CheckBox)sender).IsChecked = false;
            }
            //System.Console.WriteLine(cantidadPagosSeleccionados.ToString());
        }

        private void checkBoxAddToFactura_Unchecked(object sender, RoutedEventArgs e)
        {
            Pagos pago = ((FrameworkElement)sender).DataContext as Pagos;

            foreach (Pagos tempPago in arrayPagosSeleccionados)
            {
                if (tempPago.idPago == pago.idPago)
                {
                    arrayPagosSeleccionados.Remove(tempPago);
                    break;
                }
            }

            if (cantidadPagosSeleccionados > 0)
            {
                cantidadPagosSeleccionados--;
            }
            if (cantidadPagosSeleccionados == 0)
            {
                buttonColumn.Visibility = Visibility.Visible;
                buttonFacturasMultiples.IsEnabled = false;
            }

            //System.Console.WriteLine(cantidadPagosSeleccionados.ToString());
        }

        private void buttonFacturasMultiples_Click(object sender, RoutedEventArgs e)
        {
            /*foreach (Pagos tempPago in arrayPagosSeleccionados)
            {
                System.Console.WriteLine(tempPago.idPago);
            }*/
            Pagos[] pagosSeleccionados = arrayPagosSeleccionados.ToArray();

            if (Facturacion.DatosFactura(pagosSeleccionados, pagosSeleccionados[0].clientes))
            {
                // Se actualiza el Datagrid, para que se refleje que ya se facturó el pago.
                database1Entities = new Gimnasio.Database1Entities();
                string esql3 = "SELECT value c FROM clientes as c WHERE c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                var clientesVar3 = database1Entities.CreateQuery<clientes>(esql3);
                this.dataGridPagos.ItemsSource = clientesVar3.ToArray()[0].Pagos;

                arrayPagosSeleccionados.Clear();
                cantidadPagosSeleccionados = 0;
                buttonColumn.Visibility = Visibility.Visible;
                buttonFacturasMultiples.IsEnabled = false;
            }

        }

        #endregion

        #region "Funciones relativas al Keypad USB"
        // Funciones para evitar que el keypad USB afecte los controles de esta ventana.
        private void textBoxNroCedula_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }
        #endregion

    }

    #region "Conversores para los data bindings de esta ventana."
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            bool visibility = (bool)value;
            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility == Visibility.Visible);
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
    #endregion

}
