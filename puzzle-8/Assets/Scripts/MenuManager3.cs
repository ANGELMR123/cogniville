using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuManager3 : MonoBehaviour
{
    private int progreso;

    void Start()
    {
        // Obtener el progreso del archivo
        progreso = LoadProgress();
    }

    int LoadProgress()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "progress3.txt");
        if (File.Exists(filePath))
        {
            string levelString = File.ReadAllText(filePath);
            if (int.TryParse(levelString, out int level))
            {
                return level;
            }
        }
        return 0; // Si no se puede leer el progreso, empezar desde el nivel 0
    }

    public void EscenaJuego()
    {
        if (progreso >= 6)
        {
            SceneManager.LoadScene("MediumLevelScene");
        }
        else
        {
            SceneManager.LoadScene("LowLevelScene");
        }
    }

    public void VolverAldea()
    {
        SceneManager.LoadScene("Principal");    // Carga la escena de Juego
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
