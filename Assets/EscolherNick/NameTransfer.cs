using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NameTransfer : MonoBehaviour
{
    public string nomePlayer; //Variavel do nome do jogador que vamos usar em todo o jogo
    public GameObject inputField;// o conteudo que o jogador vai digitar
    public GameObject textDisplay;// o lugar onde o nome do jogador vai aparecer
    public void SalvarNome()
    {
        nomePlayer = inputField.GetComponent<Text>().text;// declarando que o conteudo que o jogador vai digitar é igual a nome do seu personagem
        textDisplay.GetComponent<Text>().text = "BEM VINDO A AVENTURA " + nomePlayer;// mostra a mensagem quando o jogador apertar o botao
    }
   
}
