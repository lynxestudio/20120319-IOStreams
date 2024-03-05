using System;
using Gtk;
using System.IO;
using System.Text;

public partial class MainWindow: Gtk.Window
{	
FileInfo file;
public MainWindow (): base (Gtk.WindowType.Toplevel)
{
	Build ();
}

protected void OnDeleteEvent (object sender, DeleteEventArgs a)
{
	Application.Quit ();
	a.RetVal = true;
}

protected void OnBtnOpenClicked (object sender, System.EventArgs e)
{
using(FileChooserDialog fc = new FileChooserDialog("Choose file to open",
		                             this,
		                             FileChooserAction.Open,
                                     "Cancel",ResponseType.Cancel,
                                     "Open",ResponseType.Accept))
{
if(fc.Run() == (int)ResponseType.Accept)
{
file = new FileInfo(fc.Filename);
if(file.Extension.Equals(".mp3"))
{		
	txtFileName.Text = file.FullName;
	try{
		ReadFile();
	}catch(IOException ioex){
	PutError(ioex.Message);
	}
}
else
	PutError("It's not a mp3 file");
fc.Destroy();
}else
if(fc.Run() == (int)ResponseType.Cancel)
fc.Destroy();
}
}

void ReadFile(){
byte[] b;
int size = Convert.ToInt32(file.Length);
lbFileName.Text = file.Name;
lbWriteTime.Text = file.LastWriteTime.ToString();
lbFileSize.Text = size.ToString();
FileStream fis = new FileStream(file.FullName,FileMode.Open,
		                       FileAccess.Read,FileShare.Read);
using(BinaryReader reader = new BinaryReader(fis))
{
int offset = size - 128;
reader.BaseStream.Position = offset;
b = reader.ReadBytes(128);
}
char[] c = new char[128];
for(int i = 0;i < b.Length;i++)
	c[i] = (char)b[i];
string strTag = new string(c,0,3);
if(strTag.Equals("TAG")){
	PutMsg("File loaded");
txtTitle.Text = new string(c,3,30);
txtArtist.Text = new string(c,33,30);
txtAlbum.Text = new string(c,63,30);
txtYear.Text = new string(c,93,4);
txtComments.Buffer.Text = new string(c,97,30);
}
else
	PutError("No ID3 tag to read");
}

void PutError(string error){
	lbMsg.Text = string.Empty;
	lbError.Text = error;
}

void PutMsg(string msg){
	lbError.Text = string.Empty;
	lbMsg.Text = msg;
}
}
