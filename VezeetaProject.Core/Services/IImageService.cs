using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaProject.Core.Services
{
    public interface IImageService
    {

        public bool CheckTypeOfImage(IFormFile Photo);
        Task<Byte[]> EncodeImageAsync(IFormFile Photo);
    }
}
