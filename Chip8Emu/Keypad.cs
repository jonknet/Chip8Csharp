using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Chip8Emu
{
    public class Keypad
    {
        private bool[] keyStateDown = new bool[16];

        private bool waitForInput = false;
        private byte lastKeyPressed = 0xFF;
        public bool quitReceived = false;
        public bool pause = false;
        
        private void clearKeyStates()
        {
            for(int i = 0; i < 16; i++)
            {
                keyStateDown[i] = false;
            }
        }
        
        public void pollKeyState()
        {
            clearKeyStates();
            lastKeyPressed = 0xFF;
            if(Keyboard.IsKeyDown(Key.D1))
            {
                keyStateDown[1] = true;
                lastKeyPressed = 1;
            } else if(Keyboard.IsKeyDown(Key.D2))
            {
                keyStateDown[2] = true;
                lastKeyPressed = 2;
            } else if(Keyboard.IsKeyDown(Key.D3))
            {
                keyStateDown[3] = true;
                lastKeyPressed = 3;
            } else if(Keyboard.IsKeyDown(Key.D4))
            {
                keyStateDown[0xC] = true;
                lastKeyPressed = 0xC;
            } else if(Keyboard.IsKeyDown(Key.Q))
            {
                keyStateDown[4] = true;
                lastKeyPressed = 4;
            } else if(Keyboard.IsKeyDown(Key.W))
            {
                keyStateDown[5] = true;
                lastKeyPressed = 5;
            } else if(Keyboard.IsKeyDown(Key.E))
            {
                keyStateDown[6] = true;
                lastKeyPressed = 6;
            } else if(Keyboard.IsKeyDown(Key.R))
            {
                keyStateDown[0xD] = true;
                lastKeyPressed = 0xD;
            } else if(Keyboard.IsKeyDown(Key.A))
            {
                keyStateDown[7] = true;
                lastKeyPressed = 7;
            } else if(Keyboard.IsKeyDown(Key.S))
            {
                keyStateDown[8] = true;
                lastKeyPressed = 8;
            } else if(Keyboard.IsKeyDown(Key.D))
            {
                keyStateDown[9] = true;
                lastKeyPressed = 9;
            } else if(Keyboard.IsKeyDown(Key.F))
            {
                keyStateDown[0xE] = true;
                lastKeyPressed = 0xE;
            } else if(Keyboard.IsKeyDown(Key.Z))
            {
                keyStateDown[0xA] = true;
                lastKeyPressed = 0xA;
            } else if(Keyboard.IsKeyDown(Key.X))
            {
                keyStateDown[0] = true;
                lastKeyPressed = 0;
            } else if(Keyboard.IsKeyDown(Key.C))
            {
                keyStateDown[0xB] = true;
                lastKeyPressed = 0xB;
            } else if(Keyboard.IsKeyDown(Key.V))
            {
                keyStateDown[0xF] = true;
                lastKeyPressed = 0xF;
            } 
            
            if(lastKeyPressed != 0xFF)
            {
                Console.WriteLine("Last Key Pressed: " + lastKeyPressed);
            }
            
            if(Keyboard.IsKeyDown(Key.P))
            {
                quitReceived = true;
            }
            
            if(Keyboard.IsKeyDown(Key.B))
            {
                pause = true;
            }
            
            if(Keyboard.IsKeyDown(Key.N))
            {
                pause = false;
            }
        }

        public bool getKeyState(byte k)
        {
            return keyStateDown[k];
        }
        
        public byte getCurrentPressedKey()
        {
            return lastKeyPressed;
        }
    }
}