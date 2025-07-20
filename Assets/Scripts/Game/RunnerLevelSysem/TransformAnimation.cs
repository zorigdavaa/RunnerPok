using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransformAnimation : MonoBehaviour
{
    [SerializeField] Transform Model;
    public float duration = 1;
    public bool AnimateAtStart;
    public float Delay = 0;
    public UnityEvent OnComplete;
    void Start()
    {
        if (!Model)
        {
            Model = transform;
        }
        SetTargetPos();
        if (AnimateAtStart)
        {
            Animate();
        }
    }
    void OnValidate()
    {
        SetTargetPos();
    }

    public void SetTargetPos()
    {
        TargetPos = Model.position + Pos;
    }

    public Vector3 Pos = new Vector3(0, 0, 0);
    public Vector3 TargetRot = Vector3.zero;
    public Vector3 TargetScale = Vector3.one;
    private Vector3 TargetPos;

    [ContextMenu("Animate")]
    public void Animate()
    {
        StartCoroutine(LocalCor());
    }

    private IEnumerator LocalCor()
    {
        yield return new WaitForSeconds(Delay);
        float t = 0;
        float time = 0;

        Quaternion initRot = transform.rotation;
        Vector3 initPOs = transform.position;
        Vector3 initScale = transform.localScale;
        while (time < duration)
        {
            time += Time.deltaTime;
            t = time / duration;
            Model.position = Vector3.Lerp(initPOs, TargetPos, t);
            Model.localScale = Vector3.Lerp(initScale, TargetScale, t);
            Model.rotation = Quaternion.Lerp(initRot, Quaternion.Euler(TargetRot), t);
            yield return null;
        }
        OnComplete?.Invoke();
    }
}
