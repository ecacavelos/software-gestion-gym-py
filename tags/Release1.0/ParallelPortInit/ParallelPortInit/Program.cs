using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ParallelPortInit
{
    class Program
    {
        static void Main(string[] args)
        {

            Program este = new Program();
            if (este.ResetDataBus() == 1)
            {
                Console.WriteLine("bien reseteado");
                //Console.In.Read();
            }
            else
            {
                Console.WriteLine("No se pudo cerrar el puerto paralelo, cierre manualmente con el programa");
                Console.In.Read();
                   
            }
            
        }
           
        #region "Imported Functions"

        [DllImport("ParallelPort.dll")]
        private static extern void Out32(short PortAddress, short data);

        [DllImport("ParallelPort.dll")]
        private static extern short Inp32(short PortAddress);

        [DllImport("ParallelPort.dll")]
        private static extern Int16 ReadMemShort(short Address);

        #endregion

        bool _Activado = false;
        private int _PortAddress = 0;
        string ExceptionOccured;

       

        /// <summary>
        /// Constructor.
        /// It detects the parallel port presence on the computer.
        /// If detected than _PortAddress is set to the address of LPT1.
        /// If not detected then the whole user control gets disabled.
        /// </summary>
        public Program() 
        {
            ParallelPort();
        }
        public void ParallelPort() {
            
            int[] PortAddresses = Detect_LPT_Ports();
            if (PortAddresses.Length != 0)
            {
                _PortAddress = PortAddresses[0];
            }
            else
            {
                _PortAddress = 0;
                Console.WriteLine("No se detecto el puerto paralelo en la computadora, no se podra utilizar el porton electrico.");
                Console.In.Read();
            }

        }

        /// <summary>
        /// Detecs the presence of LPT ports.
        /// </summary>
        /// <returns>Returns an integer array of all detected LPT addresses.</returns>
        private int[] Detect_LPT_Ports() {
            int Number_Of_LPT_Ports = 0;
            short[] portAddresses = new short[3];
            for (int i = 0; i < 3; i++)
            {
                portAddresses[i] = ReadMemShort(Convert.ToInt16(1032 + i * 2));
                if (portAddresses[i] != 0)
                {
                    Number_Of_LPT_Ports++;
                }
            }
            int[] PortAddresses = new int[Number_Of_LPT_Ports];
            for (int i = 0, j = 0; i < 3; i++)
            {
                if (portAddresses[i] != 0)
                {
                    PortAddresses[j] = Convert.ToInt32(portAddresses[i]);
                    j++;
                }
            }
            return PortAddresses;
        }


        #region "Data Bus"

        /// <summary>
        /// Gets the address of the Parallel Port's Data Bus.
        /// </summary>
        private short DataBusAddress
        {
            get
            {
                return Convert.ToInt16(_PortAddress);
            }
        }

        /// <summary>
        /// Read current status of the Data Bus. 
        /// </summary>
        /// <returns>
        /// Current status of all the Data Pins as an integer 
        /// whose bits give the current status of each Data Pin (D0 to D7) 
        /// with D0 being the LSB and D7 being the MSB.
        /// </returns>
        public int ReadFromDataBus()
        {
            return Convert.ToInt32(Inp32(DataBusAddress));
        }

        /// <summary>
        /// Writes data to the Data Bus.   
        /// </summary>
        /// <param name="Data">Data to be written</param>
        public void WriteToDataBus(int Data)
        {
            try
            {
                Out32(DataBusAddress, Convert.ToInt16(Data));
            }
            catch (Exception ex)
            {
                ExceptionOccured = "WriteToDataBus(int Data) called with Data = " + Data.ToString() + ". ERROR occured is ---> " + ex.Message;
            }
        }

        /// <summary>
        /// Make data at all the Data Pins HIGH.
        /// </summary>
        public void SetDataBus()
        {
            try
            {
                D0 = true;
            }
            catch (Exception ex)
            {
                ExceptionOccured = "SetDataBus() called. ERROR occured is ---> " + ex.Message;
            }
        }

        /// <summary>
        ///  Make data at all the Data Pins LOW.
        /// </summary>
        public int ResetDataBus()
        {
            try
            {
                D0 = false;
                return 1;
            }
            catch (Exception ex)
            {
                ExceptionOccured = "ResetDataBus() called. ERROR occured is ---> " + ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// Gets or Sets the data at Pin D0 (Pin no. 2).
        /// </summary>
        public bool D0
        {
            get
            {
                return Convert.ToBoolean(Inp32(DataBusAddress) & 1);
            }
            set
            {
                try
                {
                    if (value)
                    {
                        Out32(DataBusAddress, Convert.ToInt16(Inp32(DataBusAddress) | 1));
                        _Activado = true;
                       
                        //label1.Content = "D0 ON";
                    }
                    else
                    {
                        Out32(DataBusAddress, Convert.ToInt16(Inp32(DataBusAddress) & (~1)));
                        _Activado = false;
                        ;
                        //label1.Content = "D0 OFF";
                    }
                }
                catch (Exception ex)
                {
                    ExceptionOccured = "D0 assigned = " + value.ToString() + ". ERROR occured is ---> " + ex.Message;
                }
            }
        }

        #endregion

        
    }
}
