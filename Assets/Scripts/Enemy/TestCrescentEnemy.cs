using UnityEngine;
using System.Collections;
using DanmakU;
using DanmakU.Modifiers;
using DanmakU.Controllers;
using System.Collections.Generic;

public class TestCrescentEnemy : Enemy
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
        fireData.WithSpeed(10);

        ColorChangeController colorController = new ColorChangeController();
        Gradient g = new Gradient();
        GradientColorKey[] gck = new GradientColorKey[1];
        gck[0].color = Color.magenta;
        GradientAlphaKey[] gak = new GradientAlphaKey[1];
        gak[0].alpha = 1.0F;
        g.SetKeys(gck, gak);
        colorController.ColorGradient = g;

        RotateController rotateController = new RotateController();
        rotateController.RotateSpeed = 4;

        List<DanmakuController> controllers = new List<DanmakuController>();
        controllers.Add(colorController.Update);
        controllers.Add(rotateController.Update);
        
        fireData.WithControllers(controllers);

        List<AttackBehavior> attacks = new List<AttackBehavior>();
        AddMovementBehavior(new OrbitAroundPlayerBehavior(1, 4, 8, float.MaxValue));
        attacks.Add(new CircularAttackBehavior(fireData, 2, 16, 6));
        AddAttackBehavior(new CombinedAttackBehavior(attacks));
        loopBehaviors = true;
    }

    public override void NormalUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position), Time.deltaTime * 4);
    }
}
