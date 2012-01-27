using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
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
    /// Interaction logic for VistaClientes.xaml
    /// </summary>
    public partial class VistaClientes : Window
    {

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

        public VistaClientes()
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
            System.Windows.Forms.DialogResult result;

            // Advertir del cambio antes de cambiar.
            // Cuando se da click en el boton de guardar cambios, se tienen que guardar todos los objetos que fueron cambiados
            result = System.Windows.Forms.MessageBox.Show("Está seguro de que desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                // 'DialogResult.Yes' value was returned from the MessageBox
                //System.Console.WriteLine("Entramos al IF.");
                database1Entities.SaveChanges();
                label1.Content = "Se guardaron los cambios.";
                button2.IsEnabled = false;
            }
            else
            {
                //System.Console.WriteLine("No entramos al IF.");
                label1.Content = "NO se guardaron los cambios.";
                //database1Entities.SaveChanges();
                // Gimnasio.Database1Entities
            }

        }


        // Verificamos cuando hay cambios en el registro, habilitando el boton para guardarlos.
        private void clientesDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            // System.Console.WriteLine("Borramos.");
            button2.IsEnabled = true;
        }

        private void clientesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {


            clientes obj = e.Row.Item as clientes;
            if (obj.idCliente == 0)
            {// new record 
                TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                int timestamp = (int)time.TotalSeconds;
                int c = clientesDataGrid.Items.Count;
                List<clientes> clientesList = database1Entities.clientes.ToList();

                int maxVal = clientesList.Max(t => t.idCliente) + 1;

                obj.idCliente = timestamp;

            }



            button2.IsEnabled = true;
        }

        // Al momento de cerrar la ventana verificamos si hay cambios pendientes, y preguntamos para guardar
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Forms.DialogResult result;

            if (button2.IsEnabled == true)
            {
                result = System.Windows.Forms.MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // 'DialogResult.Yes' value was returned from the MessageBox
                    System.Console.WriteLine("Yes.");
                    database1Entities.SaveChanges();
                    //label1.Content = "Se guardaron los cambios.";                
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    System.Console.WriteLine("No.");
                    //label1.Content = "NO se guardaron los cambios.";
                }
                else
                {
                    System.Console.WriteLine("Cancel.");
                    e.Cancel = true;
                }
            }
        }

    }
}
