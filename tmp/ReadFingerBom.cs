using System;
using System.IO;
using System.Runtime.InteropServices;
using SolidWorks.Interop.sldworks;
class ReadFingerBom {
 [STAThread] static void Main(string[] args) {
  var sw=(SldWorks)Marshal.GetActiveObject("SldWorks.Application.32");
  int e=0,w=0;
  var doc=(ModelDoc2)sw.GetOpenDocumentByName(args[0]);
  if(doc==null) doc=(ModelDoc2)sw.OpenDoc6(args[0],2,3,"",ref e,ref w);
  if(doc==null) throw new Exception("Open failed: "+e);
  Console.WriteLine("Configuration: "+doc.ConfigurationManager.ActiveConfiguration.Name);
  foreach(Component2 c in (object[])((AssemblyDoc)doc).GetComponents(true))
   Console.WriteLine(c.Name2+" | "+Path.GetFileName(c.GetPathName())+" | suppressed="+c.IsSuppressed());
 }
}
