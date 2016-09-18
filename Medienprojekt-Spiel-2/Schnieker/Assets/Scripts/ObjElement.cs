using UnityEngine;
using System.Collections;

public class ObjElement : MonoBehaviour {
    public enum elms {
        NORMAL,
        STONE,
        SCISSOR,
        PAPER,
    };
    public elms myElement = elms.NORMAL;
    public static int calcDmg(elms attacker, elms attacked)
    {
        if(attacker == elms.PAPER && attacked == elms.STONE)
            return 34; //3 hits, stronger
        else if (attacker == elms.SCISSOR && attacked == elms.PAPER)
            return 34; //3 hits, stronger
        else if (attacker == elms.STONE && attacked == elms.SCISSOR)
            return 34; //3 hits, stronger
        else if (attacker == elms.STONE && attacked == elms.PAPER)
            return 20; //5 hits, weaker
        else if (attacker == elms.PAPER && attacked == elms.SCISSOR)
            return 20; //5 hits, weaker
        else if (attacker == elms.SCISSOR && attacked == elms.STONE)
            return 20; //5 hits, weaker

        return 25; //4 hits, default
    }
}
