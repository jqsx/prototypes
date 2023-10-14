using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : Item
{
    public Vector2 handleOffset;

    public void setHandleOffset(Vector2 handleOffset)
    {
        this.handleOffset = handleOffset;
    }
}
