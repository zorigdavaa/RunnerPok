using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class Wave : BaseSkill
{
    public Transform Model;
    public float duration = 4;
    float ownTime = 0;
    [SerializeField] AnimationCurve scaleAnimation;
    [SerializeField] AnimationCurve moveAnimation;
    public bool Casting { get; set; }
    Vector3 TargetPos;
    Vector3 InitPos;


    // Start is called before the first frame update
    void Start()
    {
        CoolDown = new Countdown(true, 3f);
        Model.localScale = Vector3.one * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (CoolDown.Progress())
        {
            Use(this, null);
            // CoolDown.Reset();
        }
        if (Casting)
        {
            float t = ownTime / duration;
            ownTime += Time.deltaTime;
            Model.localScale = Vector3.Lerp(Vector3.one * 0.1f, Vector3.one, scaleAnimation.Evaluate(t));
            Model.position = Vector3.Lerp(InitPos, TargetPos, moveAnimation.Evaluate(t));
            if (t > 1)
            {
                ownTime = 0;
                Casting = false;
                Model.gameObject.SetActive(false);
                Model.position = transform.position;
                Model.localScale = Vector3.zero;
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

    public override void Use(object sender, object e)
    {
        Casting = true;
        Model.gameObject.SetActive(true);
        InitPos = transform.position + Vector3.up;
        TargetPos = transform.position + Vector3.forward * 45 + Vector3.up;
    }
}
