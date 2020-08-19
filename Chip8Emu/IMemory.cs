namespace Chip8Emu
{
    public interface IMemory
    {
        byte readByte(ushort addr);
        ushort readWord(ushort addr);
        byte[] readBytes(ushort addr, int num);

        bool writeByte(ushort addr, byte b);
        bool writeBytes(ushort addr, byte[] b);
    }
}