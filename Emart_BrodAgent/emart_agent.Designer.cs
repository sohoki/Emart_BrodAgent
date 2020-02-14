namespace Emart_BrodAgent
{
    partial class emart_agent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(emart_agent));
            this.gbSettingInfo = new System.Windows.Forms.GroupBox();
            this.txtAgetntId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chk_RegStart = new System.Windows.Forms.CheckBox();
            this.cbo_Mac = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_ServerIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.cmsTrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SMItem01 = new System.Windows.Forms.ToolStripMenuItem();
            this.SMItem02 = new System.Windows.Forms.ToolStripMenuItem();
            this.notCon = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.playList01 = new System.Windows.Forms.ListBox();
            this.pnl_loading = new System.Windows.Forms.Panel();
            this.lbl_agentInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar_info = new System.Windows.Forms.TrackBar();
            this.gbSettingInfo.SuspendLayout();
            this.cmsTrip.SuspendLayout();
            this.gbInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_info)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSettingInfo
            // 
            this.gbSettingInfo.Controls.Add(this.txtAgetntId);
            this.gbSettingInfo.Controls.Add(this.label4);
            this.gbSettingInfo.Controls.Add(this.chk_RegStart);
            this.gbSettingInfo.Controls.Add(this.cbo_Mac);
            this.gbSettingInfo.Controls.Add(this.label2);
            this.gbSettingInfo.Controls.Add(this.txt_ServerIp);
            this.gbSettingInfo.Controls.Add(this.label1);
            this.gbSettingInfo.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbSettingInfo.Location = new System.Drawing.Point(12, 12);
            this.gbSettingInfo.Name = "gbSettingInfo";
            this.gbSettingInfo.Size = new System.Drawing.Size(294, 178);
            this.gbSettingInfo.TabIndex = 0;
            this.gbSettingInfo.TabStop = false;
            this.gbSettingInfo.Text = "Setting";
            // 
            // txtAgetntId
            // 
            this.txtAgetntId.Location = new System.Drawing.Point(107, 48);
            this.txtAgetntId.Name = "txtAgetntId";
            this.txtAgetntId.Size = new System.Drawing.Size(174, 22);
            this.txtAgetntId.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "단말기 아이디";
            // 
            // chk_RegStart
            // 
            this.chk_RegStart.AutoSize = true;
            this.chk_RegStart.Location = new System.Drawing.Point(156, 143);
            this.chk_RegStart.Name = "chk_RegStart";
            this.chk_RegStart.Size = new System.Drawing.Size(125, 17);
            this.chk_RegStart.TabIndex = 5;
            this.chk_RegStart.Text = "시작시 자동 실행";
            this.chk_RegStart.UseVisualStyleBackColor = true;
            // 
            // cbo_Mac
            // 
            this.cbo_Mac.FormattingEnabled = true;
            this.cbo_Mac.Location = new System.Drawing.Point(107, 80);
            this.cbo_Mac.Name = "cbo_Mac";
            this.cbo_Mac.Size = new System.Drawing.Size(174, 21);
            this.cbo_Mac.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mac Address";
            // 
            // txt_ServerIp
            // 
            this.txt_ServerIp.Location = new System.Drawing.Point(107, 18);
            this.txt_ServerIp.Name = "txt_ServerIp";
            this.txt_ServerIp.Size = new System.Drawing.Size(174, 22);
            this.txt_ServerIp.TabIndex = 1;
            this.txt_ServerIp.Text = "http://did.emart.com";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "서버 주소";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(12, 233);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(100, 23);
            this.btn_Save.TabIndex = 6;
            this.btn_Save.Text = "등록";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(205, 233);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(100, 23);
            this.btn_Exit.TabIndex = 7;
            this.btn_Exit.Text = "종료";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // cmsTrip
            // 
            this.cmsTrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SMItem01,
            this.SMItem02});
            this.cmsTrip.Name = "cmsTrip";
            this.cmsTrip.Size = new System.Drawing.Size(99, 48);
            // 
            // SMItem01
            // 
            this.SMItem01.Name = "SMItem01";
            this.SMItem01.Size = new System.Drawing.Size(98, 22);
            this.SMItem01.Text = "설정";
            this.SMItem01.Click += new System.EventHandler(this.SMItem01_Click);
            // 
            // SMItem02
            // 
            this.SMItem02.Name = "SMItem02";
            this.SMItem02.Size = new System.Drawing.Size(98, 22);
            this.SMItem02.Text = "종료";
            this.SMItem02.Click += new System.EventHandler(this.SMItem02_Click);
            // 
            // notCon
            // 
            this.notCon.BalloonTipText = "음원방송";
            this.notCon.ContextMenuStrip = this.cmsTrip;
            this.notCon.Icon = ((System.Drawing.Icon)(resources.GetObject("notCon.Icon")));
            this.notCon.Text = "음원방송";
            this.notCon.Visible = true;
            this.notCon.Click += new System.EventHandler(this.notCon_Click);
            this.notCon.DoubleClick += new System.EventHandler(this.notCon_DoubleClick);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.playList01);
            this.gbInfo.Controls.Add(this.pnl_loading);
            this.gbInfo.Controls.Add(this.lbl_agentInfo);
            this.gbInfo.Location = new System.Drawing.Point(13, 12);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(293, 178);
            this.gbInfo.TabIndex = 11;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "음원방송정보";
            // 
            // playList01
            // 
            this.playList01.FormattingEnabled = true;
            this.playList01.ItemHeight = 12;
            this.playList01.Location = new System.Drawing.Point(10, 38);
            this.playList01.Name = "playList01";
            this.playList01.Size = new System.Drawing.Size(272, 124);
            this.playList01.TabIndex = 5;
            // 
            // pnl_loading
            // 
            this.pnl_loading.Location = new System.Drawing.Point(10, 39);
            this.pnl_loading.Name = "pnl_loading";
            this.pnl_loading.Size = new System.Drawing.Size(272, 133);
            this.pnl_loading.TabIndex = 3;
            // 
            // lbl_agentInfo
            // 
            this.lbl_agentInfo.AutoSize = true;
            this.lbl_agentInfo.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_agentInfo.Location = new System.Drawing.Point(6, 21);
            this.lbl_agentInfo.Name = "lbl_agentInfo";
            this.lbl_agentInfo.Size = new System.Drawing.Size(46, 13);
            this.lbl_agentInfo.TabIndex = 2;
            this.lbl_agentInfo.Text = "label5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(14, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "볼륨 세팅";
            // 
            // trackBar_info
            // 
            this.trackBar_info.Location = new System.Drawing.Point(82, 202);
            this.trackBar_info.Maximum = 100;
            this.trackBar_info.Name = "trackBar_info";
            this.trackBar_info.Size = new System.Drawing.Size(223, 45);
            this.trackBar_info.TabIndex = 15;
            this.trackBar_info.Scroll += new System.EventHandler(this.trackBar_info_Scroll);
            // 
            // emart_agent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 277);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.trackBar_info);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.gbSettingInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "emart_agent";
            this.Text = "음원방송 에이전트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.emart_agent_FormClosing);
            this.Load += new System.EventHandler(this.emart_agent_Load);
            this.gbSettingInfo.ResumeLayout(false);
            this.gbSettingInfo.PerformLayout();
            this.cmsTrip.ResumeLayout(false);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_info)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSettingInfo;
        private System.Windows.Forms.CheckBox chk_RegStart;
        private System.Windows.Forms.ComboBox cbo_Mac;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ServerIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.TextBox txtAgetntId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip cmsTrip;
        private System.Windows.Forms.NotifyIcon notCon;
        private System.Windows.Forms.ToolStripMenuItem SMItem01;
        private System.Windows.Forms.ToolStripMenuItem SMItem02;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.ListBox playList01;
        private System.Windows.Forms.Panel pnl_loading;
        private System.Windows.Forms.Label lbl_agentInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar_info;
    }
}