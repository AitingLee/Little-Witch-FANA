using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterName {Fana, Fefe, Bae, Mary, Rae, Capay, Emily}

public class Villager : MonoBehaviour
{
    public CharacterName characterName;
    string nameChinese;
    public Dialogue[] dialogues;
    public Dialogue currentDialogue;
    public GameObject dialogueIcon;

    public Indicator exclamation;
    public Indicator repeat;
    public Indicator tick;

    public GameObject exclamationIcon, repeatIcon, tickIcon, normalIcon;
    public GameObject exclamationMini, repeatMini, tickMini, normalMini;
    private void Awake()
    {
        currentDialogue = dialogues[0];
        InitName();
    }

    private void Update()
    {
        UpdateIcon();
    }

    private void InitName()
    {
        switch (characterName)
        {
            case CharacterName.Fana:
                nameChinese = "猭甊";
                break;
            case CharacterName.Fefe:
                nameChinese = "滇滇";
                currentDialogue = dialogues[1];
                break;
            case CharacterName.Bae:
                nameChinese = "īㄠ";
                break;
            case CharacterName.Mary:
                nameChinese = "嚎产";
                break;
            case CharacterName.Rae:
                nameChinese = "立立";
                break;
            case CharacterName.Capay:
                nameChinese = "ㄘ";
                break;
            case CharacterName.Emily:
                nameChinese = "︺籩产";
                break;
        }

        foreach (Dialogue dialogue in dialogues)
        {
            dialogue.name = nameChinese;
            dialogue.talkPerson = this;
        }
    }

    private void UpdateIcon()
    {
        if (characterName == CharacterName.Fana)
        {
            return;
        }

        if (exclamation != null && exclamation.gameObject.activeSelf)
        {
            exclamationIcon.SetActive(true);
            exclamationMini.SetActive(true);

            if (repeatIcon != null)
            {
                repeatIcon.SetActive(false);
                repeatMini.SetActive(false);
            }
            if (tickIcon != null)
            {
                tickIcon.SetActive(false);
                tickMini.SetActive(false);
            }
            normalIcon.SetActive(false);
            normalMini.SetActive(false);
        }
        else if (repeat != null && repeat.gameObject.activeSelf)
        {
            if (exclamationIcon != null)
            {
                exclamationIcon.SetActive(false);
                exclamationMini.SetActive(false);
            }
            repeatIcon.SetActive(true);
            repeatMini.SetActive(true);
            if (tickIcon != null)
            {
                tickIcon.SetActive(false);
                tickMini.SetActive(false);
            }
            normalIcon.SetActive(false);
            normalMini.SetActive(false);
        }
        else if (tickIcon != null && tick.gameObject.activeSelf)
        {
            if (exclamationIcon != null)
            {
                exclamationIcon.SetActive(false);
                exclamationMini.SetActive(false);
            }
            if (repeatIcon != null)
            {
                repeatIcon.SetActive(false);
                repeatMini.SetActive(false);
            }
            tickIcon.SetActive(true);
            tickMini.SetActive(true);
            normalIcon.SetActive(false);
            normalMini.SetActive(false);
        }
        else
        {
            if (exclamationIcon != null)
            {
                exclamationIcon.SetActive(false);
                exclamationMini.SetActive(false);
            }
            if (repeatIcon != null)
            {
                repeatIcon.SetActive(false);
                repeatMini.SetActive(false);
            }
            if (tickIcon != null)
            {
                tickIcon.SetActive(false);
                tickMini.SetActive(false);
            }
            normalIcon.SetActive(true);
            normalMini.SetActive(true);
        }
    }

    public void TriggerCurrentDialogue()
    {
        DialogueManager.instance.StartDialogue(currentDialogue);
    }

}
