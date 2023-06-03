using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleSom : MonoBehaviour
{
   [SerializeField] private AudioSource fundoMusical; // variavel referência ao audio source.

    void Start()
    {
        DontDestroyOnLoad(fundoMusical); // função para não remover a musica de uma cena para outra.
    }

    public void VolumeMusical (float value) // metado para alterar o volume do som. O parâmetro float é usado pois é um slider.
    {
        fundoMusical.volume = value;
    }
}
