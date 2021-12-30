using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Tetris {

    class Canvas : Form {
        public Canvas() {
            DoubleBuffered = true;
        }
    }

    public abstract class Engine2D {
        private readonly Vector2 _screenSize;
        private readonly string _title;
        private readonly Canvas _window;
        private readonly Thread t_gameLoop;
        public static List<GameObject> GameObjects = new List<GameObject>();
        public static readonly object GameObjectLock = new object();
        public static bool GameOver = false;

        public Engine2D(Vector2 screenSize, string title) {
            _screenSize = screenSize;
            _title = title;
            _window = new Canvas();
            t_gameLoop = new Thread(GameLoop);

            WindowSetup();
            ThreadSetup();
            OpenWindow();
        }

        public static void RegisterObject(GameObject gameObject) {
            lock (GameObjectLock) {
                GameObjects.Add(gameObject);
            }
        }

        public static void UnregisterObject(GameObject gameObject) {
            lock (GameObjectLock) {
                GameObjects.Remove(gameObject); 
            }
        }

        private void OpenWindow() {
            Application.Run(_window);
        }

        private void ThreadSetup() {
            t_gameLoop.IsBackground = true;
            t_gameLoop.Start();
        }

        private void WindowSetup() {
            _window.Size = new Size((int)_screenSize.X, (int)_screenSize.Y);
            _window.Text = _title;
            _window.Paint += Renderer;
            _window.KeyDown += Window_KeyDown;
            _window.KeyUp += Window_KeyUp;
            _window.FormClosed += Window_FormClosed;
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e) {
            t_gameLoop.Abort();
        }

        private void GameLoop() {
            OnLoad();
            while (t_gameLoop.IsAlive && !GameOver) {
                try {
                    Redraw();
                    OnUpdate();
                    Thread.Sleep(1);
                } catch (InvalidOperationException) { }
            }
        }

        private void Redraw() {
            _window.BeginInvoke((MethodInvoker)delegate { _window.Refresh(); });
        }

        private void Renderer(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);
            g.TranslateTransform(Game.WellWidth - 9, 0);
            lock (GameObjectLock) {
                foreach (var gameObject in GameObjects)
                    gameObject.Draw(g);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e) {
            GetKeyUp(e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            GetKeyDown(e);
        }

        public abstract void OnUpdate();
        public abstract void OnLoad();

        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);
    }
}
