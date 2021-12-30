using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    class Mino : GameObject {
        public Mino(Vector2 position, Color color):base(position, color, null, "mino") { }

        public override void Draw(Graphics g) {
            g.FillRectangle(new SolidBrush(Color), new Rectangle((int)Position.X, (int)Position.Y, BlockSize, BlockSize));
        }
    }
}
