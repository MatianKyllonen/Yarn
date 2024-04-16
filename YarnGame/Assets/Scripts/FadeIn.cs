using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeDuration = 2f; // Duration of the fade in seconds

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color startingColor = sr.color;
        Color transparentColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            // Calculate the current opacity based on the elapsed time
            float t = elapsedTime / fadeDuration;
            sr.color = Color.Lerp(startingColor, transparentColor, t);

            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object is fully transparent at the end
        sr.color = transparentColor;
    }
}
