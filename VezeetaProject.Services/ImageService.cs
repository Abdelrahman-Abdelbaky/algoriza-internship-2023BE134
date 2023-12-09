
using Microsoft.AspNetCore.Http;

namespace VezeetaProject.Services
{
    public class ImageService:IImageService
    {
        
        /// <summary>
        /// Create path and save Phote 
        /// </summary>
        /// <param name="Photo"></param>
        /// <returns>array of bytes</returns>
      
        public bool CheckTypeOfImage(IFormFile Photo)
        {
            List<string> Extensions =  new List<string>() { ".png",".jpeg",".jpg"};

            var path =Path.GetExtension(Photo.FileName);

            foreach (var extension in Extensions)
            {
              if(String.Equals(path, extension,StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }

    

        public async Task<Byte[]> EncodeImageAsync(IFormFile Photo)
        {
            using (var memoryStream = new MemoryStream())
            {
                await Photo.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
             
        }

      
    }
}
