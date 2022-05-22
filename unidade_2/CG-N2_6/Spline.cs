/**
  Autor: Dalton Solano dos Reis
**/

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;



namespace gcgcg
{
  internal class Spline : ObjetoGeometria
  {

    public List<Ponto> listaPontos;
    public int quantidadeDivisoes = 10;
    public List<Ponto4D> listaPrintar = new List<Ponto4D>();
    public Spline(char rotulo, Objeto paiRef, List<Ponto> listaPontos) : base(rotulo, paiRef)
    {
        this.listaPontos = listaPontos;
    }

    public void alterarQuantidadeDivisoes(int valor) {
        if (this.quantidadeDivisoes + valor > 0) {
            this.quantidadeDivisoes += valor;
        }  
        
    }

    public void calcular() {

        this.listaPrintar.Clear();

        double step = (double) 1 / this.quantidadeDivisoes;

        if (step != 1) {

            for (double t = 0; t <= 1; t += step) {
                double Bx = Math.Pow((1 - t), 3) * listaPontos[0].ponto.X + 3 * t * Math.Pow((1-t), 2) * listaPontos[1].ponto.X + 3 * Math.Pow(t, 2) * (1 - t) * listaPontos[2].ponto.X + Math.Pow(t, 3) * listaPontos[3].ponto.X;
                double By = Math.Pow((1 - t), 3) * listaPontos[0].ponto.Y + 3 * t * Math.Pow((1-t), 2) * listaPontos[1].ponto.Y + 3 * Math.Pow(t, 2) * (1 - t) * listaPontos[2].ponto.Y + Math.Pow(t, 3) * listaPontos[3].ponto.Y;
            
                this.listaPrintar.Add(new Ponto4D(Bx, By));
            
            }
        }
    }

    protected override void DesenharObjeto()
    {

        calcular();
        GL.Begin(base.PrimitivaTipo);
        foreach (Ponto4D pto in this.listaPrintar)
        {
            GL.Vertex2(pto.X, pto.Y);
        }
        GL.End();
    }
    
    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Spline: " + base.rotulo + "\n";
      for (var i = 0; i < this.listaPrintar.Count; i++)
      {
        retorno += "P" + i + "[" + this.listaPrintar[i].X + "," + this.listaPrintar[i].Y + "," + this.listaPrintar[i].Z + "," + this.listaPrintar[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}