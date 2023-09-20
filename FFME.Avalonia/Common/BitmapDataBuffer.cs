using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FFME.Container;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace FFME.Avalonia.Common
{
    /// <summary>
    /// Contains metadata about a raw bitmap back-buffer.
    /// </summary>
    public sealed class BitmapDataBuffer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDataBuffer"/> class.
        /// </summary>
        /// <param name="w">The w.</param>
        internal BitmapDataBuffer(ILockedFramebuffer w)
            : this(w.Address, w.RowBytes, w.Format.BitsPerPixel / 8, w.Size.Width, w.Size.Height, w.Dpi.X, w.Dpi.Y, w.Format)
        {
            // placeholder
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDataBuffer"/> class.
        /// </summary>
        /// <param name="b">The block.</param>
        /// <param name="dpiX">The dpi X.</param>
        /// <param name="dpiY">The dpi Y.</param>
        internal BitmapDataBuffer(VideoBlock b, double dpiX, double dpiY)
            : this(b.Buffer, b.PictureBufferStride, 32, b.PixelWidth, b.PixelHeight, dpiX, dpiY, PixelFormats.Bgra8888)
        {
            // placeholder
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDataBuffer"/> class.
        /// </summary>
        /// <param name="scan0">The scan0.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        internal BitmapDataBuffer(IntPtr scan0, int stride, int width, int height, double dpiX, double dpiY)
            : this(scan0, stride, 32, width, height, dpiX, dpiY, PixelFormats.Bgra8888)
        {
            // placeholder
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapDataBuffer" /> class.
        /// </summary>
        /// <param name="scan0">The scan0.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="bytesPerPixel">The bytes per pixel.</param>
        /// <param name="pixelWidth">Width of the pixel.</param>
        /// <param name="pixelHeight">Height of the pixel.</param>
        /// <param name="dpiX">The dpi x.</param>
        /// <param name="dpiY">The dpi y.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="pixelFormat">The pixel format.</param>
        private BitmapDataBuffer(
            IntPtr scan0,
            int stride,
            int bytesPerPixel,
            int pixelWidth,
            int pixelHeight,
            double dpiX,
            double dpiY,
            PixelFormat pixelFormat)
        {
            Scan0 = scan0;
            Stride = stride;
            BytesPerPixel = bytesPerPixel;
            BitsPerPixel = bytesPerPixel * 8;
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
            DpiX = dpiX;
            DpiY = dpiY;

            UpdateRect = new PixelRect(0, 0, pixelWidth, pixelHeight);
            BufferLength = Convert.ToUInt32(Stride * PixelHeight);

            PixelFormat = pixelFormat;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the length of the buffer (Stride x Pixel Height).
        /// </summary>
        public uint BufferLength { get; }

        /// <summary>
        /// Gets a pointer to the raw pixel data.
        /// </summary>
        public IntPtr Scan0 { get; }

        /// <summary>
        /// Gets the byte width of each row of pixels.
        /// </summary>
        public int Stride { get; }

        /// <summary>
        /// Gets the bits per pixel.
        /// </summary>
        public int BitsPerPixel { get; }

        /// <summary>
        /// Gets the bytes per pixel.
        /// </summary>
        public int BytesPerPixel { get; }

        /// <summary>
        /// Gets width of the bitmap.
        /// </summary>
        public int PixelWidth { get; }

        /// <summary>
        /// Gets height of the bitmap.
        /// </summary>
        public int PixelHeight { get; }

        /// <summary>
        /// Gets the DPI on the X axis.
        /// </summary>
        public double DpiX { get; }

        /// <summary>
        /// Gets the DPI on the Y axis.
        /// </summary>
        public double DpiY { get; }

        /// <summary>
        /// Gets the update rect.
        /// </summary>
        public PixelRect UpdateRect { get; }

    
        /// <summary>
        /// Gets the pixel format.
        /// </summary>
        public PixelFormat PixelFormat { get; }

        #endregion

        /// <summary>
        /// Creates a Drawing Bitmap from this data buffer.
        /// </summary>
        /// <returns>The bitmap.</returns>
        public unsafe WriteableBitmap CreateDrawingBitmap( )
        {
            var dpi = new Vector(DpiX, DpiY);
            var pixelSize = new PixelSize(PixelWidth, PixelHeight);
            var format = PixelFormat.Bgra8888;

            var bitmap = new WriteableBitmap(pixelSize, dpi, format);

            using (var fb = bitmap.Lock())
            {
                // Copy the memory from scan0 to the bitmap pixel buffer.
                for (var y = 0; y < PixelHeight; y++)
                {
                    var sourcePtr = (byte*)Scan0.ToPointer() + y * Stride;
                    var targetPtr = (byte*)fb.Address.ToPointer() + y * fb.RowBytes;

                    // Copy one row of pixels.
                    Unsafe.CopyBlock(targetPtr, sourcePtr, (uint)Stride);
                }
            }

            return bitmap;
        }
    }
}
