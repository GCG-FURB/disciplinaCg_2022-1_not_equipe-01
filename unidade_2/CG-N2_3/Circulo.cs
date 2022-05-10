/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class Circulo : ObjetoGeometria
  {
    public Circulo(char rotulo, Objeto paiRef, Ponto4D centro, double raio) : base(rotulo, paiRef)
    {

        for (int i = 1; i <= 72; i++) {

            Ponto4D ponto = CG_Biblioteca.Matematica.GerarPtosCirculo(i * 5, raio);

            ponto.X += centro.X;
            ponto.Y += centro.Y;

            base.PontosAdicionar(ponto);

        }
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
      retorno = "__ Objeto Circulo: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}