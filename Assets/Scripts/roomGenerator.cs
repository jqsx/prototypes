using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class roomGenerator : MonoBehaviour
{
    public List<Room> Rooms = new List<Room>();
    public int RoomLimit = 10;
    public int RoomCount = 0;
    [HideInInspector]
    public static Vector2 seed = new Vector2();
    public static roomGenerator GEN;
    public GameObject _doorPrefab;
    public int roomCounter = 0;

    int roomStartIndex = 0;

    static bool isClosing = false;
    public GameObject PlayerPrefab;

    public static MiniMap map;

    void Start()
    {
        map = GetComponent<MiniMap>();
        isClosing = false;
        GEN = this;
        //List<Room> _sorting = new List<Room>();
        //foreach(Room r in Rooms)
        //{
        //    if (_sorting.Count == 0)
        //    {
        //        _sorting.Add(r);
        //    }
        //    else
        //    {
        //        for(int i = 0; i < _sorting.Count; i++)
        //        {
        //            if (_sorting[i].Properites.Scale.magnitude < r.Properites.Scale.magnitude)
        //            {
        //                _sorting.Insert(i, r);
        //                break;
        //            }
        //        }
        //    }
        //}
        //Rooms = _sorting;
        seed = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
        generateRoom(null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public static void closeRooms()
    {
        if (isClosing) return;
        isClosing = true;
        Room[] getallrooms = (Room[])FindObjectsOfType(typeof(Room));
        foreach (Room room in getallrooms)
        {
            for (int i = 0; i < room.Properites.entryPoints.Length; i++)
            {
                Vector2 door = room.Properites.entryPoints[i];
                Vector2 _ld;
                float deg = Mathf.Atan2(door.x, door.y);
                if (Mathf.Abs(Mathf.Cos(deg)) > Mathf.Abs(Mathf.Sin(deg)))
                {
                    _ld = new Vector2(0, Mathf.Sign(Mathf.Cos(deg)));
                }
                else
                {
                    _ld = new Vector2(Mathf.Sign(Mathf.Sin(deg)), 0f);
                }
                //Collider2D col = Physics2D.OverlapBox((Vector2)room.gameObject.transform.position + door + _ld.normalized, new Vector2(1, 1), 0);
                RaycastHit2D[] r = Physics2D.RaycastAll((Vector2)room.transform.position + door, _ld.normalized, 1f);
                bool hasroom = false;

                foreach (RaycastHit2D hit in r)
                {
                    if (hit.collider.isTrigger)
                    {
                        hasroom = true;
                        break;
                    }
                }

                if (!hasroom)
                {
                    GameObject g = Instantiate(GEN._doorPrefab, (Vector2)room.gameObject.transform.position + door, Quaternion.identity);
                    Vector3 s = new Vector3(Mathf.Abs(_ld.y), Mathf.Abs(_ld.x), 1);
                    g.transform.localScale = new Vector3(1f, 1f) + s.normalized * 1.5f;
                    g.transform.parent = room.transform;
                    GEN.roomCounter++;
                }
            }
            room.hideChildren();
        }
        spawnPlayer();
        map.GenerateMap();
    }

    static void spawnPlayer()
    {
        Instantiate(GEN.PlayerPrefab, new Vector3(GEN.transform.position.x, GEN.transform.position.y, 0), Quaternion.identity);
    }

    public void generateRoom(Room last)
    {
        if (last)
        {
            foreach(Vector2 lastDoor in last.Properites.entryPoints)
            {
                bool found = false;
                for(int i = 0; i < Rooms.Count; i++)
                {
                    Room a = Rooms[(roomStartIndex + i) % Rooms.Count];

                    if (RoomCount < a.Properites.generateAfter) continue;
                    if (RoomCount >= a.Properites.stopGeneratingAfter) continue;

                    if (found)
                    {
                        break;
                    }
                    foreach (Vector2 door in a.Properites.entryPoints)
                    {
                        //Vector2 _ld = new Vector2(Mathf.Round(lastDoor.x), Mathf.Round(Mathf.Round(lastDoor.y)));
                        float dot = Vector2.Dot(lastDoor.normalized, door.normalized);
                        if (dot > -0.3f) continue;
                        // center is 0, 0 localy
                        // add it to entrypositon to get room pos

                        Collider2D col = Physics2D.OverlapBox((Vector2)last.gameObject.transform.position + lastDoor - door, a.Properites.Scale, 0);
                        if (col) continue;
                        Instantiate(a.gameObject, (Vector2)last.gameObject.transform.position + lastDoor - door, Quaternion.identity);
                        found = true;

                        roomStartIndex += i + (int)Mathf.Round(Mathf.PerlinNoise(seed.x + last.gameObject.transform.position.x, seed.y + last.gameObject.transform.position.y) * 3f);
                        break;
                    }
                }

            }
        }
        else
        {
            foreach (Room a in Rooms)
            {
                Collider2D col = Physics2D.OverlapBox((Vector2)transform.position, a.Properites.Scale, 0);
                if (col) continue;
                Instantiate(a.Properites.Prefab, (Vector2)transform.position, Quaternion.identity);
                break;
            }
        }
    }
}
