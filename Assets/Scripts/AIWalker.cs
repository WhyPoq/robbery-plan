using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class AIWalker : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent reached = new();
    public float reachDist = 0.1f;

    private AIStopLerp aiLerp;
    private Seeker seeker;

    Vector3 reachedDest = new Vector3(0, 0, -1);

    public float timer = 0;
    float wait = 1f;

    public LayerMask obstacles;

    private void Awake()
    {
        aiLerp = GetComponent<AIStopLerp>();
        seeker = GetComponent<Seeker>();
        aiLerp.reached.AddListener(Reached);
    }

    private void Update()
    {
        //timer += Time.deltaTime;
        /*if (seeker.IsDone() && timer >= wait && aiLerp.remainingDistance <= reachDist && aiLerp.destination != reachedDest)
        {
            Reached();
        }*/

        float dist = Vector3.Distance(transform.position, aiLerp.destination);
        if (dist <= reachDist)
        {
            if (dist <= 0.01f || Physics2D.Raycast(transform.position, aiLerp.destination - transform.position, ~obstacles).collider != null)
            {
                Reached();
            }
        }
    }
    private void Reached()
    {
        reachedDest = aiLerp.destination;
        reached.Invoke();
        timer = 0;
    }
}
