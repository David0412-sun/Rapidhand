Option Explicit

Dim workspace, outDir, fso, sw, report, folderKeys, i
workspace = WScript.Arguments(0)
Set fso = CreateObject("Scripting.FileSystemObject")
outDir = fso.BuildPath(workspace, "tmp\solidworks")
If Not fso.FolderExists(outDir) Then fso.CreateFolder(outDir)

Set report = fso.OpenTextFile(fso.BuildPath(outDir, "inspection.tsv"), 2, True, -1)
report.WriteLine "assembly" & vbTab & "instance" & vbTab & "file" & vbTab & "path" & vbTab & "suppressed" & vbTab & "configuration"

Set sw = CreateObject("SldWorks.Application.32")
sw.Visible = True

folderKeys = Array("finger_with_cam", "thumb_with_cam")
For i = 0 To UBound(folderKeys)
    InspectAssembly folderKeys(i)
Next

report.Close
sw.ExitApp
WScript.Echo fso.BuildPath(outDir, "inspection.tsv")

Sub InspectAssembly(key)
    Dim folderPath, folder, file, asmPath, openErrors, openWarnings, doc, components
    Dim component, componentPath, fileName, ok, saveErrors, saveWarnings, activeErrors, j
    folderPath = fso.BuildPath(workspace, "RAPID-Hand-main\RapidHandHardware\mechanical_structure\rss_design\" & key)
    Set folder = fso.GetFolder(folderPath)
    asmPath = ""
    For Each file In folder.Files
        If LCase(fso.GetExtensionName(file.Name)) = "sldasm" And Left(file.Name, 2) <> "~$" Then
            asmPath = file.Path
            Exit For
        End If
    Next
    If asmPath = "" Then Err.Raise vbObjectError + 1000, , "No SLDASM found in " & folderPath

    openErrors = CLng(0)
    openWarnings = CLng(0)
    Set doc = sw.OpenDoc(asmPath, 2)
    If doc Is Nothing Then Err.Raise vbObjectError + 1001, , "OpenDoc failed: " & asmPath
    sw.ActivateDoc doc.GetTitle

    components = doc.GetComponents(False)
    For j = 0 To UBound(components)
        If IsObject(components(j)) Then
            Set component = components(j)
            componentPath = component.GetPathName
            fileName = ""
            If componentPath <> "" Then fileName = fso.GetFileName(componentPath)
            report.WriteLine key & vbTab & component.Name2 & vbTab & fileName & vbTab & componentPath & vbTab & CStr(component.IsSuppressed) & vbTab & component.ReferencedConfiguration
        Else
            report.WriteLine key & vbTab & "NON_OBJECT_COMPONENT" & vbTab & CStr(VarType(components(j)))
        End If
    Next

    SaveView doc, "*Isometric", 7, fso.BuildPath(outDir, key & "_isometric.png")
    SaveView doc, "*Front", 1, fso.BuildPath(outDir, key & "_front.png")
    SaveView doc, "*Right", 3, fso.BuildPath(outDir, key & "_right.png")
    sw.CloseDoc doc.GetTitle
End Sub

Sub SaveView(doc, viewName, viewId, outputPath)
    Dim ok, saveErrors, saveWarnings
    doc.ShowNamedView2 viewName, viewId
    doc.ViewZoomtofit2
    WScript.Sleep 800
    saveErrors = CLng(0)
    saveWarnings = CLng(0)
    ok = doc.SaveAs(outputPath)
    If Not ok Then report.WriteLine "VIEW_ERROR" & vbTab & outputPath
End Sub
