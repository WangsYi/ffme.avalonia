using System.Collections.Concurrent;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using FFME.Avalonia.Common;
using FFME.Common;
using FFME.Engine;
using FFME.Platform;
using FFME.Primitives;
using FFME.Rendering;

namespace FFME;

public partial class MediaElement:ImageHost,IDisposable,IUriContext
{
    /// <summary>
    /// The allow content change flag.
    /// </summary>
    private readonly bool AllowContentChange;
    private readonly ConcurrentBag<string> PropertyUpdates = new();
    private readonly AtomicBoolean m_IsStateUpdating = new(false);
    private readonly DispatcherTimer UpdatesTimer;
    
    #region Constructors

    static MediaElement()
    {
        InitPropertyAffect();
        MediaEngine.FFmpegMessageLogged += (s, message) =>
            FFmpegMessageLogged?.Invoke(typeof(MediaElement), new MediaLogMessageEventArgs(message));
        //ContentProperty.OverrideMetadata<MediaElement>(new StyledPropertyMetadata<object?>(null, coerce:OnCoerceContentValue));
        var style = new Style(x=>x.OfType<MediaElement>())
        {
            Setters =
            {
                new Setter(FlowDirectionProperty,  FlowDirection.LeftToRight)
            }
        };
        Application.Current?.Styles.Add(style);
        
    }

    static void InitPropertyAffect()
    {
        var props = new AvaloniaProperty[]
            { StretchProperty, StretchDirectionProperty, IgnorePixelAspectRatioProperty };
        AffectsMeasure<MediaElement>(props);
        AffectsRender<MediaElement>(props);
    }

    public MediaElement()
    {
        try
        {
            GuiContext = new GuiContext();
            AllowContentChange = true;
            if (!Library.IsInDesignMode)
            {
                //VideoView = new ImageHost();
                VideoView = this;
                // Setup the media engine and property updates timer
                MediaCore = new MediaEngine(this, new MediaConnector(this));
                MediaCore.State.PropertyChanged += (s, e) => PropertyUpdates.Add(e.PropertyName);

                // When the media element is removed from the visual tree
                // we want to close the current media to prevent memory leaks
                Unloaded += async (s, e) =>
                {
                    if (UnloadedBehavior != MediaPlaybackState.Close)
                        return;

                    try
                    {
                        await Close();
                    }
                    finally
                    {
                        Dispose();
                    }
                };

                UpdatesTimer = new DispatcherTimer(DispatcherPriority.DataBind)
                {
                    Interval = TimeSpan.FromMilliseconds(15),
                };

                // UpdatesTimer.Tick += CoerceMediaCoreState;
                UpdatesTimer.Start();
            }

            // InitializeComponent();
        }
        finally
        {
            AllowContentChange = false;
        }
    }
    
    #endregion

    #region Methods

    /// <summary>
    /// Called when [coerce content value].
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="baseValue">The base value.</param>
    /// <returns>The content property value.</returns>
    /// <exception cref="InvalidOperationException">When content has been locked.</exception>
    private static object? OnCoerceContentValue(AvaloniaObject d, object? baseValue)
    {
        //if (d is MediaElement element && element.AllowContentChange == false)
        //    throw new InvalidOperationException($"The '{nameof(Content)}' property is not meant to be set.");

        return baseValue;
    }
    

    #endregion
    
    
    #region Properties

    /// <inheritdoc />
    Uri IUriContext.BaseUri { get; set; }

    /// <summary>
    /// Provides access to various internal media renderer options.
    /// The default options are optimal to work for most media streams.
    /// This is an advanced feature and it is not recommended to change these
    /// options without careful consideration.
    /// </summary>
    public RendererOptions RendererOptions { get; } = new RendererOptions();

    /// <summary>
    /// The GUI context used to create this media element.
    /// </summary>
    internal IGuiContext GuiContext { get; private set; }

    /// <summary>
    /// This is the image that holds video bitmaps. It is a Hosted Image which means that in a WPF
    /// GUI context, it runs on its own dispatcher (multi-threaded UI).
    /// </summary>
    internal ImageHost VideoView { get; }

    /// <summary>
    /// Gets the closed captions view control.
    /// </summary>
    internal ClosedCaptionsControl CaptionsView { get; } = new ClosedCaptionsControl { Name = nameof(CaptionsView) };

    /// <summary>
    /// A ViewBox holding the subtitle text blocks.
    /// </summary>
    internal SubtitlesControl SubtitlesView { get; } = new SubtitlesControl { Name = nameof(SubtitlesView) };

    /// <summary>
    /// Gets the grid control holding the rest of the controls.
    /// </summary>
    internal Grid ContentGrid { get; } = new Grid { Name = nameof(ContentGrid) };

    /// <summary>
    /// Determines whether the property values are being copied over from the
    /// <see cref="MediaCore"/> state.
    /// </summary>
    internal bool IsStateUpdating
    {
        get => m_IsStateUpdating.Value;
        set => m_IsStateUpdating.Value = value;
    }

    #endregion
    public void Dispose()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// <inheritdoc cref="IUriContext"/>
    /// </summary>
    public Uri BaseUri { get; set; }
}