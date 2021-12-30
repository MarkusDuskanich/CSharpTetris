using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tetris {
    public class Game : Engine2D {

        private Tetromino _activePiece;
        private Vector2 _lastPosition;
        private readonly Stopwatch _dropTimer = new Stopwatch();
        private readonly Stopwatch _touchDownTimer = new Stopwatch();
        private Score _score;

        public static readonly int WellHight = GameObject.BlockSize * 22;
        public static readonly int WellWidth = GameObject.BlockSize * 10;

        private bool _left;
        private bool _right;
        private bool _softDrop;
        private bool _rr;
        private bool _lr;
        private bool _instantDrop;


        public Game() : base(new Vector2(WellWidth * 3, WellHight + 6 * GameObject.BlockSize), "Tetris in c#") { }

        public override void OnLoad() {
            _activePiece = TetrominoFactory.Random;
            _score = new Score(new Vector2(WellWidth + GameObject.BlockSize, 0));
            _lastPosition = new Vector2(_activePiece.Position);
            _dropTimer.Start();
            new Well(WellHight, WellWidth, Color.White);
        }

        public override void OnUpdate() {
            GravityDrop();
            UpdateTouchdownTimer();
            HandleInput();
            HandleTranslationCollision();
            StackPieceIfGroundReached();
            UpdateScore();
            CheckGameOver();
        }

        private void UpdateScore() {
            _score += AdditionalPoints();
        }

        private int AdditionalPoints() {
            int clearedRows = FindAndRemoveFullRows();
            return clearedRows * clearedRows * 100;
        }

        private int FindAndRemoveFullRows() {
            List<int> fullRows = GetFullRows();
            if (ThereAreFullRows(fullRows)) {
                fullRows.Sort();
                RemoveFullRows(fullRows);
                MoveRemainingRowsDown(fullRows);
                return fullRows.Count;
            }
            return 0;
        }

        private static bool ThereAreFullRows(List<int> fullRows) {
            return fullRows != null;
        }

        private static void MoveRemainingRowsDown(List<int> fullRows) {
            foreach (var row in fullRows) {
                lock (GameObjectLock) {
                    GameObjects.ForEach((o) => {
                        if (o.Tag == "mino" && (int)o.Position.Y / GameObject.BlockSize < row) {
                            o.Position.Y += GameObject.BlockSize;
                        }
                    }); 
                }
            }
        }

        private static void RemoveFullRows(List<int> fullRows) {
            foreach (var row in fullRows) {
                lock (GameObjectLock) {
                    GameObjects.RemoveAll(item => item.Tag == "mino" && (int)item.Position.Y / GameObject.BlockSize == row); 
                }
            }
        }

        private void CheckGameOver() {
            if (gameOver()) {
                Console.WriteLine("Game over");
                GameOver = true;
            }
        }

        private void StackPieceIfGroundReached() {
            if (_activePiece.HasTouchedGround()) {
                if (!_touchDownTimer.IsRunning)
                    _touchDownTimer.Start();
                if (ActivePieceCanBeLocked()) {
                    _activePiece.Unregister();
                    _activePiece.Split();
                    _activePiece = TetrominoFactory.Random;
                    _touchDownTimer.Reset();
                    _instantDrop = false;
                }
            }
        }

        private bool ActivePieceCanBeLocked() {
            return _touchDownTimer.ElapsedMilliseconds > 700 || _instantDrop;
        }

        private void HandleTranslationCollision() {
            if (_activePiece.IsColliding())
                _activePiece.Position = new Vector2(_lastPosition);
            else
                _lastPosition = new Vector2(_activePiece.Position);
        }

        private void UpdateTouchdownTimer() {
            if (TouchDownTimerRequiresRestart()) {
                _touchDownTimer.Restart();
            }
        }

        private bool TouchDownTimerRequiresRestart() {
            return _touchDownTimer.IsRunning && (_left || _right || _rr || _lr);
        }

        private void HandleInput() {
            if (_left) {
                _activePiece.Position.X -= GameObject.BlockSize;
                _left = false;
            } else if (_right) {
                _activePiece.Position.X += GameObject.BlockSize;
                _right = false;
            } else if (_softDrop) {
                _activePiece.Position.Y += GameObject.BlockSize;
                _softDrop = false;
            } else if (_rr && _activePiece.Tag != "O") {
                RotateRightHandleCollision();
                _rr = false;
            } else if (_lr && _activePiece.Tag != "O") {
                RotateLeftHandleCollision();
                _lr = false;
            } else if (_instantDrop) {
                InstantDrop();
            }
        }

        private void InstantDrop() {
            while (!_activePiece.IsColliding()) {
                _activePiece.Position.Y += GameObject.BlockSize;
            }
            _activePiece.Position.Y -= GameObject.BlockSize;
        }

        private void RotateLeftHandleCollision() {
            _activePiece.LeftRotation();
            if (_activePiece.IsColliding())
                _activePiece.RightRotation();
        }

        private void RotateRightHandleCollision() {
            _activePiece.RightRotation();
            if (_activePiece.IsColliding())
                _activePiece.LeftRotation();
        }

        private void GravityDrop() {
            if (_dropTimer.ElapsedMilliseconds > 1000) {
                _activePiece.Position.Y += GameObject.BlockSize;
                _dropTimer.Restart();
            }
        }

        private List<int> GetFullRows() {
            Dictionary<int, int> rowMinoCount = GetMinosCountForEachRow();
            List<int> fullRows = new List<int>();
            foreach (var keyValuePair in rowMinoCount) {
                if (keyValuePair.Value == WellWidth / GameObject.BlockSize)
                    fullRows.Add(keyValuePair.Key);
            }
            return fullRows.Count > 0 ? fullRows : null;
        }

        private static Dictionary<int, int> GetMinosCountForEachRow() {
            Dictionary<int, int> rowMinoCount = new Dictionary<int, int>();
            lock (GameObjectLock) {
                foreach (var o in GameObjects) {
                    if (o.Tag != "mino")
                        continue;
                    if (!rowMinoCount.ContainsKey((int)o.Position.Y / GameObject.BlockSize))
                        rowMinoCount.Add((int)o.Position.Y / GameObject.BlockSize, 1);
                    else
                        rowMinoCount[(int)o.Position.Y / GameObject.BlockSize]++;
                } 
            }

            return rowMinoCount;
        }

        private bool gameOver() {
            lock (GameObjectLock) {
                foreach (var o in GameObjects) {
                    if (o.Tag != "mino")
                        continue;
                    if (o.Position.Y < 0)
                        return true;
                }   
            }    
            return false;
        }

        public override void GetKeyDown(KeyEventArgs e) {
            if (e.KeyCode == Keys.A) _left = true;
            if (e.KeyCode == Keys.D) _right = true;
            if (e.KeyCode == Keys.S) _softDrop = true;
            if (e.KeyCode == Keys.Q) _lr = true;
            if (e.KeyCode == Keys.E) _rr = true;
            if (e.KeyCode == Keys.W) _instantDrop = true;
        }

        public override void GetKeyUp(KeyEventArgs e) {
            if (e.KeyCode == Keys.A) _left = false;
            if (e.KeyCode == Keys.D) _right = false;
            if (e.KeyCode == Keys.S) _softDrop = false;
            if (e.KeyCode == Keys.Q) _lr = false;
            if (e.KeyCode == Keys.E) _rr = false;
            if (e.KeyCode == Keys.W) _instantDrop = false;
        }
    }
}
