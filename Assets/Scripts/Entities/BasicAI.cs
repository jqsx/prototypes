using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicAI : Entity
{
    private Vector2 target = new Vector2(0, 0);
    private Vector2[] castPositions = new Vector2[9];
    private Rigidbody2D rb;
    public LayerMask avoid;
    public LayerMask opaqueSolids;
    public Animator animator;

    Vector2 direction = Vector2.zero;

    public State state = State.Wandering;

    public Entity targetEnemy;

    private float attackDelay = 0f;
    private float CanAttackCheckDelay = 0f;

    private void Awake()
    {
        for(int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                castPositions[(y + 1) * 3 + x + 1] = new Vector2(x / 2f, y).normalized;
            }
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //if (PlayerController.instance) target = PlayerController.instance.transform.position;

        if (faction == Faction.Aggressive && !targetEnemy)
        {
            if (PlayerController.instance) targetEnemy = PlayerController.instance.GetComponent<Entity>();
        }
        if (state == State.Chasing)
        {
            Chasing();

            if (attackDelay < Time.time && CanAttackCheckDelay < Time.time)
            {
                CanAttackCheckDelay = Time.time + Random.Range(0.1f, 0.25f);
                Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, 1.5f);
                foreach (Collider2D entity in entities)
                {
                    Entity e = entity.GetComponent<Entity>();
                    if (e && e == targetEnemy)
                    {
                        Attack(transform.position - e.transform.position, false);
                        attackDelay = Time.time + 1.0f / getTotal().AttackSpeed;
                    }
                }
            }
        }
        else if (state == State.Wandering) Wandering();

        if (targetEnemy) target = targetEnemy.transform.position;

        if (optimizedDistance(transform.position, target) > 100f) state = State.Wandering;
        if (targetEnemy && state != State.Chasing && !Physics2D.Linecast(transform.position, targetEnemy.transform.position, opaqueSolids) && optimizedDistance(transform.position, targetEnemy.transform.position) < 15f)
        {
            state = State.Chasing;
        }
        else if (optimizedDistance(transform.position, target) > 40f)
        {
            state = State.Wandering;
        }
        animator.transform.rotation = Quaternion.Euler(0, 180f * (direction.x > 0 ? 0f : 1f), 0f);
    }

    private void Wandering()
    {

    }

    private void Chasing()
    {
        Vector2 _target = CastPossibleLocations(isInTheSameRoomAsTarget() ? target : getDoorPosClosestToTarget());
        direction = _target;
        movement(direction);
    }

    void playAnimation(string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName(name) && !animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            animator.Play(name);
        }
    }

    private Vector2 CastPossibleLocations(Vector2 _target)
    {
        Vector2 closest = transform.position;
        foreach(Vector2 pos in castPositions)
        {
            if (optimizedDistance(closest, _target) > optimizedDistance(_target, pos + (Vector2)transform.position))
            {
                bool isSomethingThere = false;
                Collider2D[] col = Physics2D.OverlapBoxAll(pos + (Vector2)transform.position, Vector2.one, 0, avoid);
                foreach(Collider2D collider in col)
                {
                    if (collider.transform == transform) continue;
                    isSomethingThere = true;
                }

                if (!isSomethingThere) closest = (Vector2)transform.position + pos;
            }
        }

        return closest - (Vector2)transform.position;
    }

    private Vector2 getDoorPosClosestToTarget()
    {
        Vector2 closest = transform.position;
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach(Collider2D c in col)
        {
            Room r = c.GetComponent<Room>();
            if (r)
            {
                foreach(Vector2 door in r.Properites.entryPoints)
                {
                    Vector2 worldpos = (Vector2)r.transform.position + door;
                    
                    if (optimizedDistance(closest, target) > optimizedDistance(target, worldpos))
                    {
                        closest = worldpos;
                    }
                }
            }
        }

        return closest;
    }

    private bool isInTheSameRoomAsTarget()
    {
        Room me = null;
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D c in col)
        {
            Room r = c.GetComponent<Room>();
            if (r) me = r;
        }

        col = Physics2D.OverlapCircleAll(target, 1f);
        foreach (Collider2D c in col)
        {
            Room r = c.GetComponent<Room>();
            if (r && me != null) return r == me;
        }
        return false;
    }

    private void movement(Vector2 direction)
    {
        if (direction.magnitude > 0.3f) playAnimation("WalkRight");
        else playAnimation("idle");
        rb.velocity = Vector2.Lerp(rb.velocity, direction * EntityStatistics.MoveSpeed, Time.deltaTime * 5f);
    }

    private static float optimizedDistance(Vector2 one, Vector2 two)
    {
        return Mathf.Abs(one.x - two.x) + Mathf.Abs(one.y - two.y);
    }

    private void OnDrawGizmos()
    {
        foreach (Vector2 pos in castPositions)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + pos, Vector2.one);
        }
    }

    public override void onDamage(EntityDamageEvent e)
    {
        base.onDamage(e);

        rb.AddForce(300f * (1f - EntityStatistics.KnockBackResistance - EquipmentStatistics.KnockBackResistance) * (transform.position - e.from.transform.position).normalized);
    }

    public enum State
    {
        Wandering, Chasing
    }
}