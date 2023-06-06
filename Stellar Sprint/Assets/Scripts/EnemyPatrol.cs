using System;
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

    public bool isPlayerDetected;

    private Transform playerTransform;
    public float agroRange = 10f;

    private Vector3 currentFacing;

    private PlayerLife playerLife;

    private void Start()
    {
        if (waypoints != null)
            Init();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
    }

    public void Update()
    {
        if (playerTransform != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distToPlayer < agroRange && playerLife.isPlayerAlive)
            {
                isPlayerDetected = true;
            }
            else
            {
                isPlayerDetected = false;
            }

            if (isPlayerDetected)
            {
                if (transform.position.x < playerTransform.position.x)
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                    //Таран игрока
                    //transform.position += Vector3.right * speed * Time.deltaTime;
                }
                anim.SetFloat("speed", 0f);
                anim.SetTrigger("attack");
            }
            else
            {
                if (waypoints != null)
                {
                    if (currentFacing == waypoints[0].position && transform.localEulerAngles == new Vector3(0, 0, 0))
                    {
                        transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                    else if (currentFacing != waypoints[0].position && transform.localEulerAngles == new Vector3(0, 180, 0))
                    {
                        transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    Movement();
                }
            }
        }
    }

    public void Init()
    {
        anim = GetComponentInChildren<Animator>();
        target = waypoints[1].position;
    }

    public IEnumerator SetTarget(Vector3 position)
    {        
        yield return new WaitForSeconds(waitSeconds);
        target = position;
        FaceTowards(position - transform.position);
        currentFacing = target;
    }

    public void FaceTowards(Vector3 direction)
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(waypoints[0].transform.position, waypoints[1].transform.position);
    }
}
