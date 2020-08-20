using System;

namespace Chip8Emu
{
    public class Memory : IMemory
    {
        private readonly byte[] ram;
        
        private readonly byte[] HEX_CHARS =
        {
            0xF0,0x90,0x90,0x90,0xF0,
            0x20,0x60,0x20,0x20,0x70,
            0xF0,0x10,0xF0,0x80,0xF0,
            0xF0,0x10,0xF0,0x10,0xF0,
            0x90,0x90,0xF0,0x10,0x10,
            0xF0,0x80,0xF0,0x10,0xF0,
            0xF0,0x80,0xF0,0x90,0xF0,
            0xF0,0x10,0x20,0x40,0x40,
            0xF0,0x90,0xF0,0x90,0xF0,
            0xF0,0x90,0xF0,0x10,0xF0,
            0xF0,0x90,0xF0,0x90,0x90,
            0xE0,0x90,0xE0,0x90,0xE0,
            0xF0,0x80,0x80,0x80,0xF0,
            0xE0,0x90,0x90,0x90,0xE0,
            0xF0,0x80,0xF0,0x80,0xF0,
            0xF0,0x80,0xF0,0x80,0x80
        };
        
        public static readonly ushort[] HEX_CHAR_ADDR =
        {
            0x1AF,
            0x1B4,
            0x1B9,
            0x1BE,
            0x1C3,
            0x1C8,
            0x1CD,
            0x1D2,
            0x1D7,
            0x1DC,
            0x1E1,
            0x1E6,
            0x1EB,
            0x1F0,
            0x1F5,
            0x1FA
        };

        public Memory()
        {
            ram = new byte[0xFFF];
            Array.Copy(HEX_CHARS,0,ram,0x1AF,80);
        }

        public byte readByte(ushort addr)
        {
            return ram[addr];
        }

        public ushort readWord(ushort addr)
        {
            return (ushort) ((ram[addr] << 8) | ram[addr + 1]);
        }

        public byte[] readBytes(ushort addr, int num)
        {
            var o = new byte[num];
            for (var i = 0; i < num; i++) o[i] = ram[addr + i];
            return o;
        }

        public bool writeByte(ushort addr, byte b)
        {
            ram[addr] = b;
            return true;
        }

        public bool writeBytes(ushort addr, byte[] b)
        {
            for (var i = 0; i < b.Length; i++) ram[addr + i] = b[i];
            return true;
        }
    }
}