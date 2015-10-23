using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Character 
{
    public CharacterSheet sheet;
    public CharacterPiece piece;

    public List<Effect> effects = new List<Effect>();
    public Character selectedTarget;

    // Use this for initialization
    public Character (CharacterSheet _sheet, CharacterPiece _piece) {
        sheet = _sheet;
        piece = _piece;
	}

}
