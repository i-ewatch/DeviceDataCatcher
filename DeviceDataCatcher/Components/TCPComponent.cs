using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Enums;
using DeviceDataCatcher.Protocols.ElectricDevice;
using DeviceDataCatcher.Protocols.FlowDevice;
using DeviceDataCatcher.Protocols.SenserDevice;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Components
{
    public partial class TCPComponent : Field4Component
    {
        public TCPComponent(GatewaySetting gatewaySetting, List<DeviceSetting> deviceSettings)
        {
            InitializeComponent();
            GatewaySetting = gatewaySetting;
            DeviceSettings = deviceSettings;
        }

        public TCPComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
                foreach (var item in DeviceSettings)
                {
                    Device_Type device = (Device_Type)item.Device_Type;
                    switch (device)
                    {
                        case Device_Type.VMS_3000_WS_N01:
                            {
                                VMS_3000_WS_N01Protocol protocol = new VMS_3000_WS_N01Protocol(GatewaySetting.Gateway_Number, item);
                                AbsProtocols.Add(protocol);
                            }
                            break;
                        case Device_Type.CYUT2000:
                            {
                                CYUT2000Protocol protocol = new CYUT2000Protocol(GatewaySetting.Gateway_Number, item);
                                AbsProtocols.Add(protocol);
                            }
                            break;
                        case Device_Type.PA310:
                            {
                                PA310Protocol protocol = new PA310Protocol(GatewaySetting.Gateway_Number, item);
                                AbsProtocols.Add(protocol);
                            }
                            break;
                    }
                }
                Factory = new ModbusFactory();
                ReadThread = new Thread(Analysis);
                ReadThread.Start();
            }
            else
            {
                if (ReadThread != null)
                {
                    ReadThread.Abort();
                }
            }
        }
        private void Analysis()
        {
            while (myWorkState)
            {
                TimeSpan ReadSpan = DateTime.Now.Subtract(ReadTime);
                if (ReadSpan.TotalMilliseconds >= 5000)
                {
                    #region 通訊
                    foreach (var item in AbsProtocols)
                    {
                        if (myWorkState)
                        {
                            try
                            {
                                using (TcpClient client = new TcpClient(GatewaySetting.Gateway_Location, GatewaySetting.Gateway_Rate))
                                {
                                    master = Factory.CreateMaster(client);//建立TCP通訊
                                    master.Transport.Retries = 0;
                                    master.Transport.ReadTimeout = 2000;
                                    master.Transport.WriteTimeout = 2000;
                                    item.Read_Protocol(master);
                                    Thread.Sleep(10);
                                };
                                ReadTime = DateTime.Now;
                            }
                            catch (ThreadAbortException) { }
                            catch (Exception ex)
                            {
                                ReadTime = DateTime.Now;
                                Log.Error(ex, $"通訊失敗 IP:{GatewaySetting.Gateway_Location} Port:{GatewaySetting.Gateway_Rate} ");
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
