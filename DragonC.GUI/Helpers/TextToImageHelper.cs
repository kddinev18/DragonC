using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonC.GUI.Helpers
{
    public static class TextToImageHelper
    {
        public static ImageSource CreateImageFromText(string text, string fontFamily, float fontSize, SKColor textColor, SKColor backgroundColor, int width = 100, int height = 100)
        {
            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;
            canvas.Clear(backgroundColor);

            using var paint = new SKPaint
            {
                TextSize = fontSize,
                IsAntialias = true,
                Color = textColor,
                Typeface = SKTypeface.FromFamilyName(fontFamily),
                TextAlign = SKTextAlign.Center
            };

            // Measure text size
            var bounds = new SKRect();
            paint.MeasureText(text, ref bounds);

            // Calculate position
            float x = width / 2;
            float y = (height + bounds.Height) / 2;

            // Draw text
            canvas.DrawText(text, x, y, paint);

            // Convert to Bitmap and ImageSource
            using var img = surface.Snapshot();
            using var data = img.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new MemoryStream();
            data.SaveTo(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return ImageSource.FromStream(() => stream);
        }
    }
}
