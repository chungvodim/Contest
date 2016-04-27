using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookingGlass
{
    class Program
    {
        static void Main(string[] args)
        {
            var bitmap1 = (Bitmap)Bitmap.FromFile("alice_shocked.png");
            bitmap1.RotateFlip(RotateFlipType.RotateNoneFlipX);
            bitmap1.Save("flip_alice_shocked.png");
        }
    }
}
