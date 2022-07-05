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
  internal class Tabuleiro : ObjetoGeometria
  {

    protected List<Ficha> fichasLista = new List<Ficha>();

    int[,] tabuleiro;
    int qtdLinhas;
    int qtdColunas;
    bool turno = false;
    bool finalizado = false;

    public Tabuleiro(char rotulo, Objeto paiRef, int colunas, int linhas) : base(rotulo, paiRef)
    {
        tabuleiro = new int[linhas, colunas];
        qtdLinhas = linhas;
        qtdColunas = colunas;
    }

    public void colocarFicha(int coluna) {

        if (finalizado) return;
        
        int linhaSelecionada = -1;

        for (int linha = 0; linha < qtdLinhas; linha++)
        {
            if (tabuleiro[linha, coluna] == 0) {
                linhaSelecionada = linha;
                break;
            }
        }

        if (linhaSelecionada == -1) {
            Console.WriteLine("Não há espaço nessa coluna");
            return;
        }

        

        
        criarFicha(linhaSelecionada, coluna);
        

    }

    protected void criarFicha(int linha, int coluna) {

        Ficha modelo;
        float R = 255.0f;
        float G = 0.0f;
        float B = 0.0f;
        int turnoChar = 1;

        float fichaRaio = 3.0f;
        float espacoPorFicha = 5.0f;
        
        float x = 0;
        float y = 0;
        float z = 0;

        y += (espacoPorFicha + (fichaRaio * 2)) * linha;
        z += (espacoPorFicha + (fichaRaio * 2)) * coluna;

        if (turno) {
            R = 255.0f;
            G = 255.0f;
            B = 0.0f;
            turnoChar = 2;
        }

        tabuleiro[linha, coluna] = turnoChar;

        modelo = new Ficha(Utilitario.charProximo('@'), null, fichaRaio, x, y, z, R, G, B);
        fichasLista.Add(modelo);

        if (checarVitoria(turnoChar)) {
            finalizado = true;

            string vencedor = "vermelho";
            if (turno) vencedor = "amarelo";

            Console.WriteLine("Vitória do jogador de " + vencedor);
        }

        turno = !turno;

    }

    private bool checarVitoria(int player){

        // Vertical 
        for (int linha = 0; linha < qtdLinhas - 3 ; linha++ ){
            for (int coluna = 0; coluna < qtdColunas; coluna++){
                if (tabuleiro[linha, coluna] == player && tabuleiro[linha + 1, coluna] == player && tabuleiro[linha + 2, coluna] == player && tabuleiro[linha + 3, coluna] == player){
                    return true;
                }
            }
        }

        // Horizontal
        for (int linha = 0; linha < qtdLinhas; linha++ ){
            for (int coluna = 0; coluna < qtdColunas - 3; coluna++){
                if (tabuleiro[linha, coluna] == player && tabuleiro[linha, coluna + 1] == player && tabuleiro[linha, coluna + 2] == player && tabuleiro[linha, coluna + 3] == player){
                    return true;
                }
            }
        }

        // Diagonal / 
        for (int i = 3; i < qtdLinhas; i++){
            for (int j = 0; j < qtdColunas - 3; j++){
                if (tabuleiro[i, j] == player && tabuleiro[i - 1, j + 1] == player && tabuleiro[i - 2, j + 2] == player && tabuleiro[i - 3, j + 3] == player)
                    return true;
            }
        }

        // Diagonal \
        for (int i = 3; i<qtdLinhas; i++){
            for (int j = 3; j<qtdColunas; j++){
                if (tabuleiro[i, j] == player && tabuleiro[i - 1, j - 1] == player && tabuleiro[i - 2, j - 2] == player && tabuleiro[i - 3, j - 3] == player)
                    return true;
            }
        }

        return false;
    }

    protected override void DesenharObjeto()
    {
        for (int i = 0; i < fichasLista.Count; i++)
        {
            fichasLista[i].Desenhar();
        }
    }

    //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Tabuleiro: " + base.rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
      }
      return (retorno);
    }

  }
}