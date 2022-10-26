using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Protocols;
using NModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Components
{
    public class Field4Component:Component
    {
        /// <summary>
        /// 通道資訊
        /// </summary>
        public GatewaySetting GatewaySetting { get; set; }
        /// <summary>
        /// 系統資訊
        /// </summary>
        public SystemSetting SystemSetting { get; set; }
        /// <summary>
        /// 設備資訊
        /// </summary>
        public List<DeviceSetting> DeviceSettings { get; set; }
        /// <summary>
        /// 通訊資訊
        /// </summary>
        public List<AbsProtocol> AbsProtocols { get; set; } = new List<AbsProtocol>();
        /// <summary>
        /// 通訊執行緒
        /// </summary>
        public Thread ReadThread { get; set; }
        /// <summary>
        /// 最後讀取時間
        /// </summary>
        public DateTime ReadTime { get; set; }
        #region Nmodbus物件
        /// <summary>
        /// 通訊建置類別(通用)
        /// </summary>
        public ModbusFactory Factory { get; set; }
        /// <summary>
        /// 通訊物件
        /// </summary>
        public SerialPort rs485 { get; set; }

        #region Master
        /// <summary>
        /// 通訊物件
        /// </summary>
        public IModbusMaster master { get; set; }
        #endregion

        #region Slave
        /// <summary>
        /// Slave物件 (若要多個Slaver請不要加入在這Field4Component，請在SlaveComponent內加入)
        /// </summary>
        public IModbusSlave slave;
        /// <summary>
        /// 總Slave物件 (List類型，可以加入多個 IModbusSlave物件)
        /// </summary>
        public IModbusSlaveNetwork network;
        /// <summary>
        /// IP連線通訊
        /// </summary>
        public TcpListener slaveTcpListener;
        #endregion

        #endregion
        #region 初始設定
        public Field4Component()
        {
            OnMyWorkStateChanged += new MyWorkStateChanged(AfterMyWorkStateChanged);
        }
        /// <summary>
        /// 系統工作路徑
        /// </summary>
        protected readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        public delegate void MyWorkStateChanged(object sender, EventArgs e);
        public event MyWorkStateChanged OnMyWorkStateChanged;
        /// <summary>
        /// 通訊功能啟動判斷旗標
        /// </summary>
        protected bool myWorkState;
        /// <summary>
        /// 通訊功能啟動旗標
        /// </summary>
        public bool MyWorkState
        {
            get { return myWorkState; }
            set
            {
                if (value != myWorkState)
                {
                    myWorkState = value;
                    WhenMyWorkStateChange();
                }
            }
        }
        /// <summary>
        /// 執行續工作狀態改變觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void AfterMyWorkStateChanged(object sender, EventArgs e) { }
        protected void WhenMyWorkStateChange()
        {
            OnMyWorkStateChanged?.Invoke(this, null);
        }
        #endregion
    }
}
