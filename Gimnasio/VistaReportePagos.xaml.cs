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

        public static bool IsOpen { get; private set; }

        public VistaReportePagos()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
            // Load data into Pagos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource pagosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pagosViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Pagos> pagosQuery = this.GetPagosQuery(database1Entities);
            pagosViewSource.Source = pagosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
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
    }
}
