using DeviceDataCatcher.Configuration;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Protocols.FlowDevice
{
    public class CYUT2000Protocol : FlowData
    {
        public CYUT2000Protocol(int gateway_Number, DeviceSetting deviceSetting)
        {
            Gateway_Number = gateway_Number;
            DeviceSetting = deviceSetting;
        }
        public override void Read_Protocol(IModbusMaster master)
        {
            try
            {
                ushort[] A1 = master.ReadHoldingRegisters((byte)DeviceSetting.Device_ID, 0, 2);
                ushort[] A2 = master.ReadHoldingRegisters((byte)DeviceSetting.Device_ID, 8, 4);
                ushort[] A3 = master.ReadHoldingRegisters((byte)DeviceSetting.Device_ID, 32, 4);
                if (A1.Length == 2 || A2.Length == 2 || A3.Length == 4)
                {
                    Flow = Convert.ToDecimal(Math.Round(Calculate.work16to754(A1[1], A1[0]), 2));
                    FlowTotal = Convert.ToDecimal(Math.Round(Convert.ToDouble(Calculate.work16to10(A2[1], A2[0])), 2) + Math.Round(Calculate.work16to754(A2[3], A2[2]), 2));
                    switch (DeviceSetting.TemperatureRegulate.Count)
                    {
                        case 0:
                            {
                                InputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[1], A3[0]), 2));
                                OutputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[3], A3[2]), 2));
                            }
                            break;
                        case 1:
                            {
                                InputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[1], A3[0]), 2)) + Convert.ToDecimal(DeviceSetting.TemperatureRegulate[0]);
                                OutputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[3], A3[2]), 2));
                            }
                            break;
                        case 2:
                            {
                                InputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[1], A3[0]), 2)) + Convert.ToDecimal(DeviceSetting.TemperatureRegulate[0]);
                                OutputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[3], A3[2]), 2)) + Convert.ToDecimal(DeviceSetting.TemperatureRegulate[1]);
                            }
                            break;
                    }
                    LastTime = DateTime.Now;
                    ConnectionFlag = true;
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                Log.Error(ex, $"{DeviceSetting.Device_Name} 通訊失敗 ID : {DeviceSetting.Device_ID}");
                ConnectionFlag = false;
            }
        }
    }
}
