namespace EventBooker
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAppointmentView = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.events = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabAppointmentView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabAppointmentView);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(886, 683);
            this.tabControl1.TabIndex = 2;
            // 
            // tabAppointmentView
            // 
            this.tabAppointmentView.Controls.Add(this.events);
            this.tabAppointmentView.Location = new System.Drawing.Point(4, 22);
            this.tabAppointmentView.Name = "tabAppointmentView";
            this.tabAppointmentView.Padding = new System.Windows.Forms.Padding(3);
            this.tabAppointmentView.Size = new System.Drawing.Size(878, 657);
            this.tabAppointmentView.TabIndex = 0;
            this.tabAppointmentView.Text = "Appointments";
            this.tabAppointmentView.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // events
            // 
            this.events.Location = new System.Drawing.Point(5, 5);
            this.events.Name = "events";
            this.events.Size = new System.Drawing.Size(867, 643);
            this.events.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 700);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabAppointmentView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAppointmentView;
        private System.Windows.Forms.Panel events;
        private System.Windows.Forms.TabPage tabPage2;

    }
}

