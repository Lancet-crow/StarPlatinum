using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMinerTile : BuildingComponent
{
    public ResourceManager.Resource resourceToMine;
    public float miningSpeed;
    public float timeToMineResource; 
    private ResourcedTileComponent ResourcedTile => GetComponent<ResourcedTileComponent>();

    private void Awake()
    {
        StartCoroutine(MineAResource());
    }

    private IEnumerator MineAResource()
    {
        var estimatedTime = 0f;
        while (estimatedTime < miningSpeed)
        {
            estimatedTime += Time.deltaTime;
            yield return null;
        }
        if (!ResourcedTile.isInfinite)
        {
            ResourcedTile.amount -= 5;
        }
        ResourceManager.Instance.AddAmount(resourceToMine, 5);
        if (ResourcedTile.amount > 0 || ResourcedTile.isInfinite)
        {
            StartCoroutine(MineAResource());
        }
        else
        {
            ResourcedTile.DestroyOnTile();
        }
    }
}
