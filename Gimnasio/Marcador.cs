using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gimnasio
{
    public static class Marcador
    {
        public static Boolean conected;
        public static Boolean registeredEvent;
        public static zkemkeeper.CZKEM axCZKEM1;

      

        public static zkemkeeper.CZKEM NewZkInstance()
        {
            axCZKEM1 = new zkemkeeper.CZKEM();
            return axCZKEM1;
        }

        public static Boolean connectMarcador(String ip, String puerto)
        {
            return axCZKEM1.Connect_Net(ip, Convert.ToInt32(puerto));

        }

        public static Boolean regEvent(int iMachineNumber, int all )
        {
            registeredEvent = axCZKEM1.RegEvent(iMachineNumber, all);
            return registeredEvent;
        }

        public static Boolean desconectar()
        {
            axCZKEM1.Disconnect();
            Marcador.conected = false;
            return Marcador.conected;
            
        }

    }
}
