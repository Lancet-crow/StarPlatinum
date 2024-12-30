using UnityEngine;

public class LivingHouse : BuildingComponent
{
    public override void OnBuild()
    {
        // Устанавливается случайный скин для постройки
        SetSpriteForTile(Random.Range(0, possibleSpritesList.Count - 1));
        //UIManager.Instance.UpdateHousesAmountText();
    }
}
