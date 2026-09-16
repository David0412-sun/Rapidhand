using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

internal static class ConfigureSolidWorksLighting
{
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            bool apply = args.Length > 0 && args[0].Equals("--apply", StringComparison.OrdinalIgnoreCase);
            string output = args.Length > 1 ? Path.GetFullPath(args[1]) : null;
            string target = args.Length > 2 ? Path.GetFullPath(args[2]) : null;
            SldWorks sw = (SldWorks)Marshal.GetActiveObject("SldWorks.Application.32");
            ModelDoc2 doc;
            if (!string.IsNullOrEmpty(target))
            {
                int openErrors = 0, openWarnings = 0;
                doc = (ModelDoc2)sw.OpenDoc6(target, (int)swDocumentTypes_e.swDocASSEMBLY,
                    (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref openErrors, ref openWarnings);
                if (doc == null) throw new InvalidOperationException("Could not open target assembly; error=" + openErrors);
                int activationErrors = 0;
                sw.ActivateDoc3(doc.GetTitle(), true, (int)swRebuildOnActivation_e.swDontRebuildActiveDoc, ref activationErrors);
                Console.WriteLine("OPEN_WARNINGS=" + openWarnings + " ACTIVATION_ERRORS=" + activationErrors);
            }
            else
            {
                doc = (ModelDoc2)sw.ActiveDoc;
            }
            if (doc == null) throw new InvalidOperationException("No active SolidWorks document.");
            ModelView view = (ModelView)doc.ActiveView;

            PrintState(sw, doc, view, "BEFORE");
            if (!apply) return 0;

            // Flat technical-illustration display, matching the reference BOM style.
            sw.SetUserPreferenceIntegerValue(
                (int)swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance,
                (int)swColorsBackgroundAppearance_e.swColorsBackgroundAppearance_Plain);
            int white = ColorTranslator.ToWin32(Color.White);
            sw.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swSystemColorsBackground, white);
            sw.SetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swSystemColorsViewportBackground, white);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swColorsGradientPartBackground, false);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayShadowsInShadedMode, false);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swLargeAsmModeShadowsShadedMode, false);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAmbientOcclusionShadows, false);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDraftQualityAmbientOcclusion, false);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swEdgesAntiAlias, true);
            sw.SetUserPreferenceToggle((int)swUserPreferenceToggle_e.swColorsUseShadedEdgeColor, false);
            sw.SetUserPreferenceIntegerValue(
                (int)swUserPreferenceIntegerValue_e.swSystemColorsShadedEdge,
                ColorTranslator.ToWin32(Color.Black));

            doc.Extension.ViewDisplayRealView = false;
            view.RemovePerspective();
            view.DisplayMode = (int)swDisplayMode_e.swSHADED_EDGES;
            doc.GraphicsRedraw2();
            doc.ViewZoomtofit2();
            System.Threading.Thread.Sleep(1200);

            if (!string.IsNullOrEmpty(output))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                int result = doc.SaveAs3(output, (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                    (int)swSaveAsOptions_e.swSaveAsOptions_Silent);
                Console.WriteLine("SCREENSHOT_RESULT=" + result);
                Console.WriteLine("SCREENSHOT=" + output);
            }

            int saveErrors = 0, saveWarnings = 0;
            bool saved = doc.Save3((int)swSaveAsOptions_e.swSaveAsOptions_Silent, ref saveErrors, ref saveWarnings);
            Console.WriteLine("MODEL_SAVED=" + saved + " ERRORS=" + saveErrors + " WARNINGS=" + saveWarnings);
            PrintState(sw, doc, view, "AFTER");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.GetType().FullName);
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
            return 1;
        }
    }

    private static void PrintState(SldWorks sw, ModelDoc2 doc, ModelView view, string label)
    {
        Console.WriteLine("[" + label + "]");
        Console.WriteLine("TITLE=" + doc.GetTitle());
        Console.WriteLine("PATH=" + doc.GetPathName());
        Console.WriteLine("TYPE=" + doc.GetType());
        Console.WriteLine("SCENE=" + (doc.SceneName ?? ""));
        Console.WriteLine("DISPLAY_MODE=" + view.DisplayMode);
        Console.WriteLine("PERSPECTIVE=" + view.HasPerspective());
        Console.WriteLine("REALVIEW=" + doc.Extension.ViewDisplayRealView);
        Console.WriteLine("BG_APPEARANCE=" + sw.GetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swColorsBackgroundAppearance));
        Console.WriteLine("BG_COLOR=" + sw.GetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swSystemColorsBackground));
        Console.WriteLine("VIEWPORT_COLOR=" + sw.GetUserPreferenceIntegerValue((int)swUserPreferenceIntegerValue_e.swSystemColorsViewportBackground));
        Console.WriteLine("SHADOWS=" + sw.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayShadowsInShadedMode));
        Console.WriteLine("AMBIENT_OCCLUSION=" + sw.GetUserPreferenceToggle((int)swUserPreferenceToggle_e.swDisplayAmbientOcclusionShadows));

        int count = doc.GetLightSourceCount();
        Console.WriteLine("LIGHT_COUNT=" + count);
        for (int i = 0; i < count; i++)
        {
            string name = doc.GetLightSourceName(i);
            Console.WriteLine("LIGHT_" + i + "=" + name);
        }
    }
}
