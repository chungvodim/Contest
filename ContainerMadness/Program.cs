using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ContainerMadness
{
    class Program
    {
        static void Main(string[] args)
        {
            //TagLib.File tagFile = TagLib.File.Create("tuenti.mp3");
            //System.IO.File.WriteAllBytes("tuenti.png", tagFile.Tag.Pictures[0].Data.Data);
            //Console.WriteLine("done");

            Stream pngStream = new System.IO.FileStream("tuenti.png", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            PngBitmapDecoder pngDecoder = new PngBitmapDecoder(pngStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapFrame pngFrame = pngDecoder.Frames[0];
            InPlaceBitmapMetadataWriter pngInplace = pngFrame.CreateInPlaceBitmapMetadataWriter();
            //if (pngInplace.TrySave() == true)
            //{ 
            //    pngInplace.SetQuery("/Text/Description", "Have a nice day."); 
            //}
            Console.WriteLine(pngInplace.GetQuery("Next Level"));
            pngStream.Close();
        }
    }
}
