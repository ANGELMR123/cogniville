using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentLevel { get; private set; }

    private void Awake()
    {
        Instance = this;
        // Inicializar el nivel actual
        CurrentLevel = 1;
    }

    // Otras funciones y lógica del juego...
}
