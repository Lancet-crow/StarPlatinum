public class BuildingMode : IPlayerState
{
    public void Enter()
    {
        GameManager.Instance.buildingPanel.SetActive(true);
        GameManager.Instance.currentBuildingTypeChoice = null;
    }

    public void Exit()
    {
        GameManager.Instance.buildingPanel.SetActive(false);
        GameManager.Instance.currentBuildingTypeChoice = null;
    }

    public void HandleInput()
    {
        
    }

    public void Update()
    {
        
    }
}
