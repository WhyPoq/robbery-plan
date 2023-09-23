using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float curRotation = 0;

    public float viewDistance;
    [Range(0, 360)]
    public float viewAngle;

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistThreshold;

    public LayerMask targetMask;
    public LayerMask obstaclesMask;

    public List<GameObject> targets = new List<GameObject>();

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void LateUpdate()
    {
        DrawFOV();
    }

    public Vector2 DirFromAngle(float angle)
    {
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    void FindVisibleTargets()
    {
        targets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewDistance, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(DirFromAngle(curRotation), dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstaclesMask))
                {
                    targets.Add(target.gameObject);
                }
            }
        }
    }

    void DrawFOV()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i < stepCount + 1; i++)
        {
            float angle = curRotation - viewAngle / 2 + i * stepAngleSize;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDistThreshold;
                if (oldViewCast.isHit != newViewCast.isHit || (edgeDistThresholdExceeded && oldViewCast.isHit && newViewCast.isHit))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector2.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector2.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.hitPoint);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.fireAngle;
        float maxAngle = maxViewCast.fireAngle;
        Vector2 minPoint = Vector2.zero;
        Vector2 maxPoint = Vector2.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dist - newViewCast.dist) > edgeDistThreshold;
            if (newViewCast.isHit == minViewCast.isHit && !edgeDistThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.hitPoint;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.hitPoint;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float angle)
    {
        Vector2 dir = DirFromAngle(angle);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, obstaclesMask);

        if (hit.collider != null)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else
        {
            return new ViewCastInfo(false, (Vector2)transform.position + dir * viewDistance, viewDistance, angle);
        }
    }

    public struct ViewCastInfo
    {
        public bool isHit;
        public Vector2 hitPoint;
        public float dist;
        public float fireAngle;

        public ViewCastInfo(bool isHit, Vector2 hitPoint, float dist, float fireAngle)
        {
            this.isHit = isHit;
            this.hitPoint = hitPoint;
            this.dist = dist;
            this.fireAngle = fireAngle;
        }
    }

    public struct EdgeInfo
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 pointA, Vector2 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }

}
