using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageClick : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    private static RawImage selectedImage; // Static variable to store the first selected image
    private RawImage currentImage;
    public RawImage kim;
    public RawImage tray;
    public RawImage tissue_forceps;
    public RawImage scalpal_handle;
    public RawImage thread_cutting_scissors;
    public RawImage niti_wire_has_torque;
    // Store the correct and current arrangement of the images
    public static List<RawImage> correctArrangement = new List<RawImage>();
    public static List<RawImage> currentArrangement = new List<RawImage>();
    private void Awake()
    {
        currentImage = GetComponent<RawImage>();
        // Initialize the current arrangement when the game starts
        if (!currentArrangement.Contains(currentImage))
        {
            currentArrangement.Add(currentImage);
        }

    }



    private void DebugArrangementState()
    {
        for (int i = 0; i < correctArrangement.Count; i++)
        {
            Debug.Log($"Index {i} - Correct: {correctArrangement[i].texture.name}, Current: {currentArrangement[i].texture.name}");
        }
    }
    void Start()
    {
        Debug.Log("Starting arrangement population...");
        PopulateCorrectArrangement();
        UpdateCurrentArrangement();
        DebugArrangementState();
    }
    private void PopulateCorrectArrangement()
    {
        correctArrangement.Clear();
        // Add RawImages in the correct order
        if (kim != null) correctArrangement.Add(kim);
        if (tray != null) correctArrangement.Add(tray);
        if (tissue_forceps != null) correctArrangement.Add(tissue_forceps);
        if (scalpal_handle != null) correctArrangement.Add(scalpal_handle);
        if (thread_cutting_scissors != null) correctArrangement.Add(thread_cutting_scissors);
        if (niti_wire_has_torque != null) correctArrangement.Add(niti_wire_has_torque);

        Debug.Log("Correct arrangement populated. Count: " + correctArrangement.Count);
    }
    public void SwapTextures(RawImage imageA, RawImage imageB)
    {
        Texture tempTexture = imageA.texture;
        imageA.texture = imageB.texture;
        imageB.texture = tempTexture;

        Debug.Log($"Swapped textures: {imageA.texture.name} <=> {imageB.texture.name}");
    }
    public void OnSubmit()
    {
        CheckArrangement();
    }
    void UpdateCurrentArrangement()
    {
        currentArrangement.Clear();  // Clear before re-populating
        if (kim != null) currentArrangement.Add(kim);
        if (tray != null) currentArrangement.Add(tray);
        if (tissue_forceps != null) currentArrangement.Add(tissue_forceps);
        if (scalpal_handle != null) currentArrangement.Add(scalpal_handle);
        if (thread_cutting_scissors != null) currentArrangement.Add(thread_cutting_scissors);
        if (niti_wire_has_torque != null) currentArrangement.Add(niti_wire_has_torque);

        // Log the count for debugging
        Debug.Log("Current arrangement count: " + currentArrangement.Count);


    }
    private void UpdateArrangementIndices(RawImage selected, RawImage current)
    {
        int selectedIndex = currentArrangement.IndexOf(selected);
        int currentIndex = currentArrangement.IndexOf(current);

        if (selectedIndex >= 0 && currentIndex >= 0)
        {
            currentArrangement[selectedIndex] = current;
            currentArrangement[currentIndex] = selected;
        }
        else
        {
            Debug.LogError("Index not found in current arrangement!");
        }
    }
    // Handle image clicks to swap textures
    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedImage == null)
        {
            // Select the current image
            selectedImage = currentImage;

            Debug.Log("Selected image: " + gameObject.name);
        }
        else
        {
            // Swap the textures if the selected image is different
            if (selectedImage.gameObject != gameObject) // Ensure we are not selecting the same image
            {
                SwapTextures(selectedImage, currentImage);
                UpdateCurrentArrangement();
                UpdateArrangementIndices(selectedImage, currentImage);


            }
            // Clear the selection after swapping
            selectedImage = null;
        }
    }
    public void CheckArrangement()
    {

        // Check for null references
        if (correctArrangement == null || currentArrangement == null)
        {
            Debug.LogError("One of the arrangements is null!");
            return;
        }

        if (correctArrangement.Count != currentArrangement.Count)
        {
            Debug.LogError("The number of elements in correct and current arrangement don't match!");
            return;
        }

        bool isCorrect = true;

        // Provide feedback
        for (int i = 0; i < correctArrangement.Count; i++)
        {
            if (currentArrangement[i] == null || correctArrangement[i] == null)
            {
                Debug.LogError($"Null reference found at index {i}: currentArrangement[i] is {(currentArrangement[i] == null ? "null" : "not null")} and correctArrangement[i] is {(correctArrangement[i] == null ? "null" : "not null")}");
                isCorrect = false; // Mark as incorrect due to null reference
                continue;
            }
            var currentTexture = currentArrangement[i].texture;
            var correctTexture = correctArrangement[i].texture;
            Debug.Log($"Index {i}: Current Texture = {currentTexture.name} (Instance: {currentTexture.GetInstanceID()}), Correct Texture = {correctTexture.name} (Instance: {correctTexture.GetInstanceID()})");
            if (currentArrangement[i].texture != correctArrangement[i].texture)
            {
                isCorrect = false;
                Debug.Log($"Wrong: Image at index {i} is incorrect");

            }
            else
            {
                Debug.Log($"Correct: Image at index {i} is correct");
            }
        }

        // Provide feedback
        if (isCorrect)
        {
            Debug.Log("Correct arrangement!");
            // Hide the canvas or display success message
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0; // Hide the canvas
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
        else
        {
            Debug.Log("Wrong arrangement! Playing sound...");
            // Play wrong answer sound or display a failure message
            AudioSource wrongSound = GetComponent<AudioSource>();
            if (wrongSound != null)
            {
                wrongSound.Play();
            }
        }
    }
}
