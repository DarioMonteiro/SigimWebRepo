using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Adapter
{
    public static class ImageAdapter
    {
        public static Image ToImage(this byte[] value)
        {
            MemoryStream ms = new MemoryStream(value);
            Image img = Image.FromStream(ms);
            return img;
        }
    }
}