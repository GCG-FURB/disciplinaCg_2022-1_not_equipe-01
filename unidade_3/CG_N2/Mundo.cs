/**
  Autor: Dalton Solano dos Reis
**/

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
    private Retangulo obj_Retangulo;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = 0; camera.xmax = 600; camera.ymin = 0; camera.ymax = 600;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");

    //   objetoId = Utilitario.charProximo(objetoId);
    //   obj_Retangulo = new Retangulo(objetoId, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
    //   obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
    //   objetosLista.Add(obj_Retangulo);
    //   objetoSelecionado = obj_Retangulo;

#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      obj_Circulo.PrimitivaTipo = PrimitiveType.Points;
      obj_Circulo.PrimitivaTamanho = 5;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
#if CG_OpenGL
      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
#endif
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
#if CG_OpenGL
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();
      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
#endif
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
#if CG_OpenGL
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
#endif
#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();
#if CG_Gizmo
      if (bBoxDesenhar && (objetoSelecionado != null))
        objetoSelecionado.BBox.Desenhar();
#endif
      this.SwapBuffers();
    }

    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {
      if (e.Key == Key.H)
        Utilitario.AjudaTeclado();
      else if (e.Key == Key.Escape)
        Exit();
      else if (e.Key == Key.E)
      {
        Console.WriteLine("--- Objetos / Pontos: ");
        for (var i = 0; i < objetosLista.Count; i++)
        {
          Console.WriteLine(objetosLista[i]);
        }
      }
#if CG_Gizmo
      else if (e.Key == Key.O)
        bBoxDesenhar = !bBoxDesenhar;
#endif
      else if (e.Key == Key.V) {

          if (objetoSelecionado != null) {

              int index = objetoSelecionado.obterPontoMaisProximoIndex(mouseX, mouseY);

              indexPontoMover = index;
              mouseMoverPto2 = true;

          }

      }
        // mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto
        

      else if (e.Key == Key.Enter) {

          mouseMoverPto = false;
          mouseMoverPto2 = false;

          if (objetoCriando != null) {

            objetoCriando.PontosRemoverUltimo();
            objetoSelecionado = objetoCriando;

            objetoCriando = null;
            desenhandoObjeto = false;

          }

          

      }

      else if (e.Key == Key.Space) {

        if (!desenhandoObjeto) {

            desenhandoObjeto = true;

            objetoId = Utilitario.charProximo(objetoId);
            ObjetoPoligono teste = new ObjetoPoligono(objetoId, null);

            if (objetoSelecionado != null) {

                objetoSelecionado.FilhoAdicionar(teste);

            } else {
                objetosLista.Add(teste);
            }
            
            objetoCriando = teste;

            objetoCriando.PontosAdicionar(new Ponto4D(mouseX, mouseY, 0));

        }

        objetoCriando.PontosAdicionar(new Ponto4D(mouseX, mouseY, 0));

      } 

      else if (e.Key == Key.A) {

        objetoSelecionado = null;

        for (var i = 0; i < objetosLista.Count; i++) {

            ObjetoGeometria objeto = objetosLista[i].pontoEmPoligono(new Ponto4D(mouseX, mouseY, 0));

            if (objeto != null) {

                objetoSelecionado = objeto;
                break;

            }

        }

      }

      else if (e.Key == Key.M) {
          if (objetoSelecionado != null) {
              Console.WriteLine(objetoSelecionado.obterMatriz());
          }
      }

      else if (e.Key == Key.P) {
          if (objetoSelecionado != null) {
              Console.WriteLine(objetoSelecionado);
          }
      }

      else if (e.Key == Key.I) {
          if (objetoSelecionado != null) {
              objetoSelecionado.aplicarMatrizIdentidade();
          }
      }

      else if (e.Key == Key.Left) {
          objetoSelecionado.translacaoXYZ(-10, 0, 0);
      }

      else if (e.Key == Key.Right) {
          objetoSelecionado.translacaoXYZ(10, 0, 0);
      }
        
      else if (e.Key == Key.Up) {
          objetoSelecionado.translacaoXYZ(0, 10, 0);
      }

      else if (e.Key == Key.Down) {
          objetoSelecionado.translacaoXYZ(0, -10, 0);
      }

      else if (e.Key == Key.Number0) { // PageUp
        objetoSelecionado.escalaXYZ(2, 2, 2);
      } 

      else if (e.Key == Key.Number9) { // PageDown
        objetoSelecionado.escalaXYZ(0.5, 0.5, 0.5);
      } 

      else if (e.Key == Key.Number7) { // Home
        objetoSelecionado.escalaXYZBBox(2, 2, 2);
      } 

      else if (e.Key == Key.Number8) { // End
        objetoSelecionado.escalaXYZBBox(0.5, 0.5, 0.5);
      } 

      else if (e.Key == Key.Number1) {
        objetoSelecionado.rotacaoOrigem(10);
      } 
        
      else if (e.Key == Key.Number2) {
        objetoSelecionado.rotacaoOrigem(-10);
      } 

      else if (e.Key == Key.Number3) {
        objetoSelecionado.rotacaoBBox(10);
      } 

      else if (e.Key == Key.Number4) {
        objetoSelecionado.rotacaoBBox(-10);
      } 
        
      else if (e.Key == Key.R) {
        objetoSelecionado.ObjetoCor.CorR = 255; objetoSelecionado.ObjetoCor.CorG = 0; objetoSelecionado.ObjetoCor.CorB = 0;
      } 

      else if (e.Key == Key.G) {
        objetoSelecionado.ObjetoCor.CorR = 0; objetoSelecionado.ObjetoCor.CorG = 255; objetoSelecionado.ObjetoCor.CorB = 0;
      } 

      else if (e.Key == Key.B) {
        objetoSelecionado.ObjetoCor.CorR = 0; objetoSelecionado.ObjetoCor.CorG = 0; objetoSelecionado.ObjetoCor.CorB = 255;
      } 

      else if (e.Key == Key.S) {
          if (objetoSelecionado != null) {

              if (objetoSelecionado.PrimitivaTipo.ToString() == "LineStrip") objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
              else objetoSelecionado.PrimitivaTipo = PrimitiveType.LineStrip;
          }
      }

      else if (e.Key == Key.D) {

          if (objetoSelecionado != null) {

              int index = objetoSelecionado.obterPontoMaisProximoIndex(mouseX, mouseY);

              objetoSelecionado.PontosRemoverPosicao(index);

          }

      }

      else if (e.Key == Key.C) {

          if (objetoSelecionado != null) {

            for (var i = 0; i < objetosLista.Count; i++) {
                
                if (objetoSelecionado.obterRotulo() == objetosLista[i].obterRotulo()) {
                    objetosLista.RemoveAt(i);
                    objetoSelecionado = null;
                }

            }

          }


      }

      else if (e.Key == Key.X)
        Console.WriteLine("X");

      else if (e.Key == Key.Y)
        Console.WriteLine("Y");

      else if (e.Key == Key.Z)
        Console.WriteLine("Z");

      else
        Console.WriteLine(" __ Tecla não implementada.");
    }

    //TODO: não está considerando o NDC
    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
      if (mouseMoverPto && (objetoSelecionado != null))
      {
        objetoSelecionado.PontosUltimo().X = mouseX;
        objetoSelecionado.PontosUltimo().Y = mouseY;
      }

      if (mouseMoverPto2 && (objetoSelecionado != null))
      {
        objetoSelecionado.PontosIndex(indexPontoMover).X = mouseX;
        objetoSelecionado.PontosIndex(indexPontoMover).Y = mouseY;
      }

      if (objetoCriando != null)
      {
        objetoCriando.PontosUltimo().X = mouseX;
        objetoCriando.PontosUltimo().Y = mouseY;
      }

    }

#if CG_Gizmo
    private void Sru3D()
    {
#if CG_OpenGL
      GL.LineWidth(1);
      GL.Begin(PrimitiveType.Lines);
      // GL.Color3(1.0f,0.0f,0.0f);
      GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
      GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
      // GL.Color3(0.0f,1.0f,0.0f);
      GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
      GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
      // GL.Color3(0.0f,0.0f,1.0f);
      GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
      GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
      GL.End();
#endif
    }
#endif    
  }
  class Program
  {
    static void Main(string[] args)
    {
      ToolkitOptions.Default.EnableHighResolution = false;
      Mundo window = Mundo.GetInstance(600, 600);
      window.Title = "CG_N3";
      window.Run(1.0 / 60.0);
    }
  }
}