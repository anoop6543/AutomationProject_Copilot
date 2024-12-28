# System Architecture Diagrams

## Overall System Architecture

+-------------------------+
|     MainWindow.xaml     |
|-------------------------|
| - dbHelper              |
| - servoController       |
| - vfdController         |
| - sensorController      |
| - outputController      |
| - serialDevice1         |
| - serialDevice2         |
| - laserMarkerController |
| - ioController          |
| - dataAcquisitionService|
| - emergencyStopController|
| - safetyInterlockController|
+-------------------------+
            |
            v
+-------------------------+
|     DatabaseHelper      |
+-------------------------+
            |
            v
+-------------------------+
| BeckhoffServoController |
+-------------------------+
            |
            v
+-------------------------+
|      VfdController      |
+-------------------------+
            |
            v
+-------------------------+
|    SensorController     |
+-------------------------+
            |
            v
+-------------------------+
|    OutputController     |
+-------------------------+
            |
            v
+-------------------------+
|     SerialDevice        |
+-------------------------+
            |
            v
+-------------------------+
| KeyenceLaserMarkerController |
+-------------------------+
            |
            v
+-------------------------+
|    TurckIOController    |
+-------------------------+
            |
            v
+-------------------------+
| DataAcquisitionService  |
+-------------------------+
            |
            v
+-------------------------+
| EmergencyStopController |
+-------------------------+
            |
            v
+-------------------------+
| SafetyInterlockController |
+-------------------------+


## 2. Class Diagram for Main Components

+-------------------------+
|     MainWindow.xaml     |
+-------------------------+
            |
            v
+-------------------------+       +-------------------------+
|     DatabaseHelper      |<----->|     DataAcquisitionService  |
+-------------------------+       +-------------------------+
            |                            |
            v                            v
+-------------------------+       +-------------------------+
| BeckhoffServoController |       | EmergencyStopController |
+-------------------------+       +-------------------------+
            |                            |
            v                            v
+-------------------------+       +-------------------------+
|      VfdController      |       | SafetyInterlockController |
+-------------------------+       +-------------------------+
            |                            |
            v                            v
+-------------------------+       +-------------------------+
|    SensorController     |       | EthercatCommunication   |
+-------------------------+       +-------------------------+
            |                            |
            v                            v
+-------------------------+       +-------------------------+
|    OutputController     |       | TurckIOController       |
+-------------------------+       +-------------------------+
            |                            |
            v                            v
+-------------------------+       +-------------------------+
|     SerialDevice        |       | Mock Classes            |
+-------------------------+       +-------------------------+
            |
            v
+-------------------------+
| KeyenceLaserMarkerController |
+-------------------------+

### 3. Sequence Diagram for Emergency Stop Process

MainWindow        EmergencyStopController        TurckIOController        EthercatCommunication
    |                       |                           |                           |
    | ActivateEmergencyStop |                           |                           |
    |---------------------->|                           |                           |
    |                       |                           |                           |
    |                       | WriteDigitalOutput(false) |                           |
    |                       |-------------------------->|                           |
    |                       |                           |                           |
    |                       |                           | SendCommand("STOP_ALL")   |
    |                       |                           |-------------------------->|
    |                       |                           |                           |
    |                       |                           |                           |
    |                       |<--------------------------|                           |
    |                       |                           |                           |
    |                       |<--------------------------|                           |
    |                       |                           |                           |
    |                       | Logger.Warn("Emergency stop activated!")              |
    |                       |------------------------------------------------------>|
    |                       |                           |                           |


   ### 4. Sequence Diagram for Safety Interlock Process

   MainWindow        SafetyInterlockController        TurckIOController        EthercatCommunication
    |                       |                           |                           |
    | AreInterlocksSatisfied|                           |                           |
    |---------------------->|                           |                           |
    |                       |                           |                           |
    |                       | ReadDigitalInput(1)       |                           |
    |                       |-------------------------->|                           |
    |                       |                           |                           |
    |                       |<--------------------------|                           |
    |                       |                           |                           |
    |                       | ReadDigitalInput(2)       |                           |
    |                       |-------------------------->|                           |
    |                       |                           |                           |
    |                       |<--------------------------|                           |
    |                       |                           |                           |
    |                       | ReadStatus("SAFETY_STATUS")|                           |
    |                       |-------------------------->|                           |
    |                       |                           |                           |
    |                       |<--------------------------|                           |
    |                       |                           |                           |
    |                       | Logger.Info("All safety interlock conditions satisfied.")|
    |                       |------------------------------------------------------>|
    |                       |                           |                           |


### 5. Sequence Diagram for Data Acquisition


MainWindow        DataAcquisitionService        SensorController        TurckIOController        VfdController        BeckhoffServoController
    |                       |                           |                           |                           |                           |
    | Start                 |                           |                           |                           |                           |
    |---------------------->|                           |                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       | GetSensorState(1)         |                           |                           |                           |
    |                       |-------------------------->|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       |<--------------------------|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       | ReadAnalogInput(1)        |                           |                           |                           |
    |                       |-------------------------->|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       |<--------------------------|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       | GetSpeed                  |                           |                           |                           |
    |                       |-------------------------->|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       |<--------------------------|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       | GetServoPosition(1)       |                           |                           |                           |
    |                       |-------------------------->|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       |<--------------------------|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       | LogScadaData              |                           |                           |                           |
    |                       |-------------------------->|                           |                           |                           |
    |                       |                           |                           |                           |                           |
    |                       |<--------------------------|                           |                           |                           |
    |                       |                           |                           |                           |                           |
