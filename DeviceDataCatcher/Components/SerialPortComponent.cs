using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Enums;
using DeviceDataCatcher.Protocols.ElectricDevice;
using DeviceDataCatcher.Protocols.FlowDevice;
using DeviceDataCatcher.Protocols.SenserDevice;
using NModbus;
using NModbus.Serial;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Components
{
    public partial class SerialPortComponent : Field4Component
    {
        public SerialPortComponent(GatewaySetting gatewaySetting, List<DeviceSetting> deviceSettings)
        {
            InitializeComponent();
            GatewaySetting = gatewaySetting;
            DeviceSettings = deviceSettings;
        }

        public SerialPortComponent(IContainer container)
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
                rs485 = new SerialPort(GatewaySetting.Gateway_Location);
                rs485.BaudRate = 9600;
                rs485.DataBits = 8;
                rs485.Parity = Parity.None;
                rs485.StopBits = StopBits.One;
                ReadThread = new Thread(Analysis);
                ReadThread.Start();
            }
            else
            {
                if (rs485 != null)
                {
                    rs485.Close();
                }
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
                    #region Rs485通訊功能初始化
                    try
                    {
                        if (!rs485.IsOpen)
                        {
                            rs485.Open();
                            master = ModbusFactoryExtensions.CreateRtuMaster(Factory, rs485);
                            master.Transport.ReadTimeout = 2000;
                            master.Transport.WriteTimeout = 2000;
                            master.Transport.Retries = 0;
                        }
                    }
                    catch (ArgumentException)
                    {
                        Log.Error("通訊埠設定有誤");
                    }
                    catch (InvalidOperationException)
                    {
                        Log.Error("通訊埠被占用");
                    }
                    catch (IOException)
                    {
                        Log.Error("通訊埠無效");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "通訊埠發生不可預期的錯誤。");
                    }
                    #endregion
                    foreach (var item in AbsProtocols)
                    {
                        if (myWorkState)
                        {
                            try
                            {
                                item.Read_Protocol(master);
                                ReadTime = DateTime.Now;
                                Thread.Sleep(20);
                            }
                            catch (ThreadAbortException) { }
                            catch (Exception ex)
                            {
                                ReadTime = DateTime.Now;
                                Log.Error(ex, $"通訊失敗 COM:{GatewaySetting.Gateway_Location} Rate:{GatewaySetting.Gateway_Rate} ");
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
