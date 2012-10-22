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
using System.Globalization;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VistaReportePagos.xaml
    /// </summary>
    public partial class VistaReportePagos : Window
    {
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        public static bool IsOpen { get; private set; }

        int timestampDesde;
        int timestampHasta;
        string[] arrayClientesID;

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

            eliminarFiltros(true);
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

        private void dataGridPagos_CurrentCellChanged(object sender, EventArgs e)
        {
            /*DataGrid temp = (DataGrid)sender;
            try
            {
                Gimnasio.Pagos selectedPago = (Gimnasio.Pagos)temp.CurrentCell.Item;
                DateTime fechaIngresoPago = FromUnixTime((long)selectedPago.idPago);
                fechaIngresoPago = fechaIngresoPago.AddHours(-4);
                System.Console.WriteLine(fechaIngresoPago + " (" + selectedPago.idPago + ")");
            }
            catch
            {
            }*/
        }

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region "Funciones relativas a los CheckBox"
        /* Con los CheckBox se elige el criterio de filtrado, con los DatePicker se seleccionan los rangos.
         * En caso de destachar ambos CheckBox, se vuelven a mostrar todos los pagos. */

        private void checkBoxDesde_Checked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = true;
            buttonBuscarPagos.IsEnabled = true;
        }

        private void checkBoxDesde_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = false;
            if (checkBoxHasta.IsChecked == false && checkBoxNombre.IsChecked == false)
            {
                buttonBuscarPagos.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
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
            if (checkBoxDesde.IsChecked == false && checkBoxNombre.IsChecked == false)
            {
                buttonBuscarPagos.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
            }
        }

        private void checkBoxNombre_Checked(object sender, RoutedEventArgs e)
        {
            autoCompleteTextBoxNombre.IsEnabled = true;
            buttonBuscarPagos.IsEnabled = true;
            autoCompleteTextBoxNombre.FocusTextBox();
        }

        private void checkBoxNombre_Unchecked(object sender, RoutedEventArgs e)
        {
            autoCompleteTextBoxNombre.IsEnabled = false;
            autoCompleteTextBoxNombre.Text = "";
            if (checkBoxDesde.IsChecked == false && checkBoxHasta.IsChecked == false)
            {
                buttonBuscarPagos.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
            }
        }

        private void datePickerDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan epochTime = ((DateTime)datePickerDesde.SelectedDate + new TimeSpan(4, 0, 0) - new DateTime(1970, 1, 1));
            timestampDesde = (int)epochTime.TotalSeconds;
            //System.Console.WriteLine(datePickerDesde.SelectedDate + " (" + timestampDesde + ")");
        }

        private void datePickerHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan epochTime = ((DateTime)datePickerHasta.SelectedDate + new TimeSpan(28, 0, 0) - new DateTime(1970, 1, 1));
            timestampHasta = (int)epochTime.TotalSeconds;
            //System.Console.WriteLine(datePickerHasta.SelectedDate + " (" + timestampHasta + ")");
        }

        #endregion

        #region "Funciones relativas al Filtrado de Pagos"

        private void buttonBuscarPagos_Click(object sender, RoutedEventArgs e)
        {
            aplicarFiltros();
        }

        private void aplicarFiltros()
        {
            int sumatoriaMonto = 0;
            int tempMontoCuota = 0;

            string esql = "SELECT value p FROM Pagos as p";

            if (autoCompleteTextBoxNombre.IsEnabled == true)
            {
                string nombreBuscado = autoCompleteTextBoxNombre.Text;

                int totalHits = 0;
                int foundID = 0;
                foreach (string cacheClientes in arrayClientesID)
                {
                    if (cacheClientes.ToLower().Contains(nombreBuscado.ToLower()))
                    {
                        int startID = cacheClientes.IndexOf('(') + 1;
                        int endID = cacheClientes.IndexOf(')');
                        int lengthID = endID - startID;
                        Int32.TryParse(cacheClientes.Substring(startID, lengthID), out foundID);
                        totalHits++;
                    }
                }

                esql += " WHERE ";

                if (totalHits > 1)
                {
                    if (nombreBuscado.Contains(" "))
                    {
                        string temp_nombre = nombreBuscado.Substring(0, nombreBuscado.IndexOf(" ")).Trim().ToLower();
                        string temp_apellido = nombreBuscado.Substring(nombreBuscado.LastIndexOf(" "), nombreBuscado.Length - nombreBuscado.LastIndexOf(" ")).Trim().ToLower();
                        System.Console.WriteLine(temp_nombre + " - " + temp_apellido);
                        esql += String.Format("(p.clientes.nombre LIKE '%{0}%' AND p.clientes.apellido LIKE '%{1}%')", temp_nombre, temp_apellido);
                    }
                    else
                    {
                        esql += String.Format("(p.clientes.nombre LIKE '%{0}%' OR p.clientes.apellido LIKE '%{0}%')", nombreBuscado.ToLower());
                    }
                    /*labelSatusBar.Content = "Existe más de un cliente con ese nombre.";
                    return;*/
                }
                else if (totalHits == 1)
                {
                    esql += String.Format("(p.clientes.idCliente = {0})", foundID);
                }

            }

            if (datePickerDesde.IsEnabled == true)
            {
                if (datePickerDesde.SelectedDate != null)
                {
                    if (esql.Contains("WHERE"))
                    {
                        esql += " AND ";
                    }
                    else
                    {
                        esql += " WHERE ";
                    }
                    esql += String.Format("(p.idPago >= {0})", timestampDesde);
                }
            }

            if (datePickerHasta.IsEnabled == true)
            {
                if (datePickerHasta.SelectedDate != null)
                {
                    if (esql.Contains("WHERE"))
                    {
                        esql += " AND ";
                    }
                    else
                    {
                        esql += " WHERE ";
                    }
                    esql += String.Format("(p.idPago <= {0})", timestampHasta);
                }
            }

            //System.Console.WriteLine(esql);

            var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
            labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();

            // Calculamos el total de los montos de las cuotas resultantes del filtro.
            foreach (Gimnasio.Pagos tempPago in pagosVar.ToArray())
            {
                Int32.TryParse(tempPago.Cuotas.monto, out tempMontoCuota);
                sumatoriaMonto += tempMontoCuota;
            }

            labelMontoTotal.Content = sumatoriaMonto.ToString("#,##0");
            dataGridPagos.ItemsSource = pagosVar;

            if (pagosVar.ToList().Count > 0)
            {
                labelSatusBar.Content = "Búsqueda Completa.";
            }
            else
            {
                labelSatusBar.Content = "La búsqueda no arrojó resultados.";
            }

        }

        private void eliminarFiltros(bool firstTime)
        {
            int sumatoriaMonto = 0;
            int tempMontoCuota = 0;

            string esql = "SELECT value p FROM Pagos as p";
            var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
            labelCantidadPagos.Content = pagosVar.ToList().Count.ToString();

            // Calculamos el total de los montos de las cuotas resultantes del filtro.
            foreach (Gimnasio.Pagos tempPago in pagosVar.ToArray())
            {
                Int32.TryParse(tempPago.Cuotas.monto, out tempMontoCuota);
                sumatoriaMonto += tempMontoCuota;
            }
            labelMontoTotal.Content = sumatoriaMonto.ToString("#,##0");
            dataGridPagos.ItemsSource = pagosVar;

            if (firstTime)
            {
                string esql_clientes = "SELECT value c FROM clientes as c WHERE (c.nro_cedula IS NOT null)";
                var clientesVar = database1Entities.CreateQuery<clientes>(esql_clientes);

                //System.Console.WriteLine(clientesVar.ToList().Count.ToString() + " clientes.");

                int i = 0;
                arrayClientesID = new string[clientesVar.ToList().Count];
                foreach (Gimnasio.clientes tempCliente in clientesVar.ToArray())
                {
                    if (tempCliente.nombre != null)
                    {
                        arrayClientesID[i] += tempCliente.nombre;
                        if (tempCliente.apellido != null)
                        {
                            arrayClientesID[i] += " " + tempCliente.apellido;
                        }
                    }
                    arrayClientesID[i] += " (" + tempCliente.idCliente + ")";
                    //arrayClientesID[i] = tempCliente.nombre + " " + tempCliente.apellido + " (" + tempCliente.idCliente + ")";
                    autoCompleteTextBoxNombre.AddItem(new WPFAutoCompleteTextbox.AutoCompleteEntry(tempCliente.nombre + " " + tempCliente.apellido, tempCliente.nombre, tempCliente.apellido));
                    i++;

                }
            }

        }

        #endregion

        // Funcion que Convierte un long int de UnixTime a una estructura DateTime
        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        #region "Funciones relativas al Keypad USB"

        private void autoCompleteTextBoxNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
            else
            {
                if (e.Key.ToString() == "Up")
                {
                    if (autoCompleteTextBoxNombre.SuggestionListActive())
                    {
                        autoCompleteTextBoxNombre.ChangeComboBoxIndexUp();
                    }
                    e.Handled = true;
                }
                if (e.Key.ToString() == "Down")
                {
                    if (autoCompleteTextBoxNombre.SuggestionListActive())
                    {
                        autoCompleteTextBoxNombre.ChangeComboBoxIndexDown();
                    }
                    e.Handled = true;
                }
                if (e.Key.ToString() == "Return")
                {
                    if (autoCompleteTextBoxNombre.SuggestionListActive())
                    {
                        autoCompleteTextBoxNombre.FocusTextBox();
                    }
                    else
                    {
                        buttonBuscarPagos_Click(buttonBuscarPagos, null);
                    }
                }
            }
        }

        #endregion

    }

    #region "Conversores para los data bindings de esta ventana."
    [ValueConversion(typeof(long), typeof(string))]
    public class KeytoDateStringConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            //if (targetType != typeof(bool))
            //throw new InvalidOperationException("The target must be a boolean");           
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) - new TimeSpan(4, 0, 0);
            return epoch.AddSeconds((long)value).ToString("dd/MM/yyyy");
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
