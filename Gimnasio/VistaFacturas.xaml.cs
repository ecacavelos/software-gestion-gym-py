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
using System.Windows.Controls.Primitives;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VistaFacturas.xaml
    /// </summary>
    public partial class VistaFacturas : Window
    {
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

        public static bool IsOpen { get; private set; }

        bool anularClicked;

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

        #region "Funciones Manejadoras de Carga y Descarga de la Ventana"
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            //anularClicked = false;

            // Load data into Facturas. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource facturasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("facturasViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Facturas> facturasQuery = this.GetFacturasQuery(database1Entities);
            facturasViewSource.Source = facturasQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Forms.DialogResult result;

            if (buttonGuardarCambios.IsEnabled == true)
            {
                result = System.Windows.Forms.MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    database1Entities.SaveChanges();    // Se graban los cambios antes de cerrar la ventana.
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    // No se hace nada antes de cerrar la ventana.
                }
                else
                {
                    e.Cancel = true;    // Cancelamos el cierre de la ventana.
                }
            }
        }
        #endregion

        #region "Funciones Manejadoras de la Edición y Borrado de las Filas del DataGrid"
        // Al borrar una fila.
        private void facturasDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            buttonGuardarCambios.IsEnabled = true;
        }
        // Al editar una fila.
        private void facturasDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            buttonGuardarCambios.IsEnabled = true;
        }
        #endregion

        #region "Funciones Manejadoras de los Botones de la Ventana: 1) Guardar Cambios 2) Cancelar y Salir"
        private void buttonGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result;

            // Advertir del cambio antes de cambiar.
            // Cuando se da click en el boton de guardar cambios, se tienen que guardar todos los objetos que fueron cambiados
            result = System.Windows.Forms.MessageBox.Show("Está seguro de que desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                database1Entities.SaveChanges();
                label1.Content = "Se guardaron los cambios.";
                buttonGuardarCambios.IsEnabled = false;
            }
            else
            {
                label1.Content = "NO se guardaron los cambios.";
            }
        }

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            // Cuando se da cancelar simplemente no hacer nada y cerrar la ventana. 
            this.Close();
        }
        #endregion

        #region "Funciones para Anular y Desanular desde la vista de Facturas"
        private void anularCheckBox_Click(object sender, RoutedEventArgs e)
        {
            buttonGuardarCambios.IsEnabled = true;
        }

        private void anularCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (anularClicked.ToString() == "True")
            {
                DataGrid myDataGrid = facturasDataGrid;
                Gimnasio.Facturas currentcell = (Gimnasio.Facturas)myDataGrid.SelectedItem;

                // Indicamos que el Pago correspondiente deja de estar facturado.
                database1Entities.ExecuteStoreCommand("UPDATE Pagos SET ya_facturado = 0 WHERE (Pagos.fk_factura = {0})", currentcell.idFactura.ToString());
                // Pasamos la Factura previamente asociada a los Pagos a un campo "Factura Previa" para poder
                // verificar y prevenir posibles conflictos de facturación.
                string esql = String.Format("SELECT value p FROM Pagos as p WHERE (p.fk_factura = {0})", currentcell.idFactura.ToString());
                var pagosFacturaActual = database1Entities.CreateQuery<Pagos>(esql);
                /*System.Console.WriteLine();
                System.Console.WriteLine(esql);
                System.Console.WriteLine(pagosFacturaActual.ToList().Count);*/
                for (int i = 0; i < pagosFacturaActual.ToList().Count; i++)
                {
                    System.Console.WriteLine("FOR " + pagosFacturaActual.ToArray<Pagos>()[i].idPago.ToString());
                    /*database1Entities.ExecuteStoreCommand(
                                "UPDATE Pagos SET fk_factura_anulada = {0} WHERE (Pagos.idPago = {1})", pagosFacturaActual.ToList().ElementAt(i).fk_factura, pagosFacturaActual.ToArray<Pagos>()[i].idPago);*/
                    FacturasAnuladas nuevaFacturaAnular = new FacturasAnuladas();
                    if (nuevaFacturaAnular.idFacturaAnulada == 0)                                 // Si el ID no existe.
                    {
                        TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                        int timestamp = (int)time.TotalSeconds;
                        nuevaFacturaAnular.idFacturaAnulada = timestamp + i;                          // Nuevo ID = timestamp.
                    }
                    nuevaFacturaAnular.idFactura = currentcell.idFactura;
                    nuevaFacturaAnular.idPago = pagosFacturaActual.ToArray<Pagos>()[i].idPago;
                    database1Entities.FacturasAnuladas.AddObject(nuevaFacturaAnular);
                    //database1Entities.ExecuteStoreCommand(
                    //"UPDATE Pagos SET fk_factura = null WHERE (Pagos.idPago = {0})", pagosFacturaActual.ToArray<Pagos>()[i].idPago);
                }

                database1Entities.SaveChanges();

                anularClicked = false;
            }
        }

        private void anularCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (anularClicked.ToString() == "True")
            {
                System.Console.WriteLine("Unchecked.");

                bool permiterDesanular = true;
                DataGrid myDataGrid = facturasDataGrid;
                Gimnasio.Facturas currentcell = (Gimnasio.Facturas)myDataGrid.SelectedItem;

                // Al des-anular una factura, verificamos que no hayan ya otras facturas sin anular correspondientes al mismo pago.
                string esql0 = String.Format("SELECT value f FROM FacturasAnuladas as f WHERE f.idFactura = {0}", currentcell.idFactura);
                var facturasAnuladasVar = database1Entities.CreateQuery<FacturasAnuladas>(esql0);
                /*string esql = "SELECT value p FROM FacturasAnuladas as p WHERE (p.idFactura = " + currentcell.idFactura.ToString() + ")";
                var facturasNoAnuladas = database1Entities.CreateQuery<Facturas>(esql);*/
                for (int i = 0; i < facturasAnuladasVar.ToArray().Length; i++)
                {
                    string esql1 = String.Format("SELECT value p FROM Pagos as p WHERE (p.idPago = {0} AND p.ya_facturado = True)", facturasAnuladasVar.ToArray()[i].idPago);
                    var pagosVar = database1Entities.CreateQuery<Pagos>(esql1);
                    if (pagosVar.ToList().Count > 0)
                    {
                        System.Console.WriteLine("Advertencia: Ya existen facturas no anuladas para este pago. Verificar.");
                        permiterDesanular = false;
                    }
                }

                if (permiterDesanular)
                {
                    for (int i = 0; i < facturasAnuladasVar.ToArray().Length; i++)
                    {
                        System.Console.WriteLine(facturasAnuladasVar.ToArray()[i].idPago);
                        string esql = String.Format("SELECT value p FROM Pagos as p WHERE p.idPago = {0}", facturasAnuladasVar.ToArray()[i].idPago);
                        var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                        // Ponemos el pago correspondiente como facturado.
                        database1Entities.ExecuteStoreCommand("UPDATE Pagos SET ya_facturado = 1 WHERE (Pagos.idPago = {0})", pagosVar.ToArray()[0].idPago);
                        database1Entities.SaveChanges();
                    }
                }

                anularClicked = false;
            }
        }

        private void anularCheckBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            anularClicked = true;
        }

        #endregion

        #region "Funciones relativas al Detalle de las Facturas"

        private void DetalleFactura(object sender, RoutedEventArgs e)
        {
            Facturas thisFactura = ((FrameworkElement)sender).DataContext as Facturas;

            // Recuperamos de la Base de Datos todos los Pagos asociados a esta Factura.
            //System.Console.WriteLine(thisFactura.Anulada.ToString());
            string esql;
            Pagos[] arrayPagos;
            if (thisFactura.Anulada)
            {
                string esql0 = String.Format("SELECT value f FROM FacturasAnuladas as f WHERE f.idFactura = {0}", thisFactura.idFactura);
                var facturasAnuladasVar = database1Entities.CreateQuery<FacturasAnuladas>(esql0);

                Pagos[] pagosAfectados = new Pagos[facturasAnuladasVar.ToArray().Length];

                for (int i = 0; i < facturasAnuladasVar.ToArray().Length; i++)
                {
                    System.Console.WriteLine(facturasAnuladasVar.ToArray()[i].idPago);
                    esql = String.Format("SELECT value p FROM Pagos as p WHERE p.idPago = {0}", facturasAnuladasVar.ToArray()[i].idPago);
                    var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                    pagosAfectados[i] = pagosVar.ToArray()[0];
                }
                arrayPagos = pagosAfectados;
            }
            else
            {
                esql = String.Format("SELECT value p FROM Pagos as p WHERE p.fk_factura = {0}", thisFactura.idFactura);
                var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                arrayPagos = pagosVar.ToArray();
            }
            // Abrimos la ventana "Detalle de Facturas" pasando los datos y Pagos asociados a esta Factura.
            DialogDetalleFactura winDetalle = new DialogDetalleFactura(thisFactura, arrayPagos);
            Nullable<bool> result = winDetalle.ShowDialog();
        }

        #endregion

        #region "Funciones relativas al Keypad USB"
        // Funciones para evitar que el keypad USB afecte los controles de esta ventana.

        private void buttonGuardarCambios_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void buttonSalir_PreviewKeyDown(object sender, KeyEventArgs e)
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
