# IoTwin MVP – Unity Passthrough Demo

This is the Minimum Viable Product (MVP) for the IoTwin project, built with Unity and designed for Meta Quest 3. It demonstrates the integration of XR passthrough and a physical environmental sensor wall.

## Project Overview

This MVP includes:

- Passthrough mode enabled on Meta Quest 3
- Virtual Sensor Wall placed in the real-world environment
- Real-time display of sensor data (e.g., temperature, current)
- Dynamic temperature-to-current mapping (e.g., 25°C → 8.00 mA)
- Unity UI (Canvas) used to render text data

## Tech Stack

| Technology     | Purpose                         |
|----------------|----------------------------------|
| Unity 6.0 LTS  | XR application development       |
| Meta Quest 3   | Target headset with passthrough  |
| OpenXR         | XR integration                   |
| C#             | Scripting and logic              |
| PT100 (simulated) | Sensor input representation   |

## How to Run

1. Clone this repository and open it in Unity
2. Make sure the project is configured for **OpenXR + Meta Quest 3 + Passthrough**
3. Run the scene (e.g., `SensorWallScene`)
4. View real-time sensor data on the headset in passthrough mode

## Current Status

- MVP completed: passthrough and sensor integration functional
- Planned for full version:
  - Support for more sensor types (e.g., humidity, pressure)
  - Enhanced interactivity (e.g., gesture)
  - Full user interface and menu system
