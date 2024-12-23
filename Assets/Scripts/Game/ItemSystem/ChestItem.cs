using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ChestItem : MonoBehaviour
{

    protected ObjectPool<ChestItem> Pool;
    protected Coroutine AutoGotoPoolCor;
    [SerializeField] protected ItemData data;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    internal void GetFrompool()
    {
        gameObject.SetActive(true);
        // transform.position = Vector3.zero;
        AutoGotoPoolCor = StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(5);
            Pool.Release(this);
            AutoGotoPoolCor = null;

        }
    }
    //Should Only called from Release
    internal void GotoPool()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
    internal void SetPool(ObjectPool<ChestItem> pool)
    {
        Pool = pool;
    }
    public void RetrieveData()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
