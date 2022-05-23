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
    private Circulo obj_CirculoMaior;
    private Circulo obj_CirculoMenor;
    private Ponto4D pto_centro;
    Ponto4D pontoA;

    private List<Ponto> listaPontos = new List<Ponto>();
    private Ponto pontoSelecionado;
    
    Ponto4D ponto;

#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

    public bool dentro_quadrado(Retangulo obj_Retangulo, Ponto4D ponto) {
      return (ponto.X <= obj_Retangulo.BBox.obterMenorX || 
              ponto.X >= obj_Retangulo.BBox.obterMaiorX ||
              ponto.Y <= obj_Retangulo.BBox.obterMenorY ||
              ponto.Y >= obj_Retangulo.BBox.obterMaiorY);
    }
    public bool dentro_circulo(Circulo obj_Circulo, Ponto4D ponto, double raio) {
      double dist = Math.Sqrt(Math.Pow(obj_Circulo.BBox.obterCentro.X - ponto.X, 2) + 
                              Math.Pow(obj_Circulo.BBox.obterCentro.Y - ponto.Y, 2));
      return dist <= raio;
    }
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      camera.xmin = -400; camera.xmax = 400; camera.ymin = -400; camera.ymax = 400;

      Console.WriteLine(" --- Ajuda / Teclas: ");
      Console.WriteLine(" [  H     ] mostra teclas usadas. ");

      objetoId = Utilitario.charProximo(objetoId);

      // Círculo Maior
      objetoId = Utilitario.charProximo(objetoId);
      obj_CirculoMaior = new Circulo(objetoId, null, new Ponto4D(100, 100), 200);
      obj_CirculoMaior.ObjetoCor.CorR = 0; obj_CirculoMaior.ObjetoCor.CorG = 0; obj_CirculoMaior.ObjetoCor.CorB = 0;
      obj_CirculoMaior.PrimitivaTipo = PrimitiveType.LineLoop;
      obj_CirculoMaior.PrimitivaTamanho = 2;
      objetosLista.Add(obj_CirculoMaior);

      // Retangulo
      Ponto4D pontoDirCim = CG_Biblioteca.Matematica.GerarPtosCirculo(45, 200);
      Ponto4D pontoEsqBai = CG_Biblioteca.Matematica.GerarPtosCirculo(225, 200);

      pontoDirCim.X += 100;
      pontoDirCim.Y += 100;
      pontoEsqBai.X += 100;
      pontoEsqBai.Y += 100;

      objetoId = Utilitario.charProximo(objetoId);
      obj_Retangulo = new Retangulo(objetoId, null, pontoEsqBai, pontoDirCim);
      obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 53; obj_Retangulo.ObjetoCor.CorB = 184;
      obj_Retangulo.PrimitivaTamanho = 4;
      objetosLista.Add(obj_Retangulo);

      obj_Retangulo.BBox.Desenhar();
      

      // Circulo Menor
      pto_centro = new Ponto4D(100, 100);
      objetoId = Utilitario.charProximo(objetoId);
      obj_CirculoMenor = new Circulo(objetoId, null, pto_centro, 50);
      obj_CirculoMenor.ObjetoCor.CorR = 0; obj_CirculoMenor.ObjetoCor.CorG = 0; obj_CirculoMenor.ObjetoCor.CorB = 0;
      obj_CirculoMenor.PrimitivaTipo = PrimitiveType.LineLoop;
      obj_CirculoMenor.PrimitivaTamanho = 2;
      objetosLista.Add(obj_CirculoMenor);
      objetoSelecionado = obj_CirculoMenor;

      

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

      // obj_Retangulo.BBox.Desenhar();
      // obj_CirculoMaior.BBox.Desenhar();
      obj_CirculoMenor.BBox.Desenhar();

      if (dentro_quadrado(obj_Retangulo, obj_CirculoMenor.BBox.obterCentro)) {
            obj_Retangulo.ObjetoCor.CorR = 250; obj_Retangulo.ObjetoCor.CorG = 253; obj_Retangulo.ObjetoCor.CorB = 15;

            if (!dentro_circulo(obj_CirculoMaior, obj_CirculoMenor.BBox.obterCentro, 195)) {
                obj_Retangulo.ObjetoCor.CorR = 0; obj_Retangulo.ObjetoCor.CorG = 255; obj_Retangulo.ObjetoCor.CorB = 255;
            }

      } else {
        obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 53; obj_Retangulo.ObjetoCor.CorB = 184;
      }

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

      else if (e.Key == Key.W) {
        if (dentro_circulo(obj_CirculoMaior, new Ponto4D(pto_centro.X, pto_centro.Y + 4), 200)) pto_centro.Y += 5;
      } else if (e.Key == Key.A) {
        if (dentro_circulo(obj_CirculoMaior, new Ponto4D(pto_centro.X - 4, pto_centro.Y), 200)) pto_centro.X -= 5;
      } else if (e.Key == Key.S) {
        if (dentro_circulo(obj_CirculoMaior, new Ponto4D(pto_centro.X, pto_centro.Y - 4), 200)) pto_centro.Y -= 5;
      } else if (e.Key == Key.D) {
        if (dentro_circulo(obj_CirculoMaior, new Ponto4D(pto_centro.X + 4, pto_centro.Y), 200)) pto_centro.X += 5;
      }
      else
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
