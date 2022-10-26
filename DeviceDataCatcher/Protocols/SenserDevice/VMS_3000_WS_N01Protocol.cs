using DeviceDataCatcher.Configuration;
using NModbus;
using Serilog;
using System;
using System.Threading;

namespace DeviceDataCatcher.Protocols.SenserDevice
{
    public class VMS_3000_WS_N01Protocol : SenserData
    {
        public VMS_3000_WS_N01Protocol(int gateway_Number, DeviceSetting deviceSetting)
        {
            Rate = 10;
            Gateway_Number = gateway_Number;
            DeviceSetting = deviceSetting;
        }
        public override void Read_Protocol(IModbusMaster master)
        {
            try
            {
                ushort[] data = master.ReadHoldingRegisters((byte)DeviceSetting.Device_ID, 0, 2);
                if (data.Length == 2)
                {
                    decimal temp = data[0] > 32767 ? Convert.ToDecimal(data[0] - 65536) : Convert.ToDecimal(data[0]);
                    decimal humidity = data[1] > 32767 ? Convert.ToDecimal(data[1] - 65536) : Convert.ToDecimal(data[1]);

                    if (DeviceSetting.TemperatureRegulate.Count > 0)
                    {
                        Temp = temp / Rate + Convert.ToDecimal(DeviceSetting.TemperatureRegulate[0]);
                    }
                    else
                    {
                        Temp = temp / Rate;
                    }
                    if (DeviceSetting.HumidityRegulate.Count > 0)
                    {
                        Humidity = humidity / Rate + Convert.ToDecimal(DeviceSetting.HumidityRegulate[0]);
                    }
                    else
                    {
                        Humidity = humidity / Rate;
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
