using System;
using System.IO;
using UnityEngine;

public class FileLocationChecker : MonoBehaviour
{
    void Start()
    {
        // Obtiene la ruta donde se guardarán los datos persistentes en la plataforma actual
        string persistentDataPath = Application.persistentDataPath;

        // Imprime la ruta en la consola
        Debug.Log("Ruta de datos persistentes: " + persistentDataPath);

        // Si necesitas guardar archivos específicamente en una subcarpeta dentro de persistentDataPath, puedes hacerlo así:
        string mySubFolderPath = Path.Combine(persistentDataPath, "MySubfolder");
        Debug.Log("Ruta de mi subcarpeta: " + mySubFolderPath);
    }
}
