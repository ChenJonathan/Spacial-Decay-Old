using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Wave
{
    public abstract void Start();

    public abstract void Update();
	
	public void End()
    {
        ((GameController)GameController.Instance).EndWave();
	}

    protected Enemy Spawn(string enemy, Vector2 location)
    {
        return EnemyManager.Instance.Spawn(enemy, location);
    }
}
