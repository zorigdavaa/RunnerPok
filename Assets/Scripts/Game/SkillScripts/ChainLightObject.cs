using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChainLightObject : MonoBehaviour
{
    public List<LightningPositionTrackData> Lightnings;
    // Start is called before the first frame update
    void Start()
    {
        Lightnings = new List<LightningPositionTrackData>();
    }
    private void Update()
    {
        for (int i = Lightnings.Count - 1; i >= 0; i--)
        {
            Lightnings[i].LineRender.SetPosition(0, Lightnings[i].Start.position + Vector3.one);
            Lightnings[i].LineRender.SetPosition(1, Lightnings[i].End.position + Vector3.one);
            if (Lightnings[i].Tick())
            {
                LightningPositionTrackData done = Lightnings[i];
                Lightnings.Remove(done);
                Destroy(done.LineRender.gameObject);
                Destroy(done);
            }
        }
    }

    public void Cast()
    {
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
    }
    void CreateLightningEffect(Transform start, Transform end)
    {
        GameObject lightning = Instantiate(lightningPrefab, start.position, Quaternion.identity, transform);
        LineRenderer lineRenderer = lightning.GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            Lightnings.Add(new LightningPositionTrackData(lineRenderer, start, end));
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
    public float Timer = 1f;
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
