namespace DeviceDataCatcher.Protocols
{
    public abstract class SenserData:AbsProtocol
    {
        /// <summary>
        /// 倍率
        /// </summary>
        public int Rate { get; set; }
        /// <summary>
        /// 溫度
        /// </summary>
        public decimal Temp { get; set; }
        /// <summary>
        /// 濕度
        /// </summary>
        public decimal Humidity { get; set; }
    }
}
