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

        public static void ImprimirFactura(Facturas factura, string[] ArrayConceptos, int[] ArrayEx, int[] Array05, int[] Array10)
        {
            Configuration c2 = Configuration.Deserialize("config.xml");

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
            System.Console.WriteLine("Liq IVA Total:    " + factura.Liquidacion_IVA_Total);
            System.Console.WriteLine(Numalet.ToCardinal((int)factura.Monto_Total));

            // Create the parent FixedDocument
            FixedDocument fixedDoc = new FixedDocument();

            // Creamos la pagina.
            FixedPage page1 = new FixedPage();
            page1.Width = fixedDoc.DocumentPaginator.PageSize.Width;
            page1.Height = fixedDoc.DocumentPaginator.PageSize.Height;

            Configuration.ImpresionCoords pos = c2.CoordenadasImpresion;

            // Agregamos los elementos de la factura.
            TextBlock factFechaDia = new TextBlock();
            factFechaDia.Text = fecha.Day.ToString();
            page1.Children.Add(factFechaDia);
            FixedPage.SetLeft(factFechaDia, pos.xFechaDia);
            FixedPage.SetTop(factFechaDia, pos.yFecha);

            TextBlock factFechaMes = new TextBlock();
            factFechaMes.Text = IntToMes(fecha.Month);
            page1.Children.Add(factFechaMes);
            FixedPage.SetLeft(factFechaMes, pos.xFechaMes);
            FixedPage.SetTop(factFechaMes, pos.yFecha);

            TextBlock factFechaAño = new TextBlock();
            factFechaAño.Text = fecha.Year.ToString();
            page1.Children.Add(factFechaAño);
            FixedPage.SetLeft(factFechaAño, pos.xFechaAño);
            FixedPage.SetTop(factFechaAño, pos.yFecha);

            TextBlock factNombre = new TextBlock();
            factNombre.Text = factura.Nombre_Pagador;
            page1.Children.Add(factNombre);
            FixedPage.SetLeft(factNombre, pos.xNombre);
            FixedPage.SetTop(factNombre, pos.yNombre);

            TextBlock factRUC = new TextBlock();
            factRUC.Text = factura.RUC_Pagador;
            page1.Children.Add(factRUC);
            FixedPage.SetLeft(factRUC, pos.xRUC);
            FixedPage.SetTop(factRUC, pos.yRUC);

            TextBlock factContadoCredito = new TextBlock();
            factContadoCredito.Text = "X";
            page1.Children.Add(factContadoCredito);
            FixedPage.SetLeft(factContadoCredito, pos.xContadoCredito);
            FixedPage.SetTop(factContadoCredito, pos.yFecha);

            // Esta sección de impresión es variable y dependiente de la cantidad de "ítems" a ser facturados.

            for (int i = 0; i < ArrayEx.ToList().Count; i++)
            {
                TextBlock factCant = new TextBlock();
                factCant.Text = "1";
                page1.Children.Add(factCant);
                FixedPage.SetLeft(factCant, pos.xItemCant);
                FixedPage.SetTop(factCant, pos.yItem + 40 * i);

                TextBlock factConcepto = new TextBlock();
                /*factConcepto.Text = factura.Concepto;*/
                factConcepto.Text = ArrayConceptos[i];
                factConcepto.Width = 150;
                factConcepto.TextWrapping = TextWrapping.Wrap;
                page1.Children.Add(factConcepto);
                FixedPage.SetLeft(factConcepto, pos.xItemConcepto);
                FixedPage.SetTop(factConcepto, pos.yItem + 40 * i);

                TextBlock factMontoIVAEx = new TextBlock();
                factMontoIVAEx.Text = ArrayEx[i].ToString();
                factMontoIVAEx.TextAlignment = TextAlignment.Center;
                page1.Children.Add(factMontoIVAEx);
                FixedPage.SetLeft(factMontoIVAEx, pos.xItemIVAExentas);
                FixedPage.SetTop(factMontoIVAEx, pos.yItem + 40 * i);

                TextBlock factMontoIVA05 = new TextBlock();
                factMontoIVA05.Text = Array05[i].ToString();
                factMontoIVA05.TextAlignment = TextAlignment.Center;
                page1.Children.Add(factMontoIVA05);
                FixedPage.SetLeft(factMontoIVA05, pos.xItemIVA05);
                FixedPage.SetTop(factMontoIVA05, pos.yItem + 40 * i);

                TextBlock factMontoIVA10 = new TextBlock();
                factMontoIVA10.Text = Array10[i].ToString();
                factMontoIVA10.TextAlignment = TextAlignment.Center;
                page1.Children.Add(factMontoIVA10);
                FixedPage.SetLeft(factMontoIVA10, pos.xItemIVA10);
                FixedPage.SetTop(factMontoIVA10, pos.yItem + 40 * i);
            }

            //

            TextBlock factMontoIVAExSubTotal = new TextBlock();
            factMontoIVAExSubTotal.Text = factura.Exentas_Total.ToString();
            factMontoIVAExSubTotal.TextAlignment = TextAlignment.Center;
            page1.Children.Add(factMontoIVAExSubTotal);
            FixedPage.SetLeft(factMontoIVAExSubTotal, pos.xSubTotalIVAExentas);
            FixedPage.SetTop(factMontoIVAExSubTotal, pos.ySubTotal);

            TextBlock factMontoIVA05SubTotal = new TextBlock();
            factMontoIVA05SubTotal.Text = factura.IVA05_Total.ToString();
            factMontoIVAExSubTotal.TextAlignment = TextAlignment.Center;
            page1.Children.Add(factMontoIVA05SubTotal);
            FixedPage.SetLeft(factMontoIVA05SubTotal, pos.xSubTotalIVA05);
            FixedPage.SetTop(factMontoIVA05SubTotal, pos.ySubTotal);

            TextBlock factMontoIVA10SubTotal = new TextBlock();
            factMontoIVA10SubTotal.Text = factura.IVA10_Total.ToString();
            factMontoIVAExSubTotal.TextAlignment = TextAlignment.Center;
            page1.Children.Add(factMontoIVA10SubTotal);
            FixedPage.SetLeft(factMontoIVA10SubTotal, pos.xSubTotalIVA10);
            FixedPage.SetTop(factMontoIVA10SubTotal, pos.ySubTotal);

            TextBlock factTotalEnLetras = new TextBlock();
            factTotalEnLetras.Text = Numalet.ToCardinal((int)factura.Monto_Total).ToUpper() + " GUARANÍES";
            page1.Children.Add(factTotalEnLetras);
            FixedPage.SetLeft(factTotalEnLetras, pos.xTotalEnLetras);
            FixedPage.SetTop(factTotalEnLetras, pos.yTotalEnLetras);

            TextBlock factTotalAPagar = new TextBlock();
            factTotalAPagar.Text = factura.Monto_Total.ToString();
            page1.Children.Add(factTotalAPagar);
            FixedPage.SetLeft(factTotalAPagar, pos.xTotalPagar);
            FixedPage.SetTop(factTotalAPagar, pos.yTotalEnLetras);

            TextBlock factLiqIVA05 = new TextBlock();
            factLiqIVA05.Text = factura.Liquidacion_IVA05.ToString();
            page1.Children.Add(factLiqIVA05);
            FixedPage.SetLeft(factLiqIVA05, pos.xTotalIVA05);
            FixedPage.SetTop(factLiqIVA05, pos.yTotal);

            TextBlock factLiqIVA10 = new TextBlock();
            factLiqIVA10.Text = factura.Liquidacion_IVA10.ToString();
            page1.Children.Add(factLiqIVA10);
            FixedPage.SetLeft(factLiqIVA10, pos.xTotalIVA10);
            FixedPage.SetTop(factLiqIVA10, pos.yTotal);

            TextBlock factLiqIVATotal = new TextBlock();
            factLiqIVATotal.Text = factura.Liquidacion_IVA_Total.ToString();
            page1.Children.Add(factLiqIVATotal);
            FixedPage.SetLeft(factLiqIVATotal, pos.xTotalIVAGeneral);
            FixedPage.SetTop(factLiqIVATotal, pos.yTotal);

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
