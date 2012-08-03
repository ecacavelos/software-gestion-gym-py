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
                if (facturasVar.ToList().Count == 0)
                {
                    //System.Windows.Forms.DialogResult result;
                    //result = System.Windows.Forms.MessageBox.Show("Factura Nro: " + "001-001", "Número de Factura", System.Windows.Forms.MessageBoxButtons.OK);
                    //if (result == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    current_factura.Nro_Factura = "001-001";
                    //}

                    FacturaDialog dlg = new FacturaDialog();
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        //System.Console.WriteLine("Yes");
                        //System.Console.WriteLine(dlg.ResponseText);
                        current_factura.Nro_Factura = dlg.ResponseText;
                    }
                    else
                    {
                        //System.Console.WriteLine("No");
                        return;
                    }

                    current_factura.Fecha_Emision = DateTime.Now;
                    current_factura.Nombre_Pagador = pago.clientes.nombre + " " + pago.clientes.apellido;
                    //current_factura.RUC = pago.clientes.nro_cedula;
                    //current_factura.Monto = pago.Cuotas.monto;

                    // Se genera el texto de la factura.
                    // TODO: Llamar al modulo de impresión.
                    System.Console.WriteLine();
                    System.Console.WriteLine("Fecha de Emision de Factura: " + DateTime.Today.ToString("d"));
                    System.Console.WriteLine("Nombre: " + pago.clientes.nombre);
                    System.Console.WriteLine("Apellido: " + pago.clientes.apellido);
                    System.Console.WriteLine("RUC: " + pago.clientes.nro_cedula);
                    System.Console.WriteLine("Monto: " + pago.Cuotas.monto);
                    System.Console.WriteLine("Concepto: " + "Pago cuota " + String.Format("{0:dd/MM/yyyy}", pago.fecha) + " a " + String.Format("{0:dd/MM/yyyy}", pago.fecha_vencimiento));

                    // Se agrega la factura a la BD en la tabla "Facturas".
                    database1Entities.Facturas.AddObject(current_factura);
                    database1Entities.SaveChanges();

                    // Se actualiza el campo "ya_facturado" de la tabla Pagos para indicar que ese pago ya fue facturado.
                    database1Entities.ExecuteStoreCommand("UPDATE Pagos SET ya_facturado = 1 WHERE (Pagos.idPago = {0})", current_factura.fk_pago);
                    database1Entities.SaveChanges();

                }
                else
                {
                    System.Console.WriteLine("No puede volver a facturar esto.");
                }

            }
            else
            {
                System.Console.WriteLine("No se recibió un pago válido como parámetro.");
            }
        }
    }
}
