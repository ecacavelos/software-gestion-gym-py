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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gimnasio
{
    public static class Impresion
    {

        public static void ImprimirFactura(Facturas factura, int[] ArrayEx, int[] Array05, int[] Array10)
        {

            DateTime fecha = (DateTime)factura.Fecha_Emision;

            // Se genera el texto de la factura.
            System.Console.WriteLine();
            System.Console.WriteLine("Fecha de Emisión: " + factura.Fecha_Emision);
            System.Console.WriteLine("Nombre:           " + factura.Nombre_Pagador);
            System.Console.WriteLine("RUC:              " + factura.RUC_Pagador);
            System.Console.WriteLine("Nro de Factura:   " + factura.Nro_Factura);
            System.Console.WriteLine("Concepto:         " + factura.Concepto);
            System.Console.WriteLine("Total a Pagar:    " + factura.Monto_Total);
            System.Console.WriteLine("Liq  5% Total:    " + factura.Liquidacion_IVA05);
            System.Console.WriteLine("Liq 10% Total:    " + factura.Liquidacion_IVA10);
            System.Console.WriteLine(Numalet.ToCardinal((int)factura.Monto_Total));

            // Create the parent FixedDocument
            FixedDocument fixedDoc = new FixedDocument();

            // create a page
            FixedPage page1 = new FixedPage();
            page1.Width = fixedDoc.DocumentPaginator.PageSize.Width;
            page1.Height = fixedDoc.DocumentPaginator.PageSize.Height;

            // add some text to the page
            TextBlock factFechaDia = new TextBlock();
            factFechaDia.Text = fecha.Day.ToString();
            page1.Children.Add(factFechaDia);
            FixedPage.SetLeft(factFechaDia, 10);
            FixedPage.SetTop(factFechaDia, 20);

            TextBlock factFechaMes = new TextBlock();
            factFechaMes.Text = IntToMes(fecha.Month);
            page1.Children.Add(factFechaMes);
            FixedPage.SetLeft(factFechaMes, 70);
            FixedPage.SetTop(factFechaMes, 20);

            TextBlock factFechaAño = new TextBlock();
            factFechaAño.Text = fecha.Year.ToString();
            page1.Children.Add(factFechaAño);
            FixedPage.SetLeft(factFechaAño, 130);
            FixedPage.SetTop(factFechaAño, 20);

            TextBlock factNombre = new TextBlock();
            factNombre.Text = factura.Nombre_Pagador;
            page1.Children.Add(factNombre);
            FixedPage.SetLeft(factNombre, 10);
            FixedPage.SetTop(factNombre, 40);

            TextBlock factRUC = new TextBlock();
            factRUC.Text = factura.RUC_Pagador;
            page1.Children.Add(factRUC);
            FixedPage.SetLeft(factRUC, 200);
            FixedPage.SetTop(factRUC, 40);

            TextBlock factCant = new TextBlock();
            factCant.Text = "1";
            page1.Children.Add(factCant);
            FixedPage.SetLeft(factCant, 10);
            FixedPage.SetTop(factCant, 80);

            TextBlock factConcepto = new TextBlock();
            factConcepto.Text = factura.Concepto;
            page1.Children.Add(factConcepto);
            FixedPage.SetLeft(factConcepto, 200);
            FixedPage.SetTop(factConcepto, 80);

            //

            TextBlock factMontoIVAEx = new TextBlock();
            factMontoIVAEx.Text = ArrayEx[0].ToString();
            page1.Children.Add(factMontoIVAEx);
            FixedPage.SetLeft(factMontoIVAEx, 440);
            FixedPage.SetTop(factMontoIVAEx, 80);

            TextBlock factMontoIVA05 = new TextBlock();
            factMontoIVA05.Text = Array05[0].ToString();
            page1.Children.Add(factMontoIVA05);
            FixedPage.SetLeft(factMontoIVA05, 460);
            FixedPage.SetTop(factMontoIVA05, 80);

            TextBlock factMontoIVA10 = new TextBlock();
            factMontoIVA10.Text = Array10[0].ToString();
            page1.Children.Add(factMontoIVA10);
            FixedPage.SetLeft(factMontoIVA10, 480);
            FixedPage.SetTop(factMontoIVA10, 80);

            //

            TextBlock factMontoIVAExSubTotal = new TextBlock();
            factMontoIVAExSubTotal.Text = factura.Exentas_Total.ToString();
            page1.Children.Add(factMontoIVAExSubTotal);
            FixedPage.SetLeft(factMontoIVAExSubTotal, 440);
            FixedPage.SetTop(factMontoIVAExSubTotal, 180);

            TextBlock factMontoIVA05SubTotal = new TextBlock();
            factMontoIVA05SubTotal.Text = factura.IVA05_Total.ToString();
            page1.Children.Add(factMontoIVA05SubTotal);
            FixedPage.SetLeft(factMontoIVA05SubTotal, 460);
            FixedPage.SetTop(factMontoIVA05SubTotal, 180);

            TextBlock factMontoIVA10SubTotal = new TextBlock();
            factMontoIVA10SubTotal.Text = factura.IVA10_Total.ToString();
            page1.Children.Add(factMontoIVA10SubTotal);
            FixedPage.SetLeft(factMontoIVA10SubTotal, 480);
            FixedPage.SetTop(factMontoIVA10SubTotal, 180);

            TextBlock factTotalEnLetras = new TextBlock();
            factTotalEnLetras.Text = Numalet.ToCardinal((int)factura.Monto_Total).ToUpper() + " GUARANÍES";
            page1.Children.Add(factTotalEnLetras);
            FixedPage.SetLeft(factTotalEnLetras, 200);
            FixedPage.SetTop(factTotalEnLetras, 200);

            TextBlock factTotalAPagar = new TextBlock();
            factTotalAPagar.Text = factura.Monto_Total.ToString();
            page1.Children.Add(factTotalAPagar);
            FixedPage.SetLeft(factTotalAPagar, 480);
            FixedPage.SetTop(factTotalAPagar, 210);

            TextBlock factLiqIVA05 = new TextBlock();
            factLiqIVA05.Text = factura.Liquidacion_IVA05.ToString();
            page1.Children.Add(factLiqIVA05);
            FixedPage.SetLeft(factLiqIVA05, 230);
            FixedPage.SetTop(factLiqIVA05, 230);

            TextBlock factLiqIVA10 = new TextBlock();
            factLiqIVA10.Text = factura.Liquidacion_IVA10.ToString();
            page1.Children.Add(factLiqIVA10);
            FixedPage.SetLeft(factLiqIVA10, 310);
            FixedPage.SetTop(factLiqIVA10, 230);

            TextBlock factLiqIVATotal = new TextBlock();
            factLiqIVATotal.Text = (factura.Liquidacion_IVA05 + factura.Liquidacion_IVA10).ToString();
            page1.Children.Add(factLiqIVATotal);
            FixedPage.SetLeft(factLiqIVATotal, 380);
            FixedPage.SetTop(factLiqIVATotal, 230);

            // add the page to the document
            PageContent page1Content = new PageContent();
            ((System.Windows.Markup.IAddChild)page1Content).AddChild(page1);
            fixedDoc.Pages.Add(page1Content);

            DocumentPaginator aDocPage = ((IDocumentPaginatorSource)fixedDoc).DocumentPaginator;
            PrintDialog pDialog = new PrintDialog();

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                fixedDoc.DocumentPaginator.PageSize = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);
                pDialog.PrintDocument(aDocPage, "Table Printing Test");
            }

        }

        public static string IntToMes(int mymes)
        {
            switch (mymes)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return "Undefined";
            }
        }

    }
}
