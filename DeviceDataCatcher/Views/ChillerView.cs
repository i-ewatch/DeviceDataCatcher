using DevExpress.XtraEditors;
using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Enums;
using DeviceDataCatcher.Protocols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceDataCatcher.Views
{
    public partial class ChillerView : Field4Control
    {
        private decimal kw { get; set; }
        private decimal RT { get; set; }
        public ChillerView(List<AbsProtocol> absProtocols, GatewaySetting gatewaySetting)
        {
            InitializeComponent();

            if (File.Exists($"{WorkPath}\\Images\\{gatewaySetting.ImageName}"))
            {
                using (FileStream file = File.OpenRead($"{WorkPath}\\Images\\{gatewaySetting.ImageName}"))
                {
                    pictureEdit1.Image = Image.FromStream(file);
                }
            }
            GatewaySetting = gatewaySetting;
            AbsProtocols = absProtocols;
        }
        public override void TextChange()
        {
            if (GatewaySetting != null && AbsProtocols != null)
            {
                List<AbsProtocol> protocols = AbsProtocols.Where(g => g.Gateway_Number == GatewaySetting.Gateway_Number).ToList();
                foreach (var item in protocols)
                {
                    Device_Type device = (Device_Type)item.DeviceSetting.Device_Type;
                    switch (device)
                    {
                        case Device_Type.VMS_3000_WS_N01:
                            {

                            }
                            break;
                        case Device_Type.CYUT2000:
                            {
                                FlowData data = item as FlowData;
                                if (data.DeviceSetting.Ice_Cool_Flag)//冰水
                                {
                                    if (data.ConnectionFlag)
                                    {
                                        stateIndicatorComponent2.StateIndex = 3;
                                    }
                                    else
                                    {
                                        stateIndicatorComponent2.StateIndex = 1;
                                    }
                                    lbl_CW_Flow.Text = data.Flow.ToString("0.##") + " m\xb3/h";
                                    lbl_CW_FlowTotal.Text = data.FlowTotal.ToString("0.##") + " m\xb3";
                                    lbl_CW_InputTemp.Text = data.InputTemp.ToString("0.##") + " \xb0" + "C";
                                    lbl_CW_OutputTemp.Text = data.OutputTemp.ToString("0.##") + " \xb0"+"C";
                                    lbl_CW_Rang.Text = data.TempRang.ToString("0.##") + " \xb0" + "C";
                                    lbl_CW_RT.Text = data.RT.ToString("0.##");
                                    RT = data.RT;
                                }
                                else//冷卻水
                                {
                                    if (data.ConnectionFlag)
                                    {
                                        stateIndicatorComponent3.StateIndex = 3;
                                    }
                                    else
                                    {
                                        stateIndicatorComponent3.StateIndex = 1;
                                    }
                                    lbl_CH_Flow.Text = data.Flow.ToString("0.##") + " m\xb3/h";
                                    lbl_CH_FlowTotal.Text = data.FlowTotal.ToString("0.##") + " m\xb3";
                                    lbl_CH_InputTemp.Text = data.OutputTemp.ToString("0.##") + " \xb0" + "C";
                                    lbl_CH_OutputTemp.Text = data.InputTemp.ToString("0.##") + " \xb0" + "C";
                                    lbl_CH_Rang.Text = data.TempRang.ToString("0.##");
                                }
                            }
                            break;
                        case Device_Type.PA310:
                            {
                                ElectricData data = item as ElectricData;
                                if (data.ConnectionFlag)
                                {
                                    stateIndicatorComponent1.StateIndex = 3;
                                }
                                else
                                {
                                    stateIndicatorComponent1.StateIndex = 1;
                                }
                                lbl_RV.Text = data.RV.ToString("0.##") + " V";
                                lbl_SV.Text = data.SV.ToString("0.##") + " V";
                                lbl_TV.Text = data.TV.ToString("0.##") + " V";
                                lbl_RSV.Text = data.RSV.ToString("0.##") + " V";
                                lbl_STV.Text = data.STV.ToString("0.##") + " V";
                                lbl_TRV.Text = data.TRV.ToString("0.##") + " V";
                                lbl_RA.Text = data.RA.ToString("0.##") + " A";
                                lbl_SA.Text = data.SA.ToString("0.##") + " A";
                                lbl_TA.Text = data.TA.ToString("0.##") + " A";
                                lbl_PF.Text = data.PFE.ToString("0.##");
                                lbl_KW.Text = data.KW.ToString("0.##") + " kW";
                                lbl_KWH.Text = data.KWH.ToString("0.##") + " kWh";
                                kw = data.KW;
                            }
                            break;
                    }
                }
                if (RT != 0 & kw != 0)
                {
                    lbl_CW_COP.Text = Math.Round(kw / RT, 2).ToString("0.##");
                }
                else
                {
                    lbl_CW_COP.Text = "0";
                }
            }
        }
    }
}
