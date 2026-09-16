Option Explicit

Dim workspace, fso, inputPath, outputPath, htmlPath, word, doc
workspace = WScript.Arguments(0)
Set fso = CreateObject("Scripting.FileSystemObject")
inputPath = fso.BuildPath(workspace, "RAPID-Hand-main\RapidHandHardware\mechanical_structure\BOM_and_Assembly_Guide.pdf")
outputPath = fso.BuildPath(workspace, "tmp\reference_guide.txt")
htmlPath = fso.BuildPath(workspace, "tmp\reference_guide.html")

Set word = CreateObject("Word.Application")
word.Visible = False
word.DisplayAlerts = 0
Set doc = word.Documents.Open(inputPath, False, True, False)
WScript.Echo "pages=" & CStr(doc.ComputeStatistics(2))
doc.SaveAs2 outputPath, 7
doc.SaveAs2 htmlPath, 10
doc.Close False
word.Quit
WScript.Echo outputPath
