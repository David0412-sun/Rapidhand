using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

internal static class InspectSolidWorks
{
    private static StreamWriter report;

    [STAThread]
    private static int Main(string[] args)
    {
      try {
        if (args.Length != 1) throw new ArgumentException("Expected workspace path.");
        string workspace = Path.GetFullPath(args[0]);
        string outDir = Path.Combine(workspace, "tmp", "solidworks");
        Directory.CreateDirectory(outDir);
        report = new StreamWriter(Path.Combine(outDir, "inspection.tsv"), false, new UnicodeEncoding(false, false));
        report.WriteLine("assembly\tinstance\tfile\tpath\tsuppressed\tconfiguration");

        Type appType = Type.GetTypeFromProgID("SldWorks.Application.32", true);
        SldWorks sw = (SldWorks)Activator.CreateInstance(appType);
        sw.Visible = true;
        try
        {
            Inspect(sw, workspace, outDir, "finger_with_cam");
            Inspect(sw, workspace, outDir, "thumb_with_cam");
        }
        finally
        {
            report.Dispose();
            sw.ExitApp();
            Marshal.FinalReleaseComObject(sw);
        }
        Console.WriteLine(Path.Combine(outDir, "inspection.tsv"));
        return 0;
      } catch (Exception ex) {
        Console.Error.WriteLine(ex.GetType().FullName);
        Console.Error.WriteLine(ex.Message);
        Console.Error.WriteLine(ex.StackTrace);
        return 1;
      }
    }

    private static void Inspect(SldWorks sw, string workspace, string outDir, string key)
    {
        Console.WriteLine("Opening " + key);
        string folder = Path.Combine(workspace, "RAPID-Hand-main", "RapidHandHardware", "mechanical_structure", "rss_design", key);
        string asmPath = Directory.EnumerateFiles(folder, "*.SLDASM").First(p => !Path.GetFileName(p).StartsWith("~$"));
        int errors = 0, warnings = 0;
        ModelDoc2 doc = (ModelDoc2)sw.OpenDoc6(asmPath, (int)swDocumentTypes_e.swDocASSEMBLY,
            (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref errors, ref warnings);
        if (doc == null) throw new InvalidOperationException("Could not open " + asmPath + "; error=" + errors);
        Console.WriteLine("Opened " + key);
        try
        {
            sw.ActivateDoc3(doc.GetTitle(), true, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref errors);
            AssemblyDoc assembly = (AssemblyDoc)doc;
            object[] components = (object[])assembly.GetComponents(false);
            Console.WriteLine("Components " + key + ": " + components.Length);
            foreach (object value in components)
            {
                Component2 c = (Component2)value;
                string path = c.GetPathName() ?? "";
                report.WriteLine(string.Join("\t", new[] {
                    key, Clean(c.Name2), Clean(Path.GetFileName(path)), Clean(path),
                    c.IsSuppressed().ToString(), Clean(c.ReferencedConfiguration)
                }));
            }
            report.Flush();

            SaveView(doc, "*Isometric", (int)swStandardViews_e.swIsometricView, Path.Combine(outDir, key + "_isometric.png"));
            SaveView(doc, "*Front", (int)swStandardViews_e.swFrontView, Path.Combine(outDir, key + "_front.png"));
            SaveView(doc, "*Right", (int)swStandardViews_e.swRightView, Path.Combine(outDir, key + "_right.png"));
        }
        finally
        {
            sw.CloseDoc(doc.GetTitle());
            Marshal.FinalReleaseComObject(doc);
        }
    }

    private static void SaveView(ModelDoc2 doc, string name, int id, string output)
    {
        doc.ShowNamedView2(name, id);
        doc.ViewZoomtofit2();
        System.Threading.Thread.Sleep(700);
        int result = doc.SaveAs3(output, (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent);
        if (result != 0) report.WriteLine("VIEW_ERROR\t" + output + "\t" + result);
    }

    private static string Clean(string value)
    {
        return (value ?? "").Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");
    }
}
