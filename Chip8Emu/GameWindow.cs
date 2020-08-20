using System.Collections.Generic;
using System.Windows;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Window = SFML.Window.Window;

namespace Chip8Emu
{
    public class GameWindow 
    {
        
        RenderWindow hWind;
        const int width = 320;
        const int height = 160;
        
        RectangleShape blackRect;
        RectangleShape whiteRect;
        
        List<RectangleShape>[] rectArray; 

        public GameWindow() 
        {
            hWind = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(320, 160), "Chip8 in C#");
            hWind.Clear();
            rectArray = new List<RectangleShape>[32];
            for(int i = 0; i < 32; i++)
            {
                rectArray[i] = new List<RectangleShape>(64);
                for(int j = 0; j < 64; j++)
                {
                    var rect = new RectangleShape(new Vector2f(5,5)){ FillColor = Color.White };
                    rect.Position = new Vector2f(j*5,i*5);
                    rectArray[i].Add(rect);
                    hWind.Draw(rect);
                    
                }
            }
            hWind.Display();
            
        }
        
        public void updateWindow()
        {
            hWind.Clear();
            
            for(int i = 0; i < 32; i++)
            {
                foreach(RectangleShape r in rectArray[i])
                {
                    hWind.Draw(r);
                }
            }
            
            hWind.DispatchEvents();
            hWind.Display();
        }
        
        public void drawPixel(int x, int y, bool black)
        {
            
            var rect = rectArray[y][x];
            if(black){
                rect.FillColor = Color.Black;
            } else {
                rect.FillColor = Color.White;
            }
            
            //hWind.Display();
            
        }
        
    }
}