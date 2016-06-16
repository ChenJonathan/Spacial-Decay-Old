using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DanmakU;

public abstract class Wave : Singleton<Wave>
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

    public override void Awake()
    {
        base.Awake();

        player = ((GameController)GameController.Instance).Player;
        enemies = new List<Enemy>();
    }
	
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

    public List<Enemy> SpawnEnemyChain(string enemy, Vector2 location, Vector2 locationOffset, float timeOffset)
    {
        // TODO
        return new List<Enemy>();
    }

    public void OnEnemyDeath(Enemy enemy)
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
