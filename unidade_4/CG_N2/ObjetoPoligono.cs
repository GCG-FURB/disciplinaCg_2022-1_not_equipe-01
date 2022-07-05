/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

using System.Collections.Generic;

namespace gcgcg
{
  internal class ObjetoPoligono : ObjetoGeometria
  {

    public Ponto4D centro;
    public double raio;

    public ObjetoPoligono(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
    {
    }

    protected override void DesenharObjeto()
    {

        GL.Begin(base.PrimitivaTipo);
        foreach (Ponto4D pto in pontosLista)
        {
            GL.Vertex2(pto.X, pto.Y);
        }

        GL.End();
    }

    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto ObjetoPoligono: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}