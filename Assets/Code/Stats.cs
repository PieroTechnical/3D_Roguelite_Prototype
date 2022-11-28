using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy,
        Misc,
    }

    bool isBlocking;
    public float blockITime = 0;
    public float safeTime = 0;
    public int health = 3;

    public Team team;

    void Update()
    {
        if (team == Team.Player)
        {
            BlockTimeClock();
        }

        SafeTimeClock();
    }

    private void SafeTimeClock()
    {
        if (safeTime > 0)
        {
            safeTime -= Time.deltaTime;
        }

        else
        {
            safeTime = 0;
        }
    }

    private void BlockTimeClock()
    {
        if (blockITime > 0)
        {
            blockITime -= Time.deltaTime;
        }

        else
        {
            blockITime = 0;
        }
    }

    public void TryHit(Rigidbody attacker, float damage = 10, float knockback = 250, float knockup = 120)
    {


        //Vector3 direction = Vector3.ClampMagnitude(transform.position - other.transform.position, 1);

        //if (Time.realtimeSinceStartup > stunTime) TryHit(direction);

        if (blockITime > 0)
        {
            Debug.Log("Hit deflected by " + transform.name);
            attacker.SendMessage("TryHitMessage");
        }

        else
        {
            Debug.Log("Establishing hit on " + transform.name);

            gameObject.SendMessage("TryHitMessage");

            health--;

            safeTime = 0.1f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (safeTime > 0) return;
 
        AttackHitBox hb = other.GetComponent<AttackHitBox>();
        if (hb != null && hb.team != team)
        {
            Vector3 direction = Vector3.ClampMagnitude(transform.position - other.attachedRigidbody.position, 1);

            TryHit(hb.GetComponentInParent<Rigidbody>());    
        }
    }
}
