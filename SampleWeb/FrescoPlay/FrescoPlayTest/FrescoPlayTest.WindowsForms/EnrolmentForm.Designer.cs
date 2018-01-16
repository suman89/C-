namespace FrescoPlayTest.WindowsForms
{
    partial class EnrolmentForm
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
            this.EnrolmentIdValue = new System.Windows.Forms.TextBox();
            this.ContentIdValue = new System.Windows.Forms.TextBox();
            this.ApiKeyValue = new System.Windows.Forms.TextBox();
            this.EnrolmentId = new System.Windows.Forms.Label();
            this.ContentId = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.TrialCount = new System.Windows.Forms.Label();
            this.TrialCountValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EnrolmentIdValue
            // 
            this.EnrolmentIdValue.Location = new System.Drawing.Point(131, 78);
            this.EnrolmentIdValue.Name = "EnrolmentIdValue";
            this.EnrolmentIdValue.Size = new System.Drawing.Size(338, 20);
            this.EnrolmentIdValue.TabIndex = 0;
            // 
            // ContentIdValue
            // 
            this.ContentIdValue.Location = new System.Drawing.Point(131, 129);
            this.ContentIdValue.Name = "ContentIdValue";
            this.ContentIdValue.Size = new System.Drawing.Size(338, 20);
            this.ContentIdValue.TabIndex = 1;
            // 
            // ApiKeyValue
            // 
            this.ApiKeyValue.Location = new System.Drawing.Point(131, 176);
            this.ApiKeyValue.Name = "ApiKeyValue";
            this.ApiKeyValue.Size = new System.Drawing.Size(338, 20);
            this.ApiKeyValue.TabIndex = 2;
            // 
            // EnrolmentId
            // 
            this.EnrolmentId.AutoSize = true;
            this.EnrolmentId.Location = new System.Drawing.Point(42, 78);
            this.EnrolmentId.Name = "EnrolmentId";
            this.EnrolmentId.Size = new System.Drawing.Size(68, 13);
            this.EnrolmentId.TabIndex = 3;
            this.EnrolmentId.Text = "Enrollment Id";
            // 
            // ContentId
            // 
            this.ContentId.AutoSize = true;
            this.ContentId.Location = new System.Drawing.Point(42, 129);
            this.ContentId.Name = "ContentId";
            this.ContentId.Size = new System.Drawing.Size(56, 13);
            this.ContentId.TabIndex = 4;
            this.ContentId.Text = "Content Id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Api Key";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(222, 222);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.EnrolmentButton_Click);
            // 
            // TrialCount
            // 
            this.TrialCount.AutoSize = true;
            this.TrialCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrialCount.Location = new System.Drawing.Point(93, 19);
            this.TrialCount.Name = "TrialCount";
            this.TrialCount.Size = new System.Drawing.Size(103, 17);
            this.TrialCount.TabIndex = 7;
            this.TrialCount.Text = "Trial Count : ";
            // 
            // TrialCountValue
            // 
            this.TrialCountValue.AutoSize = true;
            this.TrialCountValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrialCountValue.Location = new System.Drawing.Point(219, 19);
            this.TrialCountValue.Name = "TrialCountValue";
            this.TrialCountValue.Size = new System.Drawing.Size(0, 17);
            this.TrialCountValue.TabIndex = 8;
            // 
            // EnrolmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 257);
            this.Controls.Add(this.TrialCountValue);
            this.Controls.Add(this.TrialCount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ContentId);
            this.Controls.Add(this.EnrolmentId);
            this.Controls.Add(this.ApiKeyValue);
            this.Controls.Add(this.ContentIdValue);
            this.Controls.Add(this.EnrolmentIdValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EnrolmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get Question Details";
            this.Load += new System.EventHandler(this.EnrolmentForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EnrolmentIdValue;
        private System.Windows.Forms.TextBox ContentIdValue;
        private System.Windows.Forms.TextBox ApiKeyValue;
        private System.Windows.Forms.Label EnrolmentId;
        private System.Windows.Forms.Label ContentId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label TrialCount;
        private System.Windows.Forms.Label TrialCountValue;
    }
}

