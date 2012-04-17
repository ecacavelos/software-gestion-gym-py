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
        public static bool IsOpen { get; private set; }

        int i = 0;
        DataGridRow[] myRow001 = new DataGridRow[999];

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
            var clientesVar2 = database1Entities.CreateQuery<clientes>(esql2);
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

        //BOTON PARA CANCELAR TODO EN LA VISTA CLIENES. 
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
            string esql = "select value c from clientes as c";
            var clientesVar = database1Entities.CreateQuery<clientes>(esql);

            if (clientesVar.ToList().Count > 0)
            {

                int b;
                Boolean[] RowFlag = new Boolean[i];

                if (textBox1.Text != "")
                {
                    b = 0;
                    DataTable main_tabla = MyRecordListToDataTable(clientesVar.ToList());
                    foreach (DataRow row in main_tabla.Rows)
                    {
                        RowFlag[b] = false;
                        foreach (DataColumn x in main_tabla.Columns)
                        {
                            if (row[x].ToString().ToLower().Contains(textBox1.Text.ToLower()))
                            {
                                myRow001[b].Visibility = Visibility.Visible;
                                RowFlag[b] = true;
                            }
                        }
                        b++;
                    }
                    b = 0;
                    foreach (DataRow row in main_tabla.Rows)
                    {
                        if (!RowFlag[b])
                        {
                            myRow001[b].Style = clientesDataGrid.RowStyle;
                            myRow001[b].Visibility = Visibility.Collapsed;
                        }
                        b++;
                    }
                }
                else
                {
                    for (b = 0; b < i; b++)
                    {
                        myRow001[b].Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private void clientesDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            i = clientesDataGrid.Items.Count;
            myRow001[e.Row.GetIndex()] = e.Row;
        }

        public static DataTable MyRecordListToDataTable(List<Gimnasio.clientes> list)
        {
            DataTable dt = new DataTable("TablaEjemplo");
            dt.Columns.Add("apellido", list[0].apellido.GetType());
            dt.Columns.Add("nombre", list[0].nombre.GetType());
            dt.Columns.Add("nro_cedula", list[0].nro_cedula.GetType());
            dt.Columns.Add("direccion", list[0].direccion.GetType());
            dt.Columns.Add("telefono", list[0].telefono.GetType());
            dt.Columns.Add("email", list[0].email.GetType());
            dt.Columns.Add("fecha_nacimiento", list[0].fecha_nacimiento.GetType());
            dt.Columns.Add("fecha_ingreso", list[0].fecha_ingreso.GetType());
            dt.Columns.Add("altura", list[0].altura.GetType());
            dt.Columns.Add("peso", list[0].peso.GetType());

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

            //Console.WriteLine(((CheckBox)e.Source).IsChecked.ToString());
            if (((CheckBox)e.Source).IsChecked == true)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = ""; // Default file name
                dlg.DefaultExt = ""; // Default file extension
                //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    Console.WriteLine(dlg.FileName);
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
            button2.IsEnabled = true;

        }

    }
}
