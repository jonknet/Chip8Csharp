using System;
using System.IO;
using System.Windows.Media.Animation;

namespace Chip8Emu
{
    public class Loader
    {
        public static void Load(string path, IMemory mem)
        {
            FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read);
            var b = new byte[fs.Length];
            fs.Read(b,0,(int)fs.Length);
            mem.writeBytes(0x200,b);
        }
    }
}