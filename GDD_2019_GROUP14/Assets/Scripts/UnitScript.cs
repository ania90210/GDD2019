using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{

    const float distance2player2 = 65;
    const float pathUpdateMoveThreshold = 0.3f;
    const float minUpdateTime = 0.2f;
    public Transform target;
    float speed = 0.05f;
    bool searching;
    Vector3[] path;
    int targetIndex;


    // light
    public UnityEngine.Experimental.Rendering.LWRP.Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        //StartCoroutine(UpdatePath());
        target = GameObject.FindGameObjectWithTag("Player").transform;
        light.enabled = false;
        StartCoroutine(DoCheck());

    }


    bool PlayerClose()
    {
        return (target.transform.position - this.transform.position).sqrMagnitude < distance2player2;
    }

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            if (PlayerClose() && !searching)
            {
                this.searching = true;
                light.enabled = true;
                StartCoroutine(UpdatePath());
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;


        while (true)
        {
            yield return new WaitForSeconds(minUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }

        }
    }

    IEnumerator FollowPath()
    {

        Debug.Log(path.Length);
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
                yield return null;

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
