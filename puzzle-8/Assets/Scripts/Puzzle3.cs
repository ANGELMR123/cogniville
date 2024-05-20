using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Puzzle3 : MonoBehaviour
{
    public NumberBox3 boxPrefab;

    public NumberBox3[,] boxes = new NumberBox3[3, 3];

    public Sprite[] sprites;

    private List<int[,]> predefinedLevels = new List<int[,]>();

    private const string ProgressFilePath = "progress3.txt";

    void Start()
    {
        // Obtiene la ruta donde se guardarán los datos persistentes en la plataforma actual
        string persistentDataPath = Application.persistentDataPath;

        // Imprime la ruta en la consola
        Debug.Log("Ruta de datos persistentes: " + persistentDataPath);

        // Cargar niveles desde el archivo
        AddPredefinedLevels();

        int currentLevel = LoadProgress();
        LoadLevel(currentLevel);
    }

    // Método para cargar los niveles desde un archivo JSON
    void AddPredefinedLevels()
    {
        int[,] level1 = new int[3, 3] {
            { 1, 2, 9 },
            { 4, 5, 3 },
            { 7, 8, 6 }
        };
        predefinedLevels.Add(level1);

        int[,] level2 = new int[3, 3] {
            { 1, 2, 3 },
            { 9, 5, 6 },
            { 4, 7, 8 }
        };
        predefinedLevels.Add(level2);

        int[,] level3 = new int[3, 3] {
            { 1, 9, 3 },
            { 4, 2, 6 },
            { 7, 5, 8 }
        };
        predefinedLevels.Add(level3);

        int[,] level4 = new int[3, 3] {
            { 9, 2, 3 },
            { 1, 5, 6 },
            { 4, 7, 8 }
        };
        predefinedLevels.Add(level4);

        int[,] level5 = new int[3, 3] {
            { 1, 8, 3 },
            { 4, 5, 6 },
            { 7, 2, 9 }
        };
        predefinedLevels.Add(level5);

        int[,] level6 = new int[3, 3] {
            { 4, 1, 3 },
            { 9, 5, 6 },
            { 7, 8, 2 }
        };
        predefinedLevels.Add(level6);
    }

    // Método para cargar un nivel específico
    void LoadLevel(int levelIndex)
    {
        // Destruir el tablero anterior antes de cargar el nuevo nivel
        DestroyBoard();

        if (levelIndex < predefinedLevels.Count)
        {
            int[,] level = predefinedLevels[levelIndex];

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    int number = level[y, x];
                    NumberBox3 box = Instantiate(boxPrefab, new Vector2(x, 2 - y), Quaternion.identity);
                    box.Init(x, 2 - y, number, sprites[number - 1], ClickToSwap);
                    boxes[x, 2 - y] = box;
                }
            }
        }
    }

    void DestroyBoard()
    {
        foreach (NumberBox3 box in boxes)
        {
            if (box != null)
            {
                Destroy(box.gameObject);
            }
        }
    }

    void ClickToSwap(int x, int y)
    {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        Swap(x, y, dx, dy);
    }

    void Swap(int x, int y, int dx, int dy)
    {
        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        // Intercambia las posiciones
        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        // Actualiza las posiciones
        from.UpdatePos(x + dx, y + dy);
        target.UpdatePos(x, y);
    }

    int getDx(int x, int y)
    {
        // Comprueba si la derecha está vacía
        if (x < 2 && boxes[x + 1, y].IsEmpty3())
            return 1;

        // Comprueba si la izquierda está vacía
        if (x > 0 && boxes[x - 1, y].IsEmpty3())
            return -1;

        return 0;
    }

    int getDy(int x, int y)
    {
        // Comprueba si arriba está vacía
        if (y < 2 && boxes[x, y + 1].IsEmpty3())
            return 1;

        // Comprueba si abajo está vacía
        if (y > 0 && boxes[x, y - 1].IsEmpty3())
            return -1;

        return 0;
    }

    // Método para avanzar al siguiente nivel
    void AdvanceToNextLevel()
    {
        int currentLevel = LoadProgress();
        currentLevel++;
        SaveProgress(currentLevel);

        if (currentLevel == 4)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("BloqueMedio");
        }
        else if (currentLevel == 7)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("BloqueDificil");
        }
        else
        {
            // Cargar la escena LevelMenu antes de cargar el siguiente nivel
            StartCoroutine(LoadLevelMenuThenNextLevel(currentLevel));
        }
    }

    IEnumerator LoadLevelMenuThenNextLevel(int nextLevel)
    {
        // Cargar la escena LevelMenu
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelMenu");

        // Esperar a que la escena LevelMenu se cargue completamente
        yield return new WaitForSeconds(1); 

        // Cargar el siguiente nivel
        LoadLevel(nextLevel);
    }

    bool IsSolved()
    {
        int n = 1;
        for (int y = 2; y >= 0; y--)
        {
            for (int x = 0; x < 3; x++)
            {
                if (boxes[x, y].index != n)
                    return false;
                n++;
                if (n == 9)
                    return true;
            }
        }
        return true;
    }

    void Update()
    {
        if (IsSolved())
        {
            Debug.Log("Puzzle Resuelto");
            AdvanceToNextLevel();
        }
    }

    void SaveProgress(int level)
    {
        string filePath = Path.Combine(Application.persistentDataPath, ProgressFilePath);
        File.WriteAllText(filePath, level.ToString());
    }

    int LoadProgress()
    {
        string filePath = Path.Combine(Application.persistentDataPath, ProgressFilePath);
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
}
