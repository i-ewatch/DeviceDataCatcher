using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Protocols
{
    public abstract class FlowData:AbsProtocol
    {
        private decimal _flow { get; set; }
        /// <summary>
        /// 順間流量
        /// </summary>
        public decimal Flow
        {
            get { return _flow; }
            set
            {
                if (value < 0)
                {
                    _flow = 0;
                }
                else
                {
                    _flow = value;
                }
            }
        }
        /// <summary>
        /// 累積流量
        /// </summary>
        public decimal FlowTotal { get; set; }
        /// <summary>
        /// 入水溫度
        /// </summary>
        public decimal InputTemp { get; set; }
        /// <summary>
        /// 出水溫度
        /// </summary>
        public decimal OutputTemp { get; set; }
        /// <summary>
        /// 水溫差
        /// </summary>
        public decimal TempRang
        {
            get
            {
                decimal data = 0;
                if (InputTemp > OutputTemp)
                {
                    data = InputTemp - OutputTemp;
                }
                return data;
            }
        }
        /// <summary>
        /// 冷凍能力
        /// </summary>
        public decimal RT
        {
            get
            {
                decimal value = 0;
                if (Flow != 0 && InputTemp != 0 && OutputTemp != 0)
                {
                    value = Math.Round(Flow * (InputTemp - OutputTemp) * 60 / 3024, 2);
                }
                return value;
            }
        }
    }
}
