/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using CG_Biblioteca;
using System;

namespace gcgcg
{
  internal abstract class Objeto
  {
    protected char rotulo;
    private Cor objetoCor = new Cor(255, 255, 255, 255);
    public Cor ObjetoCor { get => objetoCor; set => objetoCor = value; }
    private PrimitiveType primitivaTipo = PrimitiveType.LineLoop;
    public PrimitiveType PrimitivaTipo { get => primitivaTipo; set => primitivaTipo = value; }
    private float primitivaTamanho = 1;
    public float PrimitivaTamanho { get => primitivaTamanho; set => primitivaTamanho = value; }
    private BBox bBox = new BBox();
    public BBox BBox { get => bBox; set => bBox = value; }
    private List<Objeto> objetosLista = new List<Objeto>();

    private Transformacao4D matriz = new Transformacao4D();
    public Transformacao4D Matriz { get => matriz; } 

    private static Transformacao4D matrizTranslacaoTemporaria = new Transformacao4D();
    private static Transformacao4D matrizTranslacaoTemporaria2 = new Transformacao4D();
    private static Transformacao4D matrizRotacaoTemporaria = new Transformacao4D();
    private static Transformacao4D matrizGeralTemporaria = new Transformacao4D();

    private char eixoRotacao = 'z';
    public void TrocaEixoRotacao(char eixo) => eixoRotacao = eixo;

    public Objeto(char rotulo, Objeto paiRef)
    {
      this.rotulo = rotulo;
    }

    public void Desenhar()
    {

      GL.PushMatrix();                                    // N3-Exe12: grafo de cena
      GL.MultMatrix(matriz.ObterDados());
      GL.Color3(objetoCor.CorR, objetoCor.CorG, objetoCor.CorB);
      GL.LineWidth(primitivaTamanho);
      GL.PointSize(primitivaTamanho);
      DesenharGeometria();
      for (var i = 0; i < objetosLista.Count; i++)
      {
        objetosLista[i].Desenhar();
      }
      GL.PopMatrix();                                     // N3-Exe12: grafo de cena
    }

    public void rotacaoOrigem(double angulo)
    {
      matrizGeralTemporaria.AtribuirIdentidade();
      Ponto4D pontoPivo = new Ponto4D(0, 0, 0);

      matrizRotacaoTemporaria.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
      matrizGeralTemporaria = matrizRotacaoTemporaria.MultiplicarMatriz(matrizGeralTemporaria);

      matrizTranslacaoTemporaria2.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
      matrizGeralTemporaria = matrizTranslacaoTemporaria2.MultiplicarMatriz(matrizGeralTemporaria);

      matriz = matriz.MultiplicarMatriz(matrizGeralTemporaria);
    }

    public void rotacaoBBox(double angulo)
    {
      matrizGeralTemporaria.AtribuirIdentidade();
      Ponto4D pontoPivo = bBox.obterCentro;

      matrizTranslacaoTemporaria.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z);
      matrizGeralTemporaria = matrizTranslacaoTemporaria.MultiplicarMatriz(matrizGeralTemporaria);

      matrizRotacaoTemporaria.AtribuirRotacaoZ(Transformacao4D.DEG_TO_RAD * angulo);
      matrizGeralTemporaria = matrizRotacaoTemporaria.MultiplicarMatriz(matrizGeralTemporaria);

      matrizTranslacaoTemporaria2.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
      matrizGeralTemporaria = matrizTranslacaoTemporaria2.MultiplicarMatriz(matrizGeralTemporaria);

      matriz = matriz.MultiplicarMatriz(matrizGeralTemporaria);
    }

    protected abstract void DesenharGeometria();
    public void FilhoAdicionar(Objeto filho)
    {
      this.objetosLista.Add(filho);
    }
    public void FilhoRemover(Objeto filho)
    {
      this.objetosLista.Remove(filho);
    }

    public void AtribuirIdentidade()
    {
        matriz.AtribuirIdentidade();
    }

    public void translacaoXYZ(double tx, double ty, double tz)
    {
        Transformacao4D matrizTranslate = new Transformacao4D();
        matrizTranslate.AtribuirTranslacao(tx, ty, tz);
        matriz = matrizTranslate.MultiplicarMatriz(matriz);
    }

    public void escalaXYZ(double Sx, double Sy, double Sz)
    {
        Transformacao4D matrizScale = new Transformacao4D();
        matrizScale.AtribuirEscala(Sx, Sy, Sz);
        matriz = matrizScale.MultiplicarMatriz(matriz);
    }

    public void escalaXYZBBox(double Sx, double Sy, double Sz)
    {
        matrizGeralTemporaria.AtribuirIdentidade();
        Ponto4D pontoPivo = bBox.obterCentro;

        matrizTranslacaoTemporaria.AtribuirTranslacao(-pontoPivo.X, -pontoPivo.Y, -pontoPivo.Z);
        matrizGeralTemporaria = matrizTranslacaoTemporaria.MultiplicarMatriz(matrizGeralTemporaria);

        Transformacao4D matrizScale = new Transformacao4D();
        matrizScale.AtribuirEscala(Sx, Sy, Sz);
        matrizGeralTemporaria = matrizScale.MultiplicarMatriz(matrizGeralTemporaria);

        matrizTranslacaoTemporaria2.AtribuirTranslacao(pontoPivo.X, pontoPivo.Y, pontoPivo.Z);
        matrizGeralTemporaria = matrizTranslacaoTemporaria2.MultiplicarMatriz(matrizGeralTemporaria);

        matriz = matriz.MultiplicarMatriz(matrizGeralTemporaria);
    }

    public Transformacao4D obterMatriz() {
        return matriz;
    } 

    public void aplicarMatrizIdentidade() {
        matriz.AtribuirIdentidade();
    }

    public char obterRotulo() {
        return rotulo;
    }

    public List<Objeto> obterLista() {
        return objetosLista;
    }

  }
}