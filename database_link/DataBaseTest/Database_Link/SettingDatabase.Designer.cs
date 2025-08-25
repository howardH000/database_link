namespace Database_Link
{
    partial class SettingDatabase
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
            this.AddTable = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.AddConnection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddTable
            // 
            this.AddTable.Location = new System.Drawing.Point(475, 10);
            this.AddTable.Name = "AddTable";
            this.AddTable.Size = new System.Drawing.Size(75, 23);
            this.AddTable.TabIndex = 0;
            this.AddTable.Text = "AddTable";
            this.AddTable.UseVisualStyleBackColor = true;
            this.AddTable.Click += new System.EventHandler(this.AddTable_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(40, 10);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 2;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // AddConnection
            // 
            this.AddConnection.Location = new System.Drawing.Point(200, 10);
            this.AddConnection.Name = "AddConnection";
            this.AddConnection.Size = new System.Drawing.Size(75, 23);
            this.AddConnection.TabIndex = 3;
            this.AddConnection.Text = "Connection";
            this.AddConnection.UseVisualStyleBackColor = true;
            this.AddConnection.Click += new System.EventHandler(this.AddConnection_Click);
            // 
            // SettingDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1153, 605);
            this.Controls.Add(this.AddConnection);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.AddTable);
            this.Name = "SettingDatabase";
            this.Text = "SettingDatabase";
            this.Load += new System.EventHandler(this.SettingDatabase_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddTable;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button AddConnection;
    }
}