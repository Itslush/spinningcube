using System.Numerics;
using System.Timers;

namespace SpinningCubeAttempt
{
    public partial class Cube : Form
    {
        private readonly Vector3[] nodes = {
            new Vector3(-50, -50, -50), new Vector3(50, -50, -50),
            new Vector3(-50, 50, -50), new Vector3(50, 50, -50),

            new Vector3(-50, -50, 50), new Vector3(50, -50, 50),
            new Vector3(-50, 50, 50), new Vector3(50, 50, 50)
        };

        private readonly int[][] edges = {
            new int[] {0, 1}, new int[] {1, 3},
            new int[] {3, 2}, new int[] {2, 0},

            new int[] {4, 5}, new int[] {5, 7},
            new int[] {7, 6}, new int[] {6, 4},

            new int[] {0, 4}, new int[] {1, 5},
            new int[] {2, 6}, new int[] {3, 7}
        };

        private void RotateNode(ref Vector3 node, float angleX, float angleY, float angleZ)
        {
            float sinX = (float)Math.Sin(angleX);
            float sinY = (float)Math.Sin(angleY);
            float sinZ = (float)Math.Sin(angleZ);

            float cosX = (float)Math.Cos(angleX);
            float cosY = (float)Math.Cos(angleY);
            float cosZ = (float)Math.Cos(angleZ);

            float y = node.Y * cosX - node.Z * sinX;
            float z = node.Y * sinX + node.Z * cosX;
            node.Y = y;
            node.Z = z;

            float x = node.X * cosY + node.Z * sinY;
            z = -node.X * sinY + node.Z * cosY;
            node.X = x;
            node.Z = z;

            x = node.X * cosZ - node.Y * sinZ;
            y = node.X * sinZ + node.Y * cosZ;
            node.X = x;
            node.Y = y;
        }

        private void MatriceCubeRot(float angleX, float angleY, float angleZ)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                RotateNode(ref nodes[i], angleX, angleY, angleZ);
            }
        }

        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private void Timer_Elapsed(object? sender, ElapsedEventArgs args)
        {
            MatriceCubeRot(0.001f, 0.001f, 0.001f);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            base.OnPaint(args);
            var graph = args.Graphics;
            var pencil = new Pen(Color.Orange, 3);

            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            graph.TranslateTransform(Width / 2, Height / 2);

            foreach (var edge in edges)
            {
                Vector3 P1 = nodes[edge[0]];
                Vector3 P2 = nodes[edge[1]];

                graph.DrawLine(pencil, P1.X, P1.Y, P2.X, P2.Y);
            }
        }

        public Cube()
        {
            InitializeComponent();
            Width = Height = 640;
            StartPosition = FormStartPosition.CenterScreen;

            timer.Interval = 1;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
    }
}
