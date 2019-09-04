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
            this.events = new System.Windows.Forms.Panel();
            this.btnGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // events
            // 
            this.events.Location = new System.Drawing.Point(35, 12);
            this.events.Name = "events";
            this.events.Size = new System.Drawing.Size(867, 643);
            this.events.TabIndex = 1;
            this.events.Paint += new System.Windows.Forms.PaintEventHandler(this.events_Paint);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(908, 403);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(50, 109);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 667);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.events);
            this.Name = "Form1";
            this.Text = "Auto Event Booker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel events;
        private System.Windows.Forms.Button btnGo;
    }
}

