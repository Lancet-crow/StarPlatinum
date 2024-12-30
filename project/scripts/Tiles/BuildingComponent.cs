using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
/// <summary>
/// ����������� �����, ���������� ������������ ��� ���� ������� ��������.
/// </summary>
public abstract class BuildingComponent : MonoBehaviour
{
    [HideInInspector]
    public int posX;
    [HideInInspector]
    public int posY;
    /// <summary>
    /// ���������� ��������� �������, ������� ���������� ��� ��������� ���������
    /// </summary>
    public int workersAmount;
    /// <summary>
    /// ���� ������, �� ������� ����� ���� ����������� ���������
    /// </summary>
    public List<GameObject> canBePlacedOnTiles;
    /// <summary>
    /// ������� ��������, ����������� ��� ��������� ���������, ���������� �� �������� "������ - ���������� �������"
    /// </summary>
    public SerializedDictionary<ResourceManager.Resource, int> resourcesToBuildFrom;
    /// <summary>
    /// ���� ��������, ������� ����� ������������ �� ���������
    /// </summary>
    [SerializeField] protected List<Sprite> possibleSpritesList;
    /// <summary>
    /// ����������, �������� �������� �������(������������ ��� ������ <see cref="SetSpriteForTile"/>)
    /// </summary>
    protected SpriteRenderer TileSpriteRenderer => GetComponentInChildren<SpriteRenderer>();


    /// <summary>
    /// ������������� ��������� ������ (<paramref name="spriteId"/>) ��� ����� ��������� � ���������� ��������� � <see cref="generate_field1.hexSkinsGrid"/>.
    /// <para>������������ ����������� ������� ������ ��� ��������.</para>
    /// </summary>
    /// <param name="spriteId">ID ������� � <see cref="possibleSpritesList"/></param>
    public void SetSpriteForTile(int spriteId)
    {
        if (0 <= spriteId && spriteId < possibleSpritesList.Count)
        {
            TileSpriteRenderer.sprite = possibleSpritesList[spriteId];
            generate_field1.Instance.hexSkinsGrid[posX, posY] = spriteId;
        }
    }
    /// <summary>
    /// ����������� ����� ��� ��������� ��������� ����� ��������� �������
    /// </summary>
    public virtual void OnBuild() { }
}
