using UnityEngine;
public class CircleByThreePoints : MonoBehaviour
{
    public static CircleByThreePoints Instance;
    [SerializeField] private Vector3 planeNormal = Vector3.up;  
    [SerializeField] private float radius = 5.0f;              
    [SerializeField] private int segments = 100;     
    private LineRenderer lineRenderer;

    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
    }

    public Vector3 CreateCircle(Vector3 aP0, Vector3 aP1, Vector3 aP2) {
        lineRenderer = GetComponent<LineRenderer>();

        Debug.Log("Sphere Position: " + aP0 + ", " + aP1 + ", " + aP2);  
        Vector3 center = CircleCenter(aP0, aP1, aP2, out planeNormal);

        Debug.Log("Normal plane:" + planeNormal);
        DrawCircle(center, Vector3.Distance(center, aP0), segments);
    
        return center;
    }

    public Vector3 CircleCenter(Vector3 aP0, Vector3 aP1, Vector3 aP2, out Vector3 normal)
    {
        // two circle chords
        var v1 = aP1 - aP0;
        var v2 = aP2 - aP0;

        normal = Vector3.Cross(v1, v2);
        if (normal.sqrMagnitude < 0.00001f)
            return Vector3.one * float.NaN;
        normal.Normalize();

        // perpendicular of both chords
        var p1 = Vector3.Cross(v1, normal).normalized;
        var p2 = Vector3.Cross(v2, normal).normalized;
        // distance between the chord midpoints
        var r = (v1 - v2) * 0.5f;
        // center angle between the two perpendiculars
        var c = Vector3.Angle(p1, p2);
        // angle between first perpendicular and chord midpoint vector
        var a = Vector3.Angle(r, p1);
        // law of sine to calculate length of p2
        var d = r.magnitude * Mathf.Sin(a * Mathf.Deg2Rad) / Mathf.Sin(c * Mathf.Deg2Rad);
        if (Vector3.Dot(v1, aP2 - aP1) > 0)
            return aP0 + v2 * 0.5f - p2 * d;
        return aP0 + v2 * 0.5f + p2 * d;
    }


    void DrawCircle(Vector3 normal, float radius, int segments)
    {
        lineRenderer.positionCount = segments + 1;

        Vector3 basis1 = Vector3.Cross(normal, Vector3.up).normalized;
        if (basis1 == Vector3.zero)
            basis1 = Vector3.Cross(normal, Vector3.forward).normalized;
        Vector3 basis2 = Vector3.Cross(normal, basis1).normalized;

        Vector3[] positions = new Vector3[segments + 1];
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            Vector3 pointOnCircle = Mathf.Cos(angle) * basis1 * radius + Mathf.Sin(angle) * basis2 * radius;
            positions[i] = normal + pointOnCircle; // Adjust the circle position to the object's position
        }

        lineRenderer.SetPositions(positions);
    }
}