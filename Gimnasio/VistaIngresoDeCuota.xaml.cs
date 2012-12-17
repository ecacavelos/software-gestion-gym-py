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
    /// Interaction logic for VistaIngresoDeCuota.xaml
    /// </summary>
    public partial class VistaIngresoDeCuota : Window
    {
        Configuration c2;

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        public static bool IsOpen { get; private set; }

        string esqlCuotas = "SELECT value c FROM cuotas as c";
        int diasAHabilitar;
        DateTime fechaPago, /*fechaTemporal,*/ fechaUltimoVencimiento;
        int /*nroCedula,*/ idCliente, cuotaId;

        Gimnasio.clientes clienteActual;

        public VistaIngresoDeCuota()
        {
            this.diasAHabilitar = new int();
            this.fechaPago = new DateTime();
            //this.fechaTemporal = new DateTime();
            //this.nroCedula = new int();
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

        #region "Funciones Manejadoras de Carga y Descarga de la Ventana"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            textBoxNroCedula.Focus();
            // Cargar en el comboBox los parámetros de cuotas posibles, trayendo de la tabla Cuotas.
            var cuotas = database1Entities.CreateQuery<Cuotas>(esqlCuotas);
            foreach (Gimnasio.Cuotas tempCuota in cuotas)
            {
                ComboBoxItem elementoCombo = new ComboBoxItem();
                elementoCombo.Content = tempCuota.diasHabilitados + " - " + tempCuota.concepto;
                comboBoxTiposCuotas.Items.Add(elementoCombo);
            }
            //comboBoxTiposCuotas.ItemsSource = cuotas;
            //comboBoxTiposCuotas.DisplayMemberPath = "diasHabilitados";            
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        private void botonAplicarPago(object sender, RoutedEventArgs e)
        {
            // Cantidad de dias a habilitar, si está en 0 entonces aún no se seteó.
            if (diasAHabilitar == 0)
            {
                // Abrir una ventanita con el aviso de seteo de los dias que se quiere habilitar.
                MessageBox.Show("Debe seleccionar la cantidad de dias a habilitar");
            }
            else
            {
                // Si está correctamente seteada la cantidad de dias, comprobar si la fecha está seleccionada.
                if ((fechaPago.Year == 1) && (fechaPago.Day == 1) && (fechaPago.Month == 1))
                {
                    // Abrir una ventanita con el aviso de seteo de los dias que se quiere habilitar.
                    MessageBox.Show("Debe seleccionar la fecha");
                }
                else
                {
                    // Si aún no se selecciono ningún cliente.
                    if (textBoxNombre.Text == "")
                    {
                        // Todavia no está seteado ningun nombre.
                        // Abrir una ventanita con el aviso de seteo de los dias que se quiere habilitar.
                        MessageBox.Show("Debe seleccionar un cliente");
                    }
                    else
                    {
                        // Está todo seteado, aplicar el pago.
                        /* Calcular 30/7/1 dias para establecer la fecha de vencimiento en al tabla Pagos. 
                         * Por ejemplo: Dia actual  14/10/2012 y se paga por 30 dias (un mes), 
                         * entonces el 14/11/2011 sera la fecha de vencimiento */
                        if (this.fechaUltimoVencimiento < this.fechaPago)
                        {
                            TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                            int timestamp = (int)time.TotalSeconds;

                            Pagos pagoAAgregar = new Pagos();
                            pagoAAgregar.fk_cliente = this.idCliente;
                            pagoAAgregar.idPago = timestamp;
                            pagoAAgregar.fk_tipoCuota = this.cuotaId;
                            pagoAAgregar.fecha = this.fechaPago;
                            if (this.diasAHabilitar != 30)
                            {
                                pagoAAgregar.fecha_vencimiento = this.fechaPago.AddDays(this.diasAHabilitar);
                            }
                            else
                            {
                                pagoAAgregar.fecha_vencimiento = this.fechaPago.AddMonths(1);
                            }

                            // Agregamos el tipo de Pago (Cuota, Producto, Servicio, etc.) y la Descripcion.
                            pagoAAgregar.tipoPago = "Cuota";
                            pagoAAgregar.descripcionPago = "Pago cuota " + String.Format("{0:dd/MM/yyyy}", pagoAAgregar.fecha) + " a " + String.Format("{0:dd/MM/yyyy}", pagoAAgregar.fecha_vencimiento);

                            database1Entities.AddToPagos(pagoAAgregar);
                            if (database1Entities.SaveChanges() == 0)
                            {
                                MessageBox.Show("Hubo un problema de base de datos, por favor consule con los responsables de la aplicacion");
                            }
                            else
                            {
                                Pagos[] arrayPagosAgregados = new Pagos[1];
                                arrayPagosAgregados[0] = pagoAAgregar;
                                // Preguntamos si se quiere facturar el Pago siendo ingresado.
                                MessageBoxResult result;
                                result = MessageBox.Show("Desea imprimir una factura para este pago?", "Pago de Cuota", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                {
                                    Facturacion.DatosFactura(arrayPagosAgregados, clienteActual);
                                }
                                MessageBox.Show("El pago se aplicó correctamente.");
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe seleccionar una fecha posterior a la fecha del vencimiento de la cuota anterior");
                        }
                    }
                }
            }
        }

        // La selección de la fecha fue realizada.
        private void datePickerFechaInicialPago_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se obtiene la fecha elegida y se setea el campo correspondiente.
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
            string currentComboBoxContent = comboBoxTiposCuotas.SelectedValue.ToString().Remove(0, 38);
            string currentConcepto = (currentComboBoxContent.Substring(4, currentComboBoxContent.Length - 4).Trim());

            string esql = String.Format("SELECT value c FROM Cuotas as c WHERE c.concepto = '{0}'", currentConcepto);
            var tiposCuotaVar = database1Entities.CreateQuery<Cuotas>(esql);

            Cuotas cuotaSeleccionada = tiposCuotaVar.ToArray()[0];
            diasAHabilitar = (int)cuotaSeleccionada.diasHabilitados; // Acá se setean los dias a habilitar para el cliente.
            this.cuotaId = (int)cuotaSeleccionada.idCuota;
            //Console.WriteLine("Dias a habilitar = " + diasAHabilitar.ToString());

            textBoxMonto.Text = cuotaSeleccionada.monto;
        }

        /*
         * CALLBACK  que captura cualquier tecla presionada en el textbox con nombre textBoxNroCedula en el archivo VistaIngresoCuota.xaml
         * Descripcion: Cuando se presiona la tecla ENTER, se verifica si el nro. de cedula ingresado corresponde a un cliente en la base de datos. 
         * Si no existe entonces se emite una alerta y no se setea el campo nroCedula, si se encuentra el cliente, 
         * entonces se setea con el valor encontrado.
         */
        private void textBoxNroCedula_KeyDown(object sender, KeyEventArgs e)
        {
            // Se presionó ENTER.
            if (e.Key.ToString() == "Return")
            {
                string esql = "SELECT value c FROM clientes as c WHERE c.nro_cedula = '" + this.textBoxNroCedula.Text + "\'";
                var clientesVar = database1Entities.CreateQuery<clientes>(esql);
                ClienteSeleccionado.DataContext = clientesVar; // Establecer el data context para los elementos del cliente. OJO: Esto creo que no sirve.

                if (clientesVar.ToList().Count == 1) // Esta es la unica condicion correcta, que se seleccione un cliente.
                {
                    clienteActual = clientesVar.ToArray()[0];

                    this.idCliente = clientesVar.ToArray()[0].idCliente;
                    button1.IsEnabled = true;

                    if (clientesVar.ToArray()[0].Pagos.ToList().Count >= 1) // Existe al menos un pago
                    {
                        /*Pagos[] arrayListPagos; 
                        arrayListPagos = clientesVar.ToArray()[0].Pagos.ToArray();
                        Array.Sort(arrayListPagos);
                        this.fechaUltimoVencimiento = arrayListPagos[0].fecha_vencimiento.Value;
                        this.textBlockFechaVto.Text = arrayListPagos[0].fecha_vencimiento.Value.ToShortDateString(); // Traer el ultimo pago.*/
                        string fechaVencimientoQuery = "SELECT value p FROM Pagos as p WHERE p.fk_cliente=" + this.idCliente + " ORDER by p.fecha_vencimiento desc limit 1";
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
                else if (clientesVar.ToList().Count > 1) // Existe una inconsistencia en la base de datos. 
                {
                    MessageBox.Show("Existe mas de un usuario con este numero de cedula, por favor identifique el cliente y corriga el error.");
                }
                else if (clientesVar.ToList().Count == 0) // No se seleccionó ningun cliente.
                {
                    MessageBox.Show("No existe cliente con ese numero de cedula, por favor inserte o modifique el cliente con el numero de cedula.");
                }
            }
            else
            {
                //System.Console.WriteLine("Se presiono cualquier otra tecla que no es Return.");
            }
        }

        #region "Funciones relativas al Keypad USB"

        // Funciones para evitar que el keypad USB afecte los controles de esta ventana.

        private void textBoxNroCedula_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void comboBoxTiposCuotas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.c2 = Configuration.Deserialize("config.xml");
            if (this.c2.Keypad_usb == true)
            {
                e.Handled = true;
            }
        }

        private void datePickerFechaInicialPago_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void button2_PreviewKeyDown(object sender, KeyEventArgs e)
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
