using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcedTileComponent : TileComponent
{
    public ResourceManager.Resource resource;
    public int amount;
    public bool isInfinite;
}
