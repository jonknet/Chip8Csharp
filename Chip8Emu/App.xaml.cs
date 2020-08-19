using System;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Threading;
using SkiaSharp;


namespace Chip8Emu
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Chip8_Start(object sender, StartupEventArgs e)
        {
            Memory mem = new Memory();
            VRAM vram = new VRAM();
            Keypad keypad = new Keypad();
            CPU cpu = new CPU(mem,vram,keypad);
            
            var path = @"C:\Users\Commvault\RiderProjects\Chip8Emu\Chip8Emu\IBM Logo.ch8";
            
            Loader.Load(path,mem);
            
            // Main Loop
            while(!keypad.quitReceived){
                keypad.pollKeyState();
                
                cpu.executeNext();

                if(vram.change)
                {
                    Console.Clear();
                    vram._outputToConsole();
                    vram.change = false;
                }
                
                Thread.Sleep(500);
            }
            this.Shutdown();
        }
        
    }
}