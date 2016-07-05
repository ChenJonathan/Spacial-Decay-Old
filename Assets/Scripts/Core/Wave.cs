using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DanmakU;
using System;

public abstract class Wave : MonoBehaviour, IPausable
{
    protected Player player;
    public Player Player
    {
        get { return player; }
    }
    protected List<Enemy> enemies;
    public List<Enemy> Enemies
    {
        get { return enemies; }
    }

    protected int difficulty;

    public bool Paused
    {
        get;
        set;
    }

    public DanmakuField Field
    {
        get;
        set;
    }

    public void Awake()
    {
        GameController game = ((GameController)GameController.Instance);
        player = game.Player;
        enemies = new List<Enemy>();
        difficulty = game.Difficulty;
        Field = game.Field;
    }

    public void Update()
    {
        if(!Paused)
            NormalUpdate();
    }

    public virtual void NormalUpdate() { }
	
	public void End()
    {
        ClearEnemies();
        Danmaku.DeactivateAllImmediate();
        ((GameController)GameController.Instance).EndWave();
	}

    public Enemy SpawnEnemy(SpawnData enemy, Vector2 location)
    {
        Enemy temp = (Enemy)Instantiate(enemy.Prefab, location, Quaternion.identity);
        temp.transform.parent = Field.transform;
        temp.Field = Field;
        temp.Wave = this;
        enemies.Add(temp);

        temp.MaxHealth = temp.Health = enemy.Health;
        temp.AddAttackBehavior((Enemy.AttackBehavior)enemy.AttackBehavior.Clone());
        temp.AddMovementBehavior((Enemy.MovementBehavior)enemy.MovementBehavior.Clone());
        temp.FacePlayer = enemy.FacePlayer;
        temp.LoopBehaviors = enemy.LoopBehaviors;

        return temp;
    }

    public List<Enemy> SpawnEnemyChain(SpawnData enemy, float timeOffset, Vector2 location, Vector2 locationOffset, int count)
    {
        List<Vector2> locations = new List<Vector2>();
        for(int i = 0; i < count; i++)
        {
            locations.Add(location);
            location += locationOffset;
        }
        return SpawnEnemyChain(enemy, timeOffset, locations);
    }

    public List<Enemy> SpawnEnemyChain(SpawnData enemy, float timeOffset, List<Vector2> locations)
    {
        List<Enemy> enemies = new List<Enemy>();
        StartCoroutine(SpawnEnemyChain(enemies, enemy, timeOffset, locations));
        return enemies;
    }

    private IEnumerator SpawnEnemyChain(List<Enemy> enemies, SpawnData enemy, float timeOffset, List<Vector2> locations)
    {
        float timeDelay;
        foreach(Vector2 location in locations)
        {
            timeDelay = 0;
            enemies.Add(SpawnEnemy(enemy, location));
            while(timeDelay < timeOffset)
            {
                if (!Paused)
                    timeDelay += Time.deltaTime;
                yield return null;
            }
        }
        yield break;
    }

    public virtual void OnEnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void ClearEnemies()
    {
        while(enemies.Count > 0)
        {
            // Be careful of weird feedback - Consider adding an "active" wave bool
            enemies[0].Die();
        }
        enemies.Clear();
    }
}
