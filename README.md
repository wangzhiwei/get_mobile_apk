# APK Extractor - Android APK 批量导出工具

> ⚠️ **本项目代码由 AI 生成**（Trae IDE / Claude）

一个基于 C# WinForm 的 Android APK 批量导出工具，通过 ADB 识别已连接的 Android 设备，浏览并批量导出设备上已安装的应用安装包到本地 PC。

## 项目用途

在 Android 开发、应用测试、应用备份等场景中，经常需要从设备中提取已安装应用的 APK 文件。手动逐个执行 `adb pull` 命令效率低下，本工具提供了图形化界面，支持：

- **设备管理**：一键识别所有通过 USB 或无线 ADB 连接的 Android 设备
- **应用浏览**：查看设备上所有已安装的第三方应用列表（包名、名称、版本、安装路径）
- **批量导出**：勾选多个应用，一键批量导出 APK 到指定本地目录
- **显示名称**：通过 aapt 工具获取应用在手机桌面上的真实显示名称（中文名称）
- **工具配置**：在界面中配置 ADB / aapt 工具路径，无需依赖环境变量

## 功能截图

```
┌─────────────────────────────────────────────────────────────┐
│  APK导出工具 v1.0                                            │
├──────────────────────┬──────────────────────────────────────┤
│  已连接设备            │  可导出应用列表（勾选需要导出的应用）    │
│                      │                                      │
│  设备型号    序列号    │  ☐ 应用名称  包名  版本  APK路径      │
│  Vivo X100  10AC5S... │  ☐ 微信      com.tencent.mm  8.0... │
│  ...                 │  ☐ 支付宝    com.eg.android...  ...  │
│                      │  ☐ 抖音      com.ss.android...  ...  │
│                      │  ...                                  │
│ [刷新设备]  设备数: 1 │ [刷新应用] [全选] [取消全选] [获取显示名称]│
│              [⚙ 设置]│                              应用数: 19│
├──────────────────────┴──────────────────────────────────────┤
│  导出路径： F:\...\APK_Export            [浏览...] [导出选中]│
│  ████████████████░░░░░░░░  导出进度: 15 / 19                │
│  [16:47:13] 开始导出: 微信 (com.tencent.mm)                  │
│  [16:47:14] [成功] 微信 -> ...\com.tencent.mm.apk (156.3 MB) │
└─────────────────────────────────────────────────────────────┘
```

## 技术栈

- **语言/框架**：C# WinForm (.NET Framework 4.7.2+)
- **外部工具依赖**：
  - `adb.exe` — Android Debug Bridge（设备通信）
  - `aapt.exe` — Android Asset Packaging Tool（解析 APK 获取应用名称）

## 项目结构

```
APKExtractor/
├── APKExtractor.csproj          # 项目配置文件
├── Program.cs                   # 程序入口
├── AdbHelper.cs                 # ADB/aapt 命令封装核心类
├── MainForm.cs                  # 主窗体逻辑
├── MainForm.Designer.cs         # 主窗体界面布局
├── SettingsForm.cs              # 设置窗体逻辑
├── SettingsForm.Designer.cs     # 设置窗体界面布局
└── Models/
    ├── AndroidDevice.cs         # 设备数据模型
    ├── AppInfo.cs               # 应用数据模型
    └── AppConfig.cs             # 配置持久化模型
```

## 核心功能说明

### 1. 设备识别

- 启动时自动执行 `adb devices -l` 获取已连接设备
- 解析设备型号、序列号、Android 版本、制造商
- 支持设备状态异常提示（unauthorized / offline）
- 支持手动刷新设备列表，处理设备中途连接/断开

### 2. 应用列表获取

- 执行 `adb shell pm list packages -3 -f` 获取第三方应用（过滤系统应用）
- 通过 `adb shell dumpsys package <包名>` 获取版本信息和应用名称
- 多选 CheckBox 列表展示：应用名称、包名、版本、APK 安装路径

### 3. 批量导出

- 勾选应用后执行 `adb pull` 批量导出到指定目录
- 实时进度条 + 日志输出，记录每个应用的导出状态
- 导出后验证文件大小，确保文件完整无损坏
- 支持取消操作

### 4. 获取显示名称

部分应用的名称通过资源引用定义（`nonLocalizedLabel=null`），`dumpsys` 无法直接获取。本工具支持通过 aapt 解析 APK 获取真实显示名称：

- 将 APK 临时拉取到本地 → `aapt dump badging` 解析 → 提取 `application-label`
- 优先查找中文标签（`application-label-zh-CN`），其次默认标签
- 获取完成后自动清理临时文件

### 5. 工具路径配置

- 设置界面可手动指定 `adb.exe` 和 `aapt.exe` 路径
- 支持自动检测和默认路径恢复
- 配置持久化到程序目录下 `config.json`
- 路径优先级：配置文件 > 程序目录 > ANDROID_HOME > 系统 PATH

### 6. 异常处理

- ADB 未找到：弹窗引导用户配置路径
- 设备状态异常：提示检查 USB 调试授权
- 导出路径无效：自动创建或报错提示
- APK 路径含特殊字符：安全处理（如 Vivo 设备路径中的 `=` 号）
- 文件名非法字符：自动清理替换

## 使用方法

### 前置条件

1. **ADB 工具**：下载 [Android SDK Platform Tools](https://developer.android.com/studio/releases/platform-tools)，将 `adb.exe` 放在程序目录下，或在设置界面中指定路径
2. **aapt 工具**（可选，用于获取应用显示名称）：来自 [Android SDK Build Tools](https://developer.android.com/studio/releases/build-tools)，将 `aapt.exe` 放在程序目录下，或在设置界面中指定路径
3. **Android 设备**：开启「USB 调试」模式并连接到 PC

### 操作步骤

1. 启动 `APKExtractor.exe`
2. 程序自动检测已连接的设备（如未检测到，点击「刷新设备」）
3. 在左侧设备列表中点击目标设备，右侧自动加载应用列表
4. （可选）点击「获取显示名称」获取应用的真实显示名称
5. 勾选需要导出的应用
6. 设置导出路径（默认：程序目录下 `APK_Export` 文件夹）
7. 点击「导出选中」，等待导出完成

## 编译方法

### 使用 Visual Studio

1. 用 Visual Studio 2019/2022 打开 `APKExtractor.csproj`
2. 选择 Release 配置
3. 生成解决方案

### 使用 MSBuild 命令行

```bash
msbuild APKExtractor.csproj /t:Build /p:Configuration=Release
```

编译输出位于 `bin/Release/APKExtractor.exe`。

## 运行环境

- **操作系统**：Windows 7 SP1 及以上
- **.NET Framework**：4.7.2 及以上
- **ADB**：Platform Tools r28 及以上推荐
- **Android 设备**：Android 5.0 及以上，需开启 USB 调试

## 许可证

MIT License

---

> ⚠️ **本项目代码由 AI 生成**（Trae IDE / Claude），请自行验证后使用。
