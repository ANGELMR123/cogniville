using System;
using System.IO;
using UnityEngine;

public static class LevelManager
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "level.txt");

    public static void SaveLevel(int level)
    {
        try
        {
            File.WriteAllText(filePath, level.ToString());
            Debug.Log("Level saved: " + level);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save level: " + e.Message);
        }
    }

    public static int LoadLevel()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string levelString = File.ReadAllText(filePath);
                if (int.TryParse(levelString, out int level))
                {
                    Debug.Log("Level loaded: " + level);
                    return level;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load level: " + e.Message);
        }

        return 1; // Return level 1 if there is no file or if an error occurs
    }
}
