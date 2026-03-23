[![.NET](https://github.com/Spi3lot/Sharpmine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Spi3lot/Sharpmine/actions/workflows/dotnet.yml)

# Sharpmine

Sharpmine is a custom Minecraft server implementation built from the ground up using C# and .NET 10.

There are many components missing and this project is, at least as of right now, just a fun side project.

## Purpose

The project aims to provide a high-performance, efficient server engine. Instead of using reflection, it makes use of Roslyn Source Generators to automate packet registration at compile time, ensuring the networking layer is as fast as possible.

## Project Structure

* **Sharpmine.Server**: Core logic and GUI.
* **Sharpmine.Gen**: The incremental source generator for the protocol.
* **Sharpmine.Tests**: Unit tests.

## Contribute

Any contribution is greatly appreciated! Feel free to open issues or fork the repo and maybe even submit a pull request. :D
