using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour, ICastAble
{
    public Transform Model;
    public float duration = 4;
    float ownTime = 0;
    [SerializeField] AnimationCurve scaleAnimation;
    [SerializeField] AnimationCurve moveAnimation;
    public bool Casting { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Model.localScale = Vector3.one * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Casting)
        {
            ownTime += Time.deltaTime;
            float t = ownTime / duration;
            Model.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one, scaleAnimation.Evaluate(t));
            Model.localPosition = Vector3.Lerp(Vector3.zero, Vector3.forward * 6, moveAnimation.Evaluate(t));
            if (t > 1)
            {
                ownTime = 0;
                Casting = false;
                // Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Impact(enemy);
    }

    public virtual void Impact(Enemy enemy)
    {
        if (enemy && enemy.IsAlive)
        {
            // enemy.TakeDamage(data.damageData);
            enemy.TakeDamage(-10);
        }
    }

    public void Cast()
    {
        Casting = true;
    }
}
