using System;

namespace Chip8Emu
{
    public class VRAM : IVRAM
    {
        private readonly byte[,] vram;
        public bool change = false;

        public VRAM()
        {
            vram = new byte[32, 64];
        }

        public byte getPixel(int x, int y)
        {
            return vram[y, x];
        }

        public bool setPixel(int x, int y, byte b)
        {
            change = true;
            var ret = false;
            if (b > 0) b = 1;
            if (vram[y, x] == 1 && b == 1) ret = true;
            vram[y, x] ^= b;
            return ret;
        }

        public bool setByte(int x, int y, byte b)
        {
            change = true;
            var ret = false;
            var mask = 128;
            for (var i = 0; i < 8; i++)
            {
                if (x > 63) x = 0;
                if (setPixel(x, y, (byte) (b & mask))) ret = true;
                x++;
                mask = mask >> 1;
            }

            return ret;
        }

        public bool drawSprite(int x, int y, byte[] s)
        {
            change = true;
            var ret = false;
            for (var i = 0; i < s.Length; i++)
            {
                if (y > 31) y = 0;
                if (setByte(x, y, s[i])) ret = true;
                y++;
            }

            return ret;
        }

        public void clearScreen()
        {
            change = true;
            for (var i = 0; i < 32; i++)
            for (var j = 0; j < 64; j++)
                vram[i, j] = 0;
        }

        public void _outputToConsole()
        {
            for (var i = 0; i < 32; i++)
            {
                for (var j = 0; j < 64; j++)
                    if (vram[i, j] == 1)
                        Console.Write('*');
                    else
                        Console.Write('O');
                Console.WriteLine();
            }
        }
    }
}