using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Animal : Enemy
{
    public List<GameObject> ProjectilePfs;
    public AnimalAnim animationController;
    public MovementForgeRun movement;
    [SerializeField] float idleSpeed = -1;
    bool StartMove = false;
    private void Start()
    {
        Health = MaxHealth;
        if (Vector3.Distance(Z.Player.transform.position, transform.position) < 25)
        {
            StartMove = true;
        }
        else
        {
            CheckPlayerDistance();
        }
    }

    private void CheckPlayerDistance()
    {
        StartCoroutine(LocalCor());
        IEnumerator LocalCor()
        {
            // yield return new WaitUntil(() => Vector3.Distance(Z.Player.transform.position, transform.position) < 10);
            while (Vector3.Distance(Z.Player.transform.position, transform.position) > 25)
            {
                yield return null;
                // print("Waiting");
            }
            print("Moving");
            StartMove = true;
            attackTimer = 0;
        }
    }

    float attackTimer = 3;
    private void Update()
    {
        if (StartMove)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0 && IsAlive)
            {
                attackTimer = 5;
                if (Random.value > 0.3f)
                {

                    Attack();
                }
                else
                {
                    MovePositon();
                }
            }
        }

    }

    public void MovePositon()
    {
        StartCoroutine(LocalCor());
        IEnumerator LocalCor()
        {
            animationController.SetSpeed(1);
            Vector3 goPos;
            if (transform.localPosition.x > 0)
            {

                goPos = transform.localPosition + Vector3.left * Random.Range(1, 3);
            }
            else
            {
                goPos = transform.localPosition + Vector3.right * Random.Range(1, 3);

            }
            animationController.SetSpeed(-1);
            float t = 0;
            float duration = 0.5f;
            float time = 0;
            Vector3 initPos = transform.localPosition;
            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                initPos.y += 5 * Time.deltaTime;
                transform.localPosition = Vector3.Lerp(initPos, goPos, t);
                yield return null;
            }
            animationController.SetSpeed(idleSpeed);
        }
    }
    public void Attack()
    {
        StartCoroutine(LocalCor());
        IEnumerator LocalCor()
        {
            Vector3 attackPos = transform.localPosition + Vector3.back * 10 + Vector3.right * Random.Range(-3, 3);
            float t = 0;
            float duration = 0.5f;
            float time = 0;
            Vector3 initPos = transform.localPosition;
            if (Mathf.Abs(attackPos.x) > 6)
            {
                attackPos.x = Mathf.Clamp(attackPos.x, -6, 6);
            }
            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                transform.localPosition = Vector3.Lerp(initPos, attackPos, t);
                yield return null;
            }
            animationController.Attack();
            yield return new WaitForSeconds(0.3f);
            AttackProjectile();
            time = 0;
            duration = 1f;
            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                transform.localPosition = Vector3.Lerp(attackPos, initPos, t);
                yield return null;
            }
        }
    }

    public override void Die()
    {
        base.Die();
        transform.SetParent(null);
        animationController.Die();
        movement?.Cancel();
        // rb.isKinematic = true;
    }
    public override void AttackProjectile()
    {
        GameObject pf = ProjectilePfs[Random.Range(0, ProjectilePfs.Count)];
        GameObject inSob = Instantiate(pf, transform.position + Vector3.up, transform.rotation, transform.parent);
        Destroy(inSob, 10);
    }
}
