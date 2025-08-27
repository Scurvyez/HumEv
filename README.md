# HumEv
*A lightweight C# console application for simulating humidity and evaporation dynamics, with the game RimWorld in mind.*

---

## Overview
**HumEv** (short for *Humidity and Evaporation*) is a simulation tool that models how different environmental and abiotic factors influence **temperature**, **relative humidity**, and **wet bulb temperature**.

The goal is to create a flexible framework that can later be extended or integrated into other systems (e.g., RimWorld mods, games, or educational tools).

---

## Features
- 🌤️ **Weather Effects** – Models changes to temperature and humidity based on weather type (Clear, Rain, Thunderstorms, Dry Spell, etc.).
- 🏔️ **Topography Effects** – Adjusts for valleys, mountains, coasts, and plains. More can be added.
- 🌊 **Water Proximity** – Simulates effects of large bodies of water based on a proximity score (0 → far inland, 1 → coastal).
- 🌱 **Vegetation Coverage** – Accounts for plant life moderating temperature and increasing humidity.
- 🌍 **Latitude & Season** – Adjusts for seasonal cycles and solar angle depending on latitude and day-of-year.
- 📈 **Atmospheric Pressure** – Calculates pressure at elevation using the standard barometric formula.
- 🌡️ **Psychrometrics** – Computes wet bulb temperature and saturation vapor pressures using Tetens’ formula.
- 🔄 **Unit Conversion** – Includes common helpers like Fahrenheit ↔ Celsius conversion.

---

## Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later

---

## Example Output
=== HumEv Indoor/Outdoor Wet Bulb Test ===

Elevation: 1598 m, Latitude: 47.97°
Weather: Thunderstorms
Topology: Mountain

Outdoor Conditions: 23.9 °C, RH 65%, Pressure 83.5 kPa
Calculated Outdoor Wet Bulb: 20.41 °C

Indoor Conditions: 18.3 °C, RH 54%
Calculated Indoor Wet Bulb: 16.07 °C

---


