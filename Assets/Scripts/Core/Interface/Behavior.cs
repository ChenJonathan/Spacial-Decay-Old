﻿using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

public abstract partial class Enemy : DanmakuCollider, IPausable
{
    private AttackBehavior attackBehavior;
    private MovementBehavior movementBehavior;

    protected bool loopBehaviors = false;

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

    // Cannot be reused
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
            time = 0;
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
            if (behavior.next != null)
                return;
            AttackBehavior temp = this;
            while (temp.next != null)
            {
                temp = temp.next;
            }
            if (temp != behavior)
                temp.next = behavior;
        }

        public void End()
        {
            enemy.attackBehavior = next;
            next = null;
            if (enemy.loopBehaviors)
            {
                enemy.AddAttackBehavior(this);
                if (enemy.attackBehavior != this)
                    enemy.attackBehavior.Start(enemy);

            }
            else if (enemy.movementBehavior != null)
                enemy.movementBehavior.Start(enemy);
        }
    }

    public abstract class MovementBehavior : Behavior
    {
        private MovementBehavior next;

        public void Chain(MovementBehavior behavior)
        {
            if (behavior.next != null)
                return;
            MovementBehavior temp = this;
            while (temp.next != null)
            {
                temp = temp.next;
            }
            if (temp != behavior)
                temp.next = behavior;
        }

        public void End()
        {
            enemy.movementBehavior = next;
            next = null;
            if (enemy.loopBehaviors)
            {
                enemy.AddMovementBehavior(this);
                if (enemy.movementBehavior != this)
                    enemy.movementBehavior.Start(enemy);

            }
            else if (enemy.movementBehavior != null)
                enemy.movementBehavior.Start(enemy);
        }
    }
}
