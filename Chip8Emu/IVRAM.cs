namespace Chip8Emu
{
    public interface IVRAM
    {
        byte getPixel(int x, int y);

        bool setPixel(int x, int y, byte b);

        bool setByte(int x, int y, byte b);

        bool drawSprite(int x, int y, byte[] s);

        void clearScreen();
    }
}