using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

public class FormKeyCapture : Form, IMessageFilter {
	private TextBox _tb;
	private StringBuilder _sb;
	
	public FormKeyCapture(){
	  this.SuspendLayout();
	  Console.Clear();
	  _sb = new StringBuilder();
	  _tb = new TextBox();
	  _tb.Multiline = true;
	  _tb.AcceptsReturn = true;
	  _tb.AcceptsTab = true;
	  _tb.ScrollBars = ScrollBars.Vertical;
	  _tb.Dock = DockStyle.Fill;
	  _tb.Font = new Font(_tb.Font.FontFamily, 14);
	  
	  _tb.DoubleClick += DoubleClickHandler;
	  _tb.KeyDown += KeyDownHandler;
	  _tb.KeyUp += KeyUpHandler;
	  _tb.KeyPress += KeyPressHandler;
	  
	  
	  this.Controls.Add(_tb);
	  this.Text = "FormKeyCapture";
	  this.ClientSize = new Size(400, 400);
	  this.StartPosition = FormStartPosition.CenterScreen;
	  this.ResumeLayout(false);
	  this.PerformLayout();
	}
	
	private void DoubleClickHandler(object sender, EventArgs e){
	  Console.Clear();
	}
	
	public void KeyDownHandler(object sender, KeyEventArgs e){
	  Console.WriteLine();
	  Console.WriteLine("*** KeyDown Event **************************");
	}
	
	public void KeyPressHandler(object sender, KeyPressEventArgs e){
	  Console.WriteLine("**** KeyPress Event **************************");
	  Console.WriteLine();
	  // Append keychard to StringBuilder
	  _sb.Append(e.KeyChar);
	  // If character is a period '.', append contents of
	  // StringBuilder to file.
	  if(e.KeyChar == '.'){
		SaveCharsToFile();
	  }
	}
	
	private void SaveCharsToFile(){
	  Console.WriteLine("SaveCharsToFile() method called...");
	  using (StreamWriter writer = new StreamWriter("keys.txt", true))
		{
		  writer.Write(_sb.ToString());
		}
		_sb.Clear();
	}
	
	public void KeyUpHandler(object sender, KeyEventArgs e){
	  Console.WriteLine("**** KeyUp Event **************************");
	  Console.WriteLine();
	}
	
	
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData){
	  if(keyData == (Keys.Control | Keys.S)){ // ctl+s key combo 
	    SaveCharsToFile();
		return true;
	  }
	  return base.ProcessCmdKey(ref msg, keyData);
	}
	
	
	public bool PreFilterMessage(ref Message m){
	  if(m.Msg == 0x100){
		Console.WriteLine();
		Console.WriteLine("---- KeyDown Message ---------------------");
	  }
	  
	  Console.WriteLine(m);
	  
	  if(m.Msg == 0x101){
		Console.WriteLine("---- KeyUp Message ---------------------");
		Console.WriteLine();
	  }
	  return false;
	}
	
	
	[STAThread]
	public static void Main(){
	  Application.EnableVisualStyles(); 
	  Application.SetCompatibleTextRenderingDefault(true);
	  FormKeyCapture fkc = new FormKeyCapture();
	  Application.AddMessageFilter(fkc);
	  Application.Run(fkc);
	}
}