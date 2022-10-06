using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _3DPRN_Fiware
{
    [Serializable]
    public class STATUS_EXPOSE
    {
        public PrinterClass Printer = new PrinterClass();
        public class PrinterClass
        {
            public string? Status;
            public string? PRN_SerialNUmber;
            public double EstruderCurrentTemp;
            public double EstruderTargetTemp;
            public double BedCurrentTemp;
            public double BedTargetTemp;
        }
    }
    public enum PrinterStatus
    {
        Unknown,
        StandBy,
        Printing,
        PrintingPause,
        Mantainance,
        Error
    }
}
