using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInventory;

public class SafePass : MonoBehaviour
{
    [SerializeField] private List<Button> listInput = new List<Button>();
    [SerializeField] private TMP_InputField inputPassSafe;
    private string currentInput = "";
    public SafeController safeController;
    public bool checkPass = false;


    void Start()
    {
        for (int i = 0; i < listInput.Count; i++)
        {
            int index = i;
            listInput[i].onClick.AddListener(() => Number(index));
        }
        safeController = FindObjectOfType<SafeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Number(int i)
    {
        inputPassSafe.text += ""+(i+1);
        currentInput = inputPassSafe.text;
    }

    public void ButtonSubmit()
    {
        int ID = safeController.keyID;
        Debug.Log(ID);
        if(currentInput == ID.ToString())
        {
            safeController.ToggleDoorServerRpc();
        }
        else
        {
            inputPassSafe.text = "";
        }

    }

    public void Button0()
    {
        inputPassSafe.text += "0";
        currentInput = inputPassSafe.text;
    }

    public void Remove()
    {
        inputPassSafe.text = "";
        currentInput = "";
    }

    public void OFFMouse()
    {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        
    }

    public void OnMose()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
