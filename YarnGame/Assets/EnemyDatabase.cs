using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyDatabase : MonoBehaviour
{
    [Serializable]
    public class EnemyInfo
    {
        public string enemyType;
        public Sprite picture;
    }

    public List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();

}
