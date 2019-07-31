using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour {

    [SerializeField] GameObject[] Characters;

    private int currentSelection = 0;
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown("f"))
        {
            ChangeCharacter();
        }
	}

    private void ChangeCharacter()
    {
        //Set current characters as inactive
        Characters[currentSelection].SetActive(false);

        currentSelection++;

        if (currentSelection >= Characters.Length)
        {
            currentSelection = 0;
        }

        Characters[currentSelection].SetActive(true);
    }
}
