using System.Collections;
using System.Collections.Generic;
using System.IO;
using mage.constants;
using UnityEngine;

public class XMPPCmdargProcessor : MonoBehaviour {
    public bool server = false;
    public string address = "";
    public int port = -1;
    public string username = "";
    public string gameType = "";
    public XMPPGameOptions gameOptions;
    public string deckFile;
    [TextArea]
    public string deckString;
    public List<string> players = new List<string>();
    public bool clientAutoReconnect;

    // Start is called before the first frame update
    void Start() {
        string[] args = System.Environment.GetCommandLineArgs();
        string m = "CanadianHighlanderMulligan";
        int fm = 0;
        gameOptions = new XMPPGameOptions();
        gameOptions.mageGameOptions = new mage.game.GameOptions();
        for (int i = 0; i < args.Length; i++) {
            switch (args[i]) {
                case "--server":
                    server = true;
                    break;
                case "--address":
                    address = args[i + 1];
                    break;
                case "--port":
                    port = int.Parse(args[i + 1]);
                    break;
                case "--username":
                    username = args[i + 1];
                    break;
                case "--gameType":
                    gameType = args[i + 1];
                    break;
                case "--mulliganType":
                    m = args[i + 1];
                    break;
                case "--freeMulligan":
                    fm = int.Parse(args[i + 1]);
                    break;
                case "--startLife":
                    gameOptions.startLife = int.Parse(args[i + 1]);
                    break;
                case "--startingSize":
                    gameOptions.startingSize = int.Parse(args[i + 1]);
                    break;
                case "--multiplayerAttackOption":
                    switch (args[i + 1]) {
                        case "multiple":
                            gameOptions.multiplayerAttackOption = MultiplayerAttackOption.MULTIPLE;
                            break;
                        case "left":
                            gameOptions.multiplayerAttackOption = MultiplayerAttackOption.LEFT;
                            break;
                        case "right":
                            gameOptions.multiplayerAttackOption = MultiplayerAttackOption.RIGHT;
                            break;
                    }
                    break;
                case "--deckFile":
                    deckFile = args[i + 1];
                    ClientAttemptLoadDeck();
                    break;
                case "--players":
                    int num = int.Parse(args[i + 1]);
                    for (int j = 2; j <= num + 1; j++) {
                        players.Add(args[i + j]);
                    }
                    break;
                case "--clientAutoReconnect":
                    clientAutoReconnect = bool.Parse(args[i + 1]);
                    break;
            }
            gameOptions.mulligan = XMPPMulliganType.mulliganTypes[m].createMulligan(fm);
        }
        Debug.Log(JsonUtility.ToJson(this));
        if (server) {
            if (address == "" || port == -1 || gameType == "") {
                Debug.LogError("Invalid CMD Arg. Quit.");
                Application.Quit();
            }
        } else {
            if (address == "" || port == -1 || username == "") {
                Debug.LogError("Invalid CMD Arg. Quit.");
                Application.Quit();
            }
        }
    }

    public void ClientAttemptLoadDeck() {
        var f = File.OpenText(deckFile);
        deckString = f.ReadToEnd();
    }

    // Update is called once per frame
    void Update() {
        var network = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<XMPPNetworkManager>();
        // if (!server && !network.isNetworkActive && clientAutoReconnect) {
        //     Debug.Log("Client disconnected, reconnecting...");
        //     network.StartClient();
        // }
        if (!server && !network.isNetworkActive) {
            Application.Quit(); // FIXME
        }
    }
}
