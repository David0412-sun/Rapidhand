using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using SolidWorks.Interop.sldworks;

internal static class ListSolidWorksInstances
{
    [DllImport("ole32.dll")]
    private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

    [DllImport("ole32.dll")]
    private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

    [STAThread]
    private static int Main()
    {
        IRunningObjectTable rot;
        IBindCtx ctx;
        GetRunningObjectTable(0, out rot);
        CreateBindCtx(0, out ctx);
        IEnumMoniker enumerator;
        rot.EnumRunning(out enumerator);
        enumerator.Reset();
        IMoniker[] monikers = new IMoniker[1];
        int index = 0;
        IntPtr fetched = Marshal.AllocCoTaskMem(sizeof(int));
        try
        {
            while (enumerator.Next(1, monikers, fetched) == 0)
            {
                string displayName = "";
                try { monikers[0].GetDisplayName(ctx, null, out displayName); } catch { }
                object value = null;
                try { rot.GetObject(monikers[0], out value); } catch { }
                SldWorks sw = value as SldWorks;
                if (sw != null)
                {
                    ModelDoc2 doc = null;
                    try { doc = (ModelDoc2)sw.ActiveDoc; } catch { }
                    Console.WriteLine("INDEX=" + index);
                    Console.WriteLine("ROT=" + displayName);
                    Console.WriteLine("TITLE=" + (doc == null ? "" : doc.GetTitle()));
                    Console.WriteLine("PATH=" + (doc == null ? "" : doc.GetPathName()));
                    Console.WriteLine("TYPE=" + (doc == null ? 0 : doc.GetType()));
                    Console.WriteLine("VISIBLE=" + sw.Visible);
                    Console.WriteLine("---");
                    index++;
                }
                if (value != null && Marshal.IsComObject(value)) Marshal.ReleaseComObject(value);
                Marshal.ReleaseComObject(monikers[0]);
            }
        }
        finally
        {
            Marshal.FreeCoTaskMem(fetched);
            Marshal.ReleaseComObject(enumerator);
            Marshal.ReleaseComObject(ctx);
            Marshal.ReleaseComObject(rot);
        }
        Console.WriteLine("COUNT=" + index);
        return 0;
    }
}
