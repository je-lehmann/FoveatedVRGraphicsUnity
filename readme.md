## A Foveated Renderer for VR HMDs in Unity

https://github.com/user-attachments/assets/c04b08e1-aa18-4547-a63b-c25b99d5a2ae

This repository contains a demonstration of how real-time eye tracking can be used to improve rendering time for real-time virtual reality applications. A Pupil Labs eye tracker was integrated into an HTC Vive HMD. The eye positions are used to render the scene at different resolution levels and are inspired by the paper "Foveated 3D Graphics". The provided video includes demonstrations of the Unity viking scene and the Pupil Labs calibration scene. Note, that used free Assets and Pupil eye tracking software is not included in this repository and needs to be setup manually. The tested Unity Version is 2019.3.7f1.

## Acknowledgement
Eye-Tracking is based on:
[hmd-eyes](https://github.com/pupil-labs/hmd-eyes) and  [pupil](https://github.com/pupil-labs/pupil)

Foveation is loosely based on:
[Foveated 3D Graphics](https://www.microsoft.com/en-us/research/wp-content/uploads/2012/11/foveated_final15.pdf)

## Instructions

Foveation Scripts are located under: Assets/Plugins/Foveation <br />
Eye Tracking tested with Pupil Service v. 2.4 <br />

Tab - Switch Scenes <br />
F1 - Foveation Mode: Mouse 	 -> Foveation can be controlled with a Mouse, useful when no HMD is connected <br />
F2 - Foveation Mode: EyeTracking -> Foveation can be controlled via Eye Tracking: Pupil Service need to 
				    be set up and run in Background (https://github.com/pupil-labs/pupil), Eye Tracker 
				    needs to be connected and will be calibrated in game mode <br />
F3 - Foveation Mode: Fixed 	 -> Foveation Fixed to Screen Center, Game starts in this Mode <br />
F4 - Foveation Mode: Butterfly   -> Simulated Eye Tracking, Focus will always be on the Butterfly.  <br />
F5 - Toggle Contrast		 -> Toogle simple Contrast Preservation, do increase Contrast on undersampled Edges <br />
F6 - Toggle Blur		 -> Toogle simple Gaussian blur in the periphery to reduce Aliasing Artifacts <br />

F11 Activate Foveation <br />
F12 Deactivate Foveation <br />

ESC - Close Application <br />

----------------------------------------------------------

Peripherical Rendertexture Control: <br />

J decrease peripherical Resolution Reduction <br />
K increase peripherical Resolution Reduction <br />
Range: 1f, 16f <br />
2f represents half of the possible Rendertexture Resolution

----------------------------------------------------------

Foveal Rendertexture Control:

N decrease foveal Area Size Reduction <br />
M increase foveal Area Size Reduction <br />
Range: 2f, 8f <br />
2f represents half of possible Rendertexture Resolution.
The size will adjust to match the best possible resolution
in a decreasing screen area

----------------------------------------------------------

Scenes:

0 - Minimal Scene: minimal Geometry <br />
1 - LOD Scene: complex Geometry with different level of detail in the periphery <br />
2 - Viking Scene: complex Geometry, Particle Effects, Screenspace Reflections, Ambient Occlusion, Ambient Obscurance <br />

SteamVR required <br />

![FR_demo_viking](https://github.com/je-lehmann/FoveatedVRGraphics/assets/49212649/d2556186-a281-43f2-9f95-45d547551d7e)
