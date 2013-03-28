namespace EntityGenerator
{
	partial class MainForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.tbPath = new System.Windows.Forms.TextBox();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.clbTables = new System.Windows.Forms.CheckedListBox();
			this.clbViews = new System.Windows.Forms.CheckedListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.rbEntities = new System.Windows.Forms.RadioButton();
			this.rbDbContext = new System.Windows.Forms.RadioButton();
			this.cbAllTables = new System.Windows.Forms.CheckBox();
			this.cbAllViews = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbOutputs = new System.Windows.Forms.ListBox();
			this.tbDefaultNamespace = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tbDbContextName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.tbTableFilter = new System.Windows.Forms.TextBox();
			this.tbViewFilter = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Path to save:";
			// 
			// tbPath
			// 
			this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbPath.Location = new System.Drawing.Point(88, 12);
			this.tbPath.Name = "tbPath";
			this.tbPath.Size = new System.Drawing.Size(213, 20);
			this.tbPath.TabIndex = 1;
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGenerate.Location = new System.Drawing.Point(1017, 9);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(75, 23);
			this.btnGenerate.TabIndex = 2;
			this.btnGenerate.Text = "Let\'s Roll";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// clbTables
			// 
			this.clbTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.clbTables.FormattingEnabled = true;
			this.clbTables.Location = new System.Drawing.Point(15, 68);
			this.clbTables.Name = "clbTables";
			this.clbTables.Size = new System.Drawing.Size(245, 439);
			this.clbTables.TabIndex = 3;
			// 
			// clbViews
			// 
			this.clbViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.clbViews.FormattingEnabled = true;
			this.clbViews.Location = new System.Drawing.Point(266, 68);
			this.clbViews.Name = "clbViews";
			this.clbViews.Size = new System.Drawing.Size(245, 439);
			this.clbViews.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Tables:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(263, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Views:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(514, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Output:";
			// 
			// rbEntities
			// 
			this.rbEntities.AutoSize = true;
			this.rbEntities.Checked = true;
			this.rbEntities.Location = new System.Drawing.Point(6, 19);
			this.rbEntities.Name = "rbEntities";
			this.rbEntities.Size = new System.Drawing.Size(59, 17);
			this.rbEntities.TabIndex = 9;
			this.rbEntities.TabStop = true;
			this.rbEntities.Text = "Entities";
			this.rbEntities.UseVisualStyleBackColor = true;
			// 
			// rbDbContext
			// 
			this.rbDbContext.AutoSize = true;
			this.rbDbContext.Location = new System.Drawing.Point(71, 19);
			this.rbDbContext.Name = "rbDbContext";
			this.rbDbContext.Size = new System.Drawing.Size(75, 17);
			this.rbDbContext.TabIndex = 10;
			this.rbDbContext.Text = "DbContext";
			this.rbDbContext.UseVisualStyleBackColor = true;
			// 
			// cbAllTables
			// 
			this.cbAllTables.AutoSize = true;
			this.cbAllTables.Location = new System.Drawing.Point(61, 47);
			this.cbAllTables.Name = "cbAllTables";
			this.cbAllTables.Size = new System.Drawing.Size(72, 17);
			this.cbAllTables.TabIndex = 11;
			this.cbAllTables.Text = "All Tables";
			this.cbAllTables.UseVisualStyleBackColor = true;
			this.cbAllTables.CheckedChanged += new System.EventHandler(this.cbAllTables_CheckedChanged);
			// 
			// cbAllViews
			// 
			this.cbAllViews.AutoSize = true;
			this.cbAllViews.Location = new System.Drawing.Point(307, 48);
			this.cbAllViews.Name = "cbAllViews";
			this.cbAllViews.Size = new System.Drawing.Size(68, 17);
			this.cbAllViews.TabIndex = 12;
			this.cbAllViews.Text = "All Views";
			this.cbAllViews.UseVisualStyleBackColor = true;
			this.cbAllViews.CheckedChanged += new System.EventHandler(this.cbAllViews_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbEntities);
			this.groupBox1.Controls.Add(this.rbDbContext);
			this.groupBox1.Location = new System.Drawing.Point(811, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 44);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			// 
			// lbOutputs
			// 
			this.lbOutputs.FormattingEnabled = true;
			this.lbOutputs.Location = new System.Drawing.Point(518, 69);
			this.lbOutputs.Name = "lbOutputs";
			this.lbOutputs.Size = new System.Drawing.Size(574, 433);
			this.lbOutputs.TabIndex = 14;
			// 
			// tbDefaultNamespace
			// 
			this.tbDefaultNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDefaultNamespace.Location = new System.Drawing.Point(612, 11);
			this.tbDefaultNamespace.Name = "tbDefaultNamespace";
			this.tbDefaultNamespace.Size = new System.Drawing.Size(193, 20);
			this.tbDefaultNamespace.TabIndex = 16;
			this.tbDefaultNamespace.Text = ".Entities";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(539, 19);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 13);
			this.label5.TabIndex = 15;
			this.label5.Text = "Namespace:";
			// 
			// tbDbContextName
			// 
			this.tbDbContextName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDbContextName.Location = new System.Drawing.Point(403, 12);
			this.tbDbContextName.Name = "tbDbContextName";
			this.tbDbContextName.Size = new System.Drawing.Size(130, 20);
			this.tbDbContextName.TabIndex = 18;
			this.tbDbContextName.Text = "MyDbContext";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(306, 19);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(91, 13);
			this.label6.TabIndex = 17;
			this.label6.Text = "DbContext Name:";
			// 
			// tbTableFilter
			// 
			this.tbTableFilter.Location = new System.Drawing.Point(140, 47);
			this.tbTableFilter.Name = "tbTableFilter";
			this.tbTableFilter.Size = new System.Drawing.Size(100, 20);
			this.tbTableFilter.TabIndex = 19;
			this.tbTableFilter.TextChanged += new System.EventHandler(this.tbTableFilter_TextChanged);
			this.tbTableFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTableFilter_KeyPress);
			// 
			// tbViewFilter
			// 
			this.tbViewFilter.Location = new System.Drawing.Point(381, 46);
			this.tbViewFilter.Name = "tbViewFilter";
			this.tbViewFilter.Size = new System.Drawing.Size(100, 20);
			this.tbViewFilter.TabIndex = 20;
			this.tbViewFilter.TextChanged += new System.EventHandler(this.tbViewFilter_TextChanged);
			this.tbViewFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbViewFilter_KeyPress);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1104, 516);
			this.Controls.Add(this.tbViewFilter);
			this.Controls.Add(this.tbTableFilter);
			this.Controls.Add(this.tbDbContextName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tbDefaultNamespace);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbOutputs);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cbAllViews);
			this.Controls.Add(this.cbAllTables);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.clbViews);
			this.Controls.Add(this.clbTables);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.tbPath);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.Text = "Entity Framework Code Generator";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbPath;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.CheckedListBox clbTables;
		private System.Windows.Forms.CheckedListBox clbViews;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbEntities;
		private System.Windows.Forms.RadioButton rbDbContext;
		private System.Windows.Forms.CheckBox cbAllTables;
		private System.Windows.Forms.CheckBox cbAllViews;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox lbOutputs;
		private System.Windows.Forms.TextBox tbDefaultNamespace;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbDbContextName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbTableFilter;
		private System.Windows.Forms.TextBox tbViewFilter;
	}
}

