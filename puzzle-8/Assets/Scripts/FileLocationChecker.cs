using System;
using System.IO;
using UnityEngine;

public class FileLocationChecker : MonoBehaviour
{
    void Start()
    {
        // Obtiene la ruta donde se guardar�n los datos persistentes en la plataforma actual
        string persistentDataPath = Application.persistentDataPath;

        // Imprime la ruta en la consola
        Debug.Log("Ruta de datos persistentes: " + persistentDataPath);

        // Si necesitas guardar archivos espec�ficamente en una subcarpeta dentro de persistentDataPath, puedes hacerlo as�:
        string mySubFolderPath = Path.Combine(persistentDataPath, "MySubfolder");
        Debug.Log("Ruta de mi subcarpeta: " + mySubFolderPath);
    }
}
