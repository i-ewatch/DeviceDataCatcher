using DeviceDataCatcher.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeviceDataCatcher.Methods
{
    public class InitialMethod
    {
        /// <summary>
        /// 工作路徑
        /// </summary>
        private static readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        public static SystemSetting System_Load()
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\System.json";
            SystemSetting setting = null;
            try
            {
                if (File.Exists(setFile))
                {
                    string json = File.ReadAllText(setFile, Encoding.UTF8);
                    setting = JsonConvert.DeserializeObject<SystemSetting>(json);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "讀取系統資訊失敗");
            }
            return setting;
        }
        /// <summary>
        /// 儲存系統資訊
        /// </summary>
        /// <param name="setting"></param>
        public static void System_Save(SystemSetting setting)
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\System.json";
            string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
            File.WriteAllText(setFile, output);
        }
        public static List<SendSetting> Send_Load()
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\Send.json";
            List<SendSetting> setting = null;
            try
            {
                if (File.Exists(setFile))
                {
                    string json = File.ReadAllText(setFile, Encoding.UTF8);
                    setting = JsonConvert.DeserializeObject<List<SendSetting>>(json);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "讀取上傳資訊失敗");
            }
            return setting;
        }
        public static void Send_Save(List<SendSetting> setting)
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\Send.json";
            string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
            File.WriteAllText(setFile, output);
        }
        #region 按鈕Json 建檔與讀取
        /// <summary>
        /// 按鈕Json 建檔與讀取
        /// </summary>
        /// <returns></returns>
        public static ButtonSetting Button_Load()
        {
            ButtonSetting setting = null;
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string SettingPath = $"{WorkPath}\\stf\\button.json";
            if (File.Exists(SettingPath))
            {
                string json = File.ReadAllText(SettingPath, Encoding.UTF8);
                setting = JsonConvert.DeserializeObject<ButtonSetting>(json);
            }
            else
            {
                ButtonSetting Setting = new ButtonSetting()
                {
                    //群組與列表按鈕設定
                    ButtonGroupSettings =
                        {
                            new ButtonGroupSetting()
                            {
                                // 0 = 群組，1 = 列表
                                ButtonStyle = 1,
                                //群組名稱
                                GroupName = "群組名稱",
                                // 群組標註
                                GroupTag = 0,
                                //列表按鈕設定
                                ButtonItemSettings=
                                {
                                    new ButtonItemSetting()
                                    {
                                        //列表名稱
                                        ItemName = "列表名稱",
                                        //列表標註
                                        ItemTag = 0,
                                        //控制畫面顯示
                                        ControlVisible = true
                                    }
                                }
                            }
                        }
                };
                setting = Setting;
                string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                File.WriteAllText(SettingPath, output);
            }

            return setting;
        }
        #endregion
    }
}
