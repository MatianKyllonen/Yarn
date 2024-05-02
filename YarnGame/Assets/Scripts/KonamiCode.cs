using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    // Konami code sequence
    private readonly KeyCode[] konamiCode = { KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.B, KeyCode.A };
    private int currentCodeIndex = 0;

    // Animator reference
    public Animator animator;

    private void Update()
    {
        CheckKonamiCode();
    }

    private void CheckKonamiCode()
    {
        if (Input.GetKeyDown(konamiCode[currentCodeIndex]))
        {
            Debug.Log("Key Pressed: " + konamiCode[currentCodeIndex]); // Debug log for key pressed
            currentCodeIndex++;
            if (currentCodeIndex == konamiCode.Length)
            {
                Debug.Log("Konami Code Entered!"); // Debug log for full Konami code entered
                // Trigger the animator
                animator.SetTrigger("HitTheGriddy");
                // Reset code index for future inputs
                currentCodeIndex = 0;
            }
        }
        else
        {
            Debug.Log("Key Reset"); // Debug log for resetting code index
            // Reset index if the wrong key is pressed
            currentCodeIndex = 0;
        }
    }
}
