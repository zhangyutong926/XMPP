using System.Collections;
using System.Collections.Generic;
using java.util;
using mage.abilities;
using mage.abilities.costs;
using mage.abilities.costs.mana;
using mage.cards;
using mage.cards.decks;
using mage.choices;
using mage.constants;
using mage.game;
using mage.game.combat;
using mage.game.draft;
using mage.game.match;
using mage.game.tournament;
using mage.players;
using mage.target;
using Mirror;
using UnityEngine;

public class XMPPPlayer : NetworkBehaviour {

    private class XMPPMagePlayer : mage.players.PlayerImpl {
        public XMPPMagePlayer(string name) : base(name, RangeOfInfluence.ALL) {
        }

        public override void abort() {
            Debug.Log("Player - abort");
        }

        public override int announceXCost(int value1, int value2, string value3, Game value4, Ability value5, VariableCost value6) {
            Debug.Log("Player - announceXCost");
            return 0;
        }

        public override int announceXMana(int value1, int value2, int value3, string value4, Game value5, Ability value6) {
            Debug.Log("Player - announceXMana");
            return 0;
        }

        public override void assignDamage(int value1, List value2, string value3, UUID value4, Game value5) {
            Debug.Log("Player - assignDamage");
        }

        public override bool choose(Outcome value1, Target value2, UUID value3, Game value4) {
            Debug.Log("Player - choose");
            return false;
        }

        public override bool choose(Outcome value1, Cards value2, TargetCard value3, Game value4) {
            Debug.Log("Player - choose");
            return false;
        }

        public override bool choose(Outcome value1, Target value2, UUID value3, Game value4, Map value5) {
            Debug.Log("Player - choose");
            return false;
        }

        public override bool choose(Outcome value1, Choice value2, Game value3) {
            Debug.Log("Player - choose");
            return false;
        }

        public override UUID chooseAttackerOrder(List value1, Game value2) {
            Debug.Log("Player - chooseAttackerOrder");
            return null;
        }

        public override UUID chooseBlockerOrder(List value1, CombatGroup value2, List value3, Game value4) {
            Debug.Log("Player - chooseBlockerOrder");
            return null;
        }

        public override Mode chooseMode(Modes value1, Ability value2, Game value3) {
            Debug.Log("Player - chooseMode");
            return null;
        }

        public override bool chooseMulligan(Game value) {
            Debug.Log("Player - chooseMulligan");
            return false;
        }

        public override bool choosePile(Outcome value1, string value2, List value3, List value4, Game value5) {
            Debug.Log("Player - choosePile");
            return false;
        }

        public override int chooseReplacementEffect(Map value1, Game value2) {
            Debug.Log("Player - chooseReplacementEffect");
            return 0;
        }

        public override bool chooseTarget(Outcome value1, Target value2, Ability value3, Game value4) {
            Debug.Log("Player - chooseTarget");
            return false;
        }

        public override bool chooseTarget(Outcome value1, Cards value2, TargetCard value3, Ability value4, Game value5) {
            Debug.Log("Player - chooseTarget");
            return false;
        }

        public override bool chooseTargetAmount(Outcome value1, TargetAmount value2, Ability value3, Game value4) {
            Debug.Log("Player - chooseTargetAmount");
            return false;
        }

        public override TriggeredAbility chooseTriggeredAbility(List value1, Game value2) {
            Debug.Log("Player - chooseTriggeredAbility");
            return null;
        }

        public override bool chooseUse(Outcome value1, string value2, string value3, string value4, string value5, Ability value6, Game value7) {
            Debug.Log("Player - chooseUse");
            return false;
        }

        public override bool chooseUse(Outcome value1, string value2, Ability value3, Game value4) {
            Debug.Log("Player - chooseUse");
            return false;
        }

        public override void construct(Tournament value1, Deck value2) {
            Debug.Log("Player - construct");
        }

        public override Player copy() {
            return new XMPPMagePlayer(name);
        }

        public override int getAmount(int value1, int value2, string value3, Game value4) {
            Debug.Log("Player - getAmount");
            return 0;
        }

        public override void pickCard(List value1, Deck value2, Draft value3) {
            Debug.Log("Player - pickCard");
        }

        public override bool playMana(Ability value1, ManaCost value2, string value3, Game value4) {
            Debug.Log("Player - playMana");
            return false;
        }

        public override bool priority(Game value) {
            Debug.Log("Player - priority");
            return false;
        }

        public override void selectAttackers(Game value1, UUID value2) {
            Debug.Log("Player - selectAttackers");
        }

        public override void selectBlockers(Game value1, UUID value2) {
            Debug.Log("Player - selectBlockers");
        }

        public override void sideboard(Match value1, Deck value2) {
            Debug.Log("Player - sideboard");
        }

        public override void skip() {
            Debug.Log("Player - skip");
        }
    }

    [SyncVar]
    public string username;
    public mage.players.Player magePlayer;

    [SyncVar]
    public bool ready = false;
    [SyncVar]
    public string deckString;

    [Command]
    void CmdSetUsername(string username) {
        this.username = username;
    }

    [Command]
    void CmdSetDeckString(string deckString) {
        this.deckString = deckString;
    }

    [Command]
    void CmdSetReady(bool ready) {
        this.ready = ready;
    }

    [Command]
    void CmdInitMagePlayer() {
        magePlayer = new XMPPMagePlayer(username);
    }

    // Start is called before the first frame update
    public override void OnStartClient() {
        Debug.Log(isLocalPlayer);
        if (isLocalPlayer) {
            var cmd = GameObject.FindGameObjectWithTag("CMD").GetComponent<XMPPCmdargProcessor>();
            CmdSetUsername(cmd.username);
            cmd.ClientAttemptLoadDeck();
            CmdSetDeckString(cmd.deckString);
            CmdInitMagePlayer();
            CmdSetReady(true); // FIXME test code
        }
    }

    // Update is called once per frame
    void Update() {
    }
}
