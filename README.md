# Sharpmine

Sharpmine is a custom Minecraft server implementation built from the ground up using C# and .NET 10.

There are many components missing and this project is as WIP as WIP can get.

## Purpose

The project aims to provide a high-performance, efficient server engine. Instead of using reflection, it makes use of Roslyn Source Generators to automate packet registration at compile time, ensuring the networking layer is as fast as possible.

## Project Structure

* **Sharpmine.Server**: Core logic and GUI
  * The GUI has yet to be made use of - as of right now, it is just blank.
* **Sharpmine.Gen**: The incremental source generator for the protocol.
