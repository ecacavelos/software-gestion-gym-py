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
    /// Interaction logic for VistaTiposCuotas.xaml
    /// </summary>
    public partial class VistaTiposCuotas : Window
    {
        public VistaTiposCuotas()
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

            Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
            // Load data into clientes. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = this.GetclientesQuery(database1Entities);
            clientesViewSource.Source = clientesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
            // Load data into Cuotas. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource cuotasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cuotasViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Cuotas> cuotasQuery = this.GetCuotasQuery(database1Entities);
            cuotasViewSource.Source = cuotasQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private System.Data.Objects.ObjectQuery<Cuotas> GetCuotasQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.Cuotas> cuotasQuery = database1Entities.Cuotas;
            // Returns an ObjectQuery.
            return cuotasQuery;
        }
    }
}
