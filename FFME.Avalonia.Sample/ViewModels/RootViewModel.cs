using DynamicData.Binding;
using FFME.Avalonia.Sample.Views;
using FFME.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace FFME.Avalonia.Sample.ViewModels
{
    /// <summary>
    /// Represents the application-wide view model.
    /// </summary>
    /// <seealso cref="ViewModelBase" />
    public sealed class RootViewModel : ViewModelBase
    {
        // private string m_WindowTitle = string.Empty;
        // private string m_NotificationMessage = string.Empty;
        // private double m_PlaybackProgress;
        // private TaskbarItemProgressState m_PlaybackProgressState;
        private MediaElement m_MediaElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootViewModel"/> class.
        /// </summary>
        public RootViewModel()
        {
            // Set and create an app data directory
            WindowTitle = "Application Loading . . .";
            AppVersion = typeof(RootViewModel).Assembly.GetName().Version.ToString();
            AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ProductName);
            if (Directory.Exists(AppDataDirectory) == false)
                Directory.CreateDirectory(AppDataDirectory);

            // Attached ViewModel Initialization
            //Playlist = new PlaylistViewModel(this);
            //Controller = new ControllerViewModel(this);
        }

        /// <summary>
        /// Gets the product name.
        /// </summary>
        public static string ProductName => "Unosquare FFME-Play";

        ///// <summary>
        ///// Gets the playlist ViewModel.
        ///// </summary>
        //public PlaylistViewModel Playlist { get; }

        ///// <summary>
        ///// Gets the controller.
        ///// </summary>
        //public ControllerViewModel Controller { get; }

        /// <summary>
        /// Gets the application version.
        /// </summary>
        public string AppVersion { get; }

        /// <summary>
        /// Gets the application data directory.
        /// </summary>
        public string AppDataDirectory { get; }

        /// <summary>
        /// Gets the window title.
        /// </summary>
        private string windowTitle;

        public string WindowTitle
        {
            get => windowTitle;
            set => this.RaiseAndSetIfChanged(ref windowTitle, value);
        }


        /// <summary>
        /// Gets or sets the notification message to be displayed.
        /// </summary>
        private string notificationMessage;

        public string NotificationMessage
        {
            get => notificationMessage;
            set => this.RaiseAndSetIfChanged(ref notificationMessage, value);
        }


        /// <summary>
        /// Gets or sets the playback progress.
        /// </summary>
        private double playbackProgress;

        public double PlaybackProgress
        {
            get => playbackProgress;
            set => this.RaiseAndSetIfChanged(ref playbackProgress, value);
        }

        /// <summary>
        /// Gets or sets the state of the playback progress.
        /// </summary>
        // public TaskbarItemProgressState PlaybackProgressState
        // {
        //     get
        //     {
        //         return m_PlaybackProgressState;
        //     }
        //     set
        //     {
        //         m_PlaybackProgressState = value;
        //         NotifyPropertyChanged(nameof(PlaybackProgressState));
        //     }
        // }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is playlist panel open.
        /// </summary>
        private bool isPlaylistPanelOpen;

        public bool IsPlaylistPanelOpen
        {
            get => isPlaylistPanelOpen;
            set => this.RaiseAndSetIfChanged(ref isPlaylistPanelOpen, value);
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is properties panel open.
        /// </summary>
        private bool isPropertiesPanelOpen;

        public bool IsPropertiesPanelOpen
        {
            get => isPropertiesPanelOpen;
            set => this.RaiseAndSetIfChanged(ref isPropertiesPanelOpen, value);
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is application loaded.
        /// </summary>
        private bool isApplicationLoaded;

        public bool IsApplicationLoaded
        {
            get => isApplicationLoaded;
            set => this.RaiseAndSetIfChanged(ref isApplicationLoaded, value);
        }

        /// <summary>
        /// Provides access to application-wide commands.
        /// </summary>
        public AppCommands Commands { get; } = new AppCommands();

        /// <summary>
        /// Gets the media element hosted by the main window.
        /// </summary>
        public MediaElement MediaElement
        {
            get
            {
                if (m_MediaElement == null)
                {
                    if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        m_MediaElement = (desktop.MainWindow as MainWindow)?.Media;
                    }
                }


                return m_MediaElement;
            }
        }

        /// <summary>
        /// Provides access to the current media options.
        /// This is an unsupported usage of media options.
        /// </summary>
        public MediaOptions CurrentMediaOptions { get; set; }

        /// <summary>
        /// Called when application has finished loading.
        /// </summary>
        internal void OnApplicationLoaded()
        {
            if (IsApplicationLoaded)
                return;

            // Playlist.OnApplicationLoaded();
            // Controller.OnApplicationLoaded();

            var m = MediaElement;
            // m.WhenChanged(UpdateWindowTitle,
            //     nameof(m.IsOpen),
            //     nameof(m.IsOpening),
            //     nameof(m.MediaState),
            //     nameof(m.Source));

            m.MediaOpened += (s, e) =>
            {
                // Reset the Zoom
                // Controller.MediaElementZoom = 1d;

                // Update the Controls
                // Playlist.IsInOpenMode = false;
                IsPlaylistPanelOpen = false;
                // Playlist.OpenMediaSource = e.Info.MediaSource;
            };

            IsPlaylistPanelOpen = true;
            IsApplicationLoaded = true;
        }

        /// <summary>
        /// Updates the window title according to the current state.
        /// </summary>
        private void UpdateWindowTitle()
        {
            var m = MediaElement;
            var title = m?.Source?.ToString() ?? "(No media loaded)";
            var state = m?.MediaState.ToString();

            if (m?.IsOpen ?? false)
            {
                foreach (var kvp in m.Metadata)
                {
                    if (!kvp.Key.Equals("title", StringComparison.OrdinalIgnoreCase))
                        continue;

                    title = kvp.Value;
                    break;
                }
            }
            else if (m?.IsOpening ?? false)
            {
                state = "Opening . . .";
            }
            else
            {
                title = "(No media loaded)";
                state = "Ready";
            }

            WindowTitle = $"{title} - {state} - FFME Player v{AppVersion} "
                          + $"FFmpeg {Library.FFmpegVersionInfo} ({(Debugger.IsAttached ? "Debug" : "Release")})";
        }
    }
}