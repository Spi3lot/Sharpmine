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
        listBoxClients = new System.Windows.Forms.ListBox();
        listBoxLogs = new System.Windows.Forms.ListBox();
        checkBoxShowGlobal = new System.Windows.Forms.CheckBox();
        SuspendLayout();

        // 
        // listBoxClients
        // 
        listBoxClients.FormattingEnabled = true;
        listBoxClients.HorizontalScrollbar = true;
        listBoxClients.Location = new System.Drawing.Point(12, 17);
        listBoxClients.Name = "listBoxClients";
        listBoxClients.Size = new System.Drawing.Size(244, 409);
        listBoxClients.TabIndex = 2;
        listBoxClients.SelectedIndexChanged += ListBoxClients_SelectedIndexChanged;

        // 
        // listBoxLogs
        // 
        listBoxLogs.FormattingEnabled = true;
        listBoxLogs.HorizontalScrollbar = true;
        listBoxLogs.Items.AddRange(new object[] { "" });
        listBoxLogs.Location = new System.Drawing.Point(262, 17);
        listBoxLogs.Name = "listBoxLogs";
        listBoxLogs.Size = new System.Drawing.Size(704, 409);
        listBoxLogs.TabIndex = 3;

        // 
        // checkBoxShowGlobal
        // 
        checkBoxShowGlobal.Location = new System.Drawing.Point(12, 432);
        checkBoxShowGlobal.Name = "checkBoxShowGlobal";
        checkBoxShowGlobal.Size = new System.Drawing.Size(95, 20);
        checkBoxShowGlobal.TabIndex = 4;
        checkBoxShowGlobal.Text = "Global Logs";
        checkBoxShowGlobal.UseVisualStyleBackColor = true;

        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(978, 450);
        Controls.Add(listBoxLogs);
        Controls.Add(listBoxClients);
        Location = new System.Drawing.Point(15, 15);
        ResumeLayout(false);
    }

    private System.Windows.Forms.CheckBox checkBoxShowGlobal;

    private System.Windows.Forms.ListBox listBoxLogs;

    private System.Windows.Forms.ListBox listBoxClients;

    #endregion

}