using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _3DPRN_Fiware
{
   [Serializable]
   public class STATUS_SYSTEM
   {
      public SystemClass Status = new SystemClass();
      public class SystemClass
      {
         public int NTotPrinters;
         public List<string>? PrintersStatus;  //"KO" "MANTAINANCE" "WORKING" "WAITING"
         public int NTotPartsPrinting;
         public int NTotPartsFinished;
         public int NTotPartsFinishedRobotKO;
      }
   }




}
