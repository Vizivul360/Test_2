using System.Collections.Generic;

//Raw data
public partial class Player : Health
{
    public Dictionary<int, float> stats = new Dictionary<int, float>();

    public List<int> buffs = new List<int>();

    public float Max { get; private set; }
    public float Current { get { return hp; } }

    public bool Alive { get { return hp > 0; } }
}

//Build
public partial class Player
{
    public Player()
    {
        Reset();
    }

    public void AddStat(Stat stat)
    {
        addStat(stat.id, stat.value);
    }

    public void AddBuff(Buff buff)
    {
        buffs.Add(buff.id);

        foreach (var stat in buff.stats)
            addStat(stat.statId, stat.value);
    }

    private void addStat(int id, float value)
    {
        stats[id] += value;

        if (id == StatsId.LIFE_ID)
            Max += value;
    }

    public Player Reset()
    {
        Max = 0;

        for (var i = 0; i < StatsId.COUNT; i++)
            stats[i] = 0;
        
        buffs.Clear();

        return this;
    }
}

//Logic
public partial class Player
{
    public float Damage
    {
        get { return stats[StatsId.DAMAGE_ID]; }
    }

    private float hp
    {
        get { return stats[StatsId.LIFE_ID]; }
        set { stats[StatsId.LIFE_ID] = value; }
    }

    /// <summary>
    /// Returns fact damage
    /// </summary>
    public float TakeDamage(float value)
    {
        var factValue = value * (1 - stats[StatsId.ARMOR_ID] / 100);
        hp -= factValue;

        if (hp < 0)
        {
            factValue += hp;
            hp = 0;
        }

        return factValue;
    }

    /// <summary>
    /// Returns steal hp
    /// </summary>
    public float SendDamage(float value)
    {
        var stealValue = value * stats[StatsId.LIFE_STEAL_ID] / 100;
        hp += stealValue;

        if (hp > Max)
        {
            stealValue -= (hp - Max);
            hp = Max;
        }

        return stealValue;
    }
}

public interface Health
{
    bool Alive { get; }

    float Max { get; }
    float Current { get; }
}