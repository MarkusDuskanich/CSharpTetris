using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris {
    public class TetrominoFactory{

        private static Random _rng = new Random();
        private static Vector2 _spawnPoint = new Vector2(GameObject.BlockSize * 3, -2 * GameObject.BlockSize);

        public static Tetromino Random {
            get {
                switch (_rng.Next(0, 7)) {
                    case 0: return T;
                    case 1: return I;
                    case 2: return J;
                    case 3: return L;
                    case 4: return Z;
                    case 5: return S;
                    default: return O;
                }
            }
        }

        public static Tetromino T => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.Purple, new List<List<int>>{
                new List<int>{ 0, 1, 0 },
                new List<int>{ 1, 1, 1 },
                new List<int>{ 0, 0, 0 },
        }, "T");

        public static Tetromino I => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.Cyan, new List<List<int>>{
                new List<int>{ 0, 0, 0, 0 },
                new List<int>{ 1, 1, 1, 1 },
                new List<int>{ 0, 0, 0, 0 },
                new List<int>{ 0, 0, 0, 0 },
        }, "I");

        public static Tetromino J => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.DarkBlue, new List<List<int>>{
                new List<int>{ 1, 0, 0 },
                new List<int>{ 1, 1, 1 },
                new List<int>{ 0, 0, 0 },
        }, "J");

        public static Tetromino L => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.Orange, new List<List<int>>{
                new List<int>{ 0, 0, 1 },
                new List<int>{ 1, 1, 1 },
                new List<int>{ 0, 0, 0 },
        }, "L");

        public static Tetromino O => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.Yellow, new List<List<int>>{
                new List<int>{ 0, 1, 1, 0 },
                new List<int>{ 0, 1, 1, 0 },
                new List<int>{ 0, 0, 0, 0 },
        }, "O");

        public static Tetromino S => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.Green, new List<List<int>>{
                new List<int>{ 0, 1, 1 },
                new List<int>{ 1, 1, 0 },
                new List<int>{ 0, 0, 0 },
        }, "S");

        public static Tetromino Z => new Tetromino(new Vector2(_spawnPoint.X, _spawnPoint.Y), Color.Red, new List<List<int>>{
                new List<int>{ 1, 1, 0 },
                new List<int>{ 0, 1, 1 },
                new List<int>{ 0, 0, 0 },
        }, "Z");
    }
}
