using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public static List<roomData> data = new List<roomData>();
    private Vector2 far;
    private Vector2 close;

    public Texture2D mapTexture;

    private void Awake()
    {
        data.Clear();
    }

    public class roomData
    {
        public RoomProperites properites;
        public Vector2 position;

        public roomData(RoomProperites properites, Vector2 position)
        {
            this.properites = properites;
            this.position = position;

            data.Add(this);
        }
    }

    public void add(Room room)
    {
        new roomData(room.Properites, room.transform.position);
    }

    public void GenerateMap()
    {
        far = Vector2.one * -Mathf.Infinity;
        close = Vector2.one * Mathf.Infinity;

        foreach (roomData room in data)
        {
            Vector2 _a = room.position - room.properites.Scale;
            Vector2 _b = room.position + room.properites.Scale;
            if (close.x > _a.x) close.x = _a.x;
            else if (far.x < _b.x) far.x = _b.x;
            if (close.y > _a.y) close.y = _a.y;
            else if (far.y < _b.y) far.y = _b.y;
        }

        Vector2 size = far - close;
        mapTexture = new Texture2D((int)size.x, (int)size.y);
        mapTexture.filterMode = FilterMode.Point;
        for (int y = 0; y < mapTexture.height; y++)
        {
            for (int x = 0; x < mapTexture.width; x++)
            {
                mapTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }


        foreach (roomData room in data)
        {
            Vector2 pos = far - room.position;

            for(int y = (int)-room.properites.Scale.y / 2; y < (int)room.properites.Scale.y / 2; y++)
            {
                for (int x = (int)-room.properites.Scale.x / 2; x < (int)room.properites.Scale.x / 2; x++)
                {

                    mapTexture.SetPixel(mapTexture.width - (int)pos.x + x, mapTexture.height - (int)pos.y + y, room.properites.roomColor);
                }
            }
        }

        mapTexture.Apply();

        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(mapTexture, new Rect(Vector2.zero, new Vector2(mapTexture.width, mapTexture.height)), Vector2.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((far + close) / 2f, far - close);
    }
}
