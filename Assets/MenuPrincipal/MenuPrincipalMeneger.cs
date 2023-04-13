using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalMeneger : MonoBehaviour

{
    [SerializeField] private string nomeDoLevelDeJogo;   // Variavel para a cena que o jogar vai mudar
    [SerializeField] private GameObject painelMenuInicial; // variavel para o objeto do menu inicial
    [SerializeField] private GameObject painelOpcoes; // variavel para o painel de opcoes
    public string nomeCena; // variavel publica para aumentar o tempo que o jogo vai abrir, tambem reprensenta a cena que o jogo vai mudar
    public void Jogar() // funcao para o botao jogar funcinar
    {
        nomeCena = nomeDoLevelDeJogo;  
        StartCoroutine("Abrir"); // chamando a funcao abaixo
    }
    private IEnumerator Abrir() // funcao que faz com que o jogo demore um pouco mais para abrir para conseguir escutar o click do botao
    {
        yield return new WaitForSeconds (0.5f); //faz o jogo demorar 0.5 segundos a mais para abrir
        SceneManager.LoadScene(nomeCena); // faz a tela do jogo abrir
    }

    public void AbrirOpcoes() // funcao botao opcoes
    {
        painelMenuInicial.SetActive(false); // fecha o menu inicial
        painelOpcoes.SetActive(true); // abre o menu opcoes
    }

    public void FecharOpcoes() // funcao para o botao voltar do menu de opcoes
    {
        painelOpcoes.SetActive(false); // fecha o menu de opcoes
        painelMenuInicial.SetActive(true); // abre o menu principal novamente
    }

    public void SairJogo() // funcao sair do jogo
    {
        Application.Quit(); // fecha o jogo(so funciona quando o jogo for compilado ou seja estiver pronto pra jogar)
    }
}
