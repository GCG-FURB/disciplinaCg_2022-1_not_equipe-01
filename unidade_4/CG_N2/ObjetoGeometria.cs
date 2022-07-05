/**
  Autor: Dalton Solano dos Reis
**/

using System.Collections.Generic;
using CG_Biblioteca;
using System;

namespace gcgcg
{
  internal abstract class ObjetoGeometria : Objeto
  {
    public List<Ponto4D> pontosLista = new List<Ponto4D>();

    public ObjetoGeometria(char rotulo, Objeto paiRef) : base(rotulo, paiRef) { }

    protected override void DesenharGeometria()
    {
      DesenharObjeto();
    }
    protected abstract void DesenharObjeto();
    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
      if (pontosLista.Count.Equals(1))
        base.BBox.Atribuir(pto);
      else
        base.BBox.Atualizar(pto);
      base.BBox.ProcessarCentro();
    }

    public void PontosRemoverUltimo()
    {
      pontosLista.RemoveAt(pontosLista.Count - 1);
    }

    public void PontosRemoverPosicao(int index) {
        pontosLista.RemoveAt(index);
    }

    protected void PontosRemoverTodos()
    {
      pontosLista.Clear();
    }

    public Ponto4D PontosUltimo()
    {
      return pontosLista[pontosLista.Count - 1];
    }

    public Ponto4D PontosIndex(int index) {
        return pontosLista[index];
    }

    public int obterPontoMaisProximoIndex(int x, int y) {

        double menorDistancia = double.MaxValue;
        int menorPosicao = 0;
        Ponto4D menorPonto = null;

        for (int i = 0; i < pontosLista.Count; i++)
        {
            double valor = Math.Pow(pontosLista[i].X - x, 2) + Math.Pow(pontosLista[i].Y - y, 2);
            if (Math.Sqrt(valor) < menorDistancia) {
                menorDistancia = Math.Sqrt(valor);
                menorPosicao = i;
                menorPonto = pontosLista[i];
            } 
        }

        return menorPosicao;

    }

    public ObjetoGeometria pontoEmPoligono(Ponto4D Psel) {
        
        int N = 0;

        for (int i = 0; i < pontosLista.Count; i ++) {

            int proxIndex = (i + 1) % pontosLista.Count;

            if (pontosLista[i].Y != pontosLista[proxIndex].Y) {

                double t = ((Psel.Y - pontosLista[i].Y) / (pontosLista[proxIndex].Y - pontosLista[i].Y));

                double Xint = pontosLista[i].X + (pontosLista[proxIndex].X - pontosLista[i].X) * t;
                double Yint = Psel.Y;

                if (Xint == Psel.X) break;
                else if (Xint > Psel.X && 
                         Yint > Math.Min(pontosLista[i].Y, pontosLista[proxIndex].Y) && 
                         Yint <= Math.Max(pontosLista[i].Y, pontosLista[proxIndex].Y)
                ) N += 1;

            } else {

                if (Psel.Y == pontosLista[i].Y && 
                    Psel.X >= Math.Min(pontosLista[i].X, pontosLista[proxIndex].X) &&
                    Psel.X <= Math.Max(pontosLista[i].X, pontosLista[proxIndex].X))

                    break;

            }
        }

        if (N % 2 != 0) return this;
        else return null;

    }

    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

    
  }
}