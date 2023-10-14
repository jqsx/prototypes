using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Game/NewRoomGroup")]
public class RoomGroups : ScriptableObject
{
    public List<Room> Rooms = new List<Room>();
}
