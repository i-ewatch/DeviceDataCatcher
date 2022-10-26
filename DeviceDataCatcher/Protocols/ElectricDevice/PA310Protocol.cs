using DeviceDataCatcher.Configuration;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Protocols.ElectricDevice
{
    public class PA310Protocol: ElectricData
    {
        public PA310Protocol(int gateway_Number, DeviceSetting deviceSetting)
        {
            Gateway_Number = gateway_Number;
            DeviceSetting = deviceSetting;
        }
        public override void Read_Protocol(IModbusMaster master)
        {
            try
            {
                ushort[] data = master.ReadInputRegisters((byte)DeviceSetting.Device_ID, 1024, 72);
                ushort[] data1 = master.ReadInputRegisters((byte)DeviceSetting.Device_ID, 1182, 10);
                if (data.Length == 72 && data1.Length == 10)
                {
                    int k = 0;
                    RV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    VNAVG = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RSV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    STV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TRV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    VLAVG = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    AAVG = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;//保留
                    HZ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KWA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KWB = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KWC = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KW = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVARA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVARB = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVARC = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVAR = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVAA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVAB = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVAC = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    PFEA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    PFEB = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    PFEC = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    PFE = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RV_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SV_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TV_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RA_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SA_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TA_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k]));
                    k = 0;
                    KWH = Convert.ToDecimal(Calculate.work16to754(data1[k + 1], data1[k])); k += 4;
                    KVARH = Convert.ToDecimal(Calculate.work16to754(data1[k + 1], data1[k])); k += 4;
                    KVAH = Convert.ToDecimal(Calculate.work16to754(data1[k + 1], data1[k]));
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
