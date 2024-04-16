using System.Collections;
using System.Collections.Generic;
using ZPackage;
using UnityEngine;
using Unity;
using ZPackage.Helper;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    Transform insParent;
    // Start is called before the first frame update
    void Start()
    {
        // insParent = Z.Player.transform.parent;
        // for (int i = 0; i < 6; i++)
        // {
        //     Vector3 pos = insParent.position + new Vector3(Random.onUnitSphere.x, 0, Random.onUnitSphere.z) * 3;
        //     GameObject insEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity, insParent);
        // }
    }
}
