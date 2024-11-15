# Improving Measurement Techniques Through a Virtual Reality Application for Complex Cardiac Treatment
Virtual reality application focused on improving spatial perception and diagnostic capabilities for the treatment of complex cardiac conditions, particularly congenital heart diseases.


## 🎬 Demos

<p align="center">
  <img src="https://img.youtube.com/vi/U2nSuAB95VU/0.jpg" alt="Thesis demonstration" />
</p>

## 🚀 Features
- **Basic Tools**: Includes essential functions such as moving, rotating, scaling the model, and tagging areas of interest for clinical purposes.
- **Measurement Tools**: Provides advanced techniques for measuring distances, curved paths, volumes, areas, and radius directly on the 3D cardiac model.
- **Clipping Tools**: Enables precise cutting and sectional views of the heart to study internal structures in detail.
- **DICOM Images**: Integrates 2D DICOM images for enhanced visualization, aligned with the 3D heart model.
- **Internal Navigation**: Facilitates immersive navigation inside the 3D cardiac model for better spatial understanding.

<details>
<summary>Work In Progress</summary>

- [X] **Mixed Reality**: Add an option to toggle between mixed reality and virtual reality modes.
- [ ] **Multiplayer**: Allow multiple surgeons to collaborate in the same virtual environment.
- [ ] **Device Input**: Enable importing and manipulating medical devices within the application.
- [ ] **Specialist Feedback**: Collect user evaluations from specialists to refine and improve the application.
- [ ] **XR Hands**: Introduce hand-tracking for a more natural interaction experience.

</details>

## 💻 Installation

### Prerequisites
- Unity 2022.3.49f1  
- [Optional] Visual Studio Code  

### Setup Instructions
1. Clone or download the repository.
2. Open the project in Unity.
3. Configure the environment as described below.

### Changing the 3D Model
1. Import your 3D model into the **Hierarchy** panel.
2. Copy the `Measurement Manager` child object from the `Simplified Model` example and attach it to your model.
3. Update references:
   - In **GameManager** and **ScaleManager** under **Managers**, replace references to `Simplified` with your model.
   - In **MeshClipper**, update `Simplified` to point to your model.
   - In **DicomImageUI**, navigate to `XROrigin` → `CameraOffset` → `RightController` → `DicomImageUI` and replace the `Simplified` reference with your model.

Enjoy exploring and improving cardiac treatment techniques with this innovative application!
