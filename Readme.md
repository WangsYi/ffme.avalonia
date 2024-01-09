<p align="center">中文 | <a href="Readme.en.md">English</a></p>

# FFME.Avalonia

## 说明

一个基于[ffmpeg.autogen](https://github.com/Ruslan-B/FFmpeg.AutoGen)的Avalonia视频播放控件。
> 感谢 [unosquare/ffmediaelement](https://github.com/unosquare/ffmediaelement)


## 使用

### 1. 安装ffmpeg 4.4，需要动态库版本

#### Windows

从以下地址下载，或手动编译，需要选择4.4版本。
`https://github.com/GyanD/codexffmpeg/releases/download/4.4/ffmpeg-4.4-full_build-shared.zip`

#### Linux

使用包管理器，安装ffmpeg，需要确定，软件源中的ffmpeg为4.4版本，否则需要手动编译
> 测试ubuntu 22.04、deepin 23 beta2中正常运行

### 2. 配置ffmpeg动态库路径

参考FFME.Avalonia.Sample，修改App.axaml.cs中的加载路径；
   - windows设置为你解压的ffmpeg的路径
   - linux中需要设置为`/usr/lib/x86_64-linux-gnu`，具体路径可能根据发行版及cpu架构有所差异，请参照发行版的文档

### 3. 运行项目

运行FFME.Avalonia.Sample项目，点击打开视频，即可看到效果。



## TODO

- [x] 测试windows；
- [x] 测试linux；
- [ ] 测试macOS；
- [ ] 完成字幕支持测试；
- [ ] 完善Sample项目；
- [ ] 完善文档；
- [x] 英文文档。