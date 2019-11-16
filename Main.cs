//--------------------------------------------------
// Date         Name                Description
// 2019.08.01   y-strelitzia        Initial
//--------------------------------------------------

using System;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

using YellowStrelitzia.CSTools;

//--------------------------------------------------
/// <summary>
/// Main classs, template of Build and Run On demand C# executer
/// </summary>
/// <remarks>
/// Main classs, template of Build and Run On demand C# executer
/// </remarks>
//--------------------------------------------------
class MainClass : BuildAndRunTemplate
{
  //--------------------------------------------------
  /// <summary>
  /// Entry point of Bootstrap application
  /// </summary>
  /// <param name="args">
  /// commandline paramteters
  /// </param>
  /// <remarks>
  /// Entry point of Bootstrap application
  /// </remarks>
  //--------------------------------------------------            
  [STAThread]
  public static void Main(string[] args)
  {
     MainClass main = new MainClass();
     main.startup( args );
  }

  //--------------------------------------------------
  /// <summary>
  /// Constructor
  /// </summary>
  /// <remarks>
  /// Constructor
  /// </remarks>
  //--------------------------------------------------	
  public MainClass()
  {
    _isGUI = false;
  }

  //--------------------------------------------------
  /// <summary>
  /// Entry point of CUI code
  /// </summary>
  /// <remarks>
  /// Entry point of CUI code
  /// </remarks>
  //--------------------------------------------------		
  public override void startCUI()
  {
    System.Console.WriteLine("start sample");

    /* sample code, but can't run with .net 2.0 , so commented out
    Func<string, bool> handleFiles = (string path) => {
      Console.WriteLine(path);
      return true;
    };

    IEnumerable<string> results = 
      BuildAndRunUtil.ProcessRecurcively("c:\\", "*.txt", handleFiles);
    */

    System.Console.WriteLine("finish sample");
  }
    
  //--------------------------------------------------
  /// <summary>
  /// Entry point of GUI code
  /// </summary>
  /// <remarks>
  /// Entry point of GUI code
  /// </remarks>
  //--------------------------------------------------	
  public override void startGUI()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new MainWindow());
  }

  //--------------------------------------------------
  /// <summary>
  /// MainWindow classs, template of initial Form
  /// </summary>
  /// <remarks>
  /// MainWindow classs, template of initial Form
  /// </remarks>
  //--------------------------------------------------	
  public class MainWindow : System.Windows.Forms.Form
  {
    public const string WINDOW_CAPTION = "MainWindow";
    public const string LABEL_CAPTION = "Caption";
    public const string BUTTON_CAPTION = "Execute";
    private System.Windows.Forms.FlowLayoutPanel panelText;
    private System.Windows.Forms.FlowLayoutPanel panelButtons;
    private System.Windows.Forms.Button buttonExecute;
    private System.Windows.Forms.Label labelCaption;
    private System.Windows.Forms.TextBox textInput;
      
    public MainWindow()
    {
      InitializeComponent();
      CenterToScreen();
    }
  
    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        // code for dispose resources
        ;
      }
      base.Dispose( disposing );
    }
  
    private void InitializeComponent()
    {
      SuspendLayout();
              
      this.textInput = new System.Windows.Forms.TextBox();
      this.buttonExecute = new System.Windows.Forms.Button();
      this.panelText = new System.Windows.Forms.FlowLayoutPanel();
      this.labelCaption = new System.Windows.Forms.Label();
      this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
      this.panelText.SuspendLayout();
      this.panelButtons.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelCaption
      // 
      this.labelCaption.Location = new System.Drawing.Point(3, 0);
      this.labelCaption.Name = "labelCaption";
      this.labelCaption.Size = new System.Drawing.Size(200, 23);
      this.labelCaption.TabIndex = 1;
      this.labelCaption.Text = LABEL_CAPTION;
      // 
      // textInput
      // 
      this.textInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.textInput.Location = new System.Drawing.Point(3, 26);
      this.textInput.Name = "textInput";
      this.textInput.Size = new System.Drawing.Size(200, 19);
      this.textInput.TabIndex = 0;  
      // 
      // buttonExecute
      // 
    this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
    this.buttonExecute.Location = new System.Drawing.Point(3, 3);
    this.buttonExecute.Name = "buttonExecute";
    this.buttonExecute.Size = new System.Drawing.Size(120, 23);
    this.buttonExecute.TabIndex = 1;
    this.buttonExecute.Text = BUTTON_CAPTION;
    this.buttonExecute.UseVisualStyleBackColor = true;
    this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
    // 
    // panelText
    // 
    this.panelText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    | System.Windows.Forms.AnchorStyles.Right)));
    this.panelText.Controls.Add(this.labelCaption);
    this.panelText.Controls.Add(this.textInput);
    this.panelText.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
    this.panelText.Location = new System.Drawing.Point(12, 12);
    this.panelText.Name = "panelText";
    this.panelText.Size = new System.Drawing.Size(239, 67);
    this.panelText.TabIndex = 2;
    // 
    // panelButtons
    // 
    this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    | System.Windows.Forms.AnchorStyles.Right)));
    this.panelButtons.Controls.Add(this.buttonExecute);
    this.panelButtons.Location = new System.Drawing.Point(12, 85);
    this.panelButtons.Name = "panelButtons";
    this.panelButtons.Size = new System.Drawing.Size(239, 42);
    this.panelButtons.TabIndex = 3;
            
      this.panelText.Controls.AddRange(new System.Windows.Forms.Control[] {this.labelCaption, this.textInput });
        
      this.panelButtons.Controls.AddRange(new System.Windows.Forms.Control[] {this.buttonExecute});
      this.Controls.AddRange(new System.Windows.Forms.Control[] {this.panelText, this.panelButtons});
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Text = WINDOW_CAPTION;
      this.ClientSize = new System.Drawing.Size(420, 131);
      ResumeLayout();
    }
  
    private void buttonExecute_Click(object sender, System.EventArgs e)
    {
      MessageBox.Show("msg", "caption");
    }
  }
}
