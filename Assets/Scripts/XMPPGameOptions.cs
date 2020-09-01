using mage.constants;
using mage.game.mulligan;

public struct XMPPGameOptions {
    public Mulligan mulligan;
    public int startLife;
    public int startingSize;

    // Multiplayer
    public MultiplayerAttackOption multiplayerAttackOption;

    // Internal
    public mage.game.GameOptions mageGameOptions;
}
