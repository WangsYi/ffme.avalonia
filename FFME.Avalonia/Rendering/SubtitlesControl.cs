using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;

#pragma warning disable SA1202 // Elements must appear in the correct order
namespace FFME.Rendering
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// A control suitable for displaying subtitles.
    /// Layout is: UserControl:ViewBox:Grid:TextBlocks.
    /// </summary>
    /// <seealso cref="UserControl" />
    internal class SubtitlesControl : UserControl
    {
        #region Constants and Defaults

        /// <summary>
        /// The default font size.
        /// </summary>
        private const double DefaultFontSize = 56;

        /// <summary>
        /// The default text foreground.
        /// </summary>
        private static readonly IBrush DefaultTextForeground = Brushes.WhiteSmoke;

        /// <summary>
        /// The default text outline.
        /// </summary>
        private static readonly IBrush DefaultTextOutline = Brushes.Black;

        /// <summary>
        /// The default text outline width.
        /// </summary>
        private static readonly Thickness DefaultTextOutlineWidth = new Thickness(1);

        #endregion

        #region Dependency Property Registrations

        /// <summary>
        /// Gets or sets the text contents of this text block.
        /// </summary>
        [Category(nameof(SubtitlesControl))]
        [Description("Gets or sets the text contents of this text block.")]
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the SubtitlesControl.Text property.
        /// </summary>
        public static readonly StyledProperty<string> TextProperty = RegisterProperty(nameof(Text), string.Empty,
            OnTextPropertyChanged);

        /// <summary>
        /// Gets or sets the text foreground.
        /// </summary>
        [Category(nameof(SubtitlesControl))]
        [Description("Gets or sets the text contents of this text block.")]
        public IBrush TextForeground
        {
            get => GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the SubtitlesControl.TextForeground property.
        /// </summary>
        public static readonly StyledProperty<IBrush> TextForegroundProperty = RegisterProperty(nameof(TextForeground), DefaultTextForeground,
            OnTextForegroundPropertyChanged);


        /// <summary>
        /// Gets or sets the text foreground effect.
        /// </summary>
        [Category(nameof(SubtitlesControl))]
        [Description("Gets or sets the text foreground effect. It's a smooth drop shadow by default.")]
        public IEffect TextForegroundEffect
        {
            get => GetValue(TextForegroundEffectProperty);
            set => SetValue(TextForegroundEffectProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the SubtitlesControl.TextForegroundEffect property.
        /// </summary>
        public static readonly StyledProperty<IEffect> TextForegroundEffectProperty = RegisterProperty(nameof(TextForegroundEffect), GetDefaultTextForegroundEffect(),
            OnTextForegroundEffectPropertyChanged);



        /// <summary>
        /// Gets or sets the text outline width.
        /// </summary>
        [Category(nameof(SubtitlesControl))]
        [Description("Gets or sets the text outline width.")]
        public Thickness TextOutlineWidth
        {
            get => GetValue(TextOutlineWidthProperty);
            set => SetValue(TextOutlineWidthProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the SubtitlesControl.TextOutlineWidth property.
        /// </summary>
        public static readonly StyledProperty<Thickness> TextOutlineWidthProperty = RegisterProperty(nameof(TextOutlineWidth), DefaultTextOutlineWidth,
            OnTextOutlineWidthPropertyChanged);

        
        /// <summary>
        /// Gets or sets the text outline.
        /// </summary>
        [Category(nameof(SubtitlesControl))]
        [Description("Gets or sets the text outline brush.")]
        public IBrush TextOutline
        {
            get => GetValue(TextOutlineProperty);
            set => SetValue(TextOutlineProperty, value);
        }

        /// <summary>
        /// The StyledProperty for the SubtitlesControl.TextOutline property.
        /// </summary>
        public static readonly StyledProperty<IBrush> TextOutlineProperty = RegisterProperty(nameof(TextOutline), DefaultTextOutline,
            OnTextOutlinePropertyChanged);


        #endregion

        #region Private State Backing

        /// <summary>
        /// Holds the text blocks that together create an outlined subtitle text display.
        /// </summary>
        private readonly Dictionary<Block, TextBlock> TextBlocks = new Dictionary<Block, TextBlock>(5);

        /// <summary>
        /// The container for the outlined text blocks.
        /// </summary>
        private readonly Viewbox Container = new Viewbox { Name = nameof(Container) };

        /// <summary>
        /// A Layout transform to condense text.
        /// </summary>
        private readonly ScaleTransform CondenseTransform = new ScaleTransform();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitlesControl"/> class.
        /// </summary>
        public SubtitlesControl()
        {
            var layoutElement = new Grid
            {
                Name = $"{nameof(SubtitlesControl)}TextGrid"
            };

            Container.Child = layoutElement;

            for (var i = (int)Block.Bottom; i >= (int)Block.Foreground; i--)
            {
                var textBlock = new TextBlock
                {
                    Name = $"{nameof(SubtitlesControl)}_{(Block)i}",
                    TextWrapping = TextWrapping.NoWrap,
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    // avalonia not support LayoutTransform = CondenseTransform
                    //RenderTransform = CondenseTransform
                    
                };

                TextBlocks[(Block)i] = textBlock;

                var blockType = (Block)i;
                if (blockType == Block.Foreground)
                {
                    textBlock.Effect = GetDefaultTextForegroundEffect();
                    textBlock.Foreground = DefaultTextForeground;
                    textBlock.Margin = new Thickness(0);
                }
                else
                {
                    textBlock.Foreground = DefaultTextOutline;
                    textBlock.Margin = ComputeMargin(blockType, DefaultTextOutlineWidth);
                }

                layoutElement.Children.Add(textBlock);
            }

            // Add the container as the content of the control.
            Container.Stretch = Stretch.Uniform;
            Container.StretchDirection = StretchDirection.DownOnly;
            Container.VerticalAlignment = VerticalAlignment.Stretch;
            Container.HorizontalAlignment = HorizontalAlignment.Stretch;
            Content = Container;
            Height = 0;

            // set font defaults
            FontSize = DefaultFontSize;
            FontWeight = FontWeight.DemiBold;
            VerticalAlignment = VerticalAlignment.Bottom;

            if (Library.IsInDesignMode)
            {
                Text = "Subtitle TextBlock\r\n(Design-Time Preview)";
            }
        }

        private enum Block
        {
            Foreground = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            Bottom = 4
        }
        
        
        #region Helper Methods


        private static StyledProperty<T> RegisterProperty<T>(string name, T defaultVal,
            Action<SubtitlesControl, AvaloniaPropertyChangedEventArgs>? changed=null)
        {
            StyledProperty<T> prop;
            prop = AvaloniaProperty.Register<SubtitlesControl, T>(name, defaultValue: defaultVal) ;
            if (changed != null)
            {
                prop.Changed.AddClassHandler<SubtitlesControl>(changed);
            }
                AffectsMeasure<SubtitlesControl>(prop);
                AffectsRender<SubtitlesControl>(prop);
         
            return prop;
        }
        /// <inheritdoc />
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == nameof(FontSize))
            {
                var value = (double)e.NewValue;
                foreach (var t in TextBlocks)
                    t.Value.FontSize = value;
            }
            else if (e.Property.Name == nameof(FontStretch))
            {
                var value = (FontStretch)e.NewValue;
                foreach (var t in TextBlocks)
                    t.Value.FontStretch = value;
            }
            else if (e.Property.Name == nameof(FontWeight))
            {
                var value = (FontWeight)e.NewValue;
                foreach (var t in TextBlocks)
                    t.Value.FontWeight = value;
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Computes the margin according to the block type.
        /// </summary>
        /// <param name="blockType">Type of the block.</param>
        /// <param name="outlineWidth">Width of the outline.</param>
        /// <returns>A thickness depending on the block type.</returns>
        private static Thickness ComputeMargin(Block blockType, Thickness outlineWidth)
        {
            if (blockType == Block.Foreground) return default;

            var topMargin = 0d;
            var leftMargin = 0d;

            if (blockType == Block.Top)
                topMargin = -outlineWidth.Top;
            else if (blockType == Block.Bottom)
                topMargin = outlineWidth.Bottom;
            else if (blockType == Block.Left)
                leftMargin = -outlineWidth.Left;
            else if (blockType == Block.Right)
                leftMargin = outlineWidth.Right;

            return new Thickness(leftMargin, topMargin, 0, 0);
        }

        /// <summary>
        /// Gets the default text foreground effect.
        /// </summary>
        /// <returns>A new instance of a foreground effect.</returns>
        private static IEffect GetDefaultTextForegroundEffect() => new DropShadowDirectionEffect
        {
            BlurRadius = 4,
            Color = Colors.Black,
            Direction = 315,
            Opacity = 0.75,
            // ava not sup RenderingBias = RenderingBias.Performance,
            ShadowDepth = 6
        };

        #endregion

        #region Dependency Property Change Handlers

        private static void OnTextPropertyChanged(AvaloniaObject? dependencyObject, AvaloniaPropertyChangedEventArgs e)
        {
            if (dependencyObject is SubtitlesControl == false) return;

            var element = (SubtitlesControl)dependencyObject;
            var value = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(value)) value = " \r\n ";
            if (value.ContainsOrdinal("\n") == false) value = $"{value}\r\n ";
            foreach (var t in element.TextBlocks)
                t.Value.Text = value;
        }

        private static void OnTextForegroundPropertyChanged(AvaloniaObject? dependencyObject, AvaloniaPropertyChangedEventArgs e)
        {
            if (dependencyObject is SubtitlesControl == false) return;

            var element = (SubtitlesControl)dependencyObject;
            var value = e.NewValue as Brush;
            element.TextBlocks[Block.Foreground].Foreground = value;
        }

        private static void OnTextOutlinePropertyChanged(AvaloniaObject? dependencyObject, AvaloniaPropertyChangedEventArgs e)
        {
            if (dependencyObject is SubtitlesControl == false) return;

            var element = (SubtitlesControl)dependencyObject;
            var value = e.NewValue as Brush;
            foreach (var t in element.TextBlocks)
            {
                if (t.Key != Block.Foreground)
                    t.Value.Foreground = value;
            }
        }

        private static void OnTextOutlineWidthPropertyChanged(AvaloniaObject? dependencyObject, AvaloniaPropertyChangedEventArgs e)
        {
            if (dependencyObject is SubtitlesControl == false) return;

            var element = (SubtitlesControl)dependencyObject;
            var value = (Thickness)e.NewValue;
            foreach (var t in element.TextBlocks)
                t.Value.Margin = ComputeMargin(t.Key, value);
        }

        private static void OnTextForegroundEffectPropertyChanged(AvaloniaObject? dependencyObject, AvaloniaPropertyChangedEventArgs e)
        {
            if (dependencyObject is SubtitlesControl == false) return;

            var element = (SubtitlesControl)dependencyObject;
            var value = e.NewValue as IEffect;
            element.TextBlocks[Block.Foreground].Effect = value;
        }

        #endregion
    }
}
#pragma warning restore SA1202 // Elements must appear in the correct order