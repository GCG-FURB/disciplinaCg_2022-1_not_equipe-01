//#define CG_Privado // código do professor.
#define CG_Gizmo  // debugar gráfico.
#define CG_Debug // debugar texto.
#define CG_OpenGL // render OpenGL.
//#define CG_DirectX // render DirectX.

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;


namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private float fovy, aspect, near, far;
        private Vector3 eye, at, up;

        private CameraOrtho camera = new CameraOrtho();
        protected List<ObjetoGeometria> objetosLista = new List<ObjetoGeometria>();
        private ObjetoGeometria objetoSelecionado = null;
        private ObjetoGeometria objetoCriando = null;
        private char objetoId = '@';
        private bool bBoxDesenhar = false;
        private bool desenhandoObjeto = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
        private bool mouseMoverPto2 = false;
        private int indexPontoMover = 0;
        private Tabuleiro obj_Tabuleiro;
        private Retangulo obj_Retangulo;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif



        private void DesenhaCubo()
        {
            GL.Color3(0.0f, 0.0f, 255.0f);
            GL.Begin(PrimitiveType.Quads);

            // Face da frente
            GL.Normal3(0, 0, 20);
            GL.TexCoord2(0.0f, 10.0f); GL.Vertex3(-10.0f, -10.0f, 10.0f);
            GL.TexCoord2(10.0f, 10.0f); GL.Vertex3(10.0f, -10.0f, 10.0f);
            GL.TexCoord2(10.0f, 0.0f); GL.Vertex3(10.0f, 10.0f, 10.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-10.0f, 10.0f, 10.0f);
            // Face do fundo
            GL.Normal3(0, 0, -20);
            GL.TexCoord2(0.0f, 50.0f); GL.Vertex3(-50.0f, -50.0f, -50.0f);
            GL.TexCoord2(50.0f, 50.0f); GL.Vertex3(-50.0f, 50.0f, -50.0f);
            GL.TexCoord2(50.0f, 0.0f); GL.Vertex3(50.0f, 50.0f, -50.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(50.0f, -50.0f, -50.0f);
            // Face de cima
            GL.Normal3(0, 20, 0);
            GL.TexCoord2(0.0f, 10.0f); GL.Vertex3(-10.0f, 10.0f, 10.0f);
            GL.TexCoord2(10.0f, 10.0f); GL.Vertex3(10.0f, 10.0f, 10.0f);
            GL.TexCoord2(10.0f, 0.0f); GL.Vertex3(10.0f, 10.0f, -10.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-10.0f, 10.0f, -10.0f);
            // Face de baixo
            GL.Normal3(0, -20, 0);
            GL.TexCoord2(0.0f, 10.0f); GL.Vertex3(-10.0f, -10.0f, 10.0f);
            GL.TexCoord2(10.0f, 10.0f); GL.Vertex3(-10.0f, -10.0f, -10.0f);
            GL.TexCoord2(10.0f, 0.0f); GL.Vertex3(10.0f, -10.0f, -10.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(10.0f, -10.0f, 10.0f);
            // Face da direita
            GL.Normal3(-20, -10, -10);
            GL.TexCoord2(0.0f, 100.0f); GL.Vertex3(50.0f, -50.0f, 50.0f);
            GL.TexCoord2(50.0f, 50.0f); GL.Vertex3(50.0f, -50.0f, -50.0f);
            GL.TexCoord2(50.0f, 0.0f); GL.Vertex3(50.0f, 50.0f, -50.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(50.0f, 50.0f, 50.0f);
            // Face da esquerda
            GL.Normal3(-20, 0, 0);
            GL.TexCoord2(0.0f, 10.0f); GL.Vertex3(-10.0f, -10.0f, 10.0f);
            GL.TexCoord2(10.0f, 10.0f); GL.Vertex3(-10.0f, 10.0f, 10.0f);
            GL.TexCoord2(10.0f, 0.0f); GL.Vertex3(-10.0f, 10.0f, -10.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-10.0f, -10.0f, -10.0f);

            GL.End();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            // ___ parâmetros da câmera sintética
            fovy = (float)Math.PI / 4;
            aspect = Width / (float)Height;
            near = 1.0f;
            far = 1000.0f;
            eye = new Vector3(290, 0, 0);
            at = new Vector3(0, 0, 0);
            up = new Vector3(0, 1, 0);

            objetoId = Utilitario.charProximo(objetoId);
            obj_Tabuleiro = new Tabuleiro(objetoId, null, 7, 6);
            obj_Tabuleiro.ObjetoCor.CorR = 255; obj_Tabuleiro.ObjetoCor.CorG = 0; obj_Tabuleiro.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_Tabuleiro);
            objetoSelecionado = obj_Tabuleiro;

            objetoId = Utilitario.charProximo(objetoId);
            obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
            obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            obj_Retangulo.PrimitivaTipo = PrimitiveType.Polygon;
            objetosLista.Add(obj_Retangulo);
            objetoSelecionado = obj_Retangulo;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, near, far);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //   Matrix4 modelview = Matrix4.LookAt(eye, at, up);
            //   GL.MatrixMode(MatrixMode.Modelview);
            //   GL.LoadMatrix(ref modelview);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(eye, at, up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            // DesenhaCubo();

#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();

            this.SwapBuffers();
        }



        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Console.WriteLine('a');
            else if (e.Key == Key.Escape)
                Exit();

            else if (e.Key == Key.R)
            {
                eye = new Vector3(eye.X - 10, eye.Y, eye.Z);
            }
            else if (e.Key == Key.E)
            {
                eye = new Vector3(eye.X + 10, eye.Y, eye.Z);
            }

            else if (e.Key == Key.G)
            {
                eye = new Vector3(eye.X, eye.Y - 10, eye.Z);
            }
            else if (e.Key == Key.F)
            {
                eye = new Vector3(eye.X, eye.Y + 10, eye.Z);
            }
            else if (e.Key == Key.B)
            {
                eye = new Vector3(eye.X, eye.Y, eye.Z - 10);
            }
            else if (e.Key == Key.V)
            {
                eye = new Vector3(eye.X, eye.Y, eye.Z + 10);
            }
            else if (e.Key == Key.A)
            {
                Console.WriteLine(eye);
            }


            // jogo
            else if (e.Key == Key.Number1)
            {
                obj_Tabuleiro.colocarFicha(6);
            }
            else if (e.Key == Key.Number2)
            {
                obj_Tabuleiro.colocarFicha(5);
            }
            else if (e.Key == Key.Number3)
            {
                obj_Tabuleiro.colocarFicha(4);
            }
            else if (e.Key == Key.Number4)
            {
                obj_Tabuleiro.colocarFicha(3);
            }
            else if (e.Key == Key.Number5)
            {
                obj_Tabuleiro.colocarFicha(2);
            }
            else if (e.Key == Key.Number6)
            {
                obj_Tabuleiro.colocarFicha(1);
            }
            else if (e.Key == Key.Number7)
            {
                obj_Tabuleiro.colocarFicha(0);
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            GameWindow window = Mundo.GetInstance(600, 600);
            window.Title = "camera";
            window.Run(1.0 / 60.0);
        }
    }
}
