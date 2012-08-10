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
    /// Interaction logic for FacturaDialog.xaml
    /// </summary>
    public partial class FacturaDialog : Window
    {

        Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();

        public FacturaDialog(Pagos pago)
        {
            InitializeComponent();

            int lastNroFactura = 0;
            textBoxNroFactura.Text = "0000001";

            textBoxNombre.Text = pago.clientes.nombre + " " + pago.clientes.apellido;
            if (pago.clientes.RUC != null)
            {
                textBoxRUC.Text = pago.clientes.RUC;
            }
            else
            {
                System.Console.WriteLine("RUC es null, se sugiere nrodecedula.");
                textBoxRUC.Text = pago.clientes.nro_cedula;
                textBoxRUC.Background = Brushes.MistyRose;
            }

            // Hacemos un query a la base de datos para obtener todas las facturas.
            string esql = "select value f from Facturas as f";
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

        public string ResponseText
        {
            get { return textBoxNroFactura.Text; }
            set { textBoxNroFactura.Text = value; }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(textBoxNroFactura.Text);
            }
            catch (Exception h)
            {
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
                    textBoxRUC.Background = Brushes.MintCream;
                }
                else
                // Si estimamos que el RUC es incorrecto, indicamos en rojo.
                {
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

    }
}
