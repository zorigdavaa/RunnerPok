using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtilities;
using Random = UnityEngine.Random;

public class ChainLightObject : BaseSkill
{
    public bool Casting { get; set; }
    public List<LightningPositionTrackData> Lightnings;
    // FunctionUpdater updater;
    public override void Equip()
    {
        CoolDown = new Countdown(true, 3);
        base.Equip();
        Lightnings = new List<LightningPositionTrackData>();
        // updater = FunctionUpdater.Create(() => { Use(this, null); }, 3, name);
    }
    public override void UnEquip()
    {
        base.UnEquip();
        FunctionUpdater.StopTimer(name);
    }

    private void Update()
    {
        if (CoolDown.Progress())
        {
            Use(this, 0);
        }
        for (int i = Lightnings.Count - 1; i >= 0; i--)
        {
            Lightnings[i].LineRender.SetPosition(0, Lightnings[i].Start.position + Vector3.up);
            Lightnings[i].LineRender.SetPosition(1, Lightnings[i].End.position + Vector3.up);
            float Distance = Vector3.Distance(Lightnings[i].Start.position, Lightnings[i].End.position);
            float xScale = Mathf.Clamp(Distance * 0.1f, 0.01f, 1);
            Lightnings[i].LineRender.textureScale = new Vector2(xScale, 1);
            // Lightnings[i].LineRender.textureScale = new Vector2(1, 1);
            if (Lightnings[i].Tick())
            {
                LightningPositionTrackData done = Lightnings[i];
                Lightnings.Remove(done);
                Destroy(done.LineRender.gameObject);
                Destroy(done);
            }
        }
    }

    public override void Use(object sender, object e)
    {
        Casting = true;
        StartCoroutine(TriggerChainLightning(transform));
    }
    public GameObject lightningPrefab;  // Prefab with LineRenderer or VFX for lightning
    public int maxChains = 5;           // Maximum number of targets
    public float range = 10f;           // Chain range
    public float chainDelay = 0.2f;     // Delay between chains
    public LayerMask targetMask;        // Layer for valid targets

    IEnumerator TriggerChainLightning(Transform start)
    {
        Transform currentPosition = start;
        HashSet<Transform> hitTargets = new HashSet<Transform>();

        for (int i = 0; i < maxChains; i++)
        {
            Collider[] targets = Physics.OverlapSphere(currentPosition.position, range, targetMask);

            Transform nextTarget = null;
            float closestDistance = float.MaxValue;

            foreach (Collider target in targets)
            {
                if (hitTargets.Contains(target.transform)) continue;

                float distance = Vector3.Distance(currentPosition.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nextTarget = target.transform;
                }
            }

            if (nextTarget == null) break;

            // Record the target as hit
            hitTargets.Add(nextTarget);

            // Visual Effect
            CreateLightningEffect(currentPosition, nextTarget);

            // Apply damage or effect
            // nextTarget.GetComponent<Health>()?.TakeDamage(damage);

            // Update position for next chain
            currentPosition = nextTarget;

            // Wait before the next chain
            yield return new WaitForSeconds(chainDelay);
        }
        Casting = false;
    }
    void CreateLightningEffect(Transform start, Transform end)
    {
        GameObject lightning = Instantiate(lightningPrefab, start.position, Quaternion.identity, transform);
        LineRenderer lineRenderer = lightning.GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            Lightnings.Add(new LightningPositionTrackData(lineRenderer, start, end));
            IHealth StartHealth = start.GetComponent<IHealth>();
            IHealth enHealth = end.GetComponent<IHealth>();
            if (StartHealth != null)
            {
                StartHealth.TakeDamage(-5);
            }
            if (enHealth != null)
            {
                enHealth.TakeDamage(-5);
            }

        }
    }

    void CreateLightningEffect(Vector3 start, Vector3 end)
    {
        GameObject lightning = Instantiate(lightningPrefab, start, Quaternion.identity);
        LineRenderer lineRenderer = lightning.GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            // Add jaggedness (randomness to positions)
            Vector3[] positions = new Vector3[2];
            positions[0] = start;
            positions[1] = end + new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f)
            );
            lineRenderer.SetPositions(positions);
        }

        Destroy(lightning, 0.5f);  // Clean up after a short duration
    }
}
public class LightningPositionTrackData : UnityEngine.Object
{
    public LineRenderer LineRender;
    public Transform Start;
    public Transform End;
    public float Timer = 0.1f;
    public EventHandler OnTimerZero;
    public LightningPositionTrackData(LineRenderer line, Transform _start, Transform _end, float timer = 1)
    {
        LineRender = line;
        Start = _start;
        End = _end;
        Timer = timer;
    }

    public bool Tick()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            OnTimerZero?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }
}
