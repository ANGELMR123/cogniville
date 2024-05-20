using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefab;

    public NumberBox[,] boxes = new NumberBox[4, 4];

    public Sprite[] sprites;

    private List<int[,]> predefinedLevels = new List<int[,]>();

    private const string ProgressFilePath = "progress4.txt";

    // Start is called before the first frame update
    void Start()
    {
        // Cargar niveles desde el archivo
        AddPredefinedLevels();

        int currentLevel = LoadProgress();
        LoadLevel(currentLevel);
    }

    // Método para cargar los niveles desde un archivo JSON
    void AddPredefinedLevels()
    {
        int[,] level1 = new int[4, 4] {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 },
            { 13, 14, 16, 15 }
        };
        predefinedLevels.Add(level1);

        int[,] level2 = new int[4, 4] {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 16 },
            { 13, 14, 15, 12 }
        };
        predefinedLevels.Add(level2);

        int[,] level3 = new int[4, 4] {
            { 1, 2, 3, 4 },
            { 5, 6, 7, 8 },
            { 9, 10, 11, 12 },
            { 13, 16, 14, 15 }
        };
        predefinedLevels.Add(level3);

    }

    // Método para cargar un nivel específico
    void LoadLevel(int levelIndex)
    {
        // Destruir el tablero anterior antes de cargar el nuevo nivel
        DestroyAllBoxes();

        if (levelIndex < predefinedLevels.Count)
        {
            int[,] level = predefinedLevels[levelIndex];

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    int number = level[y, x];
                    NumberBox box = Instantiate(boxPrefab, new Vector2(x, 3 - y), Quaternion.identity);
                    box.Init(x, 3 - y, number, sprites[number - 1], ClickToSwap);
                    boxes[x, 3 - y] = box;
                }
            }
        }
    }

    // Método para destruir todas las instancias de NumberBox existentes
    void DestroyAllBoxes()
    {
        foreach (NumberBox box in boxes)
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
        if (x < 3 && boxes[x + 1, y].IsEmpty())
            return 1;

        // Comprueba si la izquierda está vacía
        if (x > 0 && boxes[x - 1, y].IsEmpty())
            return -1;

        return 0;
    }

    int getDy(int x, int y)
    {
        // Comprueba si arriba está vacía
        if (y < 3 && boxes[x, y + 1].IsEmpty())
            return 1;

        // Comprueba si abajo está vacía
        if (y > 0 && boxes[x, y - 1].IsEmpty())
            return -1;

        return 0;
    }

    // Método para avanzar al siguiente nivel
    void AdvanceToNextLevel()
    {
        int currentLevel = LoadProgress();
        currentLevel++;
        SaveProgress(currentLevel);

        if (currentLevel == 3)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("FinalMenu");
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
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                if (boxes[x, y].index != n)
                    return false;
                n++;
                if (n == 16)
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
