using UnityEngine;
using UnityEngine.Splines;

public class SplineInsHelper : MonoBehaviour
{
    private SplineInstantiate splineInstantiate;


    [ContextMenu("Randomize Spline")]
    public void Randomize()
    {
        if (splineInstantiate == null)
        {
            splineInstantiate = GetComponent<SplineInstantiate>();
        }
        splineInstantiate.Randomize();
        splineInstantiate.UpdateInstances();
        // splineInstantiate.enabled = false;
        // splineInstantiate.enabled = true;
    }

}
