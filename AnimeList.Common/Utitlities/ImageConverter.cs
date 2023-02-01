using Microsoft.AspNetCore.Http;

namespace AnimeList.Common.Utitlities
{
    public static class ImageConverter
    {
        public static byte[] ImageToByteArray(IFormFile image)
        {
            byte[] imageBytes = null;

            using (var binaryReader = new BinaryReader(image.OpenReadStream()))
            {
                imageBytes = binaryReader.ReadBytes((int)image.Length);
            }
            return imageBytes;
        }

        public static byte[] SetDefaultImage(string path)
        {
            byte[] imageBytes;
            using (var imageFile = new FileStream(Path.Combine("wwwroot/Images", path), FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }
            }
            return imageBytes;
        }
    }
}
