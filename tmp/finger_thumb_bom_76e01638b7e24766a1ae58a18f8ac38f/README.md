# Finger and Thumb BOM

Two-page English BOM: Finger on page 1 and Thumb on page 2, both landscape A4.
Uses the original color exploded and assembled images.
Only the tables use gray fills. Editable LaTeX vector labels identify parts and
fasteners. The views are scaled using the motor body as a visual reference.
Fasteners hidden in the assembled view are labeled in the exploded view.

Build with `xelatex main.tex` or `tectonic main.tex`.
Alternatively, run `build.ps1 -Tectonic path/to/tectonic.exe`.

Quantities were checked against the updated finger assembly: eight M2x4 screws,
two M2.5x12 screws, one M3x6 screw, one motor, one camera, two shells, and one idler pin.
The suppressed older camera instance is excluded. The two physical shell parts are
listed separately; the complete finger BOM is kept on one page.

Thumb quantities: four M2x4 screws, four M2x10 screws, two M2.5x12 screws,
one motor, one camera, one left shell (T1), and one right shell (T2).
Both pages list SONY IMX258 and DYNAMIXEL XC330-M288-T as supplied by the user.
