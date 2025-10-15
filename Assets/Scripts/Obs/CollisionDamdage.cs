using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class CollisionDamdage : MonoBehaviour, ICollisionAction
{
    public float Damage = 5;
    public GameObject feathersVFX;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            player.TakeDamage(-Damage);
            if (feathersVFX)
            {

                GameObject particle = Instantiate(feathersVFX, transform.position, Quaternion.identity);
                Destroy(particle, 2);
            }
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(LoadFeatherVFX());
    }
    IEnumerator LoadFeatherVFX()
    {
        yield return null;
        Damage *= Z.LS.LastInstLvl.DamageMultiplier;
        // Begin loading asynchronously
        ResourceRequest request = Resources.LoadAsync<GameObject>("Game/FeatherExplosion");

        // Wait until loading finishes
        yield return request;

        // Get the loaded asset
        feathersVFX = request.asset as GameObject;

        if (feathersVFX == null)
        {
            Debug.LogError("FeatherExplosion prefab not found in Resources/Game!");
            yield break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
