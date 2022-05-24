/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
  internal class Ponto : ObjetoGeometria
  {

    public int x;
    public int y;
    public Ponto4D ponto;

    public Ponto(char rotulo, Objeto paiRef, int x, int y) : base(rotulo, paiRef)
    {
        this.x = x;
        this.y = y;
        this.ponto = new Ponto4D(x, y);
    }

    public void selecionar() {
        base.ObjetoCor.CorR = 255; base.ObjetoCor.CorG = 0; base.ObjetoCor.CorB = 0;
    }

    public void deselecionar() {
        base.ObjetoCor.CorR = 0; base.ObjetoCor.CorG = 0; base.ObjetoCor.CorB = 0;
    }

    public void moverX(int valor) {
        this.x += valor;
        this.ponto.X = this.x;
    }

    public void moverY(int valor) {
        this.y += valor;
        this.ponto.Y = this.y;
    }

    protected override void DesenharObjeto()
    {

        base.PrimitivaTamanho = 10;
        base.PrimitivaTipo = PrimitiveType.Points;

        GL.Begin(base.PrimitivaTipo);
            GL.Vertex2(this.x, this.y);
        GL.End();
    }
    
    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Ponto: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}