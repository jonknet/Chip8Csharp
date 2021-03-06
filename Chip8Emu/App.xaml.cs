﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Threading;




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
            
            var path = @"C:\Users\Commvault\RiderProjects\Chip8Emu\Chip8Emu\Pong (1 Player).ch8";
            
            Loader.Load(path,mem);
            
            var gWind = new GameWindow();

            // Main Loop
            while(!keypad.quitReceived){
                keypad.pollKeyState();
                
                if(!keypad.pause){
                    cpu.executeNext();

                    if(vram.change)
                    {
                        for(int i = 0; i < 32; i++)
                        {
                            for(int j = 0; j < 64; j++)
                            {
                                if(vram.getPixel(j,i) == 1)
                                    gWind.drawPixel(j,i, true);
                                else
                                    gWind.drawPixel(j,i,false);
                            }
                        }
                        vram.change = false;
                    }
                }
                gWind.updateWindow();
                
                Thread.Sleep(16);
            }
            
            this.Shutdown();
        }
        
    }
}