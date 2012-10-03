using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Gimnasio
{

    // Modulo de Facturación
    // =====================
    // - Generar, validar y cargar las facturas en la base de datos.
    // - Imprimir las facturas.

    public static class Facturacion
    {

        static int[] myIVAsExArray = new int[1];
        static int[] myIVAs05Array = new int[1];
        static int[] myIVAs10Array = new int[1];

        // Generar, validar y almacenar las facturas. Recibe el pago a facturar.
        public static void DatosFactura(Pagos pago)
        {

            Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
            if (pago != null)   // Si se recibe un Pago válido.
            {
                Facturas current_factura = new Facturas();
                if (current_factura.idFactura == 0)                                 // Si el ID no existe.
                {
                    TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                    int timestamp = (int)time.TotalSeconds;
                    current_factura.idFactura = timestamp;                          // Nuevo ID = timestamp.
                }
                // Se cargan los datos correspondientes de la factura en el nuevo registro de la BD.
                current_factura.fk_pago = pago.idPago;
                current_factura.fk_cliente = pago.clientes.idCliente;

                // Se hace el query a la BD de facturas, para conocer si el pago ya fue facturado.
                string esql = "select value f from Facturas as f where f.fk_pago = " + (current_factura.fk_pago);
                var facturasVar = database1Entities.CreateQuery<Facturas>(esql);

                // Si no se encontró este pago entre los facturados.
                /*if (facturasVar.ToList().Count == 0)
                {*/

                FacturaDialog dlg = new FacturaDialog(pago);
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Se obtiene el número de factura ingresado en el cuadro de diálogo.
                    current_factura.Nro_Factura = dlg.ResponseText_NroFactura;
                }
                else
                {
                    return;
                }

                current_factura.Fecha_Emision = DateTime.Now;
                current_factura.Nombre_Pagador = pago.clientes.nombre + " " + pago.clientes.apellido;
                current_factura.RUC_Pagador = dlg.ResponseText_RUC;

                // Se determina el tipo de IVA y se acumula en el array correspondiente.
                myIVAsExArray[0] = 0;
                myIVAs05Array[0] = 0;
                myIVAs10Array[0] = 0;
                current_factura.Exentas_Total = 0;
                current_factura.IVA05_Total = 0;
                current_factura.IVA10_Total = 0;

                for (int i = 0; i < 1; i++)
                {
                    int thisMonto = Convert.ToInt32(pago.Cuotas.monto);
                    //myMontosArray[0] = thisMonto;
                    if (pago.Cuotas.tipoIVA == "10%")
                    {
                        myIVAs10Array[i] = thisMonto;
                    }
                    else if (pago.Cuotas.tipoIVA == "5%")
                    {
                        myIVAs05Array[i] = thisMonto;
                    }
                    else if (pago.Cuotas.tipoIVA == "Exentas")
                    {
                        myIVAsExArray[i] = thisMonto;
                    }

                    current_factura.Exentas_Total += myIVAsExArray[i];
                    current_factura.IVA05_Total += myIVAs05Array[i];
                    current_factura.IVA10_Total += myIVAs10Array[i];
                }

                // Total a Pagar
                current_factura.Monto_Total = current_factura.Exentas_Total + current_factura.IVA05_Total +
                    current_factura.IVA10_Total;
                // Liquidacion de IVA 5% y 10%
                current_factura.Liquidacion_IVA05 = current_factura.IVA05_Total / 21;
                current_factura.Liquidacion_IVA10 = current_factura.IVA10_Total / 11;
                current_factura.Liquidacion_IVA_Total = current_factura.Liquidacion_IVA05 + current_factura.Liquidacion_IVA10;

                current_factura.Concepto = "Pago cuota " + String.Format("{0:dd/MM/yyyy}", pago.fecha) +
                    " a " + String.Format("{0:dd/MM/yyyy}", pago.fecha_vencimiento);

                // Se agrega la factura a la BD en la tabla "Facturas".
                database1Entities.Facturas.AddObject(current_factura);
                database1Entities.SaveChanges();

                // Se actualiza el campo "ya_facturado" de la tabla Pagos
                // para indicar que ese pago ya fue facturado.
                database1Entities.ExecuteStoreCommand(
                    "UPDATE Pagos SET ya_facturado = 1 WHERE (Pagos.idPago = {0})", current_factura.fk_pago);
                database1Entities.SaveChanges();

                // Se llama al módulo de Impresión.
                Impresion.ImprimirFactura(current_factura, myIVAsExArray, myIVAs05Array, myIVAs10Array);

                MessageBox.Show("Se ingresó la factura al sistema.", "Nueva Factura");

            }
            else
            {
                System.Console.WriteLine("No se recibió un pago válido como parámetro.");
            }
        }
    }
}
