namespace programe_Update
{
    partial class frmUpdate
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lv_UpdateInfo = new System.Windows.Forms.ListView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_UpdateVersion = new System.Windows.Forms.Label();
            this.lbl_Version = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lv_UpdateInfo);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbl_UpdateVersion);
            this.groupBox1.Controls.Add(this.lbl_Version);
            this.groupBox1.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(443, 147);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "프로그램 정보";
            // 
            // lv_UpdateInfo
            // 
            this.lv_UpdateInfo.HideSelection = false;
            this.lv_UpdateInfo.Location = new System.Drawing.Point(21, 91);
            this.lv_UpdateInfo.Name = "lv_UpdateInfo";
            this.lv_UpdateInfo.Size = new System.Drawing.Size(406, 38);
            this.lv_UpdateInfo.TabIndex = 4;
            this.lv_UpdateInfo.UseCompatibleStateImageBehavior = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(117, 52);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(310, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "진행상태";
            // 
            // lbl_UpdateVersion
            // 
            this.lbl_UpdateVersion.AutoSize = true;
            this.lbl_UpdateVersion.Location = new System.Drawing.Point(225, 27);
            this.lbl_UpdateVersion.Name = "lbl_UpdateVersion";
            this.lbl_UpdateVersion.Size = new System.Drawing.Size(101, 13);
            this.lbl_UpdateVersion.TabIndex = 1;
            this.lbl_UpdateVersion.Text = "업데이트 버전:";
            // 
            // lbl_Version
            // 
            this.lbl_Version.AutoSize = true;
            this.lbl_Version.Location = new System.Drawing.Point(18, 27);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new System.Drawing.Size(101, 13);
            this.lbl_Version.TabIndex = 0;
            this.lbl_Version.Text = "프로그램 버전:";
            // 
            // frmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 182);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmUpdate";
            this.Text = "프로그램 업데이트";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUpdate_FormClosed);
            this.Load += new System.EventHandler(this.frmUpdate_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_UpdateVersion;
        private System.Windows.Forms.Label lbl_Version;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lv_UpdateInfo;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

