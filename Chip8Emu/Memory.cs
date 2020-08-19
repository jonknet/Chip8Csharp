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
        
        private readonly ushort[] HEX_CHAR_ADDR =
        {
            0xFAF,
            0xFB4,
            0xFB9,
            0xFBE,
            0xFC3,
            0xFC8,
            0xFCD,
            0xFD2,
            0xFD7,
            0xFDC,
            0xFE1,
            0xFE6,
            0xFEB,
            0xFF0,
            0xFF5,
            0xFFA
        };

        public Memory()
        {
            ram = new byte[0xFFF];
            Array.Copy(HEX_CHARS,0,ram,0xFAF,80);
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