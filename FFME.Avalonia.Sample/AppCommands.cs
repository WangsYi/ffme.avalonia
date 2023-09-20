using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace FFME.Avalonia.Sample
{
    /// <summary>
    /// Represents the Application-Wide Commands.
    /// </summary>
    public class AppCommands
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommands"/> class.
        /// </summary>
        internal AppCommands()
        {
            OpenCommand = ReactiveCommand.CreateFromTask<string>(Open);
            CloseCommand = ReactiveCommand.CreateFromTask(CloseMedia);
            PauseCommand = ReactiveCommand.CreateFromTask(Pause);
            PlayCommand = ReactiveCommand.CreateFromTask(Play);
            StopCommand = ReactiveCommand.CreateFromTask(Stop);
        }

        #endregion

        /// <summary>
        /// Gets the open command.
        /// </summary>
        /// <value>
        /// The open command.
        /// </value>

        public ReactiveCommand<string, Unit> OpenCommand { get; }

        private async Task Open(string a)
        {
            try
            {
                var uriString = a as string;
                if (string.IsNullOrWhiteSpace(uriString))
                    return;

                var m = App.ViewModel.MediaElement;
                var target = new Uri(uriString);
                    await m.Open(target);
            }
            catch (Exception ex)
            {
                MessageBoxManager.GetMessageBoxStandard(
                    $"{nameof(MediaElement)} Error",
                    $"Media Failed: {ex.GetType()}\r\n{ex.Message}",
                    ButtonEnum.YesNo,
                    Icon.Error);
            }
        }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>
        /// The close command.
        /// </value>
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        private async Task CloseMedia()
        {
            if (App.ViewModel != null) await App.ViewModel.MediaElement.Close();
        }
        /// <summary>
        /// Gets the pause command.
        /// </summary>
        /// <value>
        /// The pause command.
        /// </value>
        
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }

        private async Task Pause()
        {
            await App.ViewModel.MediaElement.Pause();
        }
        /// <summary>
        /// Gets the play command.
        /// </summary>
        /// <value>
        /// The play command.
        /// </value>
        public ReactiveCommand<Unit,Unit> PlayCommand { get; }

        private async Task Play()
        {
            // await Current.MediaElement.Seek(TimeSpan.Zero)
            await App.ViewModel.MediaElement.Play();
        }
        
        
        /// <summary>
        /// Gets the stop command.
        /// </summary>
        /// <value>
        /// The stop command.
        /// </value>
        public ReactiveCommand<Unit,Unit> StopCommand { get; }

        private async Task Stop()
        {
            await App.ViewModel.MediaElement.Stop();
        }
    }
}
