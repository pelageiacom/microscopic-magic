using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public int hp, maxHp;
    public EnemyMode mode;
    protected Patrol patrol;
    protected AIDestinationSetter destinationSetter;
    protected AIPath aiPath;
    public Transform sprite;
    public SpriteRenderer spriteRenderer;
    protected bool isFlipped = false;

    private void Start()
    {
        maxHp = hp;
        patrol = gameObject.GetComponent<Patrol>();
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        aiPath = gameObject.GetComponent<AIPath>();
        spriteRenderer = sprite.gameObject.GetComponent<SpriteRenderer>();
    }

    public enum EnemyMode { 
        idle, // idle mode
        aggressive, // player trigger
        lowHp, // seeking for cover if self-healing; seeking for cover that is most optimal (shortest vector abs of sum of distanceToCover vector and distanceToHealer vector)
        rage, // Low chance of rage is possible while lowHp
    }

    // public void receiveDamage(Weapon weapon)
    // {
    //     if (mode == EnemyMode.rage)
    //     {
    //         hp -= weapon.getDamage() / 2;
    //     }
    //     else
    //     {
    //         hp -= weapon.getDamage();
    //     }
    // }

    private void SeekForCover()
    {
        // Check Player's view cone, create "runaway radius", find furthest point in "runaway radius" from Player's view cone; OR find distanceToCover (?)
    }
    private void Heal()
    {
        if (mode == EnemyMode.rage)
        {
            float ctd = 2;
            if (ctd <= 0)
            {
                hp += 2;
                ctd = 2;
            }
            else
            {
                ctd -= Time.deltaTime;
            }
        }
    }
    private void Death() { Destroy(gameObject); }

    private void IdleBehaviour()
    {
        destinationSetter.enabled = false;
        patrol.enabled = true;
        aiPath.maxSpeed = 1;
    }

    private void LowHp()
    {
        Random r = new Random();
        int rageRandom = r.NextInt(0, 10);
        
        Debug.Log(rageRandom);

        if (rageRandom != 0)
        {
            mode = EnemyMode.rage;
            Rage();
        }
        else
        {
            SeekForCover();
        }
    }

    private void Rage()
    {
        aiPath.maxSpeed = 3;
        if (patrol)
        {
            patrol.enabled = false;
            destinationSetter.enabled = true;
        }
    }

    private void AggressiveBehaviour()
    {
        patrol.enabled = false;
        destinationSetter.enabled = true;
        aiPath.maxSpeed = 2;
    }

    private void Rotation()
    {
        spriteRenderer.flipX = isFlipped;
        float rotNeg = -(transform.rotation.z);
        Debug.Log("AI ROT: " + transform.rotation.z + "\n" + "SPRITE ROT: " + sprite.rotation.z + " FLIP: " + isFlipped);
        if (transform.rotation.z < .5 && transform.rotation.z > -.5) 
        {
            sprite.rotation = new Quaternion(0, 0, rotNeg, 0);
            isFlipped = false;
        }
        else
        {
            isFlipped = true;

            if (transform.rotation.z >= .5 && transform.rotation.z <= 1)
            {
                sprite.rotation = new Quaternion(0, 0, transform.rotation.z + (1 - transform.rotation.z), 0);
            }
            else
            {
                sprite.rotation = new Quaternion(0, 0, rotNeg + (-1 - transform.rotation.z), 0);
                
            }
        }
    }

    private void Update()
    {
        //GetComponentInChildren<SpriteRenderer>().
        // Subtract object's rotation from Sprite rotation for Sprite to

        Rotation();
        
        if (hp <= 10 && mode != EnemyMode.lowHp)
        {
            mode = EnemyMode.lowHp;
            LowHp();
        }

        if (hp < maxHp)
        {
            Heal();
        }
        
        if(hp <= 0){ Death(); }

        if ( mode == EnemyMode.aggressive)
        {
            AggressiveBehaviour();
        }

        if (mode == EnemyMode.idle)
        {
            IdleBehaviour();
        }
        
        //DEBUG
        if (mode == EnemyMode.rage) Rage();
    }
}