using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FIWARE;
using FIWARE.Orion.Client;
using FIWARE.Orion.Client.Models;
using _3DPRN_Fiware;
using System.Windows.Forms;
using SimpleTCP;

namespace _3DPRN_Fiware
{
    internal class clsMain
    {
        
        public SimpleTcpClient _onBoardClient;
        Fiware_Orion_Client _orionClient;
        string _prn_ip = "192.168.0.116";
        string _prn_port = "13000";
        public string _system_ID = "";
        public  ContextResponse _system_Attributes;
        string _orionClientConfig_BaseUrl = "http://192.168.0.33:1026/";
        string _orionClientConfig_Token = "2b4ba603987c4421957693e9bf14d1dbc846f473259a65e1d5bfaa21df6616ec";
        private int _retryperiod=5000;
        private readonly object _syncTimer = new object();
        string fileName_PAR = Application.StartupPath + @"\3DPRN_Fiware_Houston.ini";
        public static Thread ThreadManagePrinters;

        public void MainTimer()
        {
               do
               {
               //if (STATUS.Program_Ending) break;
               try
               {
                    if (_onBoardClient.TcpClient.Connected)
                    {
                        _retryperiod = 5000;
                        SimpleTCP.Message response = _onBoardClient.WriteLineAndGetReply("~:STATUS_SYSTEM_GET\r\n",new TimeSpan(20000));  //richiesta stato
                        System.Diagnostics.Debug.Print("STATUS_SYSTEM_GET");

                                                if (_system_ID != "")
                        {
                            _orionClient.GetSystemStatus(_system_ID);
                            _system_Attributes = _orionClient._attributes;
                        }

                        string data = response.MessageString;
                        System.Diagnostics.Debug.Print("Evento Arrivato da Server:{0}", data);

                        //parsing data
                        string[] subs = data.Split(' ', (char)2);
                        switch (subs[0])
                        {
                            case "~:STATUS_SYSTEM":
                                //deserializzazione
                                _3DPRN_Fiware.STATUS_SYSTEM systemstatus = new _3DPRN_Fiware.STATUS_SYSTEM();

                                var serializer = new XmlSerializer(typeof(_3DPRN_Fiware.STATUS_SYSTEM), new XmlRootAttribute("STATUS_SYSTEM"));
                                data = data.Substring(data.IndexOf(" ")).Trim();

                                using (var reader = new System.IO.StringReader(data))
                                {
                                    try
                                    {
                                        systemstatus = (_3DPRN_Fiware.STATUS_SYSTEM)serializer.Deserialize(reader);
                                        _system_ID = "WALL1";
                                        //_orionClient.UpdateSystemStatus(systemstatus);
                                        var threadOrion = new Thread(() =>
                                                    _orionClient.UpdateSystemStatus(systemstatus)
                                               );
                                        threadOrion.IsBackground = true;
                                        threadOrion.Start();


                                        System.Diagnostics.Debug.Print($"DataArrived End Call");

                                    }
                                    catch (InvalidCastException exc)
                                    {
                                        // recover from exception
                                        System.Diagnostics.Debug.Print($"DataArrived InvalidCastException");
                                    }

                                }
                                break;

                            default:
                                System.Diagnostics.Debug.Print($"Response {subs[0]}.");
                                break;
                        }
                    }
                    else
                    {
                        _retryperiod = 60000;
                        _onBoardClient.Connect(_prn_ip,int.Parse(_prn_port));
                    }
                    
                  }
                  catch (Exception ex)
                  {
                     System.Diagnostics.Debug.Print(ex.ToString());                    
                  }

                  Thread.Sleep(_retryperiod);
               }
               while (true);
        }

        //internal static async Task Main(frmMain frmMain)
        public  void Main(frmMain frmMain)
        {
            //var mclsMain = new clsMain();
            System.Diagnostics.Debug.Print($"Started.");

            //lettura da INI dei parametri

            _prn_ip = Properties.Settings.Default.prn_ip;
            _prn_port = Properties.Settings.Default.prn_port;
            _orionClientConfig_BaseUrl = Properties.Settings.Default.OCB_ip;
            _orionClientConfig_Token = Properties.Settings.Default.OCB_token;
                        
            //connessione stampante            
            _onBoardClient = new SimpleTcpClient().Connect(_prn_ip, int.Parse(_prn_port));
            
            
            if (_onBoardClient.TcpClient.Connected)
            {
                _onBoardClient.Write("~:ATI SW=99,TYPECLI=CONTROLBASE,NAMECLI=HOUSTON\r\n");

                //Istanzia Orion Context Broker
                _orionClient = new Fiware_Orion_Client(_orionClientConfig_BaseUrl, _orionClientConfig_Token);

                //Timer Lettura/scrittura

                //Printers
                ThreadManagePrinters = new Thread(MainTimer);
                ThreadManagePrinters.Priority = ThreadPriority.Normal;
                ThreadManagePrinters.IsBackground = true;
                ThreadManagePrinters.Start();
            }
            else
            {
            //errore connessione stampante -> chiudere app?
            DialogResult result = MessageBox.Show("Can't connect with printer at " + _prn_ip, "CONNECTION ERROR", MessageBoxButtons.OK);
            System.Environment.Exit(0);
         }
    }

      public void Delete()
      { _orionClient.ClearSystemStatus(); }

        //public  void DataArrived(object sender, EventArgs e)
        //{
        //    string data = ((TCPIP_Client)sender).DataRead;
        //    System.Diagnostics.Debug.Print("Evento Arrivato da Server:{0}", data);

        //    //parsing data
        //    string[] subs = data.Split(' ', (char)2);
        //    switch (subs[0])
        //    {
        //        case "~:STATUS_SYSTEM":
        //            //deserializzazione
        //            _3DPRN_Fiware.STATUS_SYSTEM  systemstatus = new _3DPRN_Fiware.STATUS_SYSTEM ();
                                  
        //            var serializer = new XmlSerializer(typeof(_3DPRN_Fiware.STATUS_SYSTEM), new XmlRootAttribute("STATUS_SYSTEM"));
        //            data = data.Substring(data.IndexOf(" ")).Trim();

        //            using (var reader = new System.IO.StringReader(data))
        //            {
        //                try
        //                {
        //                   systemstatus = (_3DPRN_Fiware.STATUS_SYSTEM)serializer.Deserialize(reader);
        //                   _system_ID = "WALL1";
        //                   //_orionClient.UpdateSystemStatus(systemstatus);
        //                   var threadOrion = new Thread(() =>
        //                               _orionClient.UpdateSystemStatus(systemstatus)
        //                          );
        //                   threadOrion.IsBackground = true;
        //                   threadOrion.Start();


        //                   System.Diagnostics.Debug.Print($"DataArrived End Call");

        //                }
        //                catch (InvalidCastException exc)
        //                {
        //                    // recover from exception
        //                    System.Diagnostics.Debug.Print($"DataArrived InvalidCastException");
        //                }

        //            }
        //            break;

        //        default:
        //            System.Diagnostics.Debug.Print($"Response {subs[0]}.");
        //            break;
        //    }

            


        //}
    }
}
