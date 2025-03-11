using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class UITypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public string fullText;
    public float timePerCharacter;
    public bool typeWhenReady;

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

        // Start new typing
        this.typingCoroutine = StartCoroutine(TypeRoutine());
    }

    // Called automatically when the GameObject becomes active
    private void OnEnable()
    {
        // Start typing when the button becomes visible
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

    private IEnumerator TypeRoutine()
    {
        this.textMesh.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            this.textMesh.text = fullText.Substring(0, i + 1);
            yield return new WaitForSeconds(this.timePerCharacter);
        }

        this.typingCoroutine = null;

        this.callback?.Invoke();
    }
}
