using System.Collections.Generic;

public class GameLogic : IGameModel, DataSource
{
    private Player[] players;
    private Data data;

    private Dictionary<int, Buff> buffs;
    private Dictionary<int, Stat> stats;

    private BuffRandom buffRandom;

    public Events events { get; private set; }

    public GameLogic(Data data)
    {
        store(data);

        buffRandom = new BuffRandom(data, this);
        events = new Events();

        setupPlayers();
    }

    private void store(Data data)
    {
        this.data = data;

        buffs = new Dictionary<int, Buff>();
        foreach (var buff in data.buffs) buffs[buff.id] = buff;

        stats = new Dictionary<int, Stat>();
        foreach (var stat in data.stats) stats[stat.id] = stat;
    }

    private void setupPlayers()
    {
        var count = data.settings.playersCount;

        players = new Player[count];

        for (var i = 0; i < count; i++)
            players[i] = new Player();
    }

    public void Restart(bool useBuffs)
    {
        for(var i = 0; i < players.Length; i++)
        {
            var player = players[i].Reset();

            foreach (var stat in data.stats)
                player.AddStat(stat);

            if (useBuffs)
                buffRandom.RollBuffs(player);

            events.restart(i, player, this);
        }
    }

    public void Attack(int from)
    {
        var sender = players[from];
        if (sender.Alive == false) return;

        var to = (from + 1) % players.Length;
        var receiver = players[to];

        if (receiver.Alive)
        {
            events.attack(from);

            var dmg = receiver.TakeDamage(sender.Damage);
            events.changeHp(to, receiver, -dmg);
            
            var heal = sender.SendDamage(dmg);
            events.changeHp(from, sender, heal);
        }
    }

    public Stat GetStat(int id)
    {
        return stats[id];
    }

    public Buff GetBuff(int id)
    {
        return buffs[id];
    }
}

public interface IGameModel
{
    Events events { get; }

    void Restart(bool useBuffs);

    void Attack(int indexFrom);
}

public interface DataSource
{
    Stat GetStat(int id);
    Buff GetBuff(int id);
}
