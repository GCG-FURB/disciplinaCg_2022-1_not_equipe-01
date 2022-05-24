/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

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
    protected List<Objeto> objetosLista = new List<Objeto>();
    private ObjetoGeometria objetoSelecionado = null;
    private char objetoId = '@';
    private bool bBoxDesenhar = false;
    int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
    private bool mouseMoverPto = false;
    private Retangulo obj_Retangulo;  
    private SegReta obj_SegReta;  
    private SrPalito obj_SrPalito;
    private Ponto obj_Ponto1;
    private Ponto obj_Ponto2;
    private Ponto obj_Ponto3;
    private Ponto obj_Ponto4;
    private Spline obj_Spline;
    Ponto4D pontoA;

    private List<Ponto> listaPontos = new List<Ponto>();
    private Ponto pontoSelecionado;
    
    Ponto4D ponto;

#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");

      objetoId = Utilitario.charProximo(objetoId);

      // Pontos
      obj_Ponto1 = new Ponto(objetoId, null, -100, -100);
      obj_Ponto1.ObjetoCor.CorR = 0; obj_Ponto1.ObjetoCor.CorG = 0; obj_Ponto1.ObjetoCor.CorB = 0;
      listaPontos.Add(obj_Ponto1);

      obj_Ponto2 = new Ponto(objetoId, null, -100, 100);
      obj_Ponto2.ObjetoCor.CorR = 0; obj_Ponto2.ObjetoCor.CorG = 0; obj_Ponto2.ObjetoCor.CorB = 0;
      listaPontos.Add(obj_Ponto2);

      obj_Ponto3 = new Ponto(objetoId, null, 100, 100);
      obj_Ponto3.ObjetoCor.CorR = 0; obj_Ponto3.ObjetoCor.CorG = 0; obj_Ponto3.ObjetoCor.CorB = 0;
      listaPontos.Add(obj_Ponto3);

      obj_Ponto4 = new Ponto(objetoId, null, 100, -100);
      obj_Ponto4.ObjetoCor.CorR = 0; obj_Ponto4.ObjetoCor.CorG = 0; obj_Ponto4.ObjetoCor.CorB = 0;
      listaPontos.Add(obj_Ponto4);

      // Segmento de Retas
      obj_SegReta = new SegReta(objetoId, null, obj_Ponto1.ponto, obj_Ponto2.ponto);
      obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta);

      obj_SegReta = new SegReta(objetoId, null, obj_Ponto2.ponto, obj_Ponto3.ponto);
      obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta);

      obj_SegReta = new SegReta(objetoId, null, obj_Ponto3.ponto, obj_Ponto4.ponto);
      obj_SegReta.ObjetoCor.CorR = 0; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_SegReta);
      
      obj_Spline = new Spline(objetoId, null, listaPontos);
      obj_Spline.PrimitivaTipo = PrimitiveType.LineStrip;
      obj_Spline.ObjetoCor.CorR = 255; obj_Spline.ObjetoCor.CorG = 255; obj_Spline.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_Spline);

      obj_Ponto1.selecionar();
      pontoSelecionado = obj_Ponto1;
      objetoSelecionado = obj_SegReta;

      

#if CG_Privado
      objetoId = Utilitario.charProximo(objetoId);
      obj_SegReta = new Privado_SegReta(objetoId, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Circulo = new Privado_Circulo(objetoId, null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
      GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();

      GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
#if CG_Gizmo      
      Sru3D();
#endif
      for (var i = 0; i < listaPontos.Count; i++)
        listaPontos[i].Desenhar();
      for (var i = 0; i < objetosLista.Count; i++)
        objetosLista[i].Desenhar();
      if (bBoxDesenhar && (objetoSelecionado != null))
        objetoSelecionado.BBox.Desenhar();
      this.SwapBuffers();
    }

    protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
    {
      if (e.Key == Key.H)
        Utilitario.AjudaTeclado();
      else if (e.Key == Key.Escape)
        Exit();

      else if (e.Key == Key.O)
        bBoxDesenhar = !bBoxDesenhar;
      else if (e.Key == Key.V)
        mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto

      else if (e.Key == Key.Number1) {
        pontoSelecionado.deselecionar(); listaPontos[0].selecionar(); pontoSelecionado = listaPontos[0];

      } else if (e.Key == Key.Number2) {
        pontoSelecionado.deselecionar(); listaPontos[1].selecionar(); pontoSelecionado = listaPontos[1];
      } else if (e.Key == Key.Number3) {
        pontoSelecionado.deselecionar(); listaPontos[2].selecionar(); pontoSelecionado = listaPontos[2];
      } else if (e.Key == Key.Number4) {
        pontoSelecionado.deselecionar(); listaPontos[3].selecionar(); pontoSelecionado = listaPontos[3];

      } else if (e.Key == Key.C) {
        pontoSelecionado.moverY(5);
      } else if (e.Key == Key.B) {
        pontoSelecionado.moverY(-5);
      } else if (e.Key == Key.E) {
        pontoSelecionado.moverX(-5);
      } else if (e.Key == Key.D) {
        pontoSelecionado.moverX(5);

      } else if (e.Key == Key.Plus) {
        obj_Spline.alterarQuantidadeDivisoes(1);
      } else if (e.Key == Key.Minus) {
        obj_Spline.alterarQuantidadeDivisoes(-1);
      
      } else
        Console.WriteLine(e.Key);
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
    }

#if CG_Gizmo
    private void Sru3D()
    {
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
    }
#endif    
  }
  class Program
  {
    static void Main(string[] args)
    {
      Mundo window = Mundo.GetInstance(600, 600);
      window.Title = "CG_N2";
      window.Run(1.0 / 60.0);
    }
  }
}
