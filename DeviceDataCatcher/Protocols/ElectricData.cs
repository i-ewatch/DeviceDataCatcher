using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDataCatcher.Protocols
{
    public abstract class ElectricData:AbsProtocol
    {
        /// <summary>
        /// R相電壓
        /// </summary>
        public decimal RV { get; protected set; }
        /// <summary>
        /// S相電壓
        /// </summary>
        public decimal SV { get; protected set; }
        /// <summary>
        /// T相電壓
        /// </summary>
        public decimal TV { get; protected set; }
        /// <summary>
        /// 平均相電壓
        /// </summary>
        public decimal VNAVG { get; protected set; }
        /// <summary>
        /// R線電壓
        /// </summary>
        public decimal RSV { get; protected set; }
        /// <summary>
        /// S線電壓
        /// </summary>
        public decimal STV { get; protected set; }
        /// <summary>
        /// T線電壓
        /// </summary>
        public decimal TRV { get; protected set; }
        /// <summary>
        /// 平均線電壓
        /// </summary>
        public decimal VLAVG { get; set; }
        /// <summary>
        /// R相電流
        /// </summary>
        public decimal RA { get; protected set; }
        /// <summary>
        /// S相電流
        /// </summary>
        public decimal SA { get; protected set; }
        /// <summary>
        /// T相電流
        /// </summary>
        public decimal TA { get; protected set; }
        /// <summary>
        /// 平均相電流
        /// </summary>
        public decimal AAVG { get; protected set; }
        /// <summary>
        /// a瞬間視在功率
        /// </summary>
        public decimal KVAA { get; protected set; }
        /// <summary>
        /// b瞬間視在功率
        /// </summary>
        public decimal KVAB { get; protected set; }
        /// <summary>
        /// c瞬間視在功率
        /// </summary>
        public decimal KVAC { get; protected set; }
        /// <summary>
        /// 瞬間視在功率
        /// </summary>
        public decimal KVA { get; protected set; }
        /// <summary>
        /// a瞬間功率
        /// </summary>
        public decimal KWA { get; protected set; }
        /// <summary>
        /// b瞬間功率
        /// </summary>
        public decimal KWB { get; protected set; }
        /// <summary>
        /// c瞬間功率
        /// </summary>
        public decimal KWC { get; protected set; }
        /// <summary>
        /// 瞬間功率
        /// </summary>
        public decimal KW { get; protected set; }
        /// <summary>
        /// a瞬間虛功率
        /// </summary>
        public decimal KVARA { get; protected set; }
        /// <summary>
        /// b瞬間虛功率
        /// </summary>
        public decimal KVARB { get; protected set; }
        /// <summary>
        /// c瞬間虛功率
        /// </summary>
        public decimal KVARC { get; protected set; }
        /// <summary>
        /// 瞬間虛功率
        /// </summary>
        public decimal KVAR { get; protected set; }
        /// <summary>
        /// a功率因數
        /// </summary>
        public decimal PFEA { get; protected set; }
        /// <summary>
        /// b功率因數
        /// </summary>
        public decimal PFEB { get; protected set; }
        /// <summary>
        /// c功率因數
        /// </summary>
        public decimal PFEC { get; protected set; }
        /// <summary>
        /// 功率因數
        /// </summary>
        public decimal PFE { get; protected set; }
        /// <summary>
        /// 累積功率
        /// </summary>
        public decimal KWH { get; protected set; }
        /// <summary>
        /// 累積虛功率
        /// </summary>
        public decimal KVARH { get; protected set; }
        /// <summary>
        /// 累積視在功率
        /// </summary>
        public decimal KVAH { get; protected set; }
        /// <summary>
        /// 頻率
        /// </summary>
        public decimal HZ { get; protected set; }
        /// <summary>
        /// R相電壓角度
        /// </summary>
        public decimal RV_Angle { get; protected set; }
        /// <summary>
        /// S相電壓角度
        /// </summary>
        public decimal SV_Angle { get; protected set; }
        /// <summary>
        /// T相電壓角度
        /// </summary>
        public decimal TV_Angle { get; protected set; }
        /// <summary>
        /// R相電流角度
        /// </summary>
        public decimal RA_Angle { get; protected set; }
        /// <summary>
        /// S相電流角度
        /// </summary>
        public decimal SA_Angle { get; protected set; }
        /// <summary>
        /// T相電流角度
        /// </summary>
        public decimal TA_Angle { get; protected set; }
    }
}
