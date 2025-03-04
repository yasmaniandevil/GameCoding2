using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class EnemyD
{
    public string name;
    public int health;
    public float speed;
    public float detectionRange;
    public float attackRange;
    public float attackCooldown;
}

[SerializeField]
public class EnemyDataBase
{
    public List<EnemyD> enemiesList = new List<EnemyD>();
}
