namespace AudioScanner
{
    partial class frmAudioManager
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstAudio = new System.Windows.Forms.ListBox();
            this.timerScan = new System.Windows.Forms.Timer(this.components);
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.gbAudio = new System.Windows.Forms.GroupBox();
            this.chkMute = new System.Windows.Forms.CheckBox();
            this.GbExtra = new System.Windows.Forms.GroupBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutoLock = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            this.gbAudio.SuspendLayout();
            this.GbExtra.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstAudio
            // 
            this.lstAudio.FormattingEnabled = true;
            this.lstAudio.Location = new System.Drawing.Point(12, 12);
            this.lstAudio.Name = "lstAudio";
            this.lstAudio.Size = new System.Drawing.Size(600, 277);
            this.lstAudio.TabIndex = 0;
            this.lstAudio.SelectedIndexChanged += new System.EventHandler(this.LstAudio_SelectedIndexChanged);
            // 
            // timerScan
            // 
            this.timerScan.Tick += new System.EventHandler(this.timerScan_Tick);
            // 
            // trackVolume
            // 
            this.trackVolume.Location = new System.Drawing.Point(6, 62);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(158, 45);
            this.trackVolume.TabIndex = 1;
            this.trackVolume.TickFrequency = 5;
            this.trackVolume.ValueChanged += new System.EventHandler(this.TrackVolume_ValueChanged);
            // 
            // gbAudio
            // 
            this.gbAudio.Controls.Add(this.chkMute);
            this.gbAudio.Controls.Add(this.trackVolume);
            this.gbAudio.Location = new System.Drawing.Point(618, 12);
            this.gbAudio.Name = "gbAudio";
            this.gbAudio.Size = new System.Drawing.Size(170, 133);
            this.gbAudio.TabIndex = 2;
            this.gbAudio.TabStop = false;
            this.gbAudio.Text = "Selecionar Audio...";
            // 
            // chkMute
            // 
            this.chkMute.AutoSize = true;
            this.chkMute.Location = new System.Drawing.Point(6, 39);
            this.chkMute.Name = "chkMute";
            this.chkMute.Size = new System.Drawing.Size(66, 17);
            this.chkMute.TabIndex = 2;
            this.chkMute.Text = "Silenciar";
            this.chkMute.UseVisualStyleBackColor = true;
            this.chkMute.CheckedChanged += new System.EventHandler(this.ChkMute_CheckedChanged);
            // 
            // GbExtra
            // 
            this.GbExtra.Controls.Add(this.chkAutoLock);
            this.GbExtra.Controls.Add(this.label1);
            this.GbExtra.Controls.Add(this.txtTarget);
            this.GbExtra.Location = new System.Drawing.Point(618, 151);
            this.GbExtra.Name = "GbExtra";
            this.GbExtra.Size = new System.Drawing.Size(170, 136);
            this.GbExtra.TabIndex = 3;
            this.GbExtra.TabStop = false;
            this.GbExtra.Text = "Para Aplicação";
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(6, 40);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(158, 20);
            this.txtTarget.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nome para focar";
            // 
            // chkAutoLock
            // 
            this.chkAutoLock.AutoSize = true;
            this.chkAutoLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoLock.Location = new System.Drawing.Point(13, 76);
            this.chkAutoLock.Name = "chkAutoLock";
            this.chkAutoLock.Size = new System.Drawing.Size(145, 17);
            this.chkAutoLock.TabIndex = 2;
            this.chkAutoLock.Text = "Auto-Selecionar este app";
            this.chkAutoLock.UseVisualStyleBackColor = true;
            // 
            // frmAudioManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 299);
            this.Controls.Add(this.GbExtra);
            this.Controls.Add(this.gbAudio);
            this.Controls.Add(this.lstAudio);
            this.Name = "frmAudioManager";
            this.Text = "Audio Manager";
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            this.gbAudio.ResumeLayout(false);
            this.gbAudio.PerformLayout();
            this.GbExtra.ResumeLayout(false);
            this.GbExtra.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstAudio;
        private System.Windows.Forms.Timer timerScan;
        private System.Windows.Forms.TrackBar trackVolume;
        private System.Windows.Forms.GroupBox gbAudio;
        private System.Windows.Forms.CheckBox chkMute;
        private System.Windows.Forms.GroupBox GbExtra;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.CheckBox chkAutoLock;
    }
}

