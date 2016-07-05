using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class SpawnData
{
    public Enemy Prefab;
    public int Health;
    public Color color;

    private Enemy.AttackBehavior attackBehavior;
    public Enemy.AttackBehavior AttackBehavior
    {
        get { return attackBehavior; }
    }
    private Enemy.MovementBehavior movementBehavior;
    public Enemy.MovementBehavior MovementBehavior
    {
        get { return movementBehavior; }
    }

    public bool FacePlayer;
    public bool LoopBehaviors;

    public SpawnData(Enemy prefab, int health)
    {
        Prefab = prefab;
        Health = health;
    }

    public SpawnData AddAttackBehavior(Enemy.AttackBehavior behavior)
    {
        if (attackBehavior == null)
        {
            attackBehavior = behavior;
        }
        else
        {
            attackBehavior.Chain(behavior);
        }
        return this;
    }

    public SpawnData AddMovementBehavior(Enemy.MovementBehavior behavior)
    {
        if (movementBehavior == null)
        {
            movementBehavior = behavior;
        }
        else
        {
            movementBehavior.Chain(behavior);
        }
        return this;
    }

    public SpawnData ClearAttackBehavior()
    {
        attackBehavior = null;
        return this;
    }

    public SpawnData ClearMovementBehavior()
    {
        movementBehavior = null;
        return this;
    }
}
