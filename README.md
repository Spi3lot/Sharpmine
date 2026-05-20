# Sharpmine

[![.NET](https://github.com/Spi3lot/Sharpmine/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Spi3lot/Sharpmine/actions/workflows/dotnet.yml)

Sharpmine is a custom Minecraft server implementation built from the ground up using C# 14 and .NET 10.

There are still many components missing, but this project is currently on a strong trajectory toward actually becoming somewhat useful.

## Purpose

The project aims to provide a cross-platform, high-performance, efficient server engine. Instead of using reflection, it makes use of Roslyn Source Generators to automate packet registration at compile time, ensuring the network layer is as fast as possible.

## Project Structure

* `Sharpmine.Domain`: Contains types that are not only relevant to the server but also to a possible future client.
* `Sharpmine.Server.Infrastructure`: Contains the core game logic, `System.IO.Pipelines` networking and protocol implementation.
* `Sharpmine.Server.Console`: Lightweight console application that basically just wraps `Sharpmine.Server.Infrastructure`.
* `Sharpmine.Server.Wpf`: A Windows-native desktop application built with WPF, also wrapping `Sharpmine.Server.Infrastructure` but displaying logs in a more user-friendly way.
* `Sharpmine.Gen`: The incremental source generator for the protocol. It automates the generation of a dedicated class for each and every Minecraft packet.
* `Sharpmine.Tests`: Unit tests.
* `Sharpmine.AppHost` and `Sharpmine.ServiceDefaults`: Aspire orchestration. Not yet made proper use of, but maybe in the future.

## Contribute

Any contribution is greatly appreciated! Feel free to open issues or fork the repo and maybe even submit a pull request. :D
