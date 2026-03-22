namespace Sharpmine.Server.Gui;

partial class Form1
{

    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        checkBoxShowGlobal = new System.Windows.Forms.CheckBox();
        listBoxClients = new System.Windows.Forms.ListBox();
        listBoxLogs = new System.Windows.Forms.ListBox();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();

        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.687117F));
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.31288F));
        tableLayoutPanel1.Controls.Add(checkBoxShowGlobal, 1, 0);
        tableLayoutPanel1.Controls.Add(listBoxClients, 0, 1);
        tableLayoutPanel1.Controls.Add(listBoxLogs, 1, 1);
        tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 423F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
        tableLayoutPanel1.Size = new System.Drawing.Size(978, 450);
        tableLayoutPanel1.TabIndex = 4;

        // 
        // checkBoxShowGlobal
        // 
        checkBoxShowGlobal.AutoSize = true;
        checkBoxShowGlobal.Dock = System.Windows.Forms.DockStyle.Fill;
        checkBoxShowGlobal.Location = new System.Drawing.Point(264, 3);
        checkBoxShowGlobal.Name = "checkBoxShowGlobal";
        checkBoxShowGlobal.Size = new System.Drawing.Size(711, 21);
        checkBoxShowGlobal.TabIndex = 4;
        checkBoxShowGlobal.Text = "Global Logs";
        checkBoxShowGlobal.UseVisualStyleBackColor = true;

        // 
        // listBoxClients
        // 
        listBoxClients.Dock = System.Windows.Forms.DockStyle.Fill;
        listBoxClients.FormattingEnabled = true;
        listBoxClients.HorizontalScrollbar = true;
        listBoxClients.Location = new System.Drawing.Point(3, 30);
        listBoxClients.Name = "listBoxClients";
        listBoxClients.Size = new System.Drawing.Size(255, 417);
        listBoxClients.TabIndex = 2;
        listBoxClients.SelectedIndexChanged += ListBoxClients_SelectedIndexChanged;

        // 
        // listBoxLogs
        // 
        listBoxLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        listBoxLogs.FormattingEnabled = true;
        listBoxLogs.HorizontalScrollbar = true;
        listBoxLogs.Items.AddRange(new object[] { "", "" });
        listBoxLogs.Location = new System.Drawing.Point(264, 30);
        listBoxLogs.Name = "listBoxLogs";
        listBoxLogs.Size = new System.Drawing.Size(711, 417);
        listBoxLogs.TabIndex = 3;

        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.Control;
        ClientSize = new System.Drawing.Size(978, 450);
        Controls.Add(tableLayoutPanel1);
        Location = new System.Drawing.Point(15, 15);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    private System.Windows.Forms.CheckBox checkBoxShowGlobal;

    private System.Windows.Forms.ListBox listBoxLogs;

    private System.Windows.Forms.ListBox listBoxClients;

    #endregion

}