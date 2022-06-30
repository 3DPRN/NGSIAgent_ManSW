using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using TIPS;

namespace _3DPRN_Fiware
{
    public partial class frmMain : Form
    {
        public static System.Threading.Thread mainThread;
        
        private static clsMain mcmain = new clsMain();
        static System.Windows.Forms.Timer updatetimer = new System.Windows.Forms.Timer();

      public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            
            /*
            mainThread = new System.Threading.Thread(() => mcmain.Main(this))
            {
                Name = "MAIN THREAD",
                Priority = System.Threading.ThreadPriority.AboveNormal
            };
            mainThread.Start();            
            */
            mcmain.Main(this);
            txtprnstatus.Text = "START";
  
         
            updatetimer.Interval = 5000;
            updatetimer.Tick += Updatetimer_Tick;
            updatetimer.Start();
        }

      private void Updatetimer_Tick(object sender, EventArgs e)
      {
         if (mcmain._system_Attributes != null)
         {
            if (mcmain._system_Attributes.StatusCode.Code == "200")
            {
               string _prntext = mcmain._system_ID + VBComp.vbCrLf;
               for (int i = mcmain._system_Attributes.ContextElement.Attributes.Count - 1; i >= 0; i--)
               {
                  _prntext += mcmain._system_Attributes.ContextElement.Attributes.ElementAt(i).Name + " = " + mcmain._system_Attributes.ContextElement.Attributes.ElementAt(i).Value + VBComp.vbCrLf;
               }
               txtprnstatus.Text = _prntext;
            }
            else { txtprnstatus.Text = "ERROR READING CONTEXT ELEMENT ATTRIBUTES"; }
         }
      }

      private void btndelete_Click(object sender, EventArgs e)
      {
         mcmain.Delete ();
      }
   }
}
