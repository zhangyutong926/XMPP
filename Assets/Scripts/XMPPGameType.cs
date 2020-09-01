
using System.Collections.Generic;
using mage.game;
using mage.game.mulligan;

public abstract class XMPPGameType {
    public static Dictionary<string, XMPPGameType> gameTypes = new Dictionary<string, XMPPGameType> {
        ["FreeForAll"] = new XMPPGameTypeFreeForAll { gameTypeName = "Free For All", gameTypeClass = java.lang.Class.forName("mage.game.FreeForAll") }
    };

    public string gameTypeName;
    public java.lang.Class gameTypeClass;

    public abstract mage.game.Game createGame(XMPPGameOptions options);
}

public class XMPPGameTypeFreeForAll : XMPPGameType {
    public override Game createGame(XMPPGameOptions options) {
        var ffa = new FreeForAll(options.multiplayerAttackOption, mage.constants.RangeOfInfluence.ALL, options.mulligan, options.startLife);
        ffa.setGameOptions(options.mageGameOptions);
        return ffa;
    }
}

public abstract class XMPPMulliganType {
    public static Dictionary<string, XMPPMulliganType> mulliganTypes = new Dictionary<string, XMPPMulliganType> {
        ["CanadianHighlanderMulligan"] = new XMPPMulliganTypeCanadianHighlanderMulligan()
    };

    public abstract Mulligan createMulligan(int freeMulligan);
}

public class XMPPMulliganTypeCanadianHighlanderMulligan : XMPPMulliganType {
    public override Mulligan createMulligan(int freeMulligan) {
        return new CanadianHighlanderMulligan(freeMulligan);
    }
}