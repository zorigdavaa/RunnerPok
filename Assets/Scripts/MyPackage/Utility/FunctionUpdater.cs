using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ZPackage.FunctionTimer;

public class FunctionUpdater
{
    public static List<FunctionUpdater> activeTimers;
    private static GameObject initGameObject;
    private static void InitIfNeeded()
    {
        if (initGameObject == null)
        {
            initGameObject = new GameObject("FunctionUpdaterAll");
            activeTimers = new List<FunctionUpdater>();
        }
    }
    public static FunctionUpdater Create(Action action, float timer, string name = null)
    {
        InitIfNeeded();
        GameObject gameObject = new GameObject(nameof(FunctionUpdater), typeof(MonobehaviourHook));
        gameObject.transform.SetParent(initGameObject.transform);
        FunctionUpdater functionTimer = new FunctionUpdater(action, timer, name, gameObject);
        gameObject.GetComponent<MonobehaviourHook>().onUpdate = functionTimer.Update;
        activeTimers.Add(functionTimer);
        return functionTimer;
    }
    Action _action;
    float _timer;
    float _ResetTimer;
    string _name;
    bool _isDestroyed;
    GameObject _gameObject;
    public FunctionUpdater(Action action, float timer, string name, GameObject gameObject)
    {
        _action = action;
        _timer = timer;
        _ResetTimer = timer;
        _name = name;
        _isDestroyed = false;
        _gameObject = gameObject;
    }
    public void Update()
    {
        if (_isDestroyed)
        {
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _action();
            _timer = _ResetTimer;
        }
    }
    public static void StopTimer(string name)
    {
        for (int i = 0; i < activeTimers.Count; i++)
        {
            if (activeTimers[i]._name == name)
            {
                activeTimers[i].DestroySelf();
                i--;
            }

        }
    }
    private static void RemoveTimer(FunctionUpdater timer)
    {
        InitIfNeeded();
        activeTimers.Remove(timer);
    }
    private void DestroySelf()
    {
        _isDestroyed = true;
        GameObject.Destroy(_gameObject);
        RemoveTimer(this);
    }

    public void Remove()
    {
        throw new NotImplementedException();
    }
}
