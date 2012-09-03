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
    /// Interaction logic for ConsultarPagosCliente.xaml
    /// </summary>
    public partial class ConsultarPagosCliente : Window
    {

        public ResourceDictionary Resources { get; set; }
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        Gimnasio.Database1Entities database1Entities2 = new Gimnasio.Database1Entities();

        private System.Data.Objects.DataClasses.EntityCollection<Pagos> pagosCliente;

        DataGridRow[] PagosRow001 = new DataGridRow[99999];

        public ConsultarPagosCliente()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data into Pagos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource pagosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pagosViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Pagos> pagosQuery = this.GetPagosQuery(database1Entities);
            pagosViewSource.Source = pagosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            textBoxNroCedula.Focus();
        }

        private void textBoxNroCedula_KeyDown(object sender, KeyEventArgs e)
        {
            // se presiono ENTER
            if (e.Key.ToString() == "Return")
            {
                string esql = "select value c from clientes as c where c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                var clientesVar = database1Entities.CreateQuery<clientes>(esql);

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

        private void unLoadingRow_Pagos(object sender, DataGridRowEventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            string esql = "select value c from clientes as c where c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
            var clientesVar = database1Entities2.CreateQuery<clientes>(esql);


            if (this.pagosCliente.Count < clientesVar.ToArray()[0].Pagos.Count)
            {

                result = System.Windows.Forms.MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNoCancel);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // ver los controles necesarios. 
                    database1Entities.SaveChanges();
                }
                else
                {
                    if (result == System.Windows.Forms.DialogResult.No)
                    {   // No hacer nada y poner de vuelta los elementos.
                        database1Entities = new Gimnasio.Database1Entities();
                        string esql2 = "select value c from clientes as c where c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                        var clientesVar2 = database1Entities.CreateQuery<clientes>(esql2);
                        this.pagosCliente = clientesVar2.ToArray()[0].Pagos;
                        this.dataGridPagos.ItemsSource = clientesVar2.ToArray()[0].Pagos;
                    }
                }
            }
        }

        private void PrintFactura(object sender, RoutedEventArgs e)
        {
            Pagos pago = ((FrameworkElement)sender).DataContext as Pagos;
            Facturacion.DatosFactura(pago);

            // Se actualiza el Datagrid, para que se refleje que ya se facturó el pago.
            database1Entities = new Gimnasio.Database1Entities();
            string esql3 = "select value c from clientes as c where c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
            var clientesVar3 = database1Entities.CreateQuery<clientes>(esql3);
            this.dataGridPagos.ItemsSource = clientesVar3.ToArray()[0].Pagos;
        }

        private void dataGridPagos_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //if (e.Row.GetIndex() == this.pagosCliente.Count)
            //if (e.Row.GetIndex() == 0)
            //{
            //System.Console.WriteLine("Ouch " + this.dataGridPagos.Columns[1].GetCellContent(e.Row));
            //e.Row.Background = Brushes.Beige;                                
            //System.Console.WriteLine("xxx " + this.buttonColumn.CellTemplate.LoadContent().GetValue(ContentProperty));                                
            //}
        }

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
