using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris {
    public class Vector2 {
        public float X = 0;
        public float Y = 0;

        public Vector2() { }

        public Vector2(Vector2 origin) {
            X = origin.X;
            Y = origin.Y;
        }

        public Vector2(float x, float y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return $"({X}, {Y})";
        }
    }
}
