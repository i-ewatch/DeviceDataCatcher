using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Enums;
using DeviceDataCatcher.Protocols;
using DeviceDataCatcher.Protocols.ElectricDevice;
using DeviceDataCatcher.Protocols.FlowDevice;
using DeviceDataCatcher.Protocols.SenserDevice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Components
{
    public partial class RecordComponent : Field4Component
    {
        private int Minute { get; set; } = -1;
        public RecordComponent(SystemSetting systemSetting, List<AbsProtocol> absProtocols)
        {
            InitializeComponent();
            AbsProtocols = absProtocols;
            SystemSetting = systemSetting;
            if (!Directory.Exists($"{WorkPath}\\record"))
                Directory.CreateDirectory($"{WorkPath}\\record");
        }

        public RecordComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
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
                if (DateTime.Now.Minute != Minute)
                {
                    string titleString = "時間,通訊通道名稱";
                    string dataString = $"{DateTime.Now:yyyy/MM/dd HH:mm}:00";
                    foreach (var gatewayitem in SystemSetting.GatewaySettings)
                    {
                        dataString += $",{gatewayitem.Gateway_Name}";
                        List<AbsProtocol> protocols = AbsProtocols.Where(g => g.Gateway_Number == gatewayitem.Gateway_Number).ToList();
                        if (protocols != null)
                        {
                            foreach (var protocolitem in protocols)
                            {
                                switch (protocolitem)
                                {
                                    case VMS_3000_WS_N01Protocol vMS_3000_WS_N01:
                                        {

                                            titleString += $",{vMS_3000_WS_N01.DeviceSetting.Device_Name}-溫度,{vMS_3000_WS_N01.DeviceSetting.Device_Name}-濕度";
                                            dataString += vMS_3000_WS_N01.ConnectionFlag ? $",{vMS_3000_WS_N01.Temp},{vMS_3000_WS_N01.Humidity}" : ",0,0";
                                        }
                                        break;
                                    case CYUT2000Protocol cYU2000:
                                        {
                                            if (cYU2000.DeviceSetting.Ice_Cool_Flag)
                                            {
                                                titleString += $",{cYU2000.DeviceSetting.Device_Name}-流量,{cYU2000.DeviceSetting.Device_Name}-累積流量,{cYU2000.DeviceSetting.Device_Name}-入水溫度,{cYU2000.DeviceSetting.Device_Name}-出水溫度,{cYU2000.DeviceSetting.Device_Name}-溫差,{cYU2000.DeviceSetting.Device_Name}-RT";
                                                dataString += cYU2000.ConnectionFlag ? $",{cYU2000.Flow},{cYU2000.FlowTotal},{cYU2000.InputTemp},{cYU2000.OutputTemp},{cYU2000.TempRang},{cYU2000.RT}" : ",0,0,0,0,0,0";
                                            }
                                            else
                                            {
                                                titleString += $",{cYU2000.DeviceSetting.Device_Name}-流量,{cYU2000.DeviceSetting.Device_Name}-累積流量,{cYU2000.DeviceSetting.Device_Name}-入水溫度,{cYU2000.DeviceSetting.Device_Name}-出水溫度,{cYU2000.DeviceSetting.Device_Name}-溫差";
                                                dataString += cYU2000.ConnectionFlag ? $",{cYU2000.Flow},{cYU2000.FlowTotal},{cYU2000.InputTemp},{cYU2000.OutputTemp},{cYU2000.TempRang}" : ",0,0,0,0,0";
                                            }
                                        }
                                        break;
                                    case PA310Protocol pA310:
                                        {
                                            titleString += $",{pA310.DeviceSetting.Device_Name}-RV,{pA310.DeviceSetting.Device_Name}-SV,{pA310.DeviceSetting.Device_Name}-TV,{pA310.DeviceSetting.Device_Name}-RSV,{pA310.DeviceSetting.Device_Name}-STV,{pA310.DeviceSetting.Device_Name}-TRV,{pA310.DeviceSetting.Device_Name}-RA,{pA310.DeviceSetting.Device_Name}-SA,{pA310.DeviceSetting.Device_Name}-TA,{pA310.DeviceSetting.Device_Name}-KW,{pA310.DeviceSetting.Device_Name}-KWH,{pA310.DeviceSetting.Device_Name}-PFE";
                                            dataString += pA310.ConnectionFlag ? $",{pA310.RV},{pA310.SV},{pA310.TV},{pA310.RSV},{pA310.STV},{pA310.TRV},{pA310.RA},{pA310.SA},{pA310.TA},{pA310.KW},{pA310.KWH},{pA310.PFE}" : ",0,0,0,0,0,0,0,0,0,0,0,0";
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    string LogPath = $"{WorkPath}\\record\\{DateTime.Now:yyyyMM}.csv";
                    if (!File.Exists(LogPath))
                    {
                        StreamWriter title = new StreamWriter(LogPath, false, Encoding.Default);
                        title.WriteLine(titleString);
                        title.Close();
                    }
                    StreamWriter log = new StreamWriter(LogPath, true, Encoding.Default);
                    log.WriteLine(dataString);
                    log.Close();
                    Minute = DateTime.Now.Minute;
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
