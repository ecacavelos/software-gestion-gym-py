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

            }
            else {
                System.Console.WriteLine("otra tecla");

            }
            
            
        }
    }
}
