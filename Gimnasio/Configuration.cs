using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace Gimnasio
{
    [Serializable]
    public class Configuration
    {
        int _Version;
        string _General;
        int _TiempoApertura;

        public Configuration()
        {
            _Version = 1;
            _General = "";
            _TiempoApertura = -1;
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

    }
}
