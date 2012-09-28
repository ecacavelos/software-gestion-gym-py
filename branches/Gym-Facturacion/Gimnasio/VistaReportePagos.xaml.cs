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
    /// Interaction logic for VistaReportePagos.xaml
    /// </summary>
    public partial class VistaReportePagos : Window
    {

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        public static bool IsOpen { get; private set; }

        int timestampDesde;
        int timestampHasta;

        public VistaReportePagos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Load data into Pagos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource pagosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pagosViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Pagos> pagosQuery = this.GetPagosQuery(database1Entities);
            pagosViewSource.Source = pagosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            string esql = "SELECT value p FROM Pagos as p";
            var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
            labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
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

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGrid temp = (DataGrid)sender;
            try
            {
                Gimnasio.Pagos selectedPago = (Gimnasio.Pagos)temp.CurrentCell.Item;
                DateTime fechaIngresoPago = FromUnixTime((long)selectedPago.idPago);
                fechaIngresoPago = fechaIngresoPago.AddHours(-4);
                System.Console.WriteLine(fechaIngresoPago + " (" + selectedPago.idPago + ")");
            }
            catch
            {
                System.Console.WriteLine("No hay una celda seleccionada?");
            }
        }

        #region "Funciones relativas a los CheckBox y al Filtrado de Pagos"
        /* Con los CheckBox se elige el criterio de filtrado, con los DatePicker se seleccionan los rangos,
           y con el Botón se ejecuta dicha busqueda. En caso de destachar ambos CheckBox, se vuelven a mostrar
           todos los pagos. */

        private void checkBoxDesde_Checked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = true;
            buttonBuscarPagos.IsEnabled = true;
        }

        private void checkBoxDesde_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = false;
            if (checkBoxHasta.IsChecked == false)
            {
                buttonBuscarPagos.IsEnabled = false;
                eliminarFiltros();
            }
        }

        private void checkBoxHasta_Checked(object sender, RoutedEventArgs e)
        {
            datePickerHasta.IsEnabled = true;
            buttonBuscarPagos.IsEnabled = true;
        }

        private void checkBoxHasta_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerHasta.IsEnabled = false;
            if (checkBoxDesde.IsChecked == false)
            {
                buttonBuscarPagos.IsEnabled = false;
                eliminarFiltros();
            }
        }

        private void datePickerDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan epochTime = ((DateTime)datePickerDesde.SelectedDate + new TimeSpan(4, 0, 0) - new DateTime(1970, 1, 1));
            timestampDesde = (int)epochTime.TotalSeconds;
            System.Console.WriteLine(datePickerDesde.SelectedDate + " (" + timestampDesde + ")");
        }

        private void datePickerHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan epochTime = ((DateTime)datePickerHasta.SelectedDate + new TimeSpan(4, 0, 0) - new DateTime(1970, 1, 1));
            timestampHasta = (int)epochTime.TotalSeconds;
            System.Console.WriteLine(datePickerHasta.SelectedDate + " (" + timestampHasta + ")");
        }

        private void buttonBuscarPagos_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerDesde.IsEnabled == true)
            {
                if (datePickerHasta.IsEnabled == true)
                {
                    string esql = "SELECT value p FROM Pagos as p WHERE (p.idPago > " + timestampDesde + ") AND (p.idPago < " + timestampHasta + ")";
                    var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                    labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();
                    dataGridPagos.ItemsSource = pagosVar;
                }
                else
                {
                    if (datePickerDesde.SelectedDate == null)
                    {
                    }
                    else
                    {
                        string esql = "SELECT value p FROM Pagos as p WHERE p.idPago > " + timestampDesde;
                        var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                        labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();
                        dataGridPagos.ItemsSource = pagosVar;
                    }
                }
            }
            else
            {
                if (datePickerHasta.SelectedDate == null)
                {
                }
                else
                {
                    string esql = "SELECT value p FROM Pagos as p WHERE p.idPago < " + timestampHasta;
                    var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                    labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();
                    dataGridPagos.ItemsSource = pagosVar;
                }
            }
        }

        private void eliminarFiltros()
        {
            timestampDesde = 0;
            timestampHasta = 0;

            string esql = "SELECT value p FROM Pagos as p";
            var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
            labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();
            dataGridPagos.ItemsSource = pagosVar;
        }

        #endregion

        // Funcion que Convierte un long int desde UnixTime a una estructura DateTime
        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

    }
}
