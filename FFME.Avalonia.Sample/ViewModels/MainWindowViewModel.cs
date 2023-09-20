using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using FFME.Avalonia.Sample.Services;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace FFME.Avalonia.Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private MediaElement _mediaElement;

        internal MainWindowViewModel()
        {
            LoadedCommand = ReactiveCommand.CreateFromTask<MediaElement>(OnLoaded);
            OpenCommand = ReactiveCommand.CreateFromTask(Open);
        }


        #region Commands

        /// <summary>
        /// window loaded event
        /// init media element
        /// </summary>
        public ReactiveCommand<MediaElement, Unit> LoadedCommand { get; }

        private async Task OnLoaded(MediaElement ele)
        {
            _mediaElement = ele;
            await LoadFFmpeg();
        }

        private async Task LoadFFmpeg()
        {
            var desktop = Utils.GetDesktop();

            // Pre-load FFmpeg libraries in the background. This is optional.
            // FFmpeg will be automatically loaded if not already loaded when you try to open
            // a new stream or file. See issue #242
            try
            {
                // Pre-load FFmpeg
                await Library.LoadFFmpegAsync();
            }
            catch (Exception ex)
            {
                var dispatcher = Dispatcher.UIThread;
                await dispatcher.InvokeAsync(() =>
                {
                    MessageBoxManager.GetMessageBoxStandard(
                        "FFmpeg Error",
                        $"Unable to Load FFmpeg Libraries from path:\r\n    {Library.FFmpegDirectory}" +
                        $"\r\nMake sure the above folder contains FFmpeg shared binaries (dll files) for the " +
                        $"applicantion's architecture ({(Environment.Is64BitProcess ? "64-bit" : "32-bit")})" +
                        $"\r\nTIP: You can download builds from https://ffmpeg.org/download.html" +
                        $"\r\n{ex.GetType().Name}: {ex.Message}\r\n\r\nApplication will exit.",
                        ButtonEnum.Ok,
                        Icon.Error
                    );
                    desktop!.Shutdown();
                });
            }
        }

        public ReactiveCommand<Unit, Unit> OpenCommand { get; }

        private async Task Open()
        {
            try
            {
                var fileService = App.Current?.Services?.GetService<IFileService>();
                var file = await fileService?.OpenFileAsync()!;
                if (file is null) return;

                _mediaElement.Open(file.Path);
                _mediaElement.Play();
            }
            catch (Exception e)
            {
                MessageBoxManager.GetMessageBoxStandard(
                    "Open failed",
                    $"Open file failed:{e.GetType()},{e.Message}",
                    ButtonEnum.Ok,
                    Icon.Error
                );
            }
        }

        #endregion
    }
}