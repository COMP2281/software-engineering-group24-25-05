using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Linq;

public class UITypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public string fullText;

    public float timePerCharacter = 0.05f;
    public float shortPauseMultiplier = 5.0f;
    public float longPauseMultiplier = 10.0f;

    public char[] shortPauseCharacters = new[] { ',', ';' };
    public char[] longPauseCharacters = new[] { '.', ':', '!', '?' };

    public bool typeWhenReady;
    public bool showSpinner = false;

    public string[] spinnerText = new string[] {
        ".", "..", "...", "....", ".....",
        "......",
        ".....", "....", "...", "..", ".",
    };

    private Action callback = null;

    private Coroutine typingCoroutine;

    public void Start() { }

    public void SetText(string text)
    {
        this.fullText = text;
    }

    public void SetCallback(Action callback)
    {
        this.callback = callback;
    }

    public void StartTyping()
    {
        if (this.typingCoroutine != null)
            StopCoroutine(this.typingCoroutine);

        this.typingCoroutine = StartCoroutine(TypeRoutine());
    }

    public void Clear()
    {
        this.fullText = "";
        this.textMesh.text = "";
    }

    private void OnEnable()
    {
        if (this.typeWhenReady)
        {
            StartTyping();
        }
    }

    private void OnDisable()
    {
        // Clean up if the object is disabled
        if (this.typingCoroutine != null)
        {
            StopCoroutine(this.typingCoroutine);
            this.typingCoroutine = null;
        }
    }

    public void StartSpinner()
    {
        this.showSpinner = true;
        StartCoroutine(this.SpinnerRoutine());
    }

    public void StopSpinner()
    {
        this.showSpinner = false;
    }

    private IEnumerator SpinnerRoutine()
    {
        int i = 0;

        while (this.showSpinner)
        {
            this.textMesh.text = this.spinnerText[i % this.spinnerText.Length];
            i += 1;
            yield return new WaitForSeconds(0.075f);
        }
    }

    private IEnumerator TypeRoutine()
    {
        this.textMesh.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            this.textMesh.text = fullText.Substring(0, i + 1);

            float multiplier = 1;
            if (this.shortPauseCharacters.Contains(this.textMesh.text[i]))
            {
                multiplier = this.shortPauseMultiplier;
            }
            else if (this.longPauseCharacters.Contains(this.textMesh.text[i]))
            {
                multiplier = this.longPauseMultiplier;
            }

            yield return new WaitForSeconds(this.timePerCharacter * multiplier);
        }

        this.typingCoroutine = null;

        this.callback?.Invoke();
    }
}
