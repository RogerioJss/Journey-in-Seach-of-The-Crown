using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNickManeger : MonoBehaviour
{
    [SerializeField]private string cenaMenu; //variavel para nome da cena do botao voltar
    [SerializeField]private string cenaGo; // variavel para nome da cena Go
   public void Voltar()
   {
        SceneManager.LoadScene(cenaMenu);
   }
   
    public void Go() 
    {
        SceneManager.LoadScene(cenaGo);
     }
    
    

    
   
        
    
}
