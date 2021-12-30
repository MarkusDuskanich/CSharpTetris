using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris {
    public class Tetromino : GameObject {

        public Tetromino(Vector2 position, Color color, List<List<int>> structure, string tag) : base(position, color, structure, tag) { }

        public override void Draw(Graphics g) {
            for (int i = 0; i < Structure.Count; i++)
                for (int j = 0; j < Structure[i].Count; j++)
                    if (Structure[i][j] != 0)
                        g.FillRectangle(new SolidBrush(Color), new Rectangle((int)Position.X + j * BlockSize, (int)Position.Y + i * BlockSize, BlockSize, BlockSize));
        }

        public void Split() {
            for (int i = 0; i < Structure.Count; i++) {
                for (int j = 0; j < Structure[i].Count; j++) {
                    if (Structure[i][j] == 0) 
                        continue;
                    int x = (int)Position.X + j * BlockSize;
                    int y = (int)Position.Y + i * BlockSize;
                    new Mino(new Vector2(x, y), Color);
                }
            }
        }

        public bool HasTouchedGround() {
            return FloorContact() || OnTopOfPieces();
        }

        private bool OnTopOfPieces() {
            lock (Engine2D.GameObjectLock) {
                foreach (var o in Engine2D.GameObjects) {
                    if (o.Tag != "mino")
                        continue;
                    for (int i = 0; i < Structure.Count; i++) {
                        for (int j = 0; j < Structure[i].Count; j++) {
                            if (Structure[i][j] == 0)
                                continue;
                            int x = (int)Position.X + j * BlockSize;
                            int y = (int)Position.Y + i * BlockSize + BlockSize;
                            if (y == o.Position.Y && x == o.Position.X)
                                return true;
                        }
                    }
                } 
            }
            return false;
        }

        private bool FloorContact() {
            for (int i = 0; i < Structure.Count; i++) {
                for (int j = 0; j < Structure[i].Count; j++) {
                    if (Structure[i][j] == 0)
                        continue;
                    int y = (int)Position.Y + i * BlockSize;
                    if (y + BlockSize == Game.WellHight)
                        return true;
                }
            }
            return false;
        }

        public bool IsColliding() {
            return WellCollision() || MinoCollision();
        }

        private bool MinoCollision() {
            lock (Engine2D.GameObjectLock) {
                foreach (var o in Engine2D.GameObjects) {
                    if (o.Tag != "mino")
                        continue;
                    for (int i = 0; i < Structure.Count; i++) {
                        for (int j = 0; j < Structure[i].Count; j++) {
                            if (Structure[i][j] == 0)
                                continue;
                            int x = (int)Position.X + j * BlockSize;
                            int y = (int)Position.Y + i * BlockSize;
                            if (x < o.Position.X + BlockSize && x + BlockSize > o.Position.X && y < o.Position.Y + BlockSize && y + BlockSize > o.Position.Y)
                                return true;
                        }
                    }
                } 
            }
            return false;
        }

        private bool WellCollision() {
            for (int i = 0; i < Structure.Count; i++) {
                for (int j = 0; j < Structure[i].Count; j++) {
                    if (Structure[i][j] == 0)
                        continue;
                    int x = (int)Position.X + j * BlockSize;
                    int y = (int)Position.Y + i * BlockSize;
                    if (x < 0 || x + BlockSize > Game.WellWidth || y + BlockSize > Game.WellHight)
                        return true;
                }
            }
            return false;
        }
    }
}
