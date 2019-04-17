namespace MonitorFormsGUI
{
    partial class MainWindow
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
            this.monitoredDrivesView = new System.Windows.Forms.DataGridView();
            this.Uuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.monitoredDrivesView)).BeginInit();
            this.SuspendLayout();
            // 
            // monitoredDrivesView
            // 
            this.monitoredDrivesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.monitoredDrivesView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Uuid});
            this.monitoredDrivesView.Location = new System.Drawing.Point(13, 13);
            this.monitoredDrivesView.Name = "monitoredDrivesView";
            this.monitoredDrivesView.Size = new System.Drawing.Size(775, 150);
            this.monitoredDrivesView.TabIndex = 0;
            // 
            // Uuid
            // 
            this.Uuid.HeaderText = "UUID";
            this.Uuid.Name = "Uuid";
            this.Uuid.ReadOnly = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.monitoredDrivesView);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)(this.monitoredDrivesView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView monitoredDrivesView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uuid;
    }
}