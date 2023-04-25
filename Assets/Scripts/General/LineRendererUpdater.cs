using UnityEngine;

public class LineRendererUpdater : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform end1, end2;

    void Update()
    {
        lineRenderer.SetPosition(0, end1.position);
        lineRenderer.SetPosition(1, end2.position);
    }
}
