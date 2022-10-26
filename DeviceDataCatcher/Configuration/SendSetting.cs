using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Configuration
{
    public class SendSetting
    {
        /// <summary>
        /// 卡號
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 版號
        /// </summary>
        public string BoardNo { get; set; }
        /// <summary>
        /// 傳送設備項目清單
        /// </summary>
        public List<SendItemSetting> SendItemSettings { get; set; } = new List<SendItemSetting>();
    }

    public class SendItemSetting
    {
        /// <summary>
        /// Gateway編號
        /// </summary>
        public int GatewayIndex { get; set; }
        /// <summary>
        /// 設備編號
        /// </summary>
        public int DeviceIndex { get; set; }
        /// <summary>
        /// 設備類型
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 迴路索引
        /// </summary>
        public int LoopIndex { get; set; }
        /// <summary>
        /// 使用線電壓
        /// </summary>

        public bool UseLineVoltage { get; set; }
    }
}
