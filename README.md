# WPF Magnifier

[![.NET](https://img.shields.io/badge/.NET-6.0-blue)](https://dotnet.microsoft.com/download/dotnet/6.0)
[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-purple)](https://visualstudio.microsoft.com/vs/)

WPF Magnifier is a magnification utility for WPF (Windows Presentation Foundation) applications, designed to enhance UI accessibility by providing zoom functionality. This repository is a fork of the original [WPF Magnifier project](https://github.com/fcostacampos/WPF-Magnifier), upgraded to work with Visual Studio 2022, .NET 6.0, and refactored to follow the **MVVM (Model-View-ViewModel)** architectural pattern.

## Table of Contents
- [Project Overview](#project-overview)
- [What's New](#whats-new)
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Project Overview
The WPF Magnifier provides an easy way to add a magnifying glass effect to a WPF application. This is useful for accessibility features, making it easier for users to zoom in on sections of a window or control. This fork introduces a complete structural refactor to adhere to the **MVVM** design pattern, improving maintainability and testability.

## What's New
This fork introduces the following updates:
- **Upgraded to Visual Studio 2022** for improved development experience and features.
- **Migrated to .NET 6.0**, offering better performance and long-term support.
- **Refactored to MVVM** (Model-View-ViewModel) design pattern, separating concerns for easier management and scalability.
- Updated dependencies and project structure to align with modern .NET and WPF standards.

## Features
- **Dynamic Magnification**: Allows users to zoom in on portions of the UI.
- **Easy Integration**: Can be quickly added to any WPF project.
- **MVVM Ready**: Refactored to separate UI logic (ViewModel) from UI presentation (View).
- **Customizable**: Control the size, zoom level, and behavior of the magnifier.

## Requirements
- **Visual Studio 2022** or later.
- **.NET 6.0** or later.

## Installation

### Prerequisites
Ensure you have the following installed:
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Cloning the Repository
To clone this repository, run the following command:

```bash
git clone https://github.com/GoldenxSun/WPF-Magnifier.git
```

### Restoring Dependencies
Navigate to the project directory and restore the .NET dependencies:

```bash
cd WPF-Magnifier
dotnet restore
```

### Building the Project
Once the dependencies are restored, build the project using Visual Studio or the .NET CLI:

```bash
dotnet build
```

## Usage

1. **Open the Project**: Launch the solution (`WPF-Magnifier.sln`) in Visual Studio 2022.
2. **Run the Project**: Press `F5` to build and run the project.
3. **Integrate with Your Application**: You can import the magnifier control into your own WPF projects by copying over the necessary classes or referencing this project.

### Example
```xml
<!-- Example usage of the Magnifier in a WPF XAML file -->
<Viewbox x:Name="imgViewBox" 
            Stretch="Uniform">
    
    <Canvas x:Name="imgCanvas"
            ClipToBounds="True" 
            Width="{Binding ElementName=RootImgGrid, Path=ActualWidth}" 
            Height="{Binding ElementName=RootImgGrid, Path=ActualHeight}">

        <Image x:Name="imgObj"
                Source="{Binding CurrentPicture}"
                MouseWheel="img_MouseWheel"
                Cursor="Hand"
                MouseMove="Img_MouseMove"
                MouseDown="Img_MouseDown"
                MouseUp="Img_MouseUp"
                Stretch="Uniform">
            <Image.RenderTransform>
                <TransformGroup x:Name="imgTransformGroup">
                    <ScaleTransform x:Name="imgScaleTransform"/>
                    <TranslateTransform x:Name="imgTranslateTransform"/>
                </TransformGroup>
            </Image.RenderTransform>
            <Image.LayoutTransform>
                <RotateTransform x:Name="imgRotateTransform"/>
            </Image.LayoutTransform>
        </Image>
    </Canvas>
</Viewbox>
```

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
