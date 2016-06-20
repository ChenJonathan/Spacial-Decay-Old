using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public class CombinedAttackBehavior : Enemy.AttackBehavior
{
    private List<Enemy.AttackBehavior> attackBehaviors;
    private Dictionary<Enemy.AttackBehavior, bool> attackActive;
    
    public CombinedAttackBehavior(List<Enemy.AttackBehavior> attackBehaviors)
    {
        this.attackBehaviors = attackBehaviors;
        attackActive = new Dictionary<Enemy.AttackBehavior, bool>();
        foreach (Enemy.AttackBehavior attackBehavior in attackBehaviors)
        {
            attackActive.Add(attackBehavior, true);
        }
        OnBehaviorEnd += DeactivateAll;
    }

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);
        foreach (Enemy.AttackBehavior attackBehavior in attackBehaviors)
        {
            attackBehavior.Start(enemy);
            attackBehavior.OnBehaviorEnd -= Enemy.AttackBehavior.IncrementBehavior;
            attackBehavior.OnBehaviorEnd += DeactivateBehavior;
            attackActive[attackBehavior] = true;
        }
    }

    public override void Update()
    {
        bool finished = true;
        foreach (Enemy.AttackBehavior attackBehavior in attackBehaviors)
        {
            if (attackActive[attackBehavior])
            {
                attackBehavior.Update();
                finished = false;
            }
        }
        if (finished)
            End();
    }

    private void DeactivateBehavior(Enemy.Behavior behavior)
    {
        attackActive[(Enemy.AttackBehavior)behavior] = false;
    }

    private void DeactivateAll(Enemy.Behavior behavior)
    {
        foreach (Enemy.AttackBehavior attackBehavior in attackBehaviors)
        {
            if (attackActive[attackBehavior])
            {
                attackBehavior.End();
            }
        }
    }
}
