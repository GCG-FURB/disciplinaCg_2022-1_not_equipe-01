/**
  Autor: Dalton Solano dos Reis
**/

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;


using System.Collections.Generic;

namespace gcgcg
{
  internal class Ficha : ObjetoGeometria
  {

    const int nVertices = 32;
    float corR, corG, corB;
    float spawnY = 100.0f;
    float actualY = 100.0f;
    float objetivoY;
    float gravity = -0.5f;
    float velocity = 0.0f;
    float energy_loss = 0.2f;

    public Ficha(char rotulo, Objeto paiRef, float raio, float X, float Y, float Z, float R, float G, float B) : base(rotulo, paiRef)
    {
        corR = R;
        corG = G;
        corB = B;
        objetivoY = Y;
        Calculate(raio, 1.0f, X, spawnY, Z);
    }

    protected void Calculate(float Radius, float Height, float x, float y, float z)
    {
        float theta = 0, dtheta = MathHelper.TwoPi / nVertices;

            for (int i = 0; i < 2 * nVertices; i += 2)
            {
                // Set back circle's info
                Ponto4D circuloAtras = new Ponto4D(0, Radius * Math.Cos(theta) + y, Radius * Math.Sin(theta) + z);

                // Set front circle's into
                Ponto4D circuloFrente = new Ponto4D(Height, circuloAtras.Y, circuloAtras.Z);

                // Increment the angle measuregg
                theta += dtheta;

                pontosLista.Add(circuloAtras);
                pontosLista.Add(circuloFrente);
            }
    }

    protected override void DesenharObjeto()
    {

        if (actualY > objetivoY) {

            velocity += gravity;
            actualY += velocity;

            if ((actualY + velocity) < objetivoY) {

                base.translacaoXYZ(0, objetivoY - actualY, 0);
                actualY = objetivoY;

            } else {

                base.translacaoXYZ(0, velocity, 0);

            }
        }
        

        int i = 0; 
        GL.Color3(corR, corG, corB);

        GL.Begin(PrimitiveType.Polygon);
        {
            for (i = 0; i < pontosLista.Count; i += 2)
            {
                GL.Vertex3(pontosLista[i].X, pontosLista[i].Y, pontosLista[i].Z);
            }
        }
        GL.End();

        // Circulo Frente
        GL.Begin(PrimitiveType.Polygon);
        {
            for (i = 1; i < pontosLista.Count; i += 2)
            {
                GL.Vertex3(pontosLista[i].X, pontosLista[i].Y, pontosLista[i].Z);
            }
        }
        GL.End();

        GL.Begin(PrimitiveType.TriangleStrip);
        {

            for (i = 0; i < pontosLista.Count; i += 2)
            {
                GL.Vertex3(pontosLista[i].X, pontosLista[i].Y, pontosLista[i].Z);
                GL.Vertex3(pontosLista[i + 1].X, pontosLista[i + 1].Y, pontosLista[i + 1].Z);
            }
        }
        GL.End();
    
        GL.Color3(0, 0, 0);
        GL.Begin(PrimitiveType.LineLoop);
        {
            // Circulo Atras
            for (i = 0; i < pontosLista.Count; i += 2)
            {
                GL.Vertex3(pontosLista[i].X, pontosLista[i].Y, pontosLista[i].Z);
            }
        }
        GL.End();

        GL.Begin(PrimitiveType.LineLoop);
        {
            // Circulo Frente
            for (i = 1; i < pontosLista.Count; i += 2)
            {
                GL.Vertex3(pontosLista[i].X, pontosLista[i].Y, pontosLista[i].Z);
            }
        }
        GL.End();
    }

    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Ficha: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}