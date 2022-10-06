using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIWARE;
using FIWARE.Orion.Client;
using FIWARE.Orion.Client.Models;
using TIPS;

namespace _3DPRN_Fiware
{

    class Fiware_Orion_Client
    {
        #region VARIABILI LOCALI
        OrionClient _orionClient;
        OrionClient.OrionConfig _orionClientConfig;
        public ContextResponse _attributes;
        #endregion

        #region COSTRUTTORI
        public Fiware_Orion_Client(string _baseUrl, string _token)                
        {
            _orionClientConfig = new OrionClient.OrionConfig();
            _orionClientConfig.BaseUrl = _baseUrl;
            _orionClientConfig.Token = _token;
            _orionClient = new OrionClient(_orionClientConfig);
        }
    #endregion

    #region METODI
    public async void UpdateStatus(STATUS_EXPOSE printerstatus)
        {
         //Scrittura su Orion
         try
         {
            ContextUpdate create = new ContextUpdate()
            {
               UpdateAction = FIWARE.Orion.Client.Types.UpdateActionTypes.APPEND,
               ContextElements = new List<ContextElement>(){
                                new ContextElement(){
                                  Type = "PRN3D",
                                  IsPattern = false,
                                  Id = printerstatus.Printer.PRN_SerialNUmber ,
                                  Attributes = new List<ContextAttribute>(){                                     
                                        new ContextAttribute(){
                                          Name = "Status",
                                          Type = "string",
                                          Value = printerstatus.Printer.Status
                                        },
                                        new ContextAttribute(){
                                          Name = "EstruderCurrentTemp",
                                          Type = "double",
                                          Value = printerstatus.Printer.EstruderCurrentTemp
                                        },
                                        new ContextAttribute(){
                                          Name = "EstruderTargetTemp",
                                          Type = "double",
                                          Value = printerstatus.Printer.EstruderTargetTemp
                                        },
                                        new ContextAttribute(){
                                          Name = "BedCurrentTemp",
                                          Type = "double",
                                          Value = printerstatus.Printer.BedCurrentTemp
                                        },
                                        new ContextAttribute(){
                                          Name = "BedTargetTemp",
                                          Type = "double",
                                          Value = printerstatus.Printer.BedTargetTemp
                                        },
                                   }
                                },
                }
            };

            System.Diagnostics.Debug.Print("UpdateContextAsync start");
            ContextResponses createResponses = await _orionClient.UpdateContextAsync(create);
            if (createResponses.Responses.ElementAt (0).StatusCode.Code != "200")
            {
               System.Diagnostics.Debug.Print("UpdateContextAsync Error {0}", createResponses.ErrorCode.Code);
            }
            System.Diagnostics.Debug.Print("UpdateContextAsync Fine");
         }
         catch (Exception ex)
         { System.Diagnostics.Debug.Print("UpdateContextAsync Error {0}", ex); }
            
            
        }

        public async void GetStatus(string _serialNUmber)
        {
            //Lettura da Orion
            _attributes = await _orionClient.GetAttributesForEntityAsync(_serialNUmber);
            
        }

      public async void ClearSystemStatus()
      {
         
         ContextResponse createResponse = await _orionClient.DeleteEntityAsync ("WALL1");
         
      }
      public async void UpdateSystemStatus(STATUS_SYSTEM systemstatus)
      {
         //Scrittura su Orion
         try
         {
            ContextUpdate create = new ContextUpdate()
            {
               UpdateAction = FIWARE.Orion.Client.Types.UpdateActionTypes.APPEND,
               ContextElements = new List<ContextElement>(){
                                new ContextElement(){
                                  Type = "SYSTEM_PRN3D",
                                  IsPattern = false,
                                  Id = "WALL1" ,
                                  Attributes = new List<ContextAttribute>(){
                                        new ContextAttribute(){
                                          Name = "3DPrintersTotal",
                                          Type = "int",
                                          Value = systemstatus.Status.NTotPrinters
                                        },
                                        new ContextAttribute(){
                                          Name = "3DPrintedPiecesStarted",
                                          Type = "int",
                                          Value = systemstatus.Status.NTotPartsPrinting
                                        },
                                        new ContextAttribute(){
                                          Name = "3DPrintedPiecesCompleted",
                                          Type = "int",
                                          Value = systemstatus.Status.NTotPartsFinished
                                        },
                                        new ContextAttribute(){
                                          Name = "3DPrintedPiecesCheckedOK",
                                          Type = "int",
                                          Value = (systemstatus.Status.NTotPartsFinishedRobotKO)
                                        },
                                   }
                                },
                }
            };

            int iprinter = 0;
            int iprinterproductive = 0;
            foreach (string _status in systemstatus.Status.PrintersStatus)
            {
               iprinter++;
               create.ContextElements.ElementAt(0).Attributes.Add(new ContextAttribute()
               {
                  Name = $"3DPrinterStatus{iprinter}" ,
                  Type = "string",
                  Value = _status
               });
               if (_status == "WORKING" | _status ==  "WAITING") { iprinterproductive++; }
            }
            create.ContextElements.ElementAt(0).Attributes.Add(new ContextAttribute()
            {
               Name = $"3DPrinterProductiveCount",
               Type = "string",
               Value = iprinterproductive
            });
            System.Diagnostics.Debug.Print("UpdateContextAsync start");
            ContextResponses createResponses = await _orionClient.UpdateContextAsync(create);
            if (createResponses.Responses.ElementAt(0).StatusCode.Code != "200")
            {
               System.Diagnostics.Debug.Print("UpdateContextAsync Error {0}", createResponses.ErrorCode.Code);
            }
            System.Diagnostics.Debug.Print("UpdateContextAsync Fine");
         }
         catch (Exception ex)
         { System.Diagnostics.Debug.Print("UpdateContextAsync Error {0}", ex); }


      }

      public async void GetSystemStatus(string _wallID)
      {
         //Lettura da Orion
         _attributes = await _orionClient.GetAttributesForEntityAsync(_wallID);

      }
      #endregion
   }
}
