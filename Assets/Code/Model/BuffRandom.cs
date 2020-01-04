using System;
using System.Collections.Generic;

public class BuffRandom
{
    private Random rnd = new Random();

    private DataSource source;

    private bool allowDuplicates;
    private int min, max;

    private List<int> ids = new List<int>(), unused = new List<int>();

    public BuffRandom(Data gameData, DataSource source)
    {
        this.source = source;

        foreach (var obj in gameData.buffs)
            ids.Add(obj.id);

        var settings = gameData.settings;

        allowDuplicates = settings.allowDuplicateBuffs;

        min = settings.buffCountMin;
        max = settings.buffCountMax;

        checkBorders();
    }

    private void checkBorders()
    {
        if (allowDuplicates == false)
        {
            var possible = ids.Count;

            if (min > possible)
            {
                warn("Fix buff_min {0} => {1}", min, possible);
                min = possible;
            }

            if (max > possible)
            {
                warn("Fix buff_max {0} => {1}", max, possible);
                max = possible;
            }
        }
    }

    private void warn(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    public void RollBuffs(Player player)
    {
        unused.Clear();
        unused.AddRange(ids);

        var count = getCount();

        for (var i = 0; i < count; i++)
        {
            var buff = getBuff();
            player.AddBuff(buff);

            if (allowDuplicates == false)
                unused.Remove(buff.id);
        }
    }

    private int getCount()
    {
        return rnd.Next(min, max);
    }

    private Buff getBuff()
    {
        var index = rnd.Next(0, unused.Count - 1);
        var id = unused[index];

        return source.GetBuff(id);
    }
}
