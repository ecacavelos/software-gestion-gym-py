using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Reflection;
using System.IO;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for VistaClientes.xaml
    /// </summary>
    public partial class VistaClientes : Window
    {
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesVar2;
        public static bool IsOpen { get; private set; }

        int i = 0;
        DataGridRow[] myRow001 = new DataGridRow[99999];
        Boolean[] RowFlag = new Boolean[99999];

        public VistaClientes()
        {
            InitializeComponent();
            this.c2 = Configuration.Deserialize("config.xml");
        }

        private System.Data.Objects.ObjectQuery<clientes> GetclientesQuery(Database1Entities database1Entities)
        {
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = database1Entities.clientes;
            // Returns an ObjectQuery.
            return clientesQuery;
        }

        #region "Funciones Manejadoras de Carga y Descarga de la Ventana"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Load data into clientes. You can modify this code as needed.            
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = this.GetclientesQuery(database1Entities);

            string esql2 = "SELECT value c FROM clientes as c ORDER BY c.apellido";
            clientesVar2 = database1Entities.CreateQuery<clientes>(esql2);
            clientesViewSource.Source = clientesVar2;

            labelCantidadClientes.Content = String.Format("(Total: {0})", clientesVar2.ToList().Count.ToString());
            database1Entities.SaveChanges();

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
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
                    database1Entities.SaveChanges();
                    //label1.Content = "Se guardaron los cambios.";                
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    //label1.Content = "NO se guardaron los cambios.";
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //this.Hide();
        }

        #endregion

        #region "Funciones Manejadoras de la Edición y Borrado de las Filas del DataGrid"

        // Verificamos cuando hay cambios en el registro, habilitando el boton para guardarlos.
        private void clientesDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            // System.Console.WriteLine("Borramos.");            
            button2.IsEnabled = true;
        }

        private void clientesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            clientes obj = e.Row.Item as clientes;
            // Si el registro no tiene id es porque es nuevo.
            if (obj.idCliente == 0)
            {
                // Se crea un id para el nuevo cliente a partir de la fecha actual.
                TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                int timestamp = (int)time.TotalSeconds;
                int c = clientesDataGrid.Items.Count;
                List<clientes> clientesList = database1Entities.clientes.ToList();

                int maxVal = clientesList.Max(t => t.idCliente) + 1;

                obj.idCliente = timestamp;

                if (obj.hasfoto == null)
                {
                    ((clientes)e.Row.Item).hasfoto = false;
                }
            }
            // Se habilita el botón "Guardar Cambios", puesto que se acaba de ingresar un nuevo registro.
            button2.IsEnabled = true;
        }
        #endregion

        #region "Funciones Manejadoras de los Botones de la Ventana: 1) Guardar Cambios 2) Cancelar y Salir"

        private void GuardarCambiosClientes(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result;
            // Advertir del cambio antes de cambiar.
            // Cuando se da click en el boton de guardar cambios, se tienen que guardar todos los objetos que fueron cambiados.
            result = System.Windows.Forms.MessageBox.Show("Está seguro de que desea guardar los cambios efectuados?", "Confirmar modificaciones", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                // Se guardan los cambios.
                database1Entities.SaveChanges();
                label1.Content = "Se guardaron los cambios.";
                button2.IsEnabled = false;
            }
            else
            {
                // No se guardan los cambios.
                label1.Content = "NO se guardaron los cambios.";
            }
        }

        // Botón para Cancelar y Salir de la Vista de Clientes. 
        private void btnCancelarVistaClientes(object sender, RoutedEventArgs e)
        {
            // Cuando se da cancelar simplemente no hacer nada y cerrar la ventana. 
            this.Close();

        }

        #endregion

        #region "Funciones relativas a la busqueda dinámica"

        // La búsqueda se realiza a medida que se ingresa texto en el cuadro de texto.
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Si existe al menos un cliente.
            if (clientesVar2.ToList().Count > 0)
            {

                int b;  // Índice Auxiliar.

                // Si el cuadro de texto no está en blanco.
                if (textBox1.Text != "")
                {
                    b = 0;
                    // Convertimos la lista de registros de clientes a una DataTable.
                    DataTable main_tabla = MyRecordListToDataTable(clientesVar2.ToList());
                    foreach (DataRow row in main_tabla.Rows)
                    {
                        //Console.WriteLine("Row " + b.ToString() + " - Nombre: " + row[0].ToString());
                        RowFlag[b] = false; // Antes de revisar si hay coincidencia se asume que no.
                        foreach (DataColumn x in main_tabla.Columns)
                        {
                            // Si la columna contiene el valor de texto ingresado en el cuadro de búsqueda.
                            if (row[x].ToString().ToLower().Contains(textBox1.Text.ToLower()))
                            {
                                // Se marca la columna como válida para mostrar y se ajusta su visibilidad.
                                RowFlag[b] = true;
                                myRow001[b].Visibility = Visibility.Visible;
                            }
                        }
                        b++;
                        // Se detiene la búsqueda cuando se llega a una fila nula.
                        if (myRow001[b] == null)
                        {
                            break;
                        }
                    }
                    /* Se hace una nueva iteración de las filas visibles para ocultar las que no contengan
                     * coincidencias con la búsqueda actual. */
                    b = 0;
                    foreach (DataRow row in main_tabla.Rows)
                    {
                        if (!RowFlag[b])
                        {
                            // Se oculta la fila puesto que no está marcada como coincidencia.
                            myRow001[b].Visibility = Visibility.Collapsed;
                        }
                        b++;
                        // Se termina cuando se llega a una fila nula.
                        if (myRow001[b] == null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    /* Si el usuario deja el cuadro de texto en blanco, se itera a 
                     * través de todas las filas y se las hace visibles. */
                    for (b = 0; b < i; b++)
                    {
                        RowFlag[i] = true;
                        if (myRow001[b] == null)
                        {
                            break;
                        }
                        myRow001[b].Visibility = Visibility.Visible;
                    }
                }
            }

        }

        /* A medida que se cargan nuevas filas en el DataGrid, se obtienen los índices de las filas
         * que son cargadas y se validan con la búsqueda actual.¨*/
        private void clientesDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            i = clientesDataGrid.Items.Count;
            myRow001[e.Row.GetIndex()] = e.Row;
            validarRow(e.Row);
        }


        private void validarRow(DataGridRow e)
        {
            // Si el cuadro de búsqueda no está vacío.
            if (textBox1.Text != "")
            {
                // Mostrar la nueva fila si la búsqueda marcó que contiene una coincidencia.
                if (RowFlag[e.GetIndex()] == true)
                {
                    e.Visibility = Visibility.Visible;
                }
                else
                {
                    e.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                // No hace falta ocultar una nueva fila si no existe una búsqueda actual.
                e.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Función que convierte una List a DataTable.
        /// </summary>
        /// <param name="list">La Lista a convertir.</param>
        /// <returns>Retorna una DataTable.</returns>
        public DataTable MyRecordListToDataTable(List<Gimnasio.clientes> list)
        {
            DataTable dt = new DataTable("TablaEjemplo");

            //dt.Columns.Add("apellido", list[0].apellido.GetType());
            dt.Columns.Add("apellido", "string".GetType());
            dt.Columns.Add("nombre", "string".GetType());
            dt.Columns.Add("nro_cedula", "string".GetType());
            dt.Columns.Add("direccion", "string".GetType());
            dt.Columns.Add("telefono", "string".GetType());
            dt.Columns.Add("email", "string".GetType());
            dt.Columns.Add("fecha_nacimiento", "string".GetType());
            dt.Columns.Add("fecha_ingreso", "string".GetType());
            dt.Columns.Add("altura", "string".GetType());
            dt.Columns.Add("peso", "string".GetType());

            foreach (Gimnasio.clientes item in list)
            {
                dt.Rows.Add(
                    item.apellido,
                    item.nombre,
                    item.nro_cedula,
                    item.direccion,
                    item.telefono,
                    item.email,
                    item.fecha_nacimiento,
                    item.fecha_ingreso,
                    item.altura,
                    item.peso);
            }

            return dt;
        }

        #endregion

        #region "Funciones relativas al manejo de las fotos de los clientes"

        private void OnFotoToggleTargetUpdated(Object sender, DataTransferEventArgs args)
        {
            //Console.WriteLine("Updated.");
        }

        private void FotoToggled(object sender, RoutedEventArgs e)
        {
            clientes obj = ((FrameworkElement)sender).DataContext as clientes;

            if (((CheckBox)e.Source).IsChecked == true)
            {
                if (obj != null)
                { // Existe el cliente con el nro. de cedula por lo menos.

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    // Process open file dialog box results
                    if (result == true)
                    {
                        /* String que contiene el path completo del archivo seleccionado, 
                         * incluyendo el nombre del archivo. */
                        string filename = dlg.FileName;

                        // Guardar la imagen con el nombre obj.idCliente.toString() + ".jpg";

                        // Create source. 
                        BitmapImage bi = new BitmapImage();
                        // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                        bi.BeginInit();
                        bi.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
                        bi.EndInit();

                        FileStream stream = new FileStream("fotosClientes/" + obj.idCliente.ToString() + ".jpg", FileMode.Create);
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bi));
                        encoder.Save(stream);

                        stream.Close();
                    }
                    else
                    {
                        // Se presionó 'Cancelar'.                        
                        ((CheckBox)e.Source).IsChecked = false;
                    }

                }
                else
                {
                    // Si no se cumplen los requisitos para agregar una foto, se muestra una advertencia.
                    MessageBox.Show("El cliente debe tener al menos el numero de cedula para poder seleccionar una foto.");
                    ((CheckBox)e.Source).IsChecked = false;
                }

            }
            else
            {
                System.Windows.Forms.DialogResult result;
                result = System.Windows.Forms.MessageBox.Show("Está seguro de que desea descartar la foto actual?", "Quitar foto", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // Acá recien borrar o descartar el archivo!                    
                    ((CheckBox)e.Source).IsChecked = false;
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    // Se arrepintió. No tocar el archivo!                    
                    ((CheckBox)e.Source).IsChecked = true;
                }

            }
            // Se habilita el botón "Guardar Cambios".
            button2.IsEnabled = true;

        }

        #endregion

        #region "Funciones relativas al Keypad USB"
        // Funciones para evitar que el keypad USB afecte los controles de esta ventana.

        private void clientesDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            //Console.WriteLine("C2: " + this.c2.Keypad_usb.ToString());
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void button2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void button1_PreviewKeyDown(object sender, KeyEventArgs e)
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
