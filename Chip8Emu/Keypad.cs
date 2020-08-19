using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Chip8Emu
{
    public class Keypad
    {
        private readonly Key[] keyMap =
        {
            Key.D0, Key.D1, Key.D2, Key.D3,
            Key.D4, Key.D5, Key.D6, Key.D7,
            Key.D8, Key.D9, Key.A, Key.B,
            Key.C, Key.D, Key.E, Key.F
        };

        private bool[] keyStateDown = new bool[16];

        private bool waitForInput = false;
        private Key lastKeyPressed;
        
        public bool quitReceived = false;
        
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
            if(Keyboard.IsKeyDown(Key.D1))
            {
                keyStateDown[1] = true;
            } else if(Keyboard.IsKeyDown(Key.D2))
            {
                keyStateDown[2] = true;
            } else if(Keyboard.IsKeyDown(Key.D3))
            {
                keyStateDown[3] = true;
            } else if(Keyboard.IsKeyDown(Key.D4))
            {
                keyStateDown[0xC] = true;
            } else if(Keyboard.IsKeyDown(Key.Q))
            {
                keyStateDown[4] = true;
            } else if(Keyboard.IsKeyDown(Key.W))
            {
                keyStateDown[5] = true;
            } else if(Keyboard.IsKeyDown(Key.E))
            {
                keyStateDown[6] = true;
            } else if(Keyboard.IsKeyDown(Key.R))
            {
                keyStateDown[0xD] = true;
            } else if(Keyboard.IsKeyDown(Key.A))
            {
                keyStateDown[7] = true;
            } else if(Keyboard.IsKeyDown(Key.S))
            {
                keyStateDown[8] = true;
            } else if(Keyboard.IsKeyDown(Key.D))
            {
                keyStateDown[9] = true;
            } else if(Keyboard.IsKeyDown(Key.F))
            {
                keyStateDown[0xE] = true;
            } else if(Keyboard.IsKeyDown(Key.Z))
            {
                keyStateDown[0xA] = true;
            } else if(Keyboard.IsKeyDown(Key.X))
            {
                keyStateDown[0] = true;
            } else if(Keyboard.IsKeyDown(Key.C))
            {
                keyStateDown[0xB] = true;
            } else if(Keyboard.IsKeyDown(Key.V))
            {
                keyStateDown[0xF] = true;
            } 
            
            if(Keyboard.IsKeyDown(Key.P))
            {
                quitReceived = true;
            }
        }

        public void keyDown(Key k)
        {
            Console.WriteLine("keyDown " + k);
            if ((int) k > 33 && (int) k < 50)
            {
                waitForInput = false;
                lastKeyPressed = k;
                keyStateDown[(int) k - 34] = true;
            }
        }

        public void keyUp(Key k)
        {
            waitForInput = false;
            Console.WriteLine("keyUp " + k);
            if ((int) k > 33 && (int) k < 50)
            {
                waitForInput = false;
                lastKeyPressed = k;
                keyStateDown[(int) k - 34] = false;
            }          
        }

        public bool getKeyState(byte k)
        {
            return keyStateDown[k];
        }

        public byte waitForKey()
        {
            waitForInput = true;
            while (waitForInput)
            {
                // wait here
            }

            return (byte)(lastKeyPressed - 34);
        }
    }
}