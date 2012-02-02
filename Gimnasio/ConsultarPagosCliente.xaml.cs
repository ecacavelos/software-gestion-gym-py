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
    /// Interaction logic for ConsultarPagosCliente.xaml
    /// </summary>
    public partial class ConsultarPagosCliente : Window
    {
        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        public ConsultarPagosCliente()
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
        private void textBoxNroCedula_KeyDown(object sender, KeyEventArgs e)
        {
            // se presiono ENTER
            if (e.Key.ToString() == "Return")
            {


                string esql = "select value c from clientes as c where c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                var clientesVar = database1Entities.CreateQuery<clientes>(esql);

                //Chequear si existe un cliente con ese nro. de Cedula. 
                if (clientesVar.ToList().Count == 0)// NO HAY NADA. 
                {
                    // NO hacer nada. 
                }
                else //HAY ALGO. OJO: Puede que haya mas de un cliente, porque en la base de datos no esta el nro. de cedula como unico. 
                {
                    if (clientesVar.ToList().Count == 1)// Solo se selecciono un cliente, esta correcto
                    {
                        this.textBoxNombre.Text = clientesVar.ToArray()[0].nombre;
                        this.textBoxApellido.Text = clientesVar.ToArray()[0].apellido;

                        string fechaVencimientoQuery = "select value p from Pagos as p where p.fk_cliente=" + clientesVar.ToArray()[0].idCliente + " order by p.fecha_vencimiento desc limit 1";
                        var fechaUltimoVencimientoResult = database1Entities.CreateQuery<Pagos>(fechaVencimientoQuery);

                        if (fechaUltimoVencimientoResult.ToList().Count == 0)//NO TIENE PAGOS REALIZADOS
                        {
                            MessageBox.Show("Este cliente no tiene pagos realizados.");
                        }
                        else //TIENE PAGOS REALIZADOS.
                        {
                            //llenar el datagrid.
                            this.textBlockFechaVto.Text = fechaUltimoVencimientoResult.ToArray()[0].fecha_vencimiento.Value.ToLongDateString();
                        }
                    }
                }
            }
            else
            {
                System.Console.WriteLine("se presiono cualquier otra tecla que no es Return");

            }

        }
    }
}
