using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public bool Wait = false;
    public float AfterDelay = 0;
    public float Beforedelay = 0;
    public List<Enemy> EnemyPF;
}
