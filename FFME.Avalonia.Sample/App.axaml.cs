using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FFME.Avalonia.Sample.ViewModels;
using FFME.Avalonia.Sample.Views;
using System.IO;
using System.Threading.Tasks;
using System;
using Avalonia.Controls;
using Avalonia.Threading;
using FFME.Avalonia.Sample.Services;
using FFmpeg.AutoGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace FFME.Avalonia.Sample
{
    public partial class App : Application
    {
        public App()
        {
            // Change the default location of the ffmpeg binaries (same directory as application)
            // You can get the 64-bit binaries here: https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-full-shared.7z
            Library.FFmpegDirectory = @"d:\lib\ffmpeg";

            // You can pick which FFmpeg binaries are loaded. See issue #28
            // For more specific control (issue #414) you can set Library.FFmpegLoadModeFlags to:
            // FFmpegLoadMode.LibraryFlags["avcodec"] | FFmpegLoadMode.LibraryFlags["avfilter"] | ... etc.
            // Full Features is already the default.
            Library.FFmpegLoadModeFlags = FFmpegLoadMode.FullFeatures;

            // Multi-threaded video enables the creation of independent
            // dispatcher threads to render video frames. This is an experimental feature
            // and might become deprecated in the future as no real performance enhancements
            // have been detected.
            Library.EnableWpfMultiThreadedVideo =
                false; // !System.Diagnostics.Debugger.IsAttached; // test with true and false
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var desktop = Utils.GetDesktop();

            desktop!.MainWindow = new MainWindow();
            var services = new ServiceCollection();
            services.AddSingleton<IFileService>(x=>new FileService(desktop!.MainWindow));
            Services = services.BuildServiceProvider();
            base.OnFrameworkInitializationCompleted();
        }


        /// <summary>
        /// Provides access to the root-level, application-wide VM.
        /// </summary>
        public static RootViewModel? ViewModel => Current?.Resources[nameof(ViewModel)] as RootViewModel;

        /// <summary>
        /// Determines if the Application is in design mode.
        /// </summary>
        public static bool IsInDesignMode => Design.IsDesignMode;

        /// <summary>
        /// Gets a full file path for a screen capture or stream recording.
        /// </summary>
        /// <param name="mediaPrefix">The media prefix. Use Screenshot or Capture for example.</param>
        /// <param name="extension">The file extension without a dot.</param>
        /// <returns>A full file path where the media file will be written to.</returns>
        public static string GetCaptureFilePath(string mediaPrefix, string extension)
        {
            var date = DateTime.UtcNow;
            var dateString =
                $"{date.Year:0000}-{date.Month:00}-{date.Day:00} {date.Hour:00}-{date.Minute:00}-{date.Second:00}.{date.Millisecond:000}";
            var targetFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                "ffmeplay");

            if (Directory.Exists(targetFolder) == false)
                Directory.CreateDirectory(targetFolder);

            var targetFilePath = Path.Combine(targetFolder, $"{mediaPrefix} {dateString}.{extension}");
            if (File.Exists(targetFilePath))
                File.Delete(targetFilePath);

            return targetFilePath;
        }
        public new static App? Current => Application.Current as App;
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider? Services { get; private set; }
    }
}