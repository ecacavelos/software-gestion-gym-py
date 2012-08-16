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

        public static void ImprimirFactura(Facturas factura)
        {

            // Create the parent FlowDocument
            FlowDocument flowDoc = new FlowDocument();

            // Create the Table...
            Table table1 = new Table();
            // ...and add it to the FlowDocument Blocks collection.
            flowDoc.Blocks.Add(table1);

            // Set some global formatting properties for the table.
            table1.CellSpacing = 10;
            table1.Background = Brushes.White;

            // Create 6 columns and add them to the table's Columns collection.
            int numberOfColumns = 7;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table1.Columns.Add(new TableColumn());
            }

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Add the first (title) row.
            table1.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table1.RowGroups[0].Rows[0];

            // La fila de la fecha.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(String.Format("{0:dd/MM/yyyy}", factura.Fecha_Emision)))));
            currentRow.Cells[0].ColumnSpan = 8;

            // La fila de Nombre y RUC.
            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[1];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Nombre: "))));
            currentRow.Cells[0].ColumnSpan = 1;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.Nombre_Pagador))));
            currentRow.Cells[1].ColumnSpan = 4;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("RUC.: "))));
            currentRow.Cells[2].ColumnSpan = 1;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.RUC_Pagador))));
            currentRow.Cells[3].ColumnSpan = 2;

            table1.RowGroups[0].Rows.Add(new TableRow());
            table1.RowGroups[0].Rows[2].Cells.Add(new TableCell(new Paragraph(new Run(""))));
            table1.RowGroups[0].Rows.Add(new TableRow());
            table1.RowGroups[0].Rows[3].Cells.Add(new TableCell(new Paragraph(new Run(""))));
            table1.RowGroups[0].Rows.Add(new TableRow());
            table1.RowGroups[0].Rows[4].Cells.Add(new TableCell(new Paragraph(new Run(""))));

            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[5];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(1.ToString()))));
            currentRow.Cells[0].ColumnSpan = 1;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.Concepto))));
            currentRow.Cells[1].ColumnSpan = 4;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.Exentas_Total.ToString()))));
            currentRow.Cells[2].ColumnSpan = 1;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.IVA05_Total.ToString()))));
            currentRow.Cells[3].ColumnSpan = 1;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.IVA10_Total.ToString()))));
            currentRow.Cells[4].ColumnSpan = 1;

            table1.RowGroups[0].Rows.Add(new TableRow());
            table1.RowGroups[0].Rows[6].Cells.Add(new TableCell(new Paragraph(new Run(""))));

            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[7];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Numalet.ToCardinal((int)factura.Monto_Total).ToUpper() + " GUARANÍES."))));
            currentRow.Cells[0].ColumnSpan = 7;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.Monto_Total.ToString()))));
            currentRow.Cells[1].ColumnSpan = 1;

            table1.RowGroups[0].Rows.Add(new TableRow());
            currentRow = table1.RowGroups[0].Rows[8];
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Liquidación"))));
            currentRow.Cells[0].ColumnSpan = 2;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.Liquidacion_IVA05.ToString()))));
            currentRow.Cells[1].ColumnSpan = 2;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(factura.Liquidacion_IVA10.ToString()))));
            currentRow.Cells[2].ColumnSpan = 2;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Total: " + (factura.Liquidacion_IVA05 + factura.Liquidacion_IVA10).ToString()))));
            currentRow.Cells[3].ColumnSpan = 2;

            flowDoc.PagePadding = new Thickness(100);       // Define el margen alrededor de la página

            DocumentPaginator aDocPage = ((IDocumentPaginatorSource)flowDoc).DocumentPaginator;
            PrintDialog pDialog = new PrintDialog();

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                pDialog.PrintDocument(aDocPage, "Table Printing Test");
            }

        }

    }
}
