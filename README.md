# Smart Home Simulator

A .NET-based simulation platform designed to model, control, and analyze smart home environments. This project focuses on object-oriented design, automated logic, and energy efficiency tracking without the need for physical hardware.

## Key Features
* **Role-Based Access Control (RBAC):** Distinct permissions for Administrator (full control), Resident, and Guest roles.
* **Device Management:** Support for various devices including Lights, Thermostats, Sensors, and Cameras with real-time power consumption (W) tracking.
* **Automation Engine:** Create "If-This-Then-That" rules (e.g., "At 6:00 PM, turn on hallway lights" or "If motion detected, change device state").
* **Energy Analytics:** Real-time and historical calculation of electricity consumption (kWh) and estimated costs (CZK).
* **Event Logging:** Full audit trail of device state changes, sensor values, and triggered automation rules.
* **Data Persistence:** Persistent storage using a database to maintain home configuration, user accounts, and history between sessions.

## Tech Stack
* **Language:** C# (.NET 8.0/9.0)
* **Database:** MongoDb
* **Version Control:** Git
* **Project Management:** Gitea Projects (Kanban methodology)

## Software Architecture
The project follows **Clean Architecture** principles and **Object-Oriented Programming (OOP)**. A central `Device` base class handles common properties, while specific device types inherit and extend functionality.



## Project Structure
* `/src/Core` – Models, Interfaces, and Business Logic.
* `/src/Infrastructure` – Database context, Migrations, and Data persistence.
* `/src/UI` – Console-based user interface and menu logic.

## Roadmap (Milestones)
1. **Milestone 1 (Feb 22):** Implementation of Data Models, User Authentication, and basic Room/Device CRUD operations.
2. **Milestone 2 (Mar 15):** Device control logic, time simulation engine, and energy consumption calculations.
3. **Milestone 3 (Mar 29):** Advanced automation engine, household modes (Vacation/Night), and final analytical reports.
4. **Final Submission (Apr 12):** Polishing, final bug fixes, and project documentation.
