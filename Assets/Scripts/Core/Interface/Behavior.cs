using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public abstract partial class Enemy : DanmakuCollider, IPausable
{
    private AttackBehavior attackBehavior;
    private MovementBehavior movementBehavior;

    protected void AddAttackBehavior(AttackBehavior behavior)
    {
        if (attackBehavior == null)
        {
            attackBehavior = behavior;
            attackBehavior.Start(this);
        }
        else
        {
            attackBehavior.Chain(behavior);
        }
    }

    protected void AddMovementBehavior(MovementBehavior behavior)
    {
        if (movementBehavior == null)
        {
            movementBehavior = behavior;
            movementBehavior.Start(this);
        }
        else
        {
            movementBehavior.Chain(behavior);
        }
    }

    protected void EndAttackBehavior()
    {
        attackBehavior.End();
    }

    protected void EndMovementBehavior()
    {
        movementBehavior.End();
    }

    protected void ClearAttackBehavior()
    {
        attackBehavior = null;
    }

    protected void ClearMovementBehavior()
    {
        movementBehavior = null;
    }

    public abstract class Behavior
    {
        protected Player player;
        protected Enemy enemy;
        protected List<Enemy> enemies;
        
        protected float time = 0;

        public virtual void Start(Enemy enemy)
        {
            player = enemy.player;
            this.enemy = enemy;
            enemies = enemy.enemies;
        }

        public virtual void Update()
        {
            time += Time.deltaTime;
        }
    }

    public abstract class AttackBehavior : Behavior
    {
        private AttackBehavior next;

        public void Chain(AttackBehavior behavior)
        {
            AttackBehavior temp = next;
            while(temp.next != null)
            {
                temp = temp.next;
            }
            temp.next = behavior;
        }

        public void End()
        {
            enemy.attackBehavior = null;
            if(next != null)
                enemy.AddAttackBehavior(next);
        }
    }

    public abstract class MovementBehavior : Behavior
    {
        private MovementBehavior next;

        public void Chain(MovementBehavior behavior)
        {
            MovementBehavior temp = next;
            while (temp.next != null)
            {
                temp = temp.next;
            }
            temp.next = behavior;
        }

        public void End()
        {
            enemy.movementBehavior = null;
            if(next != null)
                enemy.AddMovementBehavior(next);
        }
    }
}
