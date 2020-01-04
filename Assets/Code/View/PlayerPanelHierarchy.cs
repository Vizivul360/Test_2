using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelHierarchy : MonoBehaviour, IPlayerView
{
    public Button attackButton;
    public Animator character;

    [Space]
    public HealthBar bar;

    [Space]
    public NodePool paramNodes;

    private IStatView health;

    void Awake()
    {
        bar.Prepare(character.transform);
    }

    public void Attack()
    {
        character.SetTrigger("Attack");
    }

    public void setAttack(Action action)
    {
        attackButton.onClick.AddListener(() => action());
    }

    public void Restart(Player player, DataSource source)
    {
        paramNodes.Reset();

        foreach(var stat in player.stats)
        {
            var node = paramNodes.Get<IStatView>();
            var meta = source.GetStat(stat.Key);

            if (stat.Key == StatsId.LIFE_ID)
                health = node;

            node.UpdateValue(stat.Value);
            node.SetData(meta);
        }

        foreach(var buff in player.buffs)
        {
            var node = paramNodes.Get<IBuffView>();
            var meta = source.GetBuff(buff);

            node.SetData(meta);
        }

        setHealth(player);
    }

    public void changeHp(Health value, float delta)
    {
        setHealth(value, delta);
    }

    private void setHealth(Health value, float delta = 0)
    {
        bar.view.OnChange(value, delta);
        health.UpdateValue(value.Current);

        character.SetBool("Alive", value.Alive);
    }

    [Serializable]
    public class HealthBar
    {
        public float offsetY;
        public GameObject obj;

        public IHealthBar view { get; private set; }

        public void Prepare(Transform character)
        {
            view = obj.GetComponent<IHealthBar>();
            view.SetAnchor(character.position + Vector3.up * offsetY);
        }
    }

    [Serializable]
    public class NodePool
    {
        public Transform root;
        public GameObject template;

        private int index = 0;

        public void Reset()
        {
            for (var i = 0; i < root.childCount; i++)
                root.GetChild(i).gameObject.SetActive(false);

            index = 0;
        }

        public T Get<T>()
        {
            var obj = root.childCount > index ? root.GetChild(index) : createNode();
            obj.gameObject.SetActive(true);

            index++;

            return obj.GetComponent<T>();
        }

        private Transform createNode()
        {
            var obj = Instantiate(template).transform;
            obj.SetParent(root);

            return obj;
        }
    }
}

public interface IPlayerView
{
    void Attack();

    void setAttack(Action action);

    void changeHp(Health value, float delta);

    void Restart(Player player, DataSource source);
}
