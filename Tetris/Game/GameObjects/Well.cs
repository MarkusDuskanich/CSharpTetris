using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public class Well : GameObject{
        private int _hight;
        private int _width;
        public int Hight => _hight;
        public int Width => _width;

        public Well(int hight, int widht, Color color):base(null, color, null, "well") {
            _hight = hight;
            _width = widht;
        }

        public override void Draw(Graphics g) {
            g.DrawLine(new Pen(new SolidBrush(Color), 1), -1, 0, -1, _hight);
            g.DrawLine(new Pen(new SolidBrush(Color), 1), _width, 0, _width, _hight);
            g.DrawLine(new Pen(new SolidBrush(Color), 1), -1, _hight, _width, _hight);
        }
    }
}
