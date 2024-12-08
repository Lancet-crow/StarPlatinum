using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingHouse : BuildingComponent
{
    public override void OnBuild()
    {
        SetSpriteForTile(Random.Range(0, possibleSpritesList.Count - 1));
        //UIManager.Instance.UpdateHousesAmountText();
    }
}
