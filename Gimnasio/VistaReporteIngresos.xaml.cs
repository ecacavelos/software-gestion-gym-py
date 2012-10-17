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
    /// Interaction logic for VistaReporteIngresos.xaml
    /// </summary>
    public partial class VistaReporteIngresos : Window
    {

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        public static bool IsOpen { get; private set; }

        int timestampDesde;
        int timestampHasta;
        string[] arrayClientesID;

        int totalExitosos;
        int totalNoExitosos;

        public VistaReporteIngresos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Load data into Ingresos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource ingresosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ingresosViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Ingresos> ingresosQuery = this.GetIngresosQuery(database1Entities);
            ingresosViewSource.Source = ingresosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            eliminarFiltros(true);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private System.Data.Objects.ObjectQuery<Ingresos> GetIngresosQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.Ingresos> ingresosQuery = database1Entities.Ingresos;
            // To explicitly load data, you may need to add Include methods like below:
            // ingresosQuery = ingresosQuery.Include("Ingresos.clientes").
            // For more information, please see http://go.microsoft.com/fwlink/?LinkId=157380
            // Returns an ObjectQuery.
            return ingresosQuery;
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
            buttonBuscarIngresos.IsEnabled = true;
        }

        private void checkBoxDesde_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = false;
            if (checkBoxHasta.IsChecked == false && checkBoxNombre.IsChecked == false && checkBoxExitoso.IsChecked == false)
            {
                buttonBuscarIngresos.IsEnabled = false;
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
            buttonBuscarIngresos.IsEnabled = true;
        }

        private void checkBoxHasta_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerHasta.IsEnabled = false;
            if (checkBoxDesde.IsChecked == false && checkBoxNombre.IsChecked == false && checkBoxExitoso.IsChecked == false)
            {
                buttonBuscarIngresos.IsEnabled = false;
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
            buttonBuscarIngresos.IsEnabled = true;
        }

        private void checkBoxNombre_Unchecked(object sender, RoutedEventArgs e)
        {
            autoCompleteTextBoxNombre.IsEnabled = false;
            if (checkBoxDesde.IsChecked == false && checkBoxHasta.IsChecked == false && checkBoxExitoso.IsChecked == false)
            {
                buttonBuscarIngresos.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
            }
        }

        private void checkBoxExitoso_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxExitoso.IsEnabled = true;
            buttonBuscarIngresos.IsEnabled = true;
        }

        private void checkBoxExitoso_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxExitoso.IsEnabled = false;
            if (checkBoxDesde.IsChecked == false && checkBoxHasta.IsChecked == false && checkBoxNombre.IsChecked == false)
            {
                buttonBuscarIngresos.IsEnabled = false;
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

        private void buttonBuscarIngresos_Click(object sender, RoutedEventArgs e)
        {
            aplicarFiltros();
        }

        private void aplicarFiltros()
        {
            totalExitosos = 0;
            totalNoExitosos = 0;
            bool noQueryFlag = false;

            string esql = "SELECT value i FROM Ingresos as i";

            if (autoCompleteTextBoxNombre.IsEnabled == true)
            {
                string nombreBuscado = autoCompleteTextBoxNombre.Text;

                if (nombreBuscado.Length < 2)
                {
                    labelSatusBar.Content = "El nombre ingresado debe poseer al menos 2 caracteres.";
                    noQueryFlag = true;
                    return;
                }

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

                if (totalHits > 1)
                {
                    labelSatusBar.Content = "Existe más de un cliente con ese nombre.";
                    return;
                }

                esql += " WHERE ";
                esql += String.Format("(i.clientes.idCliente = {0})", foundID);
            }

            if (datePickerDesde.IsEnabled == true)
            {
                if (datePickerDesde.SelectedDate != null)
                {
                    DateTime tempDate = (DateTime)datePickerDesde.SelectedDate;
                    if (esql.Contains("WHERE"))
                    {
                        esql += " AND ";
                    }
                    else
                    {
                        esql += " WHERE ";
                    }
                    esql += String.Format("(i.fecha >= DATETIME'{0}')", tempDate.ToString("u").Substring(0, 16));
                }
                else
                {
                    noQueryFlag = true;
                }
            }

            if (datePickerHasta.IsEnabled == true)
            {
                if (datePickerHasta.SelectedDate != null)
                {
                    DateTime tempDate = (DateTime)datePickerHasta.SelectedDate;
                    tempDate = tempDate.AddDays(1);
                    System.Console.WriteLine(tempDate.ToString());
                    if (esql.Contains("WHERE"))
                    {
                        esql += " AND ";
                    }
                    else
                    {
                        esql += " WHERE ";
                    }
                    esql += String.Format("(i.fecha < DATETIME'{0}')", tempDate.ToString("u").Substring(0, 16));
                }
                else
                {
                    noQueryFlag = true;
                }
            }

            if (checkBoxExitoso.IsChecked == true)
            {
                string tempExitoso;
                if (comboBoxExitoso.SelectedIndex == 0)
                {
                    tempExitoso = "true";
                }
                else
                {
                    tempExitoso = "false";
                }

                if (esql.Contains("WHERE"))
                {
                    esql += " AND ";
                }
                else
                {
                    esql += " WHERE ";
                }
                esql += String.Format("(i.exitoso = {0})", tempExitoso);
            }

            if (noQueryFlag != true)
            {
                //System.Console.WriteLine(esql);

                var ingresosVar = database1Entities.CreateQuery<Ingresos>(esql);
                labelCantidadIngresosTotal.Content = ingresosVar.ToList().Count.ToString();

                dataGridIngresos.ItemsSource = ingresosVar;

                if (ingresosVar.ToList().Count > 0)
                {
                    foreach (Gimnasio.Ingresos tempIngreso in ingresosVar.ToArray())
                    {
                        if (tempIngreso.exitoso == true)
                        {
                            totalExitosos++;
                        }
                        else
                        {
                            totalNoExitosos++;
                        }
                    }
                    labelSatusBar.Content = "Búsqueda Completa.";
                }
                else
                {
                    labelSatusBar.Content = "La búsqueda no arrojó resultados.";
                }
                labelIngresosExitosos.Content = totalExitosos.ToString();
                labelIngresosNoExitosos.Content = totalNoExitosos.ToString();
            }

        }

        private void eliminarFiltros(bool firstTime)
        {
            totalExitosos = 0;
            totalNoExitosos = 0;

            string esql = "SELECT value i FROM Ingresos as i";
            var ingresosVar = database1Entities.CreateQuery<Ingresos>(esql);
            labelCantidadIngresosTotal.Content = ingresosVar.ToList().Count.ToString();

            dataGridIngresos.ItemsSource = ingresosVar;

            foreach (Gimnasio.Ingresos tempIngreso in ingresosVar.ToArray())
            {
                if (tempIngreso.exitoso == true)
                {
                    totalExitosos++;
                }
                else
                {
                    totalNoExitosos++;
                }
            }

            labelIngresosExitosos.Content = totalExitosos.ToString();
            labelIngresosNoExitosos.Content = totalNoExitosos.ToString();

            if (firstTime)
            {
                string esql_clientes = "SELECT value c FROM clientes as c WHERE (c.nro_cedula IS NOT null)";
                var clientesVar = database1Entities.CreateQuery<clientes>(esql_clientes);

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
                    autoCompleteTextBoxNombre.AddItem(new WPFAutoCompleteTextbox.AutoCompleteEntry(tempCliente.nombre + " " + tempCliente.apellido, tempCliente.nombre, tempCliente.apellido, tempCliente.nombre + " " + tempCliente.apellido));
                    i++;
                }

                List<string> listaComboBox = new List<string>();
                listaComboBox.Add("Exitosos");
                listaComboBox.Add("No Exitosos");
                System.Collections.ObjectModel.ObservableCollection<string> comboSource = new System.Collections.ObjectModel.ObservableCollection<string>(listaComboBox);

                comboBoxExitoso.ItemsSource = comboSource;
                comboBoxExitoso.SelectedIndex = 0;
            }

        }

        #endregion

        // Funcion que Convierte un long int de UnixTime a una estructura DateTime
        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

    }

    #region "Conversores para los data bindings de esta ventana."
    // Conversor de Booleano a String
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleantoStringConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            //if (targetType != typeof(bool))
            //throw new InvalidOperationException("The target must be a boolean");           
            if ((bool)value == true)
            {
                string temp = "Sí";
                return temp;
            }
            else
            {
                string temp = "No";
                return temp;
            }
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
