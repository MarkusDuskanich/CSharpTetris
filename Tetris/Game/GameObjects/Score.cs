using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public class Score : GameObject {

        private int _points;

        public Score(Vector2 position):base(position, default, null, "score") { }

        public override void Draw(Graphics g) {
            g.DrawString("Score: " + _points.ToString() + (Engine2D.GameOver ? "\nGame Over" : ""), new Font("Consolas", 16), new SolidBrush(Color.White), Position.X, Position.Y);
        }

        public static Score operator + (Score left, int right) {
            left._points += right;
            return left;
        }
    }
}
