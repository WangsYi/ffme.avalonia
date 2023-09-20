using Avalonia;
using Avalonia.Media;

namespace FFME
{
    using ClosedCaptions;
    using Common;
    using System;
    using System.ComponentModel;
    using global::Avalonia.Controls;

    public partial class MediaElement
    {
        #region IsDesignPreviewEnabled Dependency Property

        /// <summary>
        /// Gets or sets a value that indicates whether the MediaElement will display
        /// a preview image in design time. This is a dependency property.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Gets or sets a value that indicates whether the MediaElement will display a preview image in design time.")]
        public bool IsDesignPreviewEnabled
        {
            get => GetValue(IsDesignPreviewEnabledProperty);
            set => SetValue(IsDesignPreviewEnabledProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.IsDesignPreviewEnabled property.
        /// </summary>
        private static readonly StyledProperty<bool> IsDesignPreviewEnabledProperty = RegisterProperty(nameof(IsDesignPreviewEnabled), true, OnIsDesignPreviewEnabledPropertyChanged);

        private static void OnIsDesignPreviewEnabledPropertyChanged(Control d, AvaloniaPropertyChangedEventArgs e)
        {
            
            if (!Library.IsInDesignMode)
                return;
            if (!(d is MediaElement element))
                return;

            if ((bool)e.NewValue)
            {
                element.VideoView.Source ??= Properties.Resources.FFmpegMediaElementBackground;

                element.CaptionsView.IsVisible = true;
                element.SubtitlesView.IsVisible = true;
            }
            else
            {
                element.VideoView.Source = null;
                element.CaptionsView.IsVisible = false;
                element.SubtitlesView.IsVisible = false;
            }
        }

        #endregion

        #region Volume Dependency Property

        /// <summary>
        /// Gets/Sets the Volume property on the MediaElement.
        /// Note: Valid values are from 0 to 1.
        /// </summary>


        [Category(nameof(MediaElement))]
        public double Volume
        {
            get => GetValue(VolumeProperty);
            set => SetValue(VolumeProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.Volume property.
        /// </summary>
        private static readonly StyledProperty<double>
            VolumeProperty = RegisterProperty(nameof(Volume), Constants.DefaultVolume, null, coerce: OnVolumePropertyChanging);

        private static double OnVolumePropertyChanging(AvaloniaObject? d, double value)
        {
            if (d == null || d is MediaElement == false)
                return Constants.DefaultVolume;

            var element = (MediaElement)d;
            if (element.IsStateUpdating)
                return value;

            element.MediaCore.State.Volume = (double)value;
            return element.MediaCore.State.Volume;
        }

        #endregion

        #region Balance Dependency Property


        /// <summary>
        /// Gets/Sets the Balance property on the MediaElement.
        /// </summary>
        [Category(nameof(MediaElement))]
        public double Balance
        {
            get => GetValue(BalanceProperty);
            set => SetValue(BalanceProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.Balance property.
        /// </summary>
        private static readonly StyledProperty<double> BalanceProperty =
            RegisterProperty(nameof(Balance), Constants.DefaultBalance, null, coerce: OnBalancePropertyChanging);
        
        private static double OnBalancePropertyChanging(AvaloniaObject? d, double value)
        {
            if (d == null || d is MediaElement == false)
                return Constants.DefaultBalance;

            var element = (MediaElement)d;
            if (element.IsStateUpdating)
                return value;

            element.MediaCore.State.Balance = value;
            return element.MediaCore.State.Balance;
        }

        #endregion

        #region IsMuted Dependency Property


        /// <summary>
        /// Gets/Sets the IsMuted property on the MediaElement.
        /// </summary>
        [Category(nameof(MediaElement))]
        public bool IsMuted
        {
            get => GetValue(IsMutedProperty);
            set => SetValue(IsMutedProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.IsMuted property.
        /// </summary>
        private static readonly StyledProperty<bool> IsMutedProperty =
            RegisterProperty(nameof(IsMuted), false, null, OnIsMutedPropertyChanging);

        private static bool OnIsMutedPropertyChanging(AvaloniaObject? d, bool value)
        {
            if (d == null || d is MediaElement == false)
                return false;

            var element = (MediaElement)d;
            if (element.IsStateUpdating)
                return value;

            element.MediaCore.State.IsMuted = value;
            return element.MediaCore.State.IsMuted;
        }

        #endregion

        #region ScrubbingEnabled Dependency Property
         
        /// <summary>
        /// Gets or sets a value that indicates whether the MediaElement will update frames
        /// for seek operations while paused. This is a dependency property.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Gets or sets a value that indicates whether the MediaElement will display frames for seek operations before the final seek position is reached.")]
        public bool ScrubbingEnabled
        {
            get => GetValue(ScrubbingEnabledProperty);
            set => SetValue(ScrubbingEnabledProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.ScrubbingEnabled property.
        /// </summary>
        private static readonly StyledProperty<bool> ScrubbingEnabledProperty = RegisterProperty(nameof(ScrubbingEnabled), true,
            null, OnScrubbingEnabledPropertyChanging);
        
        private static bool OnScrubbingEnabledPropertyChanging(AvaloniaObject? d, bool value)
        {
            if (d == null || d is MediaElement == false)
                return false;

            var element = (MediaElement)d;
            if (element.IsStateUpdating)
                return value;

            element.MediaCore.State.ScrubbingEnabled = value;
            return element.MediaCore.State.ScrubbingEnabled;
        }

        #endregion

        #region VerticalSyncEnabled Dependency Property


        /// <summary>
        /// Gets or sets a value that indicates whether the MediaElement will update frames
        /// for seek operations while paused. This is a dependency property.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Gets or sets a value that indicates whether the MediaElement will display frames for seek operations before the final seek position is reached.")]
        public bool VerticalSyncEnabled
        {
            get => GetValue(VerticalSyncEnabledProperty);
            set => SetValue(VerticalSyncEnabledProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.VerticalSyncEnabled property.
        /// </summary>
        private static readonly StyledProperty<bool> VerticalSyncEnabledProperty = RegisterProperty(nameof(VerticalSyncEnabled), true,
            null, OnVerticalSyncEnabledPropertyChanging);

        private static bool OnVerticalSyncEnabledPropertyChanging(AvaloniaObject? d, bool value)
        {
            if (d == null || d is MediaElement == false)
                return false;

            var element = (MediaElement)d;
            if (element.IsStateUpdating)
                return value;

            element.MediaCore.State.VerticalSyncEnabled = value;
            return element.MediaCore.State.VerticalSyncEnabled;
        }

        #endregion

        #region SpeedRatio Dependency Property


        /// <summary>
        /// Gets/Sets the SpeedRatio property on the MediaElement.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Specifies how quickly or how slowly the media should be rendered. 1.0 is normal speed. Value must be greater then or equal to 0.0")]
        public double SpeedRatio
        {
            get => GetValue(SpeedRatioProperty);
            set => SetValue(SpeedRatioProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.SpeedRatio property.
        /// </summary>
        private static readonly StyledProperty<double> SpeedRatioProperty = RegisterProperty(nameof(SpeedRatio), Constants.DefaultSpeedRatio,
            null, OnSpeedRatioPropertyChanging);
        
        private static double OnSpeedRatioPropertyChanging(AvaloniaObject? d, double value)
        {
            if (d == null || d is MediaElement == false)
                return Constants.DefaultSpeedRatio;

            var element = (MediaElement)d;
            if (element.IsStateUpdating)
                return value;

            element.MediaCore.State.SpeedRatio = value;
            return element.MediaCore.State.SpeedRatio;
        }

        #endregion

        #region Position Dependency Property


        
        /// <summary>
        /// Gets/Sets the Position property on the MediaElement.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Specifies the position of the underlying media. Set this property to seek though the media stream.")]
        public TimeSpan Position
        {
            get => GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.Position property.
        /// </summary>
        private static readonly StyledProperty<TimeSpan> PositionProperty = RegisterProperty(nameof(Position), TimeSpan.Zero, 
            null, OnPositionPropertyChanging);

        private static TimeSpan OnPositionPropertyChanging(AvaloniaObject? d, TimeSpan value)
        {
            if (d == null || d is MediaElement == false) return value;

            var element = (MediaElement)d;
            var mediaCore = element.MediaCore;

            if (mediaCore.IsDisposed || !element.IsOpen)
                return TimeSpan.Zero;

            if (element.IsSeekable == false || element.IsStateUpdating)
                return value;

            // Clamp from minimum to maximum
            var targetSeek = value;
            var minTarget = mediaCore.State.PlaybackStartTime ?? TimeSpan.Zero;
            var maxTarget = mediaCore.State.PlaybackEndTime ?? TimeSpan.Zero;
            var hasValidTaget = maxTarget > minTarget;

            if (hasValidTaget)
            {
                targetSeek = targetSeek.Clamp(minTarget, maxTarget);
                mediaCore?.Seek(targetSeek);
            }
            else
            {
                targetSeek = mediaCore.State.Position;
            }

            return targetSeek;
        }

        #endregion

        #region LoadedBahavior Dependency Property


   
        /// <summary>
        /// Specifies the action that the media element should execute when it
        /// is loaded. The default behavior is that it is under manual control
        /// (i.e. the caller should call methods such as Play in order to play
        /// the media). If a source is set, then the default behavior changes to
        /// to be playing the media. If a source is set and a loaded behavior is
        /// also set, then the loaded behavior takes control.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Specifies how the underlying media should behave when it has loaded. The default behavior is to Play the media.")]
        public MediaPlaybackState LoadedBehavior
        {
            get => GetValue(LoadedBehaviorProperty);
            set => SetValue(LoadedBehaviorProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.LoadedBehavior property.
        /// </summary>
        private static readonly StyledProperty<MediaPlaybackState> LoadedBehaviorProperty = RegisterProperty(nameof(LoadedBehavior), MediaPlaybackState.Manual,
            null, null);
        
        #endregion

        #region UnoadedBahavior Dependency Property



        /// <summary>
        /// Specifies how the underlying media engine's resources should be handled when
        /// the unloaded event gets fired. The default behavior is to Close and release the resources.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Specifies how the underlying media engine's resources should be handled when the unloaded event gets fired.")]
        public MediaPlaybackState UnloadedBehavior
        {
            get => GetValue(UnloadedBehaviorProperty);
            set => SetValue(UnloadedBehaviorProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.UnloadedBehavior property.
        /// </summary>
        private static readonly StyledProperty<MediaPlaybackState> UnloadedBehaviorProperty = RegisterProperty(nameof(UnloadedBehavior), MediaPlaybackState.Close,
            OnUnloadedBehaviorPropertyChanged, null);
        
        private static void OnUnloadedBehaviorPropertyChanged(AvaloniaObject? d, AvaloniaPropertyChangedEventArgs e)
        {
            if (d == null || d is MediaElement == false)
                return;

            var element = (MediaElement)d;

            var behavior = element.UnloadedBehavior;
            element.VideoView.PreventShutdown = behavior != MediaPlaybackState.Close;
        }

        #endregion

        #region LoopingBehavior Dependency Property
     
        /// <summary>
        /// Specifies how the media should behave when it has ended. The default behavior is to Pause the media.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("Specifies how the media should behave when it has ended. The default behavior is to Pause the media.")]
        public MediaPlaybackState LoopingBehavior
        {
            get => GetValue(LoopingBehaviorProperty);
            set => SetValue(LoopingBehaviorProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.LoopingBehavior property.
        /// </summary>
        private static readonly StyledProperty<MediaPlaybackState> LoopingBehaviorProperty = RegisterProperty(nameof(LoopingBehavior), MediaPlaybackState.Pause,
            null, null);
        #endregion

        #region ClosedCaptionsChannel Dependency Property

        /// <summary>
        /// Gets/Sets the ClosedCaptionsChannel property on the MediaElement.
        /// Note: Valid values are from 0 to 1.
        /// </summary>
        [Category(nameof(MediaElement))]
        [Description("The video CC Channel to render. Ranges from 0 to 4")]
        public CaptionsChannel ClosedCaptionsChannel
        {
            get => GetValue(ClosedCaptionsChannelProperty);
            set => SetValue(ClosedCaptionsChannelProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.ClosedCaptionsChannel property.
        /// </summary>
        public static readonly StyledProperty<CaptionsChannel> ClosedCaptionsChannelProperty = RegisterProperty(nameof(ClosedCaptionsChannel), Constants.DefaultClosedCaptionsChannel,
            OnClosedCaptionsChannelPropertyChanged, null);
        
        private static void OnClosedCaptionsChannelPropertyChanged(AvaloniaObject? d, AvaloniaPropertyChangedEventArgs e)
        {
            if (d is MediaElement m) m.CaptionsView.Reset();
        }

        #endregion

        #region Stretch Dependency Property


        /// <summary>
        /// Gets/Sets the Stretch on this MediaElement.
        /// The Stretch property determines how large the MediaElement will be drawn.
        /// </summary>
        [Category(nameof(MediaElement))]
        public Stretch Stretch
        {
            get => GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.Stretch property.
        /// </summary>
        public static readonly StyledProperty<Stretch> StretchProperty = RegisterProperty(nameof(Stretch), global::Avalonia.Media.Stretch.Uniform,
            OnStretchPropertyChanged, null);

        private static void OnStretchPropertyChanged(AvaloniaObject? d, AvaloniaPropertyChangedEventArgs e)
        {
            if (d is MediaElement { VideoView.IsLoaded: true } m && e.NewValue is Stretch v)
                m.VideoView.Stretch = v;
        }

        #endregion

        #region StretchDirection Dependency Property

        /// <summary>
        /// Gets/Sets the stretch direction of the ViewBox, which determines the restrictions on
        /// scaling that are applied to the content inside the ViewBox.  For instance, this property
        /// can be used to prevent the content from being smaller than its native size or larger than
        /// its native size.
        /// </summary>
        public StretchDirection StretchDirection
        {
            get => GetValue(StretchDirectionProperty);
            set => SetValue(StretchDirectionProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.StretchDirection property.
        /// </summary>
        private static readonly StyledProperty<StretchDirection> StretchDirectionProperty = RegisterProperty(nameof(StretchDirection), StretchDirection.Both,
            OnStretchDirectionPropertyChanged, null);
        

        private static void OnStretchDirectionPropertyChanged(AvaloniaObject? d, AvaloniaPropertyChangedEventArgs e)
        {
            if (d is MediaElement { VideoView.IsLoaded: true } m && e.NewValue is StretchDirection v)
                m.VideoView.StretchDirection = v;
        }

        #endregion

        #region IgnorePixelAspectRatio Dependency Property


        
        /// <summary>
        /// Gets/Sets the stretch direction of the ViewBox, which determines the restrictions on
        /// scaling that are applied to the content inside the ViewBox.  For instance, this property
        /// can be used to prevent the content from being smaller than its native size or larger than
        /// its native size.
        /// </summary>
        public bool IgnorePixelAspectRatio
        {
            get => GetValue(IgnorePixelAspectRatioProperty);
            set => SetValue(IgnorePixelAspectRatioProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the MediaElement.IgnorePixelAspectRatio property.
        /// </summary>
        private static readonly StyledProperty<bool> IgnorePixelAspectRatioProperty = RegisterProperty(nameof(IgnorePixelAspectRatio), false);

        #endregion


        private static StyledProperty<T> RegisterProperty<T>(string name, T defaultVal,
            Action<MediaElement, AvaloniaPropertyChangedEventArgs>? changed=null,  Func<AvaloniaObject, T,T>? coerce = null)
        {
            StyledProperty<T> prop;
            prop = coerce != null?AvaloniaProperty.Register<MediaElement, T>(
                name, defaultValue: defaultVal, coerce: coerce): AvaloniaProperty.Register<MediaElement, T>(name, defaultValue: defaultVal) ;
            if (changed != null)
            {
                prop.Changed.AddClassHandler<FFME.MediaElement>(changed);
            }
            return prop;
        }
    }

}
