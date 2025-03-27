using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public List<ISave> Saves = new List<ISave>();
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one SaveManager in the scene");
        }
        else
        {
            Instance = this;
            Saves = FindObjectsOfType<MonoBehaviour>().OfType<ISave>().ToList();
        }
    }

    public static void Save()
    {
        foreach (var save in Instance.Saves)
        {
            save.Save();
        }
    }
    public static void Load()
    {
        foreach (var save in Instance.Saves)
        {
            save.Load();
        }
    }
}
