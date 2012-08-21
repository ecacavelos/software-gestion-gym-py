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
        //private bool _Keypad_usb = false;

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
            //_Keypad_usb = this.c2.Keypad_usb;
        }

        private System.Data.Objects.ObjectQuery<clientes> GetclientesQuery(Database1Entities database1Entities)
        {
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = database1Entities.clientes;
            // Returns an ObjectQuery.
            return clientesQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data into clientes. You can modify this code as needed.
            IsOpen = true;
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.clientes> clientesQuery = this.GetclientesQuery(database1Entities);
            string esql2 = "select value c from clientes as c order by c.apellido";
            clientesVar2 = database1Entities.CreateQuery<clientes>(esql2);
            clientesViewSource.Source = clientesVar2;
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
            //Console.WriteLine("UnloadingRow.");
            button2.IsEnabled = true;
        }

        private void clientesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            clientes obj = e.Row.Item as clientes;
            if (obj.idCliente == 0)
            {   // new record 
                //Console.WriteLine("Vamos a crear un nuevo registro.");

                TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                int timestamp = (int)time.TotalSeconds;
                int c = clientesDataGrid.Items.Count;
                List<clientes> clientesList = database1Entities.clientes.ToList();

                int maxVal = clientesList.Max(t => t.idCliente) + 1;

                obj.idCliente = timestamp;

            }

            //Console.WriteLine("RowEditEnding.");
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
                    //System.Console.WriteLine("Yes.");
                    database1Entities.SaveChanges();
                    //label1.Content = "Se guardaron los cambios.";                
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    //System.Console.WriteLine("No.");
                    //label1.Content = "NO se guardaron los cambios.";
                }
                else
                {
                    //System.Console.WriteLine("Cancel.");
                    e.Cancel = true;
                }
            }
        }

        //BOTON PARA CANCELAR TODO EN LA VISTA CLIENTES. 
        private void btnCancelarVistaClientes(object sender, RoutedEventArgs e)
        {
            // Cuando se da cancelar simplemente no hacer nada y cerrar la ventana. 
            this.Close();

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //this.Hide();
        }

        #region "Funciones relativas a la busqueda dinámica"
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //string esql = "select value c from clientes as c";
            //var clientesVar = database1Entities.CreateQuery<clientes>(esql);

            if (clientesVar2.ToList().Count > 0)
            {

                int b;

                if (textBox1.Text != "")
                {
                    b = 0;
                    DataTable main_tabla = MyRecordListToDataTable(clientesVar2.ToList());
                    foreach (DataRow row in main_tabla.Rows)
                    {
                        //Console.WriteLine("Row " + b.ToString() + " - Nombre: " + row[0].ToString());
                        RowFlag[b] = false;
                        foreach (DataColumn x in main_tabla.Columns)
                        {
                            if (row[x].ToString().ToLower().Contains(textBox1.Text.ToLower()))
                            {
                                //Console.WriteLine("Hit! " + b.ToString());
                                RowFlag[b] = true;
                                myRow001[b].Visibility = Visibility.Visible;
                                //validarRow(myRow001[b]);
                            }
                        }
                        b++;
                        if (myRow001[b] == null)
                        {
                            break;
                        }
                    }
                    b = 0;
                    foreach (DataRow row in main_tabla.Rows)
                    {
                        if (!RowFlag[b])
                        {
                            //validarRow(myRow001[b]);
                            myRow001[b].Visibility = Visibility.Collapsed;
                        }
                        b++;
                        if (myRow001[b] == null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    for (b = 0; b < i; b++)
                    {
                        RowFlag[i] = true;
                        //validarRow(myRow001[b]);                        
                        if (myRow001[b] == null)
                        {
                            break;
                        }
                        myRow001[b].Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private void clientesDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            i = clientesDataGrid.Items.Count;
            myRow001[e.Row.GetIndex()] = e.Row;
            validarRow(e.Row);
        }

        private void validarRow(DataGridRow e)
        {
            if (textBox1.Text != "")
            {
                //Console.WriteLine(e.GetIndex().ToString());
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
                e.Visibility = Visibility.Visible;
            }
        }

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

        private void OnFotoToggleTargetUpdated(Object sender, DataTransferEventArgs args)
        {

            //Console.WriteLine("Updated.");

        }

        private void FotoToggled(object sender, RoutedEventArgs e)
        {
            clientes obj = ((FrameworkElement)sender).DataContext as clientes;

            //Console.WriteLine(((CheckBox)e.Source).IsChecked.ToString());
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
                        // String que contiene el path completo del archivo seleccionado, incluyendo el nombre del archivo.
                        string filename = dlg.FileName;

                        //guardar la imagen con el nombre obj.idCliente.toString() + ".jpg";

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
                        // Se presionó 'Cancelar'
                        //Console.WriteLine("Cancelaste.");
                        ((CheckBox)e.Source).IsChecked = false;
                    }

                }
                else
                {
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
                    //Console.WriteLine("Confirmaste descarte de foto.");
                    ((CheckBox)e.Source).IsChecked = false;
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    // Se arrepintió. No tocar el archivo!!!
                    //Console.WriteLine("Te arrepentiste.");
                    ((CheckBox)e.Source).IsChecked = true;
                }

            }
            //Console.WriteLine("FotoToggled.");
            button2.IsEnabled = true;

        }

    }
}
