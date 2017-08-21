using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RdpHelper.Business.Data;

namespace RdpHelper.Business.View.RdpMain
{
    public partial class RdpMainForm : Form
    {
        private const int RAW_HEIGHT = 40;
        public RdpMainForm()
        {
            InitializeComponent();
        }

        private void RefreshPanelItem()
        {
            this.panelItemList.Controls.Clear();
            this.sendButtonList.Clear();
            this.textBoxTargetList.Clear();
            this.textBoxProtoList.Clear();

            this.sendBtnMap.Clear(); List<RdpWrapVO> list = Proxy.Instance.GetList();
            for (int i = 0, count = list.Count; i < count; i++)
            {
                RdpWrapVO vo = list[i];
                CommonUtils.CreateRdpFile(vo);
                AddProtoItem(vo, i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Proxy.Instance.RequestDataFromRemote();
            RefreshPanelItem();
        }

        private void RdpMainForm_Load(object sender, EventArgs e)
        {
            Proxy.Instance.RequestDataFromRemote();
            RefreshPanelItem();
        }

        private void OnSendBtnClick(object sender, EventArgs e)
        {
            RdpWrapVO vo;
            if (sender is Button && sendBtnMap.TryGetValue((Button)sender, out vo))
            {
                CommonUtils.StartRdp(vo);
            }
        }

        private List<Button> sendButtonList = new List<Button>();
        private List<TextBox> textBoxTargetList = new List<TextBox>();
        private List<TextBox> textBoxProtoList = new List<TextBox>();
        private Dictionary<Button, RdpWrapVO> sendBtnMap = new Dictionary<Button, RdpWrapVO>();

        private void AddProtoItem(RdpWrapVO vo, int index)
        {
            var buttonSend = new Button();
            var textBoxTarget = new TextBox();
            var textBoxProto = new TextBox();
            this.sendButtonList.Add(buttonSend);
            this.textBoxTargetList.Add(textBoxTarget);
            this.textBoxProtoList.Add(textBoxProto);
            this.sendBtnMap.Add(buttonSend, vo);

            int offsetY = RAW_HEIGHT * index - 125;
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            buttonSend.Location = new System.Drawing.Point(88 -78, 131 + offsetY);
            buttonSend.Name = "buttonSend" + index;
            buttonSend.Size = new System.Drawing.Size(75, 23);
            buttonSend.TabIndex = 0;
            buttonSend.Text = "发送";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += OnSendBtnClick;
            // 
            // textBoxTarget
            // 
            textBoxTarget.Location = new System.Drawing.Point(169 - 78, 131 + offsetY);
            textBoxTarget.Name = "textBoxTarget" + index;
            textBoxTarget.ReadOnly = true;
            textBoxTarget.Size = new System.Drawing.Size(108, 21);
            textBoxTarget.TabIndex = 1;
            textBoxTarget.Text = vo.server;
            // 
            // textBoxProto
            // 
            textBoxProto.Location = new System.Drawing.Point(283 - 78, 131 + offsetY);
            textBoxProto.Name = "textBoxProto" + index;
            textBoxProto.ReadOnly = true;
            textBoxProto.Size = new System.Drawing.Size(471, 21);
            textBoxProto.TabIndex = 2;
            textBoxProto.Text = CommonUtils.EncodeRdpProto(vo);

            this.panelItemList.Controls.Add(buttonSend);
            this.panelItemList.Controls.Add(textBoxProto);
            this.panelItemList.Controls.Add(textBoxTarget);
            this.ResumeLayout(false);
            this.Refresh();
        }

    }
}
