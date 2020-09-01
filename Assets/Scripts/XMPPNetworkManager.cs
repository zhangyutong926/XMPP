using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System.Linq;

public class XMPPNetworkManager : NetworkManager {

    public GameObject tableGo;
    public GameObject gameMasterGo;

    public override void Start() {
        base.Start();
        Shutdown();
        var cmdarg = GameObject.FindGameObjectWithTag("CMD").GetComponent<XMPPCmdargProcessor>();
        networkAddress = cmdarg.address;
        GetComponent<TelepathyTransport>().port = (ushort)cmdarg.port;
        if (cmdarg.server) {
            StartServer();
            NetworkServer.Spawn(Instantiate(tableGo));
            NetworkServer.Spawn(Instantiate(gameMasterGo));
        } else {
            StartClient();
        }
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);
        ClientScene.AddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        // TODO Mark player temporarily absent
        var gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<XMPPGameMaster>();
        gm.usernameToConnection.Where(i => i.Value == conn).Select(i => i.Key).ToList().ForEach(s => {
            gm.UnregisterPlayerUsername(s);
        });
    }
}
