# IndustrialAutomationSuite

## Overview

IndustrialAutomationSuite is a comprehensive machine automation project designed to control and monitor various industrial components such as servo motors, variable frequency drives (VFDs), sensors, digital outputs, laser markers, and IO-Link devices. The project is built using .NET 8 and WPF for the Human-Machine Interface (HMI), providing a robust and user-friendly interface for operators and maintenance personnel.

## Purpose

The primary purpose of this project is to demonstrate advanced capabilities in machine automation, including:

- **Precise Control**: Managing servo motors and VFDs for accurate positioning and speed control.
- **Monitoring**: Reading sensor states and analog inputs to ensure the machine operates within specified parameters.
- **Communication**: Utilizing serial communication, Profinet, and EtherCAT to interface with various devices.
- **Logging and Diagnostics**: Implementing comprehensive logging and diagnostics to track machine performance and troubleshoot issues.
- **Simulation Mode**: Enabling a simulation mode to test and debug the application without requiring actual hardware.
- **SCADA Service**: Providing real-time data acquisition, monitoring, and control capabilities.

## Features

### 1. Servo Motor Control

The project includes a `BeckhoffServoController` class that provides methods to move, start, stop, and get the status of servo motors. The controller supports both real and simulated modes, allowing for flexible testing and debugging.

### 2. Variable Frequency Drive (VFD) Control

The `VfdController` class manages the speed and operation of VFDs. It includes methods to set speed, start, stop, get speed, get fault codes, and reset faults. The controller supports simulation mode for testing without hardware.

### 3. Sensor Monitoring

The `SensorController` class reads the state of digital sensors. It supports simulation mode to provide simulated sensor states for testing purposes.

### 4. Digital and Analog IO Control

The `TurckIOController` class handles digital and analog IO operations. It includes methods to read digital inputs, write digital outputs, read analog inputs, and write analog outputs. The controller supports simulation mode for testing without hardware.

### 5. Laser Marker Control

The `KeyenceLaserMarkerController` class manages laser marking operations. It includes methods to start marking, stop marking, set marking parameters, get marking status, and reset alarms. The controller supports simulation mode for testing without hardware.

### 6. Serial Communication

The `SerialCommunication` and `SerialDevice` classes provide methods to send and receive data over serial ports. They support simulation mode to log simulated communication for testing purposes.

### 7. Database Interaction

The `DatabaseHelper` class interacts with a Microsoft SQL database to store and retrieve parameter recipes, results, alarms, and error logs. It supports simulation mode to log simulated database operations for testing purposes.

### 8. Human-Machine Interface (HMI)

The project includes a WPF-based HMI with three main screens:
- **Main Screen**: Displays machine status, including servo position, VFD speed, sensor state, digital output state, and analog input value.
- **Maintenance Screen**: Provides maintenance information, including last and next maintenance dates and maintenance notes.
- **Diagnostics Screen**: Displays diagnostics information, including the last error, error timestamp, and error details.

### 9. SCADA Service

The `DataAcquisitionService` class provides SCADA (Supervisory Control and Data Acquisition) capabilities, including:

- **Real-Time Data Acquisition**: Periodically collects data from sensors and devices.
- **Data Storage**: Logs the collected data to a database for historical analysis.
- **Real-Time Monitoring**: Updates the HMI with real-time data.
- **Control**: Allows operators to control devices from the HMI.
- **Alarming**: Generates alarms based on specific conditions.

## Why This Project?

This project showcases my capabilities in developing complex machine automation systems. It demonstrates proficiency in:

- **.NET and C#**: Leveraging the latest features of .NET 8 and C# 12.0 to build robust and maintainable code.
- **WPF**: Creating user-friendly interfaces for machine operators and maintenance personnel.
- **Industrial Communication Protocols**: Implementing serial communication, Profinet, and EtherCAT to interface with various industrial devices.
- **Logging and Diagnostics**: Using NLog for comprehensive logging and diagnostics to track machine performance and troubleshoot issues.
- **Simulation Mode**: Enabling a simulation mode to test and debug the application without requiring actual hardware, ensuring flexibility and efficiency in the development process.
- **SCADA Capabilities**: Implementing real-time data acquisition, monitoring, and control to provide a comprehensive view of the machine's operation.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022
- Microsoft SQL Server (for database operations)

### Installation

1. Clone the repository: 
```
git clone https://github.com/yourusername/TestProjectAnoop.git
```
2. Open the solution in Visual Studio 2022.
3. Restore NuGet packages.
4. Build the solution.

### Running the Application

1. Set the `SimulationMode` property in `Configuration.cs` to `true` or `false` based on your testing requirements.
2. Run the application from Visual Studio.

### Configuration

- **Database Connection**: Update the connection string in `DatabaseHelper.cs` to point to your SQL Server instance.
- **Serial Ports**: Update the port names in the constructors of `SerialCommunication` and `SerialDevice` classes to match your hardware configuration.

## Conclusion

IndustrialAutomationSuite is a comprehensive demonstration of my skills in machine automation, software development, and user interface design. It highlights my ability to integrate various industrial components, implement robust communication protocols, and provide a user-friendly interface for operators and maintenance personnel. The inclusion of a simulation mode further showcases my commitment to efficient and flexible development practices.
