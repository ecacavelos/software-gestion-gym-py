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
using System.Data.EntityClient;

namespace Gimnasio
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

        public Window4()
        {
            InitializeComponent();
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
        private System.Data.Objects.ObjectQuery<Cuotas> GetcuotasQuery(Database1Entities database1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<Gimnasio.Cuotas> cuotasQuery = database1Entities.Cuotas;
            // Returns an ObjectQuery.
            return cuotasQuery;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data into Pagos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource pagosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pagosViewSource")));
            System.Data.Objects.ObjectQuery<Gimnasio.Pagos> pagosQuery = this.GetPagosQuery(database1Entities);
            pagosViewSource.Source = pagosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        //obtiene cualquier caracter. 
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            /*Si (la tecla presionada es ENTER)
                    entonces, evaluar el numero de cedula y traer el cliente y llenar los datos
              Si no, no hacer nada. 
             
             */
            
            if (e.Key.ToString() == "Return")
            {
                
               

                String numeroCedula = e.OriginalSource.ToString().Substring(33);
                //System.Console.WriteLine("ve.OriginalSource.ToString()" + e.OriginalSource.ToString().Remove(4) + "\n");
                System.Console.WriteLine("substring  : " + numeroCedula + "\n");
                // comprobar si existe este objeto y llenar el formulario

                Console.WriteLine("a buscar el cliente y cuotas");
                string esql = "select value c from clientes as c where c.nombre= '" + numeroCedula +  "\'";
                string esqlCuotas = "select value c from cuotas as c";

                Console.WriteLine("consulta: " + esql);
                var clientes = database1Entities.CreateQuery<clientes>(esql);
                var cuotas = database1Entities.CreateQuery<Cuotas>(esqlCuotas);
                //foreach (var cliente in clientes)
                //{
                //    Console.WriteLine("apellido:" + cliente.apellido);
                //}
                ClienteSeleccionado.DataContext = clientes; // Establecer el data context para los elementos del cliente
                comboBoxTiposCuotas.ItemsSource = cuotas;
                comboBoxTiposCuotas.DisplayMemberPath = "monto";
                // traer todos los tipos de cuotas que existen
                // Load data into Pagos. You can modify this code as needed.

                
                //System.Windows.Data.CollectionViewSource cuotasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cuotasViewSource")));
                //System.Data.Objects.ObjectQuery<Gimnasio.Cuotas> cuotasQuery = this.GetcuotasQuery(database1Entities);
                //cuotasViewSource.Source = cuotasQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            }
            else {
                System.Console.WriteLine("otra tecla");

            }
            
            
        }
    }
}
