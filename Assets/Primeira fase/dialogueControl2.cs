using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl2 : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    public Text nameNpc;
    public Image imageNpc;
    public Sprite spriteNpc;
    public AudioSource VitoriaAudio;

    public bool readyToSpeak;
    public bool startDialogue;
    

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueIndex = -1; // Inicia com -1 para que o primeiro diálogo seja o de índice 0
        VitoriaAudio = GameObject.Find("VitoriaSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && readyToSpeak)
        {
            
            Debug.Log("Enter key pressed. Ready to speak: " + readyToSpeak);
            if (!startDialogue)
            {
                VitoriaAudio.Play();
                StartDialogue();
                
            }
            else
            {
                NextDialogue();
            }
        }
    }

    void StartDialogue()
    {
        startDialogue = true;
        dialogueIndex = 0;
        nameNpc.text = "Prof:Sandro";
        dialogueText.text = dialogueNpc[dialogueIndex];
        imageNpc.sprite = spriteNpc;
        dialoguePanel.SetActive(true);
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < dialogueNpc.Length)
        {
            dialogueText.text = dialogueNpc[dialogueIndex];
            StartCoroutine(ShowDialogue());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        startDialogue = false;
        dialogueIndex = 0;
        FindObjectOfType<Movimentação>().speed = 5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = false;
        }
    }
}
