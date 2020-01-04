using UnityEngine;

public class GameController : MonoBehaviour
{
    public TextAsset config;

    public GameObject viewRoot;

    private IPlayerView[] players;
    private IMainView mainMenu;

    private IGameModel model;

    private void Awake()
    {
        var data = deserializeConfig();

        model = new GameLogic(data);
        
        setupViews();
        setupEvents();
        setupCamera(data);

        model.Restart(true);
    }

    private void setupViews()
    {
        players = viewRoot.GetComponentsInChildren<IPlayerView>();
        mainMenu = viewRoot.GetComponentInChildren<IMainView>();

        for (var i = 0; i < players.Length; i++)
        {
            var index = i;
            players[i].setAttack(() => model.Attack(index));
        }

        mainMenu.setRestart(useBuffs => model.Restart(useBuffs));
    }

    private void setupEvents()
    {
        var events = model.events;

        events.attack += onAttack;
        events.changeHp += onHealth;
        events.restart += onRestart;
    }

    private void setupCamera(Data data)
    {
        GetComponent<ICameraController>().SetConfig(data.cameraSettings);
    }

    private Data deserializeConfig()
    {
        return JsonUtility.FromJson<Data>(config.text);
    }

    private void onAttack(int index)
    {
        players[index].Attack();
    }

    private void onHealth(int index, Health health, float delta)
    {
        players[index].changeHp(health, delta);
    }

    private void onRestart(int index, Player player, DataSource source)
    {
        players[index].Restart(player, source);
    }
}