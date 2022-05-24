/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class SrPalito : ObjetoGeometria
  {

    private int raio;
    private int grau;

    public SrPalito(char rotulo, Objeto paiRef, Ponto4D pontoA, int raio, int grau) : base(rotulo, paiRef)
    {
        base.PontosAdicionar(pontoA);
        this.raio = raio;
        this.grau = grau;
    }

    public void moverX(int valor) {
        base.PontosUltimo().X += valor;
    }

    public void alterarRaio(int valor) {
        this.raio += valor;
    }

    public void alterarAngulo(int valor) {
        this.grau += valor;
    }

    protected override void DesenharObjeto()
    {
        GL.Begin(base.PrimitivaTipo);
        GL.Vertex2(base.PontosUltimo().X, base.PontosUltimo().Y);

        Ponto4D pontoB = CG_Biblioteca.Matematica.GerarPtosCirculo(this.grau, this.raio);

        pontoB.X += base.PontosUltimo().X;
        pontoB.Y += base.PontosUltimo().Y;

        GL.Vertex2(pontoB.X, pontoB.Y);
        GL.End();
    }
    
    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto SrPalito: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}