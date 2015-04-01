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
                    database1Entities.ExecuteStoreCommand("DELETE FROM FacturasAnuladas WHERE (idFactura = 0) AND (idPago = 0)");
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
                database1Entities.ExecuteStoreCommand("DELETE FROM FacturasAnuladas WHERE (idFactura = 0) AND (idPago = 0)");
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
                anularClicked = false;

                DataGrid myDataGrid = facturasDataGrid;
                Gimnasio.Facturas currentcell = (Gimnasio.Facturas)myDataGrid.SelectedItem;

                // Seleccionamos los Pagos asociados a la Factura que queremos anular.
                string esql = String.Format("SELECT value p FROM Pagos as p WHERE (p.fk_factura = {0})", currentcell.idFactura.ToString());
                var pagosFacturaActual = database1Entities.CreateQuery<Pagos>(esql);
                // Indicamos que los Pagos correspondientes dejan de estar facturado.                
                foreach (Pagos pago in pagosFacturaActual)
                {
                    pago.ya_facturado = false;
                }
                // Pasamos la Factura previamente asociada a los Pagos a un campo "Factura Previa" para poder
                // verificar y prevenir posibles conflictos de facturación.
                for (int i = 0; i < pagosFacturaActual.ToList().Count; i++)
                {
                    // Se crea una nueva entrada en la tabla FacturasAnuladas.
                    // Esta entrada tiene la relacion entre el pago y en cual factura estaba contenido.
                    FacturasAnuladas nuevaFacturaAnular = new FacturasAnuladas();
                    if (nuevaFacturaAnular.idFacturaAnulada == 0)
                    {
                        TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                        int timestamp = (int)time.TotalSeconds;
                        nuevaFacturaAnular.idFacturaAnulada = timestamp + i;
                    }
                    nuevaFacturaAnular.idFactura = currentcell.idFactura;
                    nuevaFacturaAnular.idPago = pagosFacturaActual.ToArray<Pagos>()[i].idPago;
                    database1Entities.FacturasAnuladas.AddObject(nuevaFacturaAnular);
                }
            }
        }

        private void anularCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (anularClicked.ToString() == "True")
            {
                anularClicked = false;

                bool permitirDesanular = true;
                DataGrid myDataGrid = facturasDataGrid;
                Gimnasio.Facturas currentcell = (Gimnasio.Facturas)myDataGrid.SelectedItem;

                // Al des-anular una factura, verificamos que no hayan ya otras facturas sin anular correspondientes al mismo pago.
                string esql0 = String.Format("SELECT value f FROM FacturasAnuladas as f WHERE f.idFactura = {0}", currentcell.idFactura);
                var facturasAnuladasVar = database1Entities.CreateQuery<FacturasAnuladas>(esql0);

                for (int i = 0; i < facturasAnuladasVar.ToArray().Length; i++)
                {
                    string esql1 = String.Format("SELECT value p FROM Pagos as p WHERE (p.idPago = {0} AND p.ya_facturado = True)", facturasAnuladasVar.ToArray()[i].idPago);
                    var pagosVar = database1Entities.CreateQuery<Pagos>(esql1);
                    if (pagosVar.ToList().Count > 0)
                    {
                        // Indicamos cual Factura no anulada está en conflicto con algun ítem de la 
                        // Factura que se pretende desanular, y no se hace nada.
                        string esql2 = String.Format("SELECT value f FROM Facturas as f WHERE f.idFactura = {0}", pagosVar.ToArray<Pagos>()[0].fk_factura);
                        var facturasConflictoVar = database1Entities.CreateQuery<Facturas>(esql2);
                        ((CheckBox)(sender)).IsChecked = true;
                        permitirDesanular = false;
                        MessageBox.Show("Ya existen facturas no anuladas para este pago. Verificar factura " + facturasConflictoVar.Single<Facturas>().Nro_Factura + ".", "No se puede cancelar la anulación de la Factura.");
                    }
                }

                // Si no hay conflictos con los Pagos y alguna Factura no anulada.
                if (permitirDesanular)
                {
                    for (int i = 0; i < facturasAnuladasVar.ToArray().Length; i++)
                    {
                        string esql = String.Format("SELECT value p FROM Pagos as p WHERE p.idPago = {0}", facturasAnuladasVar.ToArray()[i].idPago);
                        var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                        // Ponemos el pago correspondiente como facturado.
                        foreach (Pagos pago in pagosVar)
                        {
                            pago.fk_factura = currentcell.idFactura;
                            pago.ya_facturado = true;
                        }
                    }
                    // Eliminamos las entradas correspondientes a la Factura en la tabla 'FacturasAnuladas'.                        
                    foreach (FacturasAnuladas factura in facturasAnuladasVar)
                    {
                        factura.idFactura = 0;
                        factura.idPago = 0;
                    }
                }
            }
        }

        private void anularCheckBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Se usa el bool 'anularClicked' para indicar que fue efectivamente el usuario el
            // que marcó o desmarcó algun CheckBox para anular o desanular Facturas.
            anularClicked = true;
        }

        #endregion

        #region "Funciones relativas al Detalle de las Facturas"

        private void DetalleFactura(object sender, RoutedEventArgs e)
        {
            Facturas thisFactura = ((FrameworkElement)sender).DataContext as Facturas;

            // Recuperamos de la Base de Datos todos los Pagos asociados a esta Factura.            
            string esql;
            Pagos[] arrayPagos;
            if (thisFactura.Anulada)
            {
                // Si la Factura está anulada, los Pagos se recuperan de la tabla 'FacturasAnuladas'.
                string esql0 = String.Format("SELECT value f FROM FacturasAnuladas as f WHERE f.idFactura = {0}", thisFactura.idFactura);
                var facturasAnuladasVar = database1Entities.CreateQuery<FacturasAnuladas>(esql0);

                Pagos[] pagosAfectados = new Pagos[facturasAnuladasVar.ToArray().Length];

                for (int i = 0; i < facturasAnuladasVar.ToArray().Length; i++)
                {
                    esql = String.Format("SELECT value p FROM Pagos as p WHERE p.idPago = {0}", facturasAnuladasVar.ToArray()[i].idPago);
                    var pagosVar = database1Entities.CreateQuery<Pagos>(esql);
                    pagosAfectados[i] = pagosVar.ToArray()[0];
                }
                arrayPagos = pagosAfectados;
            }
            else
            {
                // Si la Factura está activa, los Pagos se recuperan directamente de la tabla 'Pagos'.
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
