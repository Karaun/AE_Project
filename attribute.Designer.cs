
namespace AE教程2
{
    partial class attribute
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
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtWhereClause = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lstvalue = new System.Windows.Forms.ListBox();
            this.lstopts = new System.Windows.Forms.ListBox();
            this.cboLayers = new System.Windows.Forms.ComboBox();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(245, 451);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 23;
            this.button3.Text = "清空";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(335, 451);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // txtWhereClause
            // 
            this.txtWhereClause.Location = new System.Drawing.Point(5, 334);
            this.txtWhereClause.Name = "txtWhereClause";
            this.txtWhereClause.Size = new System.Drawing.Size(405, 96);
            this.txtWhereClause.TabIndex = 21;
            this.txtWhereClause.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 306);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "Where";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 306);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 307);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "Selec * Form";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(290, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "获取唯一值";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lstvalue
            // 
            this.lstvalue.FormattingEnabled = true;
            this.lstvalue.ItemHeight = 12;
            this.lstvalue.Location = new System.Drawing.Point(290, 86);
            this.lstvalue.Name = "lstvalue";
            this.lstvalue.Size = new System.Drawing.Size(120, 160);
            this.lstvalue.TabIndex = 16;
            this.lstvalue.DoubleClick += new System.EventHandler(this.lstvalue_DoubleClick_1);
            // 
            // lstopts
            // 
            this.lstopts.FormattingEnabled = true;
            this.lstopts.ItemHeight = 12;
            this.lstopts.Location = new System.Drawing.Point(149, 86);
            this.lstopts.Name = "lstopts";
            this.lstopts.Size = new System.Drawing.Size(120, 196);
            this.lstopts.TabIndex = 15;
            this.lstopts.DoubleClick += new System.EventHandler(this.lstopts_DoubleClick_1);
            // 
            // cboLayers
            // 
            this.cboLayers.FormattingEnabled = true;
            this.cboLayers.Location = new System.Drawing.Point(75, 37);
            this.cboLayers.Name = "cboLayers";
            this.cboLayers.Size = new System.Drawing.Size(335, 20);
            this.cboLayers.TabIndex = 14;
            this.cboLayers.SelectedIndexChanged += new System.EventHandler(this.cboLayers_SelectedIndexChanged_1);
            // 
            // lstFields
            // 
            this.lstFields.FormattingEnabled = true;
            this.lstFields.ItemHeight = 12;
            this.lstFields.Location = new System.Drawing.Point(5, 86);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(120, 196);
            this.lstFields.TabIndex = 13;
            this.lstFields.SelectedIndexChanged += new System.EventHandler(this.lstFields_SelectedIndexChanged);
            this.lstFields.DoubleClick += new System.EventHandler(this.lstFields_DoubleClick_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 13F);
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 12;
            this.label1.Tag = "";
            this.label1.Text = "图层名";
            // 
            // attribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 495);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtWhereClause);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstvalue);
            this.Controls.Add(this.lstopts);
            this.Controls.Add(this.cboLayers);
            this.Controls.Add(this.lstFields);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "attribute";
            this.Text = "属性查询";
            this.Load += new System.EventHandler(this.attribute_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox txtWhereClause;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstvalue;
        private System.Windows.Forms.ListBox lstopts;
        private System.Windows.Forms.ComboBox cboLayers;
        private System.Windows.Forms.ListBox lstFields;
        private System.Windows.Forms.Label label1;
    }
}