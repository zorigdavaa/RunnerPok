using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HeadItem : MonoBehaviour, ISaveAble
{
    protected ObjectPool<ChestItem> Pool;
    protected Coroutine AutoGotoPoolCor;
    [SerializeField] protected HeadItemData data;
    public void RetrieveData()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
