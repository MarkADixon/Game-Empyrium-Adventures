using UnityEngine;
using System.Collections;

public class Character
{
    public CharacterSheet sheet;
    public CharacterPiece piece;

	// Use this for initialization
	public Character (CharacterSheet _sheet, CharacterPiece _piece) {
        sheet = _sheet;
        piece = _piece;
	}
	
}
