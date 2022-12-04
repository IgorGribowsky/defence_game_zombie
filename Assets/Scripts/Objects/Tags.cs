using UnityEngine;

enum UnityTags
{
    WithTag,
    EnemySpawnPoint,
    EnemyMovePoint
}


public class Tags : MonoBehaviour
{
    public bool Wall;
    public bool Enemy;
    public bool Player;
    public bool PlayerBullet;
    public bool EnemyTarget;
    public bool Turrel;
    public bool DestroyableObject;
    public bool HeroTarget;
    public bool Shootable;
}
