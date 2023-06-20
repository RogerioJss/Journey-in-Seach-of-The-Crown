using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [Header("GameOver")]
    public GameObject painelGameOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GameOver(){
        painelGameOver.SetActive(true);
    }

    public void Renascer(){
        Debug.Log("Botao renascer pressionado");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VoltarMenu(){
        Debug.Log("Botao voltar menu pressionado");
        SceneManager.LoadScene("Menu");
    }

}
