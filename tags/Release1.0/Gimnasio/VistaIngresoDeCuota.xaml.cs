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
    public partial class VistaIngresoDeCuota : Window
    {
         Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        string esqlCuotas = "select value c from cuotas as c";
        int diasAHabilitar;
        DateTime fechaPago, fechaUltimoVencimiento;
        int nroCedula, idCliente, cuotaId;

        public VistaIngresoDeCuota()
        {
            this.diasAHabilitar = new int();
            this.fechaPago = new DateTime();
            this.nroCedula = new int();
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
            // cargar en el combobox los parametros de cuotas posibles, trayendo de la tabla Cuotas. 
            var cuotas = database1Entities.CreateQuery<Cuotas>(esqlCuotas);
            comboBoxTiposCuotas.ItemsSource = cuotas;
            comboBoxTiposCuotas.DisplayMemberPath = "diasHabilitados";

        }

        private void botonAplicarPago(object sender, RoutedEventArgs e)
        {
            //Cantidad de dias a habilitar, si esta en 0 entonces aun no se seteo
            if (diasAHabilitar == 0)
            {
                //abrir una ventanita con el aviso de seteo de los dias que se quiere habilitar
                MessageBox.Show("Debe seleccionar la cantidad de dias a habilitar");

            }
            else
            { // esta correctamente seteada la cantidad de dias, comprobar si la fecha esta seleccionada
                if ((fechaPago.Year == 1) && (fechaPago.Day == 1) && (fechaPago.Month == 1))
                {
                    //abrir una ventanita con el aviso de seteo de los dias que se quiere habilitar
                    MessageBox.Show("Debe seleccionar la fecha");

                }
                else
                {
                    if (textBoxNombre.Text == "")// aun no se selecciono ningun cliente
                    {//todavia no esta seteado ningun nombre.
                        //abrir una ventanita con el aviso de seteo de los dias que se quiere habilitar
                        MessageBox.Show("Debe seleccionar un cliente");

                    }
                    else
                    { // esta todo seteado, aplicar el pago 
                        /* Calcular 30/7/1 dias para establecer la fecha de vencimiento en al tabla Pagos. 
                        *Por ejemplo: Dia actual  14/10/2012 y se paga por 30 dias, entonces el 13/11/2011 sera la fecha de vencimiento
                       */
                        if (this.fechaUltimoVencimiento < this.fechaPago)
                        {
                            TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                            int timestamp = (int)time.TotalSeconds;

                            Pagos pagoAAgregar = new Pagos();
                            pagoAAgregar.fk_cliente = this.idCliente;
                            pagoAAgregar.idPago = timestamp;
                            pagoAAgregar.fk_tipoCuota = this.cuotaId;
                            pagoAAgregar.fecha = this.fechaPago;
                            pagoAAgregar.fecha_vencimiento = this.fechaPago.AddDays(this.diasAHabilitar);
                            database1Entities.AddToPagos(pagoAAgregar);
                            if (database1Entities.SaveChanges() == 0)
                            {

                                MessageBox.Show("Hubo un problema de base de datos, por favor consule con los responsables de la aplicacion");

                            }
                            else
                            {
                                MessageBox.Show("El pago se aplico correctamente");
                                this.Close();
                            }
                        }
                        else {

                            MessageBox.Show("Debe seleccionar una fecha posterior a la fecha del vencimiento de la cuota anterior");
                        }
                        
                        //pagoAAgregar.fech
                        //pagoAAgregar.fk_tipoCuota = 
                    }
                }

            }



        }

        // la seleccion de la fecha fue realizada
        private void datePickerFechaInicialPago_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            // se obtiene la fecha elegida y se setea el campo correspondiente
            if (this.fechaUltimoVencimiento < (System.DateTime)datePickerFechaInicialPago.SelectedDate)
            {
                this.fechaPago = (System.DateTime)datePickerFechaInicialPago.SelectedDate;
            }
            else 
            {
                MessageBox.Show("Debe seleccionar una fecha posterior a la fecha del vencimiento de la ultima cuota");
            }
            
        }

        private void comboBoxTiposCuotas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //comboBoxTiposCuotas.
            Cuotas cuotaSeleccionada = (Cuotas)comboBoxTiposCuotas.SelectedItem;
            diasAHabilitar = (int)cuotaSeleccionada.diasHabilitados; // aca se setean los dias a habilitar para el cliente
            this.cuotaId = (int)cuotaSeleccionada.idCuota;
            Console.WriteLine("Dias a habilitar = " + diasAHabilitar.ToString());

        }

        /*
         * CALLBACK  que captura cualquier tecla presionada en el textbox con nombre textBoxNroCedula en el archivo VistaIngresoCuota.xaml
         * Descripcion: Cuando se presiona la tecla ENTER, se verifica si el nro. de cedula ingresado corresponde a un cliente en la base de datos. 
         * Si no existe entonces se emite una alerta y no se setea el campo nroCedula, si se encuentra el cliente, 
         * entonces se setea con el valor encontrado.
         */
        private void textBoxNroCedula_KeyDown(object sender, KeyEventArgs e)
        {
            // se presiono ENTER
            if (e.Key.ToString() == "Return")
            {
                string esql = "select value c from clientes as c where c.nro_cedula= '" + this.textBoxNroCedula.Text + "\'";
                var clientesVar = database1Entities.CreateQuery<clientes>(esql);
                ClienteSeleccionado.DataContext = clientesVar; // Establecer el data context para los elementos del cliente. OJO: Esto creo que no sirve.

                if (clientesVar.ToList().Count == 1) // Esta es la unica condicion correcta, que se seleccione un cliente.  
                {
                    this.idCliente = clientesVar.ToArray()[0].idCliente;

                    if (clientesVar.ToArray()[0].Pagos.ToList().Count >= 1)// Existe al menos un pago
                    {
                        /*Pagos[] arrayListPagos; 
                        arrayListPagos = clientesVar.ToArray()[0].Pagos.ToArray();
                        Array.Sort(arrayListPagos);
                        this.fechaUltimoVencimiento = arrayListPagos[0].fecha_vencimiento.Value;
                        this.textBlockFechaVto.Text = arrayListPagos[0].fecha_vencimiento.Value.ToShortDateString(); // Traer el ultimo pago.*/
                        string fechaVencimientoQuery = "select value p from Pagos as p where p.fk_cliente=" + this.idCliente + " order by p.fecha_vencimiento desc limit 1";
                        var fechaUltimoVencimientoResult = database1Entities.CreateQuery<Pagos>(fechaVencimientoQuery);
                        this.fechaUltimoVencimiento = fechaUltimoVencimientoResult.ToArray()[0].fecha_vencimiento.Value;
                        this.textBlockFechaVto.Text = fechaUltimoVencimientoResult.ToArray()[0].fecha_vencimiento.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        this.fechaUltimoVencimiento = new DateTime();
                        this.textBlockFechaVto.Text = "Sin pagos de cuotas";
                    }
                }
                else if (clientesVar.ToList().Count > 1)// Existe una inconsistencia en la base de datos. 
                     { 
                        MessageBox.Show("Existe mas de un usuario con este numero de cedula, por favor identifique el cliente y corriga el error.");
                     }
                else if (clientesVar.ToList().Count == 0)// No se selecciono ningun cliente.
                {
                    MessageBox.Show("No existe cliente con ese numero de cedula, por favor inserte o modifique el cliente con el numero de cedula.");
                }
            }
            else
            {
                //System.Console.WriteLine("se presiono cualquier otra tecla que no es Return");

            }
        }

    }
}
