using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DanmakU;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]
    private DanmakuField field;
    public DanmakuField Field
    {
        get
        {
            return field;
        }
    }

    // TODO Limit accessibility
    public List<Enemy> Enemies;

    [SerializeField]
    private List<EnemyData> enemyList;
    private Dictionary<string, Enemy> enemyMap;

    [System.Serializable]
    public class EnemyData
    {
        public string name;
        public Enemy prefab;
    }

	public override void Awake()
    {
        base.Awake();

        Enemies = new List<Enemy>();
        
        enemyMap = new Dictionary<string, Enemy>();
        foreach(EnemyData enemy in enemyList)
        {
            enemyMap.Add(enemy.name, enemy.prefab);
        }
    }

    public Enemy Spawn(string enemy, Vector2 location)
    {
        if(enemyMap[enemy] != null)
        {
            Enemy clone = (Enemy)Instantiate(enemyMap[enemy], location, Quaternion.identity);
            if(clone != null)
            {
                clone.transform.parent = field.transform;
                return clone;
            }
        }
        return null;
    }

    public void Clear()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.Die();
        }
        Enemies.Clear();
    }
}
