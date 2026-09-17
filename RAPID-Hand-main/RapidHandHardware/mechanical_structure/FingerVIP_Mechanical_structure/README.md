# FingerVIP Mechanical Structure Assembly Guide

This directory contains the mechanical models, printable shells, and bill of materials for the **FingerVIP camera-equipped finger and thumb modules**. Each module integrates a SONY IMX258 camera with a DYNAMIXEL XC330-M288-T motor.

FingerVIP focuses on camera-equipped fingertips. The remaining joints, palm, and hand integration follow [RAPID Hand](https://github.com/SYSU-RoboticsLab/RAPID-Hand), cited below.

## 📁 Directory Structure

```text
FingerVIP_Mechanical_structure/
├── README.md                           # This guide
├── FingerVIP_BOM_assembly_guide.pdf      # Finger, thumb, and total BOM; labeled views
├── 3D_Printing_Parts/
│   ├── Finger/
│   │   ├── F1.STL                      # Right camera-finger shell
│   │   └── F2.STL                      # Left camera-finger shell
│   └── Thumb/
│       ├── T1.STL                      # Left camera-thumb shell
│       └── T2.STL                      # Right camera-thumb shell
├── Model/
│   ├── Finger/
│   │   └── Finger.STEP                 # Camera-finger assembly
│   └── Thumb/
│       └── Thumb.STEP                  # Camera-thumb assembly
└── FingerVIP_Description/               # Currently empty
```

The finger idler pin **P1** is a RAPID Hand part; its STL is not included in this package.

## 🧩 Bill of Materials (BOM)

Refer to [FingerVIP_BOM_assembly_guide.pdf](FingerVIP_BOM_assembly_guide.pdf) for part identifiers, screw locations, exploded views, and assembled views. The quantities below match that document. Screw dimensions are in millimeters.

### 1. Camera Finger Module (×4)

Quantities are **per camera finger module**, not per complete articulated finger.

| Category | Name / Model | Quantity |
| --- | --- | ---: |
| Electronic Devices | Motor — DYNAMIXEL XC330-M288-T | 1 |
| Electronic Devices | Camera — SONY IMX258 | 1 |
| 3D Printed Parts | F1 — Right shell | 1 |
| 3D Printed Parts | F2 — Left shell | 1 |
| 3D Printed Parts | P1 — Idler pin (RAPID Hand part) | 1 |
| Mechanical Standard Parts | Screw M2×4 | 8 |
| Mechanical Standard Parts | Screw M2.5×12 | 2 |
| Mechanical Standard Parts | Screw M3×6 | 1 |

### 2. Camera Thumb Module (×1)

Quantities are **per camera thumb module**, not per complete articulated thumb.

| Category | Name / Model | Quantity |
| --- | --- | ---: |
| Electronic Devices | Motor — DYNAMIXEL XC330-M288-T | 1 |
| Electronic Devices | Camera — SONY IMX258 | 1 |
| 3D Printed Parts | T1 — Left shell | 1 |
| 3D Printed Parts | T2 — Right shell | 1 |
| Mechanical Standard Parts | Screw M2×4 | 4 |
| Mechanical Standard Parts | Screw M2×10 | 4 |
| Mechanical Standard Parts | Screw M2.5×12 | 2 |

### 3. Total BOM — FingerVIP Camera Modules Only

Quantity basis: **4 camera finger modules + 1 camera thumb module**.

| Category | Name / Model | Total Quantity |
| --- | --- | ---: |
| Electronic Devices | Motor — DYNAMIXEL XC330-M288-T | 5 |
| Electronic Devices | Camera — SONY IMX258 | 5 |
| 3D Printed Parts | F1 — Right shell | 4 |
| 3D Printed Parts | F2 — Left shell | 4 |
| 3D Printed Parts | P1 — Idler pin | 4 |
| 3D Printed Parts | T1 — Left shell | 1 |
| 3D Printed Parts | T2 — Right shell | 1 |
| Mechanical Standard Parts | Screw M2×4 | 36 |
| Mechanical Standard Parts | Screw M2×10 | 4 |
| Mechanical Standard Parts | Screw M2.5×12 | 10 |
| Mechanical Standard Parts | Screw M3×6 | 4 |

This BOM covers only the camera modules. Use the RAPID Hand BOM for the rest of the hand, without double-counting the replaced fingertip assemblies.

## 🔧 Assembly Instructions

Use the labeled PDF views to identify part orientation and fastener placement. The steps below cover the FingerVIP-specific modules; the rest of the hand follows RAPID Hand.

### 1. Prepare the Parts

1. Print F1 and F2 for each camera finger, and T1 and T2 for the camera thumb. Prepare one P1 idler pin per finger.
2. Remove printing supports and check the fit of the shell mating surfaces and the alignment of the camera and motor mounting holes.
3. Check that the actual IMX258 camera module, including its board and ribbon cable, fits the supplied geometry before final fastening.
4. Configure the motors using the RAPID Hand procedure and joint-ID assignments.

### 2. Assemble the Camera Finger Module

1. Identify **F1 (right shell)**, **F2 (left shell)**, and **P1 (idler pin)** using the finger exploded view.
2. Position the motor between the shells and align the motor-side and idler-side mounting features as shown in the finger exploded view. Install P1 with the **M3×6 screw** at the indicated location.
3. Fit the camera assembly into its illustrated position. Route the ribbon cable without trapping it between shell faces or fasteners, and keep the camera viewing opening unobstructed.
4. Align and join the shells using **two M2.5×12 screws**. Use **four M2×4 screws** at the motor-side mounting locations and **four M2×4 screws** at the camera mounting locations, following the PDF callouts. Check alignment before final tightening.
5. Check the assembled module against the assembled view. Verify that the cable is not under tension and does not interfere with joint movement.
6. Repeat for the four camera finger modules.

### 3. Assemble the Camera Thumb Module

1. Identify **T1 (left shell)** and **T2 (right shell)** using the thumb exploded view. Do not substitute the finger shells.
2. Position the motor and camera as shown in the thumb exploded view. Route the camera ribbon cable clear of the shell mating surfaces and screw holes.
3. Align and join the shells using **two M2.5×12 screws**. Secure the motor-side mounting locations with **four M2×10 screws** and the camera mounting locations with **four M2×4 screws**, following the PDF callouts.
4. Check the result against the thumb assembled view. Unlike the camera finger module, this module does **not** use P1 or an M3×6 screw.

### 4. Integrate with the RAPID Hand Structure

Follow the [RAPID Hand project](https://github.com/SYSU-RoboticsLab/RAPID-Hand) for the remaining hand assembly:

- Assemble the remaining joints, gears, bearings, and linkages using the RAPID Hand instructions.
- Replace the fingertip assemblies with the FingerVIP camera modules.
- Assemble the palm, attach the finger and thumb chains, and complete wiring and motor setup as specified by RAPID Hand.
- Before powered motion, check screw seating, joint clearance, and cable slack throughout the intended range of motion. Verify each camera image before final cable securing.

## Notes & Precautions

- Match screw lengths to the labeled views; the finger and thumb use different motor-side fasteners.
- Avoid overtightening screws in printed shells or against the camera board.
- Do not pinch, sharply crease, or pull the camera ribbon cable during assembly.
- Confirm mechanical fit before powering the motors. Camera electrical connection and software setup are outside the scope of this mechanical guide.

## Citation

For the hand platform and assembly procedure, cite:

Zhaoliang Wan et al. **RAPID Hand: A Robust, Affordable, Perception-Integrated, Dexterous Manipulation Platform for Generalist Robot Autonomy.** 2025. [Paper](https://arxiv.org/abs/2506.07490) · [Project](https://rapid-hand.github.io/) · [Source repository](https://github.com/SYSU-RoboticsLab/RAPID-Hand).

```bibtex
@article{wan2025rapid,
  title={RAPID Hand: A Robust, Affordable, Perception-Integrated, Dexterous Manipulation Platform for Generalist Robot Autonomy},
  author={Wan, Zhaoliang and Bi, Zetong and Zhou, Zida and Ren, Hao and Zeng, Yiming and Li, Yihan and Qi, Lu and Yang, Xu and Yang, Ming-Hsuan and Cheng, Hui},
  journal={arXiv preprint arXiv:2506.07490},
  year={2025}
}
```
