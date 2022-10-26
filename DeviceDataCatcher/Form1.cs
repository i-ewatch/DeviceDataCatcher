using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DeviceDataCatcher.Components;
using DeviceDataCatcher.Configuration;
using DeviceDataCatcher.Enums;
using DeviceDataCatcher.Methods;
using DeviceDataCatcher.Protocols;
using DeviceDataCatcher.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TextEdit = DevExpress.XtraEditors.TextEdit;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;

namespace DeviceDataCatcher
{
    public partial class Form1 : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        /// <summary>
        /// 彈跳視窗
        /// </summary>
        public FlyoutAction action = new FlyoutAction();
        /// <summary>
        /// 登入旗標
        /// </summary>
        public bool LoginFlag { get; set; }
        /// <summary>
        /// 設定登出時間(ms)
        /// </summary>
        private int LogoutTime { get; set; } = 100000;
        /// <summary>
        /// 登入時間
        /// </summary>
        private DateTime LoginTime { get; set; }
        private SystemSetting SystemSetting { get; set; }
        private List<SendSetting> SendSettings { get; set; }
        private ButtonSetting ButtonSetting { get; set; }
        private List<Field4Component> Field4Components { get; set; } = new List<Field4Component>();
        private RecordComponent RecordComponent { get; set; }
        private List<AbsProtocol> AbsProtocols { get; set; } = new List<AbsProtocol>();
        private NavigationFrame NavigationFrame { get; set; } = null;
        private ButtonMethod ButtonMethod { get; set; }
        private List<Field4Control> Field4Controls { get; set; } = new List<Field4Control>();
        #region 方法
        /// <summary>
        /// Excel檔案匯入方法
        /// </summary>
        private ExcelMethod ExcelMethod = new ExcelMethod();
        #endregion
        public Form1()
        {
            InitializeComponent();
            #region Serilog
            Log.Logger = new LoggerConfiguration()
                       .WriteTo.Console()
                       .WriteTo.File(path: $"{AppDomain.CurrentDomain.BaseDirectory}\\log\\log.txt",
                                     rollingInterval: RollingInterval.Day,
                                     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                       .CreateLogger();        //宣告Serilog初始化
            #endregion
            CloseBox = false;
            MaximizeBox = false;
            SystemSetting = InitialMethod.System_Load();
            SendSettings = InitialMethod.Send_Load();
            ButtonSetting = InitialMethod.Button_Load();
            if (SystemSetting != null)
            {
                if (SystemSetting.WorkFlag)
                {
                    #region 通訊
                    foreach (var item in SystemSetting.GatewaySettings)
                    {
                        Gateway_Type gateway = (Gateway_Type)item.Gateway_Type;
                        switch (gateway)
                        {
                            case Gateway_Type.Serialport:
                                {
                                    SerialPortComponent component = new SerialPortComponent(item, item.DeviceSettings);
                                    component.MyWorkState = true;
                                    Field4Components.Add(component);
                                    AbsProtocols.AddRange(component.AbsProtocols);
                                }
                                break;
                            case Gateway_Type.TCP:
                                {
                                    TCPComponent component = new TCPComponent(item, item.DeviceSettings);
                                    component.MyWorkState = true;
                                    Field4Components.Add(component);
                                    AbsProtocols.AddRange(component.AbsProtocols);
                                }
                                break;
                        }
                    }
                    #endregion
                    #region 畫面
                    NavigationFrame = new NavigationFrame() { Dock = System.Windows.Forms.DockStyle.Fill, Parent = pcl_View };
                    ButtonMethod = new ButtonMethod() { navigationFrame = NavigationFrame };
                    ButtonMethod.AccordionLoad(accordionControl1, ButtonSetting);
                    HomeView homeView = new HomeView();
                    Field4Controls.Add(homeView);
                    NavigationFrame.AddPage(homeView);
                    foreach (var item in SystemSetting.GatewaySettings)
                    {
                        ChillerView chiller = new ChillerView(AbsProtocols, item);
                        Field4Controls.Add(chiller);
                        NavigationFrame.AddPage(chiller);
                    }
                    #endregion
                }
                if (SystemSetting.RecordFlag)
                {
                    RecordComponent = new RecordComponent(SystemSetting, AbsProtocols);
                    RecordComponent.MyWorkState = true;
                }
            }
            #region 登入按鈕
            bbtn_Login.ImageOptions.Image = imageCollection.Images["Login.png"];
            bbtn_Login.ItemClick += (s, e) =>
            {
                if (LoginFlag)
                {
                    bbtn_ImportExcel.Visibility = BarItemVisibility.Never;
                    LoginFlag = false;
                    bbtn_Login.ImageOptions.Image = imageCollection.Images["Login.png"];
                    bbtn_Login.Caption = "登入";
                }
                else
                {
                    UserControl control = new UserControl() { Padding = new Padding(0, 30, 0, 20), Size = new Size(400, 200) };
                    TextEdit textEdit = new TextEdit() { Dock = DockStyle.Top, Size = new Size(400, 40) };
                    textEdit.Properties.Appearance.FontSizeDelta = 12;
                    textEdit.Properties.Appearance.Options.UseFont = true;
                    textEdit.Properties.Appearance.Options.UseTextOptions = true;
                    textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    textEdit.Parent = control;
                    textEdit.Properties.UseSystemPasswordChar = true;
                    LabelControl labelControl = new LabelControl() { Dock = DockStyle.Top, Size = new Size(400, 50) };
                    labelControl.Appearance.FontSizeDelta = 18;
                    labelControl.AutoSizeMode = LabelAutoSizeMode.None;
                    labelControl.Text = "請輸入登入密碼";
                    labelControl.Appearance.Options.UseFont = true;
                    labelControl.Appearance.Options.UseTextOptions = true;
                    labelControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    labelControl.Parent = control;
                    SimpleButton okButton = new SimpleButton() { Dock = DockStyle.Bottom, Text = "確定", Size = new Size(400, 40) };
                    okButton.Appearance.BackColor = Color.FromArgb(80, 80, 80);
                    okButton.Appearance.FontSizeDelta = 12;
                    okButton.DialogResult = DialogResult.OK;
                    okButton.Parent = control;
                    if (FlyoutDialog.Show(FindForm(), control) == DialogResult.OK && (string.Compare(textEdit.Text, "d001007", true) == 0 || string.Compare(textEdit.Text, "Administrator", true) == 0))
                    {
                        bbtn_ImportExcel.Visibility = BarItemVisibility.Always;
                        LoginFlag = true;
                        bbtn_Login.ImageOptions.Image = imageCollection.Images["Admin.png"];
                        bbtn_Login.Caption = "Admin";
                        LoginTime = DateTime.Now;
                    }
                }
            };
            #endregion
            #region Excel匯入按鈕
            bbtn_ImportExcel.Visibility = BarItemVisibility.Never;
            bbtn_ImportExcel.ItemClick += (s, e) =>
            {
                if (ExcelMethod.Excel_Load())
                {
                    foreach (var item in Field4Components)
                    {
                        item.MyWorkState = false;
                    }
                    if (RecordComponent != null)
                    {
                        RecordComponent.MyWorkState = false;
                    }
                    timer.Enabled = false;
                    action.Caption = "設備資料匯入";
                    action.Description = "匯入完成，請重新啟動!!";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                    Application.ExitThread();
                }
                else
                {
                    action.Caption = $"設備資料匯入";
                    action.Description = $"匯入失敗!!\r\n{ExcelMethod.ErrorStr}";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            #region 登出時間
            TimeSpan LoginTimeSpan = DateTime.Now.Subtract(LoginTime);
            if (LoginTimeSpan.TotalMilliseconds >= LogoutTime && LoginFlag)
            {
                bbtn_ImportExcel.Visibility = BarItemVisibility.Never;
                LoginFlag = false;
                bbtn_Login.ImageOptions.Image = imageCollection.Images["Login.png"];
                bbtn_Login.Caption = "登入";
            }
            #endregion
            if (ButtonMethod != null)
            {
                if (ButtonMethod.ViewIndex < Field4Controls.Count)
                {
                    Field4Controls[ButtonMethod.ViewIndex].TextChange();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserControl control = new UserControl() { Padding = new Padding(0, 30, 0, 20), Size = new Size(400, 200) };
            DevExpress.XtraEditors.TextEdit textEdit = new DevExpress.XtraEditors.TextEdit() { Dock = DockStyle.Top, Size = new Size(400, 40) };
            textEdit.Properties.Appearance.FontSizeDelta = 12;
            textEdit.Properties.Appearance.Options.UseFont = true;
            textEdit.Properties.Appearance.Options.UseTextOptions = true;
            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            textEdit.Parent = control;
            textEdit.Properties.UseSystemPasswordChar = true;
            LabelControl labelControl = new LabelControl() { Dock = DockStyle.Top, Size = new Size(400, 50) };
            labelControl.Appearance.FontSizeDelta = 18;
            labelControl.AutoSizeMode = LabelAutoSizeMode.None;
            labelControl.Text = "請輸入關閉軟體密碼";
            labelControl.Appearance.Options.UseFont = true;
            labelControl.Appearance.Options.UseTextOptions = true;
            labelControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            labelControl.Parent = control;
            SimpleButton okButton = new SimpleButton() { Dock = DockStyle.Bottom, Text = "確定", Size = new Size(400, 40) };
            okButton.Appearance.BackColor = Color.FromArgb(80, 80, 80);
            okButton.Appearance.FontSizeDelta = 12;
            okButton.DialogResult = DialogResult.OK;
            okButton.Parent = control;
            if (FlyoutDialog.Show(FindForm(), control) == DialogResult.OK && string.Compare(textEdit.Text, "qu!t", true) == 0)
            {
                foreach (var item in Field4Components)
                {
                    item.MyWorkState = false;
                }
                if (RecordComponent != null)
                {
                    RecordComponent.MyWorkState = false;
                }
                timer.Enabled = false;
                Application.ExitThread();
                this.Dispose();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
