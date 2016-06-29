using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Modifiers;
using DanmakU.Controllers;
using System.Collections.Generic;

public class TestEnemy : Enemy
{
    [SerializeField]
    private DanmakuPrefab bulletPrefab;
    private FireBuilder fireData;

    [SerializeField]
    private float fireRate = 1f;
    private float fireDelay = 0;

    void Start()
    {
        health = maxHealth = 100 * difficulty;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position);

        fireData = new FireBuilder(bulletPrefab, Field);
        fireData.From(transform);
        fireData.Towards(player);
        fireData.WithSpeed(100);

        AutoDeactivateController beamController = new AutoDeactivateController(60);

        ColorChangeController colorController = new ColorChangeController();
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[1];
        gck[0].color = Color.cyan;
        GradientAlphaKey[] gak = new GradientAlphaKey[1];
        gak[0].alpha = 1.0F;
        g.SetKeys(gck, gak);
        colorController.ColorGradient = g;

        List<DanmakuController> controllers = new List<DanmakuController>();
        controllers.Add(beamController.Update);
        controllers.Add(colorController.Update);

        fireData.WithControllers(controllers);
    }

    public override void NormalUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > 10)
            transform.position = Vector2.Lerp(transform.position, player.transform.position, 0.005f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);

        fireDelay -= Time.deltaTime;

        if (fireDelay <= 0)
        {
            fireDelay += 1 / fireRate;
            fireData.Fire();
        }
    }
}
