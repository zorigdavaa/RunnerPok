using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastAble
{
    public Transform transform { get; }
    public GameObject gameObject { get; }
    public bool Casting { get; set; }
    public void Cast();
}
