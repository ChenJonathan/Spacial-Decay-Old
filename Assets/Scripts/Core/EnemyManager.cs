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
                clone.Field = field;
                return clone;
            }
        }
        return null;
    }
}
