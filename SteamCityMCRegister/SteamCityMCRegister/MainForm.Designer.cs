namespace SteamCityMCRegister
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBox_MachineCode = new System.Windows.Forms.TextBox();
            this.button_Register = new System.Windows.Forms.Button();
            this.button_RefreshMC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_MachineCode
            // 
            this.textBox_MachineCode.Location = new System.Drawing.Point(12, 12);
            this.textBox_MachineCode.Name = "textBox_MachineCode";
            this.textBox_MachineCode.Size = new System.Drawing.Size(385, 25);
            this.textBox_MachineCode.TabIndex = 0;
            // 
            // button_Register
            // 
            this.button_Register.Location = new System.Drawing.Point(217, 43);
            this.button_Register.Name = "button_Register";
            this.button_Register.Size = new System.Drawing.Size(180, 60);
            this.button_Register.TabIndex = 2;
            this.button_Register.Text = "注册";
            this.button_Register.UseVisualStyleBackColor = true;
            this.button_Register.Click += new System.EventHandler(this.button_Register_Click);
            // 
            // button_RefreshMC
            // 
            this.button_RefreshMC.Location = new System.Drawing.Point(12, 43);
            this.button_RefreshMC.Name = "button_RefreshMC";
            this.button_RefreshMC.Size = new System.Drawing.Size(180, 60);
            this.button_RefreshMC.TabIndex = 3;
            this.button_RefreshMC.Text = "刷新";
            this.button_RefreshMC.UseVisualStyleBackColor = true;
            this.button_RefreshMC.Click += new System.EventHandler(this.button_RefreshMC_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 133);
            this.Controls.Add(this.button_RefreshMC);
            this.Controls.Add(this.button_Register);
            this.Controls.Add(this.textBox_MachineCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "蒸汽之城注册机";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_MachineCode;
        private System.Windows.Forms.Button button_Register;
        private System.Windows.Forms.Button button_RefreshMC;
    }
}

