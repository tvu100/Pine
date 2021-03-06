﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameManager gameManager;
    public CharacterBuy[] characterButtons;
    public Transform spawnPos;
    public Dictionary<int, CharacterBuy> characters = new Dictionary<int, CharacterBuy>();
    public Dictionary<CharacterBuy, int> charactersKey = new Dictionary<CharacterBuy, int>();
    public static GameObject activeCharacter;
    public static Outfits activeVisual;
    private CharacterBuy lastAcitveCharacterButton;

    void Awake()
    {
        activeCharacter = GameObject.FindGameObjectWithTag("Player");
        //defualt pine on first startup
        if(PlayerPrefs.GetInt("ActiveCharacter") == 0)
            PlayerPrefs.SetInt("ActiveCharacter", 1);
        
        for(int i = 0; i < characterButtons.Length; i++)
        {
            characters.Add(i + 1, characterButtons[i]);  
            charactersKey.Add(characterButtons[i], i + 1);
        }
        lastAcitveCharacterButton = characters[PlayerPrefs.GetInt("ActiveCharacter")];      
        //equipped the previous equipped character
        characters[PlayerPrefs.GetInt("ActiveCharacter")].equipped = true;   
        CharacterBuy tempChar = characters[PlayerPrefs.GetInt("ActiveCharacter")];
        if(tempChar.equipped)
            EquipCharacter(tempChar);
        
    }

    public void moveToSpawnPos(GameObject target, Transform spawnPos)
    {
        target.transform.position = spawnPos.position;
    }

    public void EquipCharacter(CharacterBuy charBuy)
    {
            //reset all equip bool to false first
            lastAcitveCharacterButton.equipped = false;
            lastAcitveCharacterButton.equipButton.GetComponent<Button>().interactable = true;
            lastAcitveCharacterButton.characterPrefab.SetActive(false); 
            lastAcitveCharacterButton.purchaseText.text = "Equip";
                
            //saves who was last equipped
            PlayerPrefs.SetInt("ActiveCharacter",charactersKey[charBuy]);

            //then equip the new character  
            activeVisual = charBuy.characterPrefab.GetComponent<Outfits>();
            charBuy.equipped = true;    
            charBuy.equipButton.GetComponent<Button>().interactable = false;
            charBuy.purchaseText.text = "Equipped"; 
            //turn on the visuals
            charBuy.characterPrefab.SetActive(true);
            gameManager.InitialisePlayer();
            lastAcitveCharacterButton = charBuy;
    }
}
