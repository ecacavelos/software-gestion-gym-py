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
    /// Interaction logic for DialogFactura.xaml
    /// </summary>
    public partial class DialogFactura : Window
    {

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
        bool validRUC = false;
        Pagos thisPago;

        public string ResponseText_NroFactura
        {
            get { return textBoxNroFactura.Text; }
            set { textBoxNroFactura.Text = value; }
        }

        public string ResponseText_NombreApellido
        {
            get { return textBoxNombre.Text; }
            set { textBoxNombre.Text = value; }
        }

        public string ResponseText_RUC
        {
            get { return textBoxRUC.Text; }
            set { textBoxRUC.Text = value; }
        }

        public DialogFactura(Pagos[] pagos)
        {
            InitializeComponent();

            foreach (Pagos tempPago in pagos)
            {
                // Ejecutar el siguiente Query en la BD para actualizar las descripciones de los Pagos:
                // UPDATE Pagos SET descripcionPago = 'Pago Cuota ' + CONVERT(nvarchar(100), fecha, 103) + 
                //    ' a ' + CONVERT(nvarchar(100), fecha_vencimiento, 103)                
                if (tempPago.descripcionPago == null)
                {
                    System.Console.WriteLine("Generando nueva descripción del Pago.");
                    tempPago.descripcionPago = "Pago cuota " + String.Format("{0:dd/MM/yyyy}", tempPago.fecha) +
                        " a " + String.Format("{0:dd/MM/yyyy}", tempPago.fecha_vencimiento);
                }
            }

            dataGridFacturaPreview.ItemsSource = pagos;
            int montoTotal = 0;
            thisPago = pagos[0];

            int lastNroFactura = 0;
            textBoxNroFactura.Text = "0000001";

            textBoxNombre.Text = thisPago.clientes.nombre + " " + thisPago.clientes.apellido;
            // Si existe un RUC cargado en la tabla clientes, se usa dicho RUC, si no, usamos el Nro de Cédula.
            if (thisPago.clientes.RUC != null)
            {
                textBoxRUC.Text = thisPago.clientes.RUC;
            }
            else
            {
                textBoxRUC.Text = thisPago.clientes.nro_cedula;
                textBoxRUC.Background = Brushes.MistyRose;
            }

            for (int i = 0; i < pagos.ToList().Count; i++)
            {
                int tempMonto = 0;
                Int32.TryParse(pagos[i].Cuotas.monto, out tempMonto);
                montoTotal += tempMonto;
            }

            labelMontoTotal.Content = "Monto Total: " + montoTotal.ToString("#,##0");

            // Hacemos un query a la base de datos para obtener todas las facturas.
            string esql = "SELECT value f FROM Facturas as f";
            var facturasVar = database1Entities.CreateQuery<Facturas>(esql);

            // Si existe al menos una factura.
            if (facturasVar.ToList().Count > 0)
            {
                // Pasamos el string "Nro_Factura" obtenido de la ultima entrada de la tabla Facturas a int
                Int32.TryParse(facturasVar.ToList().ElementAt(facturasVar.ToList().Count - 1).Nro_Factura, out lastNroFactura);
                // Incrementamos en 1 la ultima factura y desplegamos el valor sugerido en el textbox
                lastNroFactura++;
                textBoxNroFactura.Text = lastNroFactura.ToString("0000000");
            }

        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(textBoxNroFactura.Text);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                MessageBox.Show("Por favor introduzca sólo números.");
            }

            if (temp > 0)
            {
                // Hacemos un query a la base de datos para obtener todas las facturas.
                string esql = "select value f from Facturas as f where f.Nro_Factura = '" + textBoxNroFactura.Text + "'";
                var facturasVar = database1Entities.CreateQuery<Facturas>(esql);

                // Si ya no existe una factura con ese número.
                if (facturasVar.ToList().Count == 0)
                {
                    if (validRUC == true)
                    {
                        // Se actualiza el campo "RUC" de la tabla Clientes si el RUC incluye digito verificador.                                        
                        database1Entities.ExecuteStoreCommand("UPDATE clientes SET RUC = {0} WHERE (clientes.idCliente = {1})", textBoxRUC.Text, thisPago.clientes.idCliente);
                        database1Entities.SaveChanges();
                    }
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Este número de factura ya existe.");
                }
            }
            else
            {

            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        #region "Funciones de validación para el RUC"

        private void textBoxRUC_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Verificamos que haya al menos 2 caracteres en el cuadro de RUC.
            if (textBoxRUC.Text.Length > 2)
            {
                string last2 = textBoxRUC.Text.Substring(textBoxRUC.Text.Length - 2);
                char lastChar = last2[1];

                // Si el RUC termina en un guión seguido de un número, indicamos en verde.
                if (last2.IndexOf("-") == 0 && Char.IsNumber(lastChar))
                {
                    validRUC = true;
                    textBoxRUC.Background = Brushes.MintCream;
                }
                else
                // Si estimamos que el RUC es incorrecto, indicamos en rojo.
                {
                    validRUC = false;
                    textBoxRUC.Background = Brushes.MistyRose;
                }
            }
            else
            {
                textBoxRUC.Background = Brushes.MistyRose;
            }
        }

        private void textBoxRUC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char tempChar = e.Text[0];

            // No se puedan agregar letras ni símbolos (excepto el guion '-').
            if (Char.IsLetter(tempChar))
            {
                e.Handled = true;
            }
            if (Char.IsSymbol(tempChar))
            {
                e.Handled = true;
            }
            if (Char.IsPunctuation(tempChar) && tempChar != '-')
            {
                e.Handled = true;
            }

        }

        private void textBoxRUC_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Evitamos que la Barra Espaciadora agregue espacios en blanco al textBox.
            if (e.Key.GetHashCode() == 18 || e.Key.ToString() == "Space")
            {
                e.Handled = true;
            }
        }

        #endregion

    }
}
