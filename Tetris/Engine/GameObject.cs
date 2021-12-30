using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tetris {
    public abstract class GameObject {

        public Vector2 Position;
        public static readonly int BlockSize = 25;
        public Color Color;
        public string Tag;
        public List<List<int>> Structure;

        public GameObject(Vector2 position, Color color, List<List<int>> structure, string tag) {
            Position = position;
            Color = color;
            Structure = structure;
            Tag = tag;
            Register();
        }

        public void Register() {
            Engine2D.RegisterObject(this);
        }

        public void Unregister() {
            Engine2D.UnregisterObject(this);
        }

        abstract public void Draw(Graphics g);

        public void RightRotation() {
            if (!StructureExists()) 
                return;
            Transpose();
            InvertRows();
        }

        public void LeftRotation() {
            if (!StructureExists()) 
                return;
            InvertRows();
            Transpose();
        }

        private void Transpose() {
            Structure = Structure
                        .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                        .GroupBy(i => i.index, i => i.item)
                        .Select(g => g.ToList())
                        .ToList();
        }

        private void InvertRows() {
            foreach (var list in Structure) {
                list.Reverse();
            }
        }

        private bool StructureExists() {
            return Structure != null;
        }

    }
}
