namespace Chip8Emu
{
    internal class Program
    {
        private static void Test()
        {
            var vram = new VRAM();
            for (var i = 0; i < 32; i++)
            for (var j = 0; j < 64; j++)
                vram.setPixel(j, i, 1);

            vram._outputToConsole();
        }
    }
}