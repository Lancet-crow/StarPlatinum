using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class BuildingComponent : MonoBehaviour
{
    [HideInInspector]
    public int posX;
    [HideInInspector]
    public int posY;
    public int workersAmount;
    public List<GameObject> canBePlacedOnTiles;
    public SerializedDictionary<ResourceManager.Resource, int> resourcesToBuildFrom;
    [SerializeField] protected List<Sprite> possibleSpritesList;
    protected SpriteRenderer TileSpriteRenderer => GetComponentInChildren<SpriteRenderer>();


    public void SetSpriteForTile(int spriteId)
    {
        if (0 <= spriteId && spriteId < possibleSpritesList.Count)
        {
            TileSpriteRenderer.sprite = possibleSpritesList[spriteId];
            generate_field1.Instance.hexSkinsGrid[posX, posY] = spriteId;
        }
    }

    public virtual void OnBuild() { }
}
