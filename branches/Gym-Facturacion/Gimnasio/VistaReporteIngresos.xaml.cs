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

        int totalExitosos = 0;
        int totalNoExitosos = 0;

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
            if (checkBoxHasta.IsChecked == false && checkBoxNombre.IsChecked == false)
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
            if (checkBoxDesde.IsChecked == false && checkBoxNombre.IsChecked == false)
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
            if (checkBoxDesde.IsChecked == false && checkBoxHasta.IsChecked == false)
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

        }

        private void eliminarFiltros(bool firstTime)
        {
            if (firstTime)
            {
                string esql = "SELECT value i FROM Ingresos as i";
                var ingresosVar = database1Entities.CreateQuery<Ingresos>(esql);

                labelCantidadIngresosTotal.Content = ingresosVar.ToList().Count.ToString();

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

            }
        }

        #endregion

    }

    #region "Conversores para los data bindings de esta ventana."
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
