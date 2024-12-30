using System.Collections;
using UnityEngine;
/// <summary>
/// ����������� �����, �� �������� ����������� ��� ���������, ��������� �������� �������
/// </summary>
public abstract class ResourceMinerTile : BuildingComponent
{
    /// <summary>
    /// ������, ������� ����� �������� ���������
    /// </summary>
    public ResourceManager.Resource resourceToMine;
    /// <summary>
    /// �����, �� ������� ���������� ������
    /// </summary>
    public float miningSpeed;
    /// <summary>
    /// ���������� �������, ���������� ���������� �� ���� ���
    /// </summary>
    public int resourceMinedAtOnce = 5;

    /// <summary>
    /// ������ �� ��������� ���������� �����
    /// </summary>
    private ResourcedTileComponent ResourcedTile => GetComponent<ResourcedTileComponent>();

    private void Awake()
    {
        StartCoroutine(MineAResource());
    }

    /// <summary>
    /// ��������, ����������� ������� ������ �������.
    /// <para>������� � ������� ������������ ������� (<see cref="miningSpeed"/>)</para>
    /// <para>� ����� �������� ������ � ���������� ����� (<see cref="ResourcedTile"/>)</para>
    /// <para>� ��������� ��� � �������� ������ (<see cref="ResourceManager"/>)</para>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>���� �������� �� ����� ������ ��� ����� �������� <see cref="resourceMinedAtOnce"/>, �� ���������� ������ <see cref="resourceMinedAtOnce"/></item>
    /// <item>���� �������� �� ����� ������ 0, �� ������ �������� <see cref="resourceMinedAtOnce"/>, �� ���������� ������� �������, ������� ��� ���� �� �����</item>
    /// <item>���� �������� �� ����� �� ��������, �� ��������� ���������</item>
    /// <item>���� ���� ����� ����������� ������ (<see cref="ResourcedTileComponent.isInfinite"/>), �� ������ ����� ���������� ������ � � ������ ������ (<see cref="resourceMinedAtOnce"/>)</item>
    /// </list>
    /// </remarks>
    private IEnumerator MineAResource()
    {
        var estimatedTime = 0f;
        while (estimatedTime < miningSpeed)
        {
            estimatedTime += Time.deltaTime;
            yield return null;
        }
        var resourceAmountWasMined = resourceMinedAtOnce;
        if (!ResourcedTile.isInfinite)
        {
            if (ResourcedTile.amount < resourceMinedAtOnce)
            {
                resourceAmountWasMined = ResourcedTile.amount;
            }
        }
        ResourcedTile.amount -= resourceAmountWasMined;
        ResourceManager.Instance.AddAmount(resourceToMine, resourceAmountWasMined);
        if (ResourcedTile.amount > 0 || ResourcedTile.isInfinite)
        {
            StartCoroutine(MineAResource());
        }
        else
        {
            ResourcedTile.DestroyOnTile(gameObject);
        }
    }
}
