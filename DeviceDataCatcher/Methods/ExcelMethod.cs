using DeviceDataCatcher.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceDataCatcher.Methods
{
    public class ExcelMethod
    {
        /// <summary>
        /// 開啟檔案
        /// </summary>
        private OpenFileDialog openFileDialog { get; set; }
        /// <summary>
        /// 載入檔案
        /// </summary>
        private XSSFWorkbook xworkbook { get; set; }
        /// <summary>
        /// 分頁數量
        /// </summary>
        private int SheetIndex { get; set; } = 0;
        /// <summary>
        /// 檔案名稱
        /// </summary>
        private string FileName { get; set; }
        /// <summary>
        /// 讀取系統資訊
        /// </summary>
        public SystemSetting SystemSetting { get; set; }
        private bool SystemFlag { get; set; } = false;
        /// <summary>
        /// 錯誤資訊
        /// </summary>
        public string ErrorStr
        {
            get
            {
                string error = "";
                if (!SystemFlag)
                {
                    error += "系統新增失敗 \r\n";
                }
                return error;
            }
        }
        public bool Excel_Load()
        {
            SystemFlag = false;
            SystemSetting = new SystemSetting();
            try
            {
                openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "*.Xlsx| *.xlsx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName;
                    using (FileStream file = new FileStream($"{openFileDialog.FileName}", FileMode.Open, FileAccess.Read))
                    {
                        xworkbook = new XSSFWorkbook(file);//Excel檔案載入
                    }
                    SheetIndex = xworkbook.NumberOfSheets;//取得分頁數量
                    for (int Sheetnum = 0; Sheetnum < SheetIndex; Sheetnum++)
                    {
                        string SheetName = xworkbook.GetSheetName(Sheetnum).Trim();
                        var data = xworkbook.GetSheetAt(Sheetnum);//載入分頁資訊
                        switch (SheetName)
                        {
                            case "System":
                                {
                                    for (int Rownum = 1; Rownum < data.LastRowNum + 1; Rownum++)
                                    {
                                        IRow row = data.GetRow(Rownum);
                                        if (row != null)
                                        {
                                            ICell sendflag = row.GetCell(0);
                                            ICell workflag = row.GetCell(1);
                                            ICell recordflag = row.GetCell(2);
                                            SystemSetting.SendFlag = Convert.ToBoolean(Convert.ToInt32(sendflag.ToString()));
                                            SystemSetting.WorkFlag = Convert.ToBoolean(Convert.ToInt32(workflag.ToString()));
                                            SystemSetting.RecordFlag = Convert.ToBoolean(Convert.ToInt32(recordflag.ToString()));
                                        }
                                    }
                                }
                                break;
                            case "Gateway":
                                {
                                    for (int Rownum = 1; Rownum < data.LastRowNum + 1; Rownum++)
                                    {
                                        IRow row = data.GetRow(Rownum);
                                        if (row != null)
                                        {
                                            GatewaySetting gateway = new GatewaySetting();
                                            ICell gatewayindex = row.GetCell(0);
                                            ICell gatewaylocation = row.GetCell(1);
                                            ICell gatewayrate = row.GetCell(2);
                                            ICell gatewaytype = row.GetCell(3);
                                            ICell gatewayname = row.GetCell(4);
                                            ICell imagename = row.GetCell(5);
                                            if (SystemSetting.GatewaySettings.Count > 0)
                                            {
                                                var setting = SystemSetting.GatewaySettings.Where(g => g.Gateway_Number == Convert.ToInt32(gatewayindex.ToString())).ToList();
                                                if (setting.Count == 0)
                                                {
                                                    gateway.Gateway_Number = Convert.ToInt32(gatewayindex.ToString());
                                                    gateway.Gateway_Location = gatewaylocation.ToString();
                                                    gateway.Gateway_Rate = Convert.ToInt32(gatewayrate.ToString());
                                                    gateway.Gateway_Type = Convert.ToInt32(gatewaytype.ToString());
                                                    gateway.Gateway_Name = gatewayname.ToString();
                                                    gateway.ImageName = imagename.ToString();
                                                    SystemSetting.GatewaySettings.Add(gateway);
                                                }
                                            }
                                            else
                                            {
                                                gateway.Gateway_Number = Convert.ToInt32(gatewayindex.ToString());
                                                gateway.Gateway_Location = gatewaylocation.ToString();
                                                gateway.Gateway_Rate = Convert.ToInt32(gatewayrate.ToString());
                                                gateway.Gateway_Type = Convert.ToInt32(gatewaytype.ToString());
                                                gateway.Gateway_Name = gatewayname.ToString();
                                                gateway.ImageName = imagename.ToString();
                                                SystemSetting.GatewaySettings.Add(gateway);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "Device":
                                {
                                    for (int Rownum = 1; Rownum < data.LastRowNum + 1; Rownum++)
                                    {
                                        IRow row = data.GetRow(Rownum);
                                        if (row != null)
                                        {
                                            DeviceSetting deviceSetting = new DeviceSetting();
                                            ICell gatewayindex = row.GetCell(0);
                                            ICell deviceindex = row.GetCell(1);
                                            ICell devicetype = row.GetCell(2);
                                            ICell deviceID = row.GetCell(3);
                                            ICell devicename = row.GetCell(4);
                                            ICell icecoolflag = row.GetCell(5);
                                            ICell temperatureregulate = row.GetCell(6);
                                            ICell humidityregulate = row.GetCell(7);
                                            GatewaySetting gatewaysetting = SystemSetting.GatewaySettings.SingleOrDefault(g => g.Gateway_Number == Convert.ToInt32(gatewayindex.ToString()));
                                            if (gatewaysetting != null)
                                            {
                                                if (gatewaysetting.DeviceSettings.Count > 0)
                                                {
                                                    var setting = gatewaysetting.DeviceSettings.SingleOrDefault(g => g.Device_Number == Convert.ToInt32(deviceindex.ToString()));
                                                    if (setting == null)
                                                    {
                                                        deviceSetting.Device_Number = Convert.ToInt32(deviceindex.ToString());
                                                        deviceSetting.Device_Type = Convert.ToInt32(devicetype.ToString());
                                                        deviceSetting.Device_ID = Convert.ToInt32(deviceID.ToString());
                                                        deviceSetting.Device_Name = devicename.ToString();
                                                        if (icecoolflag != null)
                                                        {
                                                            deviceSetting.Ice_Cool_Flag = Convert.ToBoolean(Convert.ToInt32(icecoolflag.ToString()));
                                                        }
                                                        if (temperatureregulate != null)
                                                        {
                                                            List<decimal> temperature = Array.ConvertAll(temperatureregulate.ToString().Trim().Split(','), decimal.Parse).ToList();
                                                            deviceSetting.TemperatureRegulate = temperature;
                                                        }
                                                        if (humidityregulate != null)
                                                        {
                                                            List<decimal> humidity = Array.ConvertAll(humidityregulate.ToString().Trim().Split(','), decimal.Parse).ToList();
                                                            deviceSetting.HumidityRegulate = humidity;
                                                        }
                                                        gatewaysetting.DeviceSettings.Add(deviceSetting);
                                                    }
                                                }
                                                else
                                                {
                                                    deviceSetting.Device_Number = Convert.ToInt32(deviceindex.ToString());
                                                    deviceSetting.Device_Type = Convert.ToInt32(devicetype.ToString());
                                                    deviceSetting.Device_ID = Convert.ToInt32(deviceID.ToString());
                                                    deviceSetting.Device_Name = devicename.ToString();
                                                    if (icecoolflag != null)
                                                    {
                                                        deviceSetting.Ice_Cool_Flag = Convert.ToBoolean(Convert.ToInt32(icecoolflag.ToString()));
                                                    }
                                                    if (temperatureregulate != null)
                                                    {
                                                        List<decimal> temperature = Array.ConvertAll(temperatureregulate.ToString().Trim().Split(','), decimal.Parse).ToList();
                                                        deviceSetting.TemperatureRegulate = temperature;
                                                    }
                                                    if (humidityregulate != null)
                                                    {
                                                        List<decimal> humidity = Array.ConvertAll(humidityregulate.ToString().Trim().Split(','), decimal.Parse).ToList();
                                                        deviceSetting.HumidityRegulate = humidity;
                                                    }
                                                    gatewaysetting.DeviceSettings.Add(deviceSetting);
                                                }
                                            }
                                        }
                                    }
                                    InitialMethod.System_Save(SystemSetting);
                                    SystemFlag = true;
                                }
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(ErrorStr))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { Log.Error(ex, $"資料匯入失敗  檔案名稱 : {FileName}"); return false; }
        }
    }
}
