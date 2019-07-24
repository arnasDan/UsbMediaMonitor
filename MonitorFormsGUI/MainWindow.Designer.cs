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
            ((System.ComponentModel.ISupportInitialize)(this.monitoredDrivesView)).BeginInit();
            this.SuspendLayout();
            // 
            // monitoredDrivesView
            // 
            this.monitoredDrivesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.monitoredDrivesView.Location = new System.Drawing.Point(13, 13);
            this.monitoredDrivesView.Name = "monitoredDrivesView";
            this.monitoredDrivesView.Size = new System.Drawing.Size(775, 150);
            this.monitoredDrivesView.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(71, 187);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // addDriveButton
            // 
            this.addDriveButton.Location = new System.Drawing.Point(200, 186);
            this.addDriveButton.Name = "addDriveButton";
            this.addDriveButton.Size = new System.Drawing.Size(75, 23);
            this.addDriveButton.TabIndex = 2;
            this.addDriveButton.Text = "Add drive";
            this.addDriveButton.UseVisualStyleBackColor = true;
            this.addDriveButton.Visible = false;
            this.addDriveButton.Click += new System.EventHandler(this.AddDriveButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.addDriveButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.monitoredDrivesView);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)(this.monitoredDrivesView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView monitoredDrivesView;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button addDriveButton;
    }
}