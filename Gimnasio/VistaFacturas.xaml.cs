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
    /// Interaction logic for VistaFacturas.xaml
    /// </summary>
    public partial class VistaFacturas : Window
    {

        public static bool IsOpen { get; private set; }

        public VistaFacturas()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectQuery<Facturas> GetFacturasQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.Facturas> facturasQuery = database1Entities.Facturas;
            // To explicitly load data, you may need to add Include methods like below:
            // facturasQuery = facturasQuery.Include("Facturas.clientes").
            // For more information, please see http://go.microsoft.com/fwlink/?LinkId=157380
            // Returns an ObjectQuery.
            return facturasQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
            // Load data into Facturas. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource facturasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("facturasViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Facturas> facturasQuery = this.GetFacturasQuery(database1Entities);
            facturasViewSource.Source = facturasQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            // Cuando se da cancelar simplemente no hacer nada y cerrar la ventana. 
            this.Close();
        }

        private void facturasDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {

        }
    }
}
