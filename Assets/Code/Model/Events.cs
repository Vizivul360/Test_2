using System;

public class Events
{
    public Action<int, Player, DataSource> restart = (index, p, s) => { };

    public Action<int> attack = (index) => { };    
    public Action<int, Health, float> changeHp = (index, HP, delta) => { };
}
