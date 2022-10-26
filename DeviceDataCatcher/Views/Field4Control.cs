using DevExpress.XtraEditors;
using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Views
{
    public class Field4Control: XtraUserControl
    {
        /// <summary>
        /// 工作路徑
        /// </summary>
        public string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 設備通訊
        /// </summary>
        public GatewaySetting GatewaySetting { get; set; }
        /// <summary>
        /// 總設備通訊物件
        /// </summary>
        public List<AbsProtocol> AbsProtocols { get; set; }
        /// <summary>
        /// 更新畫面
        /// </summary>
        public virtual void TextChange() { }
    }
}
