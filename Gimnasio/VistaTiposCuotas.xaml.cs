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
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        public static bool IsOpen { get; private set; }

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

            IsOpen = true;
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

        private void RowEditEnding_TiposCuotas(object sender, DataGridRowEditEndingEventArgs e)
        {
            Cuotas obj = e.Row.Item as Cuotas;
            if (obj.idCuota == 0)
            {   // new record 
                TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                obj.idCuota = (int)time.TotalSeconds;

            }
            // Se habilita el boton para poder guardar los cambios.
            button_GuardarTiposCuotas.IsEnabled = true;

        }

        private void click_GuardarCambios(object sender, RoutedEventArgs e)
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
                label_Cuotas_CambiosGuardados.Content = "Se guardaron los cambios.";
                button_GuardarTiposCuotas.IsEnabled = false;
            }
            else
            {
                //System.Console.WriteLine("No entramos al IF.");
                label_Cuotas_CambiosGuardados.Content = "NO se guardaron los cambios.";
                //database1Entities.SaveChanges();
                // Gimnasio.Database1Entities
            }

        }

        private void unloadingRow_BorrarTipoCuota(object sender, DataGridRowEventArgs e)
        {
            button_GuardarTiposCuotas.IsEnabled = true;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }


        #region "Funciones para la captura y manejo de teclas"
        // Esto es necesario para permitir el ingreso de numeros de cedula mediante el keypad USB
        private void cuotasDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }
        #endregion

    }
}
