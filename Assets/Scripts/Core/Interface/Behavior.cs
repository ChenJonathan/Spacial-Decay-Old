using UnityEngine;
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

        public void End()
        {
            if (OnBehaviorEnd != null)
                OnBehaviorEnd(this);
        }

        public delegate void BehaviorEvent(Behavior behavior);
        public BehaviorEvent OnBehaviorEnd;
    }

    public abstract class AttackBehavior : Behavior
    {
        private AttackBehavior next;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            OnBehaviorEnd += IncrementBehavior;
        }

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

        protected static void IncrementBehavior(Behavior behavior)
        {
            AttackBehavior attackBehavior = (AttackBehavior)behavior;
            Enemy enemy = attackBehavior.enemy;
            // Only increment behavior if the finished behavior is the current one
            if (attackBehavior != enemy.attackBehavior)
                return;

            enemy.attackBehavior = attackBehavior.next;
            attackBehavior.next = null;
            if (enemy.loopBehaviors)
            {
                enemy.AddAttackBehavior(attackBehavior);
                if (enemy.attackBehavior != attackBehavior)
                    enemy.attackBehavior.Start(enemy);

            }
            else if (enemy.attackBehavior != null)
                enemy.attackBehavior.Start(enemy);
        }
    }

    public abstract class MovementBehavior : Behavior
    {
        private MovementBehavior next;

        public override void Start(Enemy enemy)
        {
            base.Start(enemy);
            OnBehaviorEnd += IncrementBehavior;
        }

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

        protected static void IncrementBehavior(Behavior behavior)
        {
            MovementBehavior movementBehavior = (MovementBehavior)behavior;
            Enemy enemy = movementBehavior.enemy;
            // Only increment behavior if the finished behavior is the current one
            if (movementBehavior != enemy.movementBehavior)
                return;

            enemy.movementBehavior = movementBehavior.next;
            movementBehavior.next = null;
            if (enemy.loopBehaviors)
            {
                enemy.AddMovementBehavior(movementBehavior);
                if (enemy.movementBehavior != movementBehavior)
                    enemy.movementBehavior.Start(enemy);

            }
            else if (enemy.movementBehavior != null)
                enemy.movementBehavior.Start(enemy);
        }
    }
}
