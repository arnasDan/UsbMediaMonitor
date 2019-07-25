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
            this.saveButton = new System.Windows.Forms.Button();
            this.addDriveButton = new System.Windows.Forms.Button();
            this.randomDriveCheckbox = new System.Windows.Forms.CheckBox();
            this.saveRequiredLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.monitoredDrivesView)).BeginInit();
            this.SuspendLayout();
            // 
            // monitoredDrivesView
            // 
            this.monitoredDrivesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.monitoredDrivesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.monitoredDrivesView.Location = new System.Drawing.Point(13, 13);
            this.monitoredDrivesView.Name = "monitoredDrivesView";
            this.monitoredDrivesView.Size = new System.Drawing.Size(1035, 155);
            this.monitoredDrivesView.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(13, 174);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // addDriveButton
            // 
            this.addDriveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addDriveButton.Location = new System.Drawing.Point(870, 174);
            this.addDriveButton.Name = "addDriveButton";
            this.addDriveButton.Size = new System.Drawing.Size(75, 23);
            this.addDriveButton.TabIndex = 2;
            this.addDriveButton.Text = "AddDrive";
            this.addDriveButton.UseVisualStyleBackColor = true;
            this.addDriveButton.Visible = false;
            this.addDriveButton.Click += new System.EventHandler(this.AddDriveButton_Click);
            // 
            // randomDriveCheckbox
            // 
            this.randomDriveCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.randomDriveCheckbox.AutoSize = true;
            this.randomDriveCheckbox.Location = new System.Drawing.Point(960, 178);
            this.randomDriveCheckbox.Name = "randomDriveCheckbox";
            this.randomDriveCheckbox.Size = new System.Drawing.Size(91, 17);
            this.randomDriveCheckbox.TabIndex = 3;
            this.randomDriveCheckbox.Text = "RandomDrive";
            this.randomDriveCheckbox.UseVisualStyleBackColor = true;
            this.randomDriveCheckbox.Visible = false;
            // 
            // saveRequiredLabel
            // 
            this.saveRequiredLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveRequiredLabel.AutoSize = true;
            this.saveRequiredLabel.Location = new System.Drawing.Point(94, 179);
            this.saveRequiredLabel.Name = "saveRequiredLabel";
            this.saveRequiredLabel.Size = new System.Drawing.Size(75, 13);
            this.saveRequiredLabel.TabIndex = 4;
            this.saveRequiredLabel.Text = "SaveRequired";
            this.saveRequiredLabel.Visible = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 206);
            this.Controls.Add(this.saveRequiredLabel);
            this.Controls.Add(this.randomDriveCheckbox);
            this.Controls.Add(this.addDriveButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.monitoredDrivesView);
            this.MinimumSize = new System.Drawing.Size(1075, 245);
            this.Name = "MainWindow";
            this.Text = "UsbMediaMonitor";
            ((System.ComponentModel.ISupportInitialize)(this.monitoredDrivesView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView monitoredDrivesView;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button addDriveButton;
        private System.Windows.Forms.CheckBox randomDriveCheckbox;
        private System.Windows.Forms.Label saveRequiredLabel;
    }
}