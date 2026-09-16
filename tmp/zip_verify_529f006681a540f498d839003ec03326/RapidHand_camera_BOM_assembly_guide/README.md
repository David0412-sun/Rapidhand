# RAPID-Hand camera fingertip BOM and assembly guide

This folder is a self-contained LaTeX source package for the Chinese BOM and assembly guide covering:

- `rss_design/finger_with_cam`
- `rss_design/thumb_with_cam`

## Build

Use XeLaTeX (TeX Live 2023+ or MiKTeX):

```powershell
xelatex -interaction=nonstopmode -halt-on-error main.tex
xelatex -interaction=nonstopmode -halt-on-error main.tex
```

Or use Tectonic:

```powershell
tectonic main.tex
```

The second XeLaTeX pass resolves the total page count. The source uses the `ctexart` class and standard packages available in a normal full TeX Live/MiKTeX installation.

## Contents

- `main.tex`: document source
- `figures/*.png`: screenshots exported directly from SolidWorks 2024
- `RapidHand_camera_BOM_assembly_guide.pdf`: verified build output

The BOM reflects the top-level components in the active `默认` configuration. The thumb assembly does not contain fastener component instances; the guide deliberately flags this instead of guessing their specifications.
