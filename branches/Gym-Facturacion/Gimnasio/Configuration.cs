using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Gimnasio
{
    [Serializable]
    public class Configuration
    {
        // Datos Relativos a las Coordenadas de Impresión.
        public struct ImpresionCoords
        {
            public int yFecha;
            public int xFechaDia, xFechaMes, xFechaAño;

            public int xContadoCredito;

            public int xNombre, yNombre;
            public int xRUC, yRUC;

            public int yItem;
            public int xItemCant, xItemConcepto;
            public int xItemIVAExentas, xItemIVA05, xItemIVA10;

            public int ySubTotal;
            public int xSubTotalIVAExentas, xSubTotalIVA05, xSubTotalIVA10;

            public int xTotalPagar;

            public int xTotalEnLetras, yTotalEnLetras;

            public int yTotal;
            public int xTotalIVA05, xTotalIVA10, xTotalIVAGeneral;
        }

        int _Version;
        string _General;
        int _TiempoApertura;
        bool _Keypad_usb;
        string _MainDeviceID;
        ImpresionCoords _ImpresionCoords;

        public Configuration()
        {
            _Version = 1;
            _General = "";
            _TiempoApertura = -1;
            _Keypad_usb = false;
            _MainDeviceID = "";

            InicializarCoordenadas();
        }

        public static void Serialize(string file, Configuration c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static Configuration Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(Configuration));
            StreamReader reader = File.OpenText(file);
            Configuration c = (Configuration)xs.Deserialize(reader);
            reader.Close();
            return c;
        }

        public int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        public string General
        {
            get { return _General; }
            set { _General = value; }
        }
        public int TiempoApertura
        {
            get { return _TiempoApertura; }
            set { _TiempoApertura = value; }
        }
        public bool Keypad_usb
        {
            get { return _Keypad_usb; }
            set { _Keypad_usb = value; }
        }
        public string MainDeviceID
        {
            get { return _MainDeviceID; }
            set { _MainDeviceID = value; }
        }

        // Interfaz para la Estructura con las Coordenadas de Impresión.
        public ImpresionCoords CoordenadasImpresion
        {
            get { return _ImpresionCoords; }
            set { _ImpresionCoords = value; }
        }

        public void InicializarCoordenadas()
        {
            // Creamos e inicializamos la Estructura con las Coordenadas para la Impresión.
            _ImpresionCoords = new ImpresionCoords();
            _ImpresionCoords.yFecha = 15;
            _ImpresionCoords.xFechaDia = 120;
            _ImpresionCoords.xFechaMes = 160;
            _ImpresionCoords.xFechaAño = 230;

            _ImpresionCoords.xContadoCredito = 520;

            _ImpresionCoords.xRUC = 180;
            _ImpresionCoords.yRUC = 40;
            _ImpresionCoords.xNombre = 180;
            _ImpresionCoords.yNombre = 65;

            _ImpresionCoords.yItem = 150;
            _ImpresionCoords.xItemCant = 120;
            _ImpresionCoords.xItemConcepto = 160;
            _ImpresionCoords.xItemIVAExentas = 400;
            _ImpresionCoords.xItemIVA05 = 470;
            _ImpresionCoords.xItemIVA10 = 540;

            _ImpresionCoords.ySubTotal = 450;
            _ImpresionCoords.xSubTotalIVAExentas = _ImpresionCoords.xItemIVAExentas;
            _ImpresionCoords.xSubTotalIVA05 = _ImpresionCoords.xItemIVA05;
            _ImpresionCoords.xSubTotalIVA10 = _ImpresionCoords.xItemIVA10;

            _ImpresionCoords.xTotalPagar = 540;

            _ImpresionCoords.xTotalEnLetras = 160;
            _ImpresionCoords.yTotalEnLetras = 475;

            _ImpresionCoords.yTotal = 500;
            _ImpresionCoords.xTotalIVA05 = 220;
            _ImpresionCoords.xTotalIVA10 = 350;
            _ImpresionCoords.xTotalIVAGeneral = 480;
        }

    }
}
