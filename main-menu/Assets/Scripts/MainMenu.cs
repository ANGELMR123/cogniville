using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscenaJuego()
    {
        SceneManager.LoadScene("Aldea");    // Carga la escena de Juego
    }

    public void EscenaArcade()
    {
        SceneManager.LoadScene("Arcade");    // Carga la escena de Arcade
    }

    public void Salir()
    {
        Application.Quit();    // Cierra la aplicación
    }
}
