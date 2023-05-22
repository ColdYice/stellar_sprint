using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://bergstrand-niklas.medium.com/simple-patrolling-enemies-in-unity-e763fc3e054a

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    [SerializeField]
    protected float waitSeconds = 2f;

    protected Vector3 target; 
    protected Vector3 velocity;
    protected Vector3 previousPosition;
    protected Animator anim;
    protected bool isFlipped;

    [SerializeField]
    protected Transform[] waypoints;

    [SerializeField]
    protected bool isPlayerSpotted = false;

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (!isPlayerSpotted)
        {
            Movement();
        }
        else
        {
            anim.SetFloat("speed", 0f);
        }
    }

    public virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        target = waypoints[1].position;
    }

    public virtual IEnumerator SetTarget(Vector3 position)
    {
        yield return new WaitForSeconds(waitSeconds);
        target = position;
        FaceTowards(position - transform.position);
    }

    public virtual void FaceTowards(Vector3 direction)
    {
        if (direction.x < .1f)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public virtual void Movement()
    {
        velocity = ((transform.position - previousPosition) / Time.deltaTime);
        previousPosition = transform.position;

        anim.SetFloat("speed", velocity.magnitude);

        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if (target == waypoints[0].position)
            {
                if (isFlipped)
                {
                    isFlipped = !isFlipped;
                    StartCoroutine("SetTarget", waypoints[1].position);
                }
            }
            else
            {
                if (!isFlipped)
                {
                    isFlipped = !isFlipped;
                    StartCoroutine("SetTarget", waypoints[0].position);
                }
            }
        }
    }
}
