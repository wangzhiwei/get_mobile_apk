# APK Extractor - Android APK Batch Export Tool

> ⚠️ **This project code is AI-generated** (Trae IDE / Claude)

A C# WinForm-based Android APK batch export tool that identifies connected Android devices via ADB and exports installed application packages to your local PC.

## Purpose

In scenarios such as Android development, app testing, and app backup, it's often necessary to extract APK files from devices. Manually executing `adb pull` commands for each app is inefficient. This tool provides a graphical interface that supports:

- **Device Management**: One-click detection of all Android devices connected via USB or wireless ADB
- **App Browsing**: View all installed third-party apps on the device (package name, name, version, installation path)
- **Batch Export**: Select multiple apps and export APKs to a specified local directory with one click
- **Display Names**: Use aapt tool to get the actual display names of apps on the phone's home screen (including Chinese names)
- **Tool Configuration**: Configure ADB / aapt tool paths in the interface without relying on environment variables

## Interface Preview

```
┌─────────────────────────────────────────────────────────────┐
│  APK Extractor Tool v1.0                                    │
├──────────────────────┬──────────────────────────────────────┤
│  Connected Devices   │  Exportable Apps (Check apps to export)│
│                      │                                      │
│  Model      Serial   │  ☐ App Name    Package  Ver  Path   │
│  Vivo X100  10AC5S...│  ☐ WeChat      com.tencent.mm  8.0..│
│  ...                 │  ☐ Alipay      com.eg.android... ...│
│                      │  ☐ TikTok      com.ss.android... ...│
│                      │  ...                                  │
│ [Refresh]  Devices: 1│ [Refresh] [All] [None] [Fetch Labels]│
│             [⚙ Settings]                          Apps: 19 │
├──────────────────────┴──────────────────────────────────────┤
│  Export Path: F:\...\APK_Export            [Browse] [Export] │
│  ████████████████░░░░░░░░  Progress: 15 / 19                │
│  [16:47:13] Exporting: WeChat (com.tencent.mm)              │
│  [16:47:14] [Success] WeChat -> ...\com.tencent.mm.apk (156.3 MB) │
└─────────────────────────────────────────────────────────────┘
```

## Tech Stack

- **Language/Framework**: C# WinForm (.NET Framework 4.7.2+)
- **External Tool Dependencies**:
  - `adb.exe` — Android Debug Bridge (device communication)
  - `aapt.exe` — Android Asset Packaging Tool (parse APK to get app names)

## Project Structure

```
APKExtractor/
├── APKExtractor.csproj          # Project configuration file
├── Program.cs                   # Program entry point
├── AdbHelper.cs                 # Core class for ADB/aapt command wrapper
├── MainForm.cs                  # Main form logic
├── MainForm.Designer.cs         # Main form UI layout
├── SettingsForm.cs              # Settings form logic
├── SettingsForm.Designer.cs     # Settings form UI layout
└── Models/
    ├── AndroidDevice.cs         # Device data model
    ├── AppInfo.cs               # App info data model
    └── AppConfig.cs             # Configuration persistence model
```

## Core Features

### 1. Device Detection

- Automatically executes `adb devices -l` on startup to detect connected devices
- Parses device model, serial number, Android version, manufacturer
- Supports abnormal device status alerts (unauthorized / offline)
- Manual refresh device list, handles device connection/disconnection during use

### 2. App List Retrieval

- Executes `adb shell pm list packages -3 -f` to get third-party apps (filters system apps)
- Retrieves version info and app names via `adb shell dumpsys package <package_name>`
- Multi-select CheckBox list display: app name, package name, version, APK installation path

### 3. Batch Export

- Executes `adb pull` to batch export to specified directory after selecting apps
- Real-time progress bar + log output, records export status of each app
- Validates file size after export to ensure complete, non-corrupted files
- Supports cancellation

### 4. Fetch Display Names

Some apps define names through resource references (`nonLocalizedLabel=null`), which `dumpsys` cannot directly retrieve. This tool supports using aapt to parse APKs and get actual display names:

- Temporarily pulls APK to local → `aapt dump badging` parsing → extracts `application-label`
- Prioritizes Chinese labels (`application-label-zh-CN`), then default labels
- Automatically cleans up temporary files after fetching

### 5. Tool Path Configuration

- Settings interface allows manual specification of `adb.exe` and `aapt.exe` paths
- Supports auto-detection and default path restoration
- Configuration persisted to `config.json` in program directory
- Path priority: Config file > Program directory > ANDROID_HOME > System PATH

### 6. Exception Handling

- ADB not found: Popup guides user to configure path
- Abnormal device status: Prompts to check USB debugging authorization
- Invalid export path: Automatically creates or shows error message
- APK paths with special characters: Safe handling (e.g., `=` in Vivo device paths)
- Invalid filename characters: Automatically cleaned and replaced

## Usage

### Prerequisites

1. **ADB Tool**: Download [Android SDK Platform Tools](https://developer.android.com/studio/releases/platform-tools), place `adb.exe` in the program directory, or specify path in settings
2. **aapt Tool** (optional, for getting app display names): From [Android SDK Build Tools](https://developer.android.com/studio/releases/build-tools), place `aapt.exe` in the program directory, or specify path in settings
3. **Android Device**: Enable "USB debugging" mode and connect to PC

### Steps

1. Launch `APKExtractor.exe`
2. Program automatically detects connected devices (if not detected, click "Refresh Devices")
3. Click target device in left device list, right side automatically loads app list
4. (Optional) Click "Fetch Display Names" to get actual app display names
5. Check the apps to export
6. Set export path (default: `APK_Export` folder in program directory)
7. Click "Export Selected", wait for export to complete

## Build Instructions

### Using Visual Studio

1. Open `APKExtractor.csproj` with Visual Studio 2019/2022
2. Select Release configuration
3. Build solution

### Using MSBuild Command Line

```bash
msbuild APKExtractor.csproj /t:Build /p:Configuration=Release
```

Build output is located at `bin/Release/APKExtractor.exe`.

## Runtime Environment

- **Operating System**: Windows 7 SP1 and above
- **.NET Framework**: 4.7.2 and above
- **ADB**: Platform Tools r28 and above recommended
- **Android Device**: Android 5.0 and above, USB debugging required

## License

MIT License

---

> ⚠️ **This project code is AI-generated** (Trae IDE / Claude). Please verify before use.
