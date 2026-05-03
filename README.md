# Smart Home Simulator

A .NET-based simulation platform designed to model, control, and analyze smart home environments. This project focuses on object-oriented design, automated logic, and energy efficiency tracking without the need for physical hardware.

## Key Features
* **Role-Based Access Control (RBAC):** Distinct permissions for **Administrator** (full control over rooms and devices) and **Guest** (view and control only).
* **Device Management:** Support for devices including **Lights** and **Thermostats** with real-time power consumption (W) tracking.
* **Search Engine:** Global search functionality to find any device across the entire house by name.
* **Automation Engine:** Time-based rules to automate device states (e.g., "At 6:00 PM, turn on lights").
* **Energy Analytics:** Real-time calculation of total household electricity consumption in **kWh**.
* **Household Modes:** Support for global modes like *Vacation* or *Night* to optimize energy usage.

## Tech Stack
* **Language:** C# (.NET 8.0)
* **Database:** **MongoDB Atlas** (Cloud NoSQL)
* **Version Control:** Git / GitHub

## Project Structure
* `Models/` – Core entities (Device, Room, User, Automation).
* `Data/` – Database logic (MongoService) and Simulation engine (Simulator).
* `UI/` – Console-based user interface and menu navigation (MenuHandler).

## Milestones (Status)
1. **Milestone 1:** Data Models and MongoDB integration. (COMPLETED)
2. **Milestone 2:** Device control and energy consumption logic. (COMPLETED)
3. **Milestone 3:** RBAC, Search system, and Automation triggers. (COMPLETED)
4. **Final Submission:** Bug fixes and documentation. (DONE)

## Author
* **Viktor Knespl**
