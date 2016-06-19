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
        get
        {
            return player;
        }
    }

    protected List<Enemy> enemies;
    public List<Enemy> Enemies
    {
        get
        {
            return enemies;
        }
    }

    public bool Paused
    {
        get;
        set;
    }

    public void Awake()
    {
        player = ((GameController)GameController.Instance).Player;
        enemies = new List<Enemy>();
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
        ((GameController)GameController.Instance).EndWave();
	}

    public Enemy SpawnEnemy(string enemy, Vector2 location)
    {
        Enemy temp = EnemyManager.Instance.Spawn(enemy, location);
        temp.Wave = this;
        enemies.Add(temp);
        return temp;
    }

    public List<Enemy> SpawnEnemyChain(string enemy, float timeOffset, Vector2 location, Vector2 locationOffset, int count)
    {
        List<Vector2> locations = new List<Vector2>();
        for(int i = 0; i < count; i++)
        {
            locations.Add(location);
            location += locationOffset;
        }
        return SpawnEnemyChain(enemy, timeOffset, locations);
    }

    public List<Enemy> SpawnEnemyChain(string enemy, float timeOffset, List<Vector2> locations)
    {
        List<Enemy> enemies = new List<Enemy>();
        StartCoroutine(SpawnEnemyChain(enemies, enemy, timeOffset, locations));
        return enemies;
    }

    private IEnumerator SpawnEnemyChain(List<Enemy> enemies, string enemy, float timeOffset, List<Vector2> locations)
    {
        foreach(Vector2 location in locations)
        {
            enemies.Add(SpawnEnemy(enemy, location));
            yield return new WaitForSeconds(timeOffset);
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
