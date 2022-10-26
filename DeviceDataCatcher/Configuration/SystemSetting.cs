using System.Collections.Generic;

namespace DeviceDataCatcher.Configuration
{
    /// <summary>
    /// 系統資訊
    /// </summary>
    public class SystemSetting
    {
        /// <summary>
        /// 200碼傳送旗標
        /// </summary>
        public bool SendFlag { get; set; }
        /// <summary>
        /// 通訊運作啟動旗標
        /// </summary>
        public bool WorkFlag { get; set; }
        /// <summary>
        /// 紀錄旗標
        /// </summary>
        public bool RecordFlag { get; set; }
        /// <summary>
        /// 總通道資訊
        /// </summary>
        public List<GatewaySetting> GatewaySettings { get; set; } = new List<GatewaySetting>();
    }
    /// <summary>
    /// 通道資訊
    /// </summary>
    public class GatewaySetting
    {
        /// <summary>
        /// 通道編碼
        /// </summary>
        public int Gateway_Number { get; set; }
        /// <summary>
        /// 通道位址
        /// </summary>
        public string Gateway_Location { get; set; }
        /// <summary>
        /// 通道Rate
        /// </summary>
        public int Gateway_Rate { get; set; }
        /// <summary>
        /// 通道類型
        /// </summary>
        public int Gateway_Type { get; set; }
        /// <summary>
        /// 通道名稱
        /// </summary>
        public string Gateway_Name { get; set; }
        /// <summary>
        /// 圖片名稱
        /// </summary>
        public string ImageName { get; set; }
        /// <summary>
        /// 總設備資訊
        /// </summary>
        public List<DeviceSetting> DeviceSettings { get; set; } = new List<DeviceSetting>();
    }
    /// <summary>
    /// 設備資訊
    /// </summary>
    public class DeviceSetting
    {
        /// <summary>
        /// 設備編碼
        /// </summary>
        public int Device_Number { get; set; }
        /// <summary>
        /// 設備類型
        /// </summary>
        public int Device_Type { get; set; }
        /// <summary>
        /// 設備站號
        /// </summary>
        public int Device_ID { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string Device_Name { get; set; }
        /// <summary>
        /// 冰/冷卻水旗標
        /// </summary>
        public bool Ice_Cool_Flag { get; set; }
        /// <summary>
        /// 溫度較正值
        /// </summary>
        public List<decimal> TemperatureRegulate = new List<decimal>();
        /// <summary>
        /// 濕度較正值
        /// </summary>
        public List<decimal> HumidityRegulate = new List<decimal>();
    }
}
