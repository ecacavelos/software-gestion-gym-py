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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

        public Window2()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectQuery<clientes> GetclientesQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = database1Entities.clientes;
            // Returns an ObjectQuery.
            return clientesQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Load data into clientes. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = this.GetclientesQuery(database1Entities);
            clientesViewSource.Source = clientesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
            database1Entities.SaveChanges();
        }

        private void GuardarCambiosClientes(object sender, RoutedEventArgs e)
        {
            // advertir del cambio antes de cambiar. 
            database1Entities.SaveChanges();
           // Gimnasio.Database1Entities
            // cuando se da click en el boton de guardar cambios, se tienen que guardar todos los objetos que fueron
            //cambiados

        }

        private void c(object sender, MouseWheelEventArgs e)
        {

        }

        private void clientesDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("hola");
        }

        private void clientesDataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_SourceUpdated");
        }

        private void clientesDataGrid_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_IsEnabledChanged");
        }

        private void clientesDataGrid_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_IsHitTestVisibleChanged");
        }

        private void clientesDataGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_IsVisibleChanged");

        }

        private void clientesDataGrid_RowDetailsVisibilityChanged(object sender, DataGridRowDetailsEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_RowDetailsVisibilityChanged");


        }

        private void clientesDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_SelectedCellsChanged");
        }

        private void clientesDataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Console.WriteLine("clientesDataGrid_SizeChanged");
        }

        private void clientesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            button2.IsEnabled = true;
            
        }

    }
}
