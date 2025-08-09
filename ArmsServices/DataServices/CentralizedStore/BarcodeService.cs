using BarcodeLib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ArmsServices.DataServices
{
    public interface IBarcodeService
    {
        string GenerateBase64Barcode(string code, int width = 250, int height = 60);
    }

    public class BarcodeService : IBarcodeService
    {
        public string GenerateBase64Barcode(string code, int width = 250, int height = 60)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var barcode = new Barcode
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.CENTER,
                LabelFont = new Font(FontFamily.GenericSansSerif, 10)
            };

            Image image = barcode.Encode(TYPE.CODE128, code, Color.Black, Color.White, width, height);

            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            var imageBytes = ms.ToArray();
            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}
