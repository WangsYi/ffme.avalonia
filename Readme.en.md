<p align="center"><a href="README.md">ÖÐÎÄ</a> | English</p>

# FFME.Avalonia

## Description

A video playback control for Avalonia based on ffmpeg.autogen (https://github.com/Ruslan-B/FFmpeg.AutoGen).

## Usage

### 1. Install ffmpeg 4.4, the shared library version

#### Windows

Download from the following link or compile manually, make sure to choose version 4.4.
`https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-full-shared.7z`

#### Linux

Use a package manager to install ffmpeg. Ensure that the software source's ffmpeg version is 4.4, or manually compile it.
> Tested and running fine on Ubuntu 22.04 and Deepin 23 beta2.

### 2. Configure the ffmpeg dynamic library path

Refer to FFME.Avalonia.Sample, modify the loading path in App.axaml.cs:
   - For Windows, set it to the path where you extracted ffmpeg.
   - For Linux, it should be set to `/usr/lib/x86_64-linux-gnu`. The specific path may vary depending on the distribution and CPU architecture, so please refer to your distribution's documentation.

### 3. Run the project

Run the FFME.Avalonia.Sample project, click on "Open Video," and you'll see the results.

## TODO

- [x] Test on Windows.
- [x] Test on Linux.
- [ ] Test on macOS.
- [ ] Complete subtitle support testing.
- [ ] Improve the Sample project.
- [ ] Enhance documentation.
- [x] Create English documentation.