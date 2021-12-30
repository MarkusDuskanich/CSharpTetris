using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Tetris {
    class Program {
        static void Main(string[] args) {

            Console.WriteLine("Left: A");
            Console.WriteLine("Right: D");
            Console.WriteLine("Soft Drop: S");
            Console.WriteLine("Instant Drop: W");
            Console.WriteLine("Rotate Left: Q");
            Console.WriteLine("Rotate Right: E");

            new Game();
        }
    }
}
