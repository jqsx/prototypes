using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Animator animator;
    Rigidbody2D rb;
    float lastDirectionx = 1f;

    public static bool setLoadedPosition = false;
    public static Vector2 loadedPosition = Vector2.zero;

    public static Player player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        player = this;
        if (setLoadedPosition) { transform.position = loadedPosition; setLoadedPosition = false; }
        StartCoroutine(init());
    }

    IEnumerator init()
    {
        yield return new WaitWhile(() =>
        {
            return JQUI.InventoryController.instance != null;
        });

        if (JQUI.InventoryController.instance) setTool(JQUI.InventoryController.inventory.slots[JQUI.InventoryController.instance.selectedSlot]);
    }

    void Update()
    {
        regenCycle();
        StatisticsDisplay.instance.setTrackerValue("Health", Health, getTotal().MaxHealth);

        Vector2 moveDirection = new Vector2(Mathf.Clamp(Mathf.Round(rb.velocity.x / 3f), -1f, 1f), Input.GetAxisRaw("Vertical"));

        //lastDirectionx = moveDirection.x != 0 ? moveDirection.x : lastDirectionx;
        float differnce = PlayerController.mouseWorldPosition.x - transform.position.x;
        lastDirectionx = differnce != 0 ? differnce : lastDirectionx;
        lastDirectionx = moveDirection.x != 0 ? moveDirection.x : lastDirectionx;

        animator.transform.parent.rotation = Quaternion.Euler(0, lastDirectionx < 0 ? 180f : 0f, 0);

        if (Input.GetButtonDown("Fire1"))
        {
            if (Random.Range(0f, 1f) < Mathf.Clamp(EntityStatistics.CritChance + EquipmentStatistics.CritChance, 0f, 1f) && !isPlaying("Stab") && playAnimation("SwingRight"))
            {
                Attack(((Vector2)transform.position - PlayerController.mouseWorldPosition).normalized, true);
            }
            else if (!isPlaying("SwingRight") && playAnimation("Stab"))
            {
                Attack(((Vector2)transform.position - PlayerController.mouseWorldPosition).normalized, false);
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            playAnimation("Block");
        }

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            playAnimation("WalkRight");
        }
        else
        {
            playAnimation("idle");
        }
    }

    public bool playAnimation(string name)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName(name) && !animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            animator.Play(name);
            return true;
        }
        return false;
    }

    public bool isPlaying(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(1).IsName(name) || animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public override void onDamage(EntityDamageEvent e)
    {
        base.onDamage(e);

        rb.AddForce(500f * (1f - EntityStatistics.KnockBackResistance - EquipmentStatistics.KnockBackResistance) * (transform.position - e.from.transform.position).normalized);
        PlayerController.CameraShake(5f);
    }
}
