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

        /// <summary>
        /// Genera, valida y almacena las facturas.
        /// </summary>
        /// <param name="pago">El pago a facturar.</param>
        public static bool DatosFactura(Pagos[] pagos, clientes clienteFacturado)
        {

            int[] myIVAsExArray = new int[pagos.ToList().Count];
            int[] myIVAs05Array = new int[pagos.ToList().Count];
            int[] myIVAs10Array = new int[pagos.ToList().Count];
            string[] conceptosArray = new string[pagos.ToList().Count];

            Gimnasio.Database1Entities database1Entities = new Gimnasio.Database1Entities();
            if (pagos != null)   // Si se recibe un array de Pagos válido.
            {
                Facturas current_factura = new Facturas();
                if (current_factura.idFactura == 0)                                 // Si el ID no existe.
                {
                    TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                    int timestamp = (int)time.TotalSeconds;
                    current_factura.idFactura = timestamp;                          // Nuevo ID = timestamp.
                }
                // Se cargan los datos correspondientes de la factura en el nuevo registro de la BD.                
                current_factura.fk_cliente = clienteFacturado.idCliente;

                // Se hace el query a la BD de facturas, para conocer si el pago ya fue facturado.
                //string esql = "SELECT value f FROM Facturas as f WHERE f.fk_pago = " + (current_factura.fk_pago);
                //var facturasVar = database1Entities.CreateQuery<Facturas>(esql);

                // Si no se encontró este pago entre los facturados.
                /*if (facturasVar.ToList().Count == 0)
                {*/

                DialogFactura dlg = new DialogFactura(pagos);
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Se obtiene el número de factura ingresado en el cuadro de diálogo.
                    current_factura.Nro_Factura = dlg.ResponseText_NroFactura;
                }
                else
                {
                    return false;
                }

                current_factura.Fecha_Emision = DateTime.Now;
                current_factura.Nombre_Pagador = dlg.ResponseText_NombreApellido;
                current_factura.RUC_Pagador = dlg.ResponseText_RUC;

                // Se determina el tipo de IVA y se acumula en el array correspondiente.
                current_factura.Exentas_Total = 0;
                current_factura.IVA05_Total = 0;
                current_factura.IVA10_Total = 0;

                System.Console.WriteLine();
                System.Console.WriteLine(pagos.ToList().Count + " pagos en esta factura.");

                for (int i = 0; i < pagos.ToList().Count; i++)
                {
                    myIVAsExArray[i] = 0;
                    myIVAs05Array[i] = 0;
                    myIVAs10Array[i] = 0;

                    int thisMonto = Convert.ToInt32(pagos[i].Cuotas.monto);
                    //myMontosArray[0] = thisMonto;
                    if (pagos[i].Cuotas.tipoIVA == "10%")
                    {
                        myIVAs10Array[i] = thisMonto;
                    }
                    else if (pagos[i].Cuotas.tipoIVA == "5%")
                    {
                        myIVAs05Array[i] = thisMonto;
                    }
                    else if (pagos[i].Cuotas.tipoIVA == "Exentas")
                    {
                        myIVAsExArray[i] = thisMonto;
                    }

                    current_factura.Exentas_Total += myIVAsExArray[i];
                    current_factura.IVA05_Total += myIVAs05Array[i];
                    current_factura.IVA10_Total += myIVAs10Array[i];

                    conceptosArray[i] = "";
                    conceptosArray[i] = pagos[i].descripcionPago;

                    /*conceptosArray[i] = "Pago cuota " + String.Format("{0:dd/MM/yyyy}", pagos[i].fecha) +
                    " a " + String.Format("{0:dd/MM/yyyy}", pagos[i].fecha_vencimiento);*/

                }

                // Total a Pagar
                current_factura.Monto_Total = current_factura.Exentas_Total + current_factura.IVA05_Total +
                    current_factura.IVA10_Total;
                // Liquidacion de IVA 5% y 10%
                current_factura.Liquidacion_IVA05 = current_factura.IVA05_Total / 21;
                current_factura.Liquidacion_IVA10 = current_factura.IVA10_Total / 11;
                current_factura.Liquidacion_IVA_Total = current_factura.Liquidacion_IVA05 + current_factura.Liquidacion_IVA10;

                /*current_factura.Concepto = "Pago cuota " + String.Format("{0:dd/MM/yyyy}", pagos[0].fecha) +
                    " a " + String.Format("{0:dd/MM/yyyy}", pagos[pagos.ToList().Count - 1].fecha_vencimiento);*/
                current_factura.Concepto = "Pago de Cuotas...";

                // Se llama al módulo de Impresión.
                //Impresion.ImprimirFactura(current_factura, conceptosArray, myIVAsExArray, myIVAs05Array, myIVAs10Array);

                if (Impresion.ImprimirFactura(current_factura, conceptosArray, myIVAsExArray, myIVAs05Array, myIVAs10Array) == true)
                {
                    // Se agrega la factura a la BD en la tabla "Facturas".                
                    database1Entities.Facturas.AddObject(current_factura);
                    database1Entities.SaveChanges();

                    for (int i = 0; i < pagos.ToList().Count; i++)
                    {
                        // Se actualiza el campo "ya_facturado" de la tabla Pagos
                        // para indicar que ese pago ya fue facturado.
                        database1Entities.ExecuteStoreCommand(
                            "UPDATE Pagos SET ya_facturado = 1 WHERE (Pagos.idPago = {0})", pagos[i].idPago);
                        database1Entities.ExecuteStoreCommand(
                            "UPDATE Pagos SET fk_factura = {0} WHERE (Pagos.idPago = {1})", current_factura.idFactura, pagos[i].idPago);
                        database1Entities.SaveChanges();
                    }

                    MessageBox.Show("Se ingresó la factura al sistema.", "Nueva Factura");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                System.Console.WriteLine("No se recibió un pago válido como parámetro.");
                return false;
            }
        }
    }
}
