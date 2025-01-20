
[System.Serializable]
public struct DamageData
{
    public float damage;
    public ElementType Type;

    public DamageData(ElementType type, float dam = 1)
    {
        Type = type;
        damage = dam;
    }
}
