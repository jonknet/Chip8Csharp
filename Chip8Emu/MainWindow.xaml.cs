using System.Windows;
using System.Windows.Input;

namespace Chip8Emu
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Keypad keypad;
        
        public MainWindow()
        {
            InitializeComponent();
            keypad = new Keypad();
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            keypad.keyDown(e.Key);
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            keypad.keyUp(e.Key);
        }
    }
}