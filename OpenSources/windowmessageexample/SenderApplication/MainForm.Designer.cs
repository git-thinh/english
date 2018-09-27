namespace SenderApplication
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.MainMenu mainMenu1;

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
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.btnSend = new System.Windows.Forms.Button();
      this.btnClickAnotherButton = new System.Windows.Forms.Button();
      this.btnMessageBox = new System.Windows.Forms.Button();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.btnIPC = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btnSend
      // 
      this.btnSend.Location = new System.Drawing.Point(3, 3);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new System.Drawing.Size(234, 20);
      this.btnSend.TabIndex = 0;
      this.btnSend.Text = "Close other app";
      this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
      // 
      // btnClickAnotherButton
      // 
      this.btnClickAnotherButton.Location = new System.Drawing.Point(3, 104);
      this.btnClickAnotherButton.Name = "btnClickAnotherButton";
      this.btnClickAnotherButton.Size = new System.Drawing.Size(234, 20);
      this.btnClickAnotherButton.TabIndex = 1;
      this.btnClickAnotherButton.Text = "Click another button";
      this.btnClickAnotherButton.Click += new System.EventHandler(this.btnClickAnotherButton_Click);
      // 
      // btnMessageBox
      // 
      this.btnMessageBox.Location = new System.Drawing.Point(3, 131);
      this.btnMessageBox.Name = "btnMessageBox";
      this.btnMessageBox.Size = new System.Drawing.Size(234, 20);
      this.btnMessageBox.TabIndex = 2;
      this.btnMessageBox.Text = "Show a Message Box";
      this.btnMessageBox.Click += new System.EventHandler(this.btnMessageBox_Click);
      // 
      // checkBox1
      // 
      this.checkBox1.Location = new System.Drawing.Point(4, 30);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(233, 20);
      this.checkBox1.TabIndex = 3;
      this.checkBox1.Text = "A random checkbox";
      // 
      // btnIPC
      // 
      this.btnIPC.Location = new System.Drawing.Point(3, 56);
      this.btnIPC.Name = "btnIPC";
      this.btnIPC.Size = new System.Drawing.Size(234, 20);
      this.btnIPC.TabIndex = 4;
      this.btnIPC.Text = "Display message in other app";
      this.btnIPC.Click += new System.EventHandler(this.btnIPC_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.AutoScroll = true;
      this.ClientSize = new System.Drawing.Size(240, 268);
      this.Controls.Add(this.btnIPC);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.btnMessageBox);
      this.Controls.Add(this.btnClickAnotherButton);
      this.Controls.Add(this.btnSend);
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Name = "MainForm";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnSend;
    private System.Windows.Forms.Button btnClickAnotherButton;
    private System.Windows.Forms.Button btnMessageBox;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Button btnIPC;
  }
}

