using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tube_scipts : MonoBehaviour
{
    [SerializeField] GameObject tube_balls_holder;
    [SerializeField] Transform tube_start_pos;
    [SerializeField] GameObject Selected_Holder;
    [HideInInspector]public bool isSelectedthis;
    bool isCompleted;
    
    // Start is called before the first frame update
    void Start()
    {
        isCompleted = false;
        isSelectedthis =false;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance.isSelected) 
        { 
            if (isSelectedthis)
            {
                if (Vector3.Distance(Selected_Holder.transform.GetChild(0).gameObject.transform.position, tube_start_pos.transform.position) >= 0.1f)
                {
                    Vector3 newpos = tube_start_pos.transform.position - Selected_Holder.transform.GetChild(0).transform.position;
                    Selected_Holder.transform.GetChild(0).transform.Translate(newpos * 9 * Time.deltaTime, Space.World);
                }
            }
                    
        }
    }

    void Check_Tube()
    {
        if(tube_balls_holder.transform.childCount == 3)
        {
            if(tube_balls_holder.transform.GetChild(0).gameObject.tag == tube_balls_holder.transform.GetChild(1).gameObject.tag &&
               tube_balls_holder.transform.GetChild(0).gameObject.tag == tube_balls_holder.transform.GetChild(2).gameObject.tag)
            {
                if (!isCompleted)
                {
                    isCompleted = true;
                    for (int i = 0; i < LevelManager.instance.level_tubes_completed.Length; i++)
                    {
                        if (LevelManager.instance.level_tubes_completed[i] == false)
                        {
                            LevelManager.instance.level_tubes_completed[i] = true;
                            break;
                        }                        
                    }
                }
            }
        }
    }


    private void OnMouseDown()
    {
        if (!isCompleted)
        {
            if (!LevelManager.instance.isSelected)
            {
                if (!isSelectedthis)
                {
                    isSelectedthis = true;
                    Debug.Log(transform.parent.gameObject.name.ToString() + " Clicked");
                    Debug.Log(tube_balls_holder.transform.GetChild(tube_balls_holder.transform.childCount - 1).gameObject.name.ToString() + " Selected");
                    tube_balls_holder.transform.GetChild(tube_balls_holder.transform.childCount - 1).gameObject.transform.parent = Selected_Holder.transform;
                    Selected_Holder.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = false;
                    LevelManager.instance.isSelected = true;
                }
            }
            else if (LevelManager.instance.isSelected)
            {
                if (isSelectedthis)
                {
                    isSelectedthis = false;
                    LevelManager.instance.isSelected = false;
                    Selected_Holder.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
                    Selected_Holder.transform.GetChild(0).gameObject.transform.parent = tube_balls_holder.transform;
                }
                else
                {
                    if (tube_balls_holder.transform.childCount < 3)
                    {
                        LevelManager.instance.isSelected = false;
                        Selected_Holder.transform.GetChild(0).gameObject.transform.position = tube_start_pos.position;
                        Selected_Holder.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
                        Selected_Holder.transform.GetChild(0).gameObject.transform.parent = tube_balls_holder.transform;
                        LevelManager.instance.Reset_Selected();
                    }
                    else
                    {
                        Debug.Log("Tube is full");
                    }
                }
                Check_Tube();
                LevelManager.instance.CheckGameComplete();
            }
        }
        else
        {
            Debug.Log("Tube is Completed");
        }
    }
}
