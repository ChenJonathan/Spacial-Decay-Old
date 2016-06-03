using UnityEngine;
using System.Collections;
using DanmakU;
using System;

public class Player : MonoBehaviour, IDanmakuCollider, IPausable
{

    [SerializeField]
    private DanmakuPrefab prefab;
    [SerializeField]
    private GameObject fireTargetPrefab;
    [SerializeField]
    private GameObject moveTargetPrefab;

    private FireBuilder fireData;
    private GameObject fireCrosshair;
    private GameObject moveCrosshair;

    [SerializeField]
    private int lives = 5;

    [SerializeField]
    private float fireRate = 12;
    private float fireDelay = 0;
    [SerializeField]
    private float moveSpeed = 16;
    [SerializeField]
    private float rotateSpeed = 10;
    
    private Vector2 fireTarget;
    private Vector2 moveTarget;
   

    public virtual DanmakuField Field
    {
        get;
        set;
    }

    public bool Paused
    {
        get;
        set;
    }

    public void OnDanmakuCollision(Danmaku danmaku, RaycastHit2D info)
    {
        lives--;
        danmaku.Deactivate();
    }

    void Start ()
    {
        fireTarget = new Vector2(transform.position.x, transform.position.y + 10);
        moveTarget = new Vector2(transform.position.x, transform.position.y);
        transform.rotation = Quaternion.identity;

        fireData = new FireBuilder(prefab, Field);
        fireData.From(transform);
        fireData.Towards(fireTarget);
        fireData.WithSpeed(32, 48);
        fireData.WithRotation(-2, 2);

        fireCrosshair = (GameObject)Instantiate(fireTargetPrefab, fireTarget, Quaternion.identity);
        fireCrosshair.transform.parent = Field.transform;
        moveCrosshair = (GameObject)Instantiate(moveTargetPrefab, moveTarget, Quaternion.identity);
        moveCrosshair.transform.parent = Field.transform;
    }
	
	void Update ()
    {
	    if(!Paused)
        {
            HandleInput();

            fireCrosshair.transform.position = fireTarget;
            moveCrosshair.transform.position = moveTarget;

            Renderer moveTargetRenderer = moveCrosshair.GetComponent<Renderer>();

            transform.position = Vector2.Lerp(transform.position, moveTarget, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, (Vector3)fireTarget - transform.position), Time.deltaTime * rotateSpeed);

            if (Vector2.Distance((Vector2)transform.position, moveTarget) < 0.1)
            {   
                fireDelay -= Time.deltaTime;
                moveTargetRenderer.enabled = false;
            }
            else
            {
                moveTargetRenderer.enabled = true;
            }

            if (fireDelay <= 0)
            {
                fireDelay += 1 / fireRate;
                fireData.Fire();
            }
        }
	}

    private void HandleInput()
    {
        if(Input.GetMouseButton(0))
        {
            moveTarget = Field.WorldPoint(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
        }
        if(Input.GetMouseButton(1))
        {
            fireTarget = Field.WorldPoint(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
            fireData.Towards(fireTarget);
        }
    }
}
