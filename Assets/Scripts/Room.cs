using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private static List<Room> lastVisible = new List<Room>();
    public RoomProperites Properites;
    public roomGenerator GEN;
    void Start()
    {
        if (!GEN)
        {
            GEN = roomGenerator.GEN;
        }

        if (GEN.RoomCount < GEN.RoomLimit)
        {
            GEN.RoomCount++;
            StartCoroutine(neighbors());
        }
        else
        {
            StartCoroutine(closerooms());
        }
    }

    IEnumerator neighbors()
    {
        yield return new WaitForSeconds(0.01f);
        GEN.generateRoom(this);
    }

    IEnumerator closerooms ()
    {
        yield return new WaitForSeconds(0.1f);
        roomGenerator.closeRooms();
    }

    private void OnDrawGizmos()
    {
        foreach(Vector2 h in Properites.entryPoints)
        {
            Gizmos.DrawWireSphere(((Vector2)transform.position) + h, 0.2f);
            Gizmos.DrawRay(((Vector2)transform.position) + h, -h);
            Vector2 _ld = new Vector2(Mathf.Round(h.x), Mathf.Round(Mathf.Round(h.y)));
            Gizmos.DrawRay(transform.position + (Vector3)h, (Vector3)_ld.normalized);
        }
        Gizmos.DrawWireCube(((Vector2)transform.position), Properites.Scale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(Room room in lastVisible) {
            room.hideChildren();
        }
        lastVisible.Clear();

        foreach(Vector2 room in Properites.entryPoints)
        {
            Vector2 _ld = new Vector2(Mathf.Round(room.x), Mathf.Round(Mathf.Round(room.y)));
            RaycastHit2D[] castedRays = Physics2D.RaycastAll(transform.position + (Vector3)room, (Vector3)_ld.normalized, 1f);
            foreach(RaycastHit2D ray in castedRays)
            {
                Room r = ray.transform.GetComponent<Room>();
                if (r)
                {
                    r.showChildren();
                    lastVisible.Add(r);
                }
            }
        }
        lastVisible.Add(this);
        showChildren();
    }

    public void hideChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Renderer renderer = transform.GetChild(i).GetComponent<Renderer>();
            if (renderer) renderer.enabled = false;
        }
    }

    public void showChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Renderer renderer = transform.GetChild(i).GetComponent<Renderer>();
            if (renderer) renderer.enabled = true;
        }
    }
}
