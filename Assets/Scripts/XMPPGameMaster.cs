using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using mage.cards.decks;
using mage.cards.decks.importer;
using mage.game.events;
using Mirror;
using UnityEngine;

public class XMPPGameMaster : NetworkBehaviour {

    mage.game.Game mageGame;

    private class XMPPEventListener : mage.game.events.Listener {
        private XMPPGameMaster gm;

        public XMPPEventListener(XMPPGameMaster _gm) {
            gm = _gm;
        }

        public void @event(ExternalEvent ee) {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                gm.OnMageGameStateChange(ee);
            });
        }
    }

    [System.Serializable]
    public class PlayerSyncDictionary : SyncDictionary<string, GameObject> { }
    [SerializeField]
    public readonly PlayerSyncDictionary players = new PlayerSyncDictionary();

    public GameObject playerPrefab;

    [Server]
    void InitGame(List<string> playerUsernames, XMPPGameType gameType, XMPPGameOptions gameOptions) {
        mageGame = gameType.createGame(gameOptions);
        mageGame.addTableEventListener(new XMPPEventListener(this));
        // mageGame.addPlayerQueryEventListener(new XMPPEventListener(this));
        var networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<XMPPNetworkManager>();
        foreach (var s in playerUsernames) {
            var p = Instantiate(playerPrefab);
            // NetworkServer.Spawn(p);
            players.Add(s, p);
            playerPresent.Add(s, false);
        }
    }

    [Server]
    void PrepareGame() {
        Debug.Log("PrepareGame");
        foreach (var username in players.Keys) {
            string deckGuid = System.Guid.NewGuid().ToString();
            var filename = string.Concat(Application.temporaryCachePath, "/", deckGuid);
            var sr = File.CreateText(filename);
            var player = players[username].GetComponent<XMPPPlayer>();
            sr.WriteLine(player.deckString);
            sr.Close();
            DeckCardLists l = DeckImporter.importDeckFromFile(filename, true);
            Deck d = Deck.load(l, false, false);
            Debug.Log(player.magePlayer.getId().ToString());
            mageGame.loadCards(d.getCards(), player.magePlayer.getId());
            mageGame.loadCards(d.getSideboard(), player.magePlayer.getId());
            mageGame.addPlayer(player.magePlayer, d);
        }
    }

    [Server]
    void OnMageGameStateChange(ExternalEvent ee) {
        Debug.Log("OnMageGameStateChange");
    }

    [Server]
    void StartGame() {
        Debug.Log("StartGame");
        var id = players.ElementAt(Random.Range(0, players.Count)).Value.GetComponent<XMPPPlayer>().magePlayer.getId();
        new Thread(() => {
            try {
                mageGame.start(id);
            } catch (System.NullReferenceException e) {
                var frames = (new System.Diagnostics.StackTrace(e)).GetFrames();
            }
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                Debug.Log("EndGame"); 
            });
        }).Start();;
    }

    // Start is called before the first frame update
    void Start() {
        if (isServer) {
            var processor = GameObject.FindGameObjectWithTag("CMD").GetComponent<XMPPCmdargProcessor>();
            InitGame(processor.players, XMPPGameType.gameTypes[processor.gameType], processor.gameOptions);
        } else {
            CmdRegisterPlayerUsername(GameObject.FindGameObjectWithTag("CMD").GetComponent<XMPPCmdargProcessor>().username);
        }
    }

    [SyncVar]
    public bool playing = false;

    public Dictionary<string, NetworkConnectionToClient> usernameToConnection = new Dictionary<string, NetworkConnectionToClient>();

    [System.Serializable]
    public class PlayerPresentSyncDictionary : SyncDictionary<string, bool> { }
    [SerializeField]
    public PlayerPresentSyncDictionary playerPresent = new PlayerPresentSyncDictionary();

    [Command(ignoreAuthority = true)]
    public void CmdRegisterPlayerUsername(string username, NetworkConnectionToClient sender = null) {
        if (!players.ContainsKey(username) || usernameToConnection.ContainsKey(username) || usernameToConnection.ContainsValue(sender)) {
            sender.Disconnect();
            return;
        }
        GameObject player = players[username];
        NetworkServer.AddPlayerForConnection(sender, player);
        player.GetComponent<NetworkIdentity>().AssignClientAuthority(sender);
        usernameToConnection.Add(username, sender);
        playerPresent[username] = true;
        // var networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<XMPPNetworkManager>();
        // NetworkServer.AddPlayerForConnection(sender, player);
    }

    [Server]
    public void UnregisterPlayerUsername(string username) {
        NetworkServer.UnSpawn(players[username]);
        playerPresent[username] = false;
    }

    private bool prepared = false;

    // Update is called once per frame
    void Update() {
        foreach (var go in GameObject.FindGameObjectsWithTag("Empty")) {
            NetworkServer.Destroy(go);
        }
        if (isServer) {
            if (!playing) {
                bool allReady = true;
                foreach (var key in players.Keys) {
                    if (!players[key].GetComponent<XMPPPlayer>().ready) {
                        allReady = false;
                    }
                }
                if (allReady && !prepared) {
                    PrepareGame();
                    prepared = true;
                }
                if (prepared) {
                    StartGame();
                    playing = true;
                }
            }
        }
    }
}
