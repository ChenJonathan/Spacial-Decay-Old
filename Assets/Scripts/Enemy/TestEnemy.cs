using UnityEngine;
using System.Collections;
using DanmakU;

public class TestEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    private FireBuilder fireData;

    [SerializeField]
    private float fireRate = 1;
    private float fireDelay = 0;

    public override void Start ()
    {
        base.Start();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.Towards(player);
        fireData.WithSpeed(10);
    }
    
    void Update ()
    {
        if(Vector2.Distance(transform.position, player.transform.position) > 10)
            transform.position = Vector2.Lerp(transform.position, player.transform.position, 0.001f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);

        fireDelay -= Time.deltaTime;

        if (fireDelay <= 0)
        {
            fireDelay += 1 / fireRate;
            fireData.Fire();
        }
    }
}
