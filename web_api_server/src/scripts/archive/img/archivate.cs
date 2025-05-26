using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Pack.ImagePk
{
    public class ImageCompressor
    {
        public static string CompressBase64Image(string base64Image, int quality = 75)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            using (var image = Image.Load(imageBytes))
            using (var msOutput = new MemoryStream())
            {
                // Опциональное изменение размера
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(800, 0), // Ширина 800px, высота автоматическая
                    Mode = ResizeMode.Max
                }));

                // Сохраняем с указанным качеством
                var encoder = new JpegEncoder { Quality = quality };
                image.SaveAsJpeg(msOutput, encoder);
                
                return Convert.ToBase64String(msOutput.ToArray());
            }
        }
    }
}