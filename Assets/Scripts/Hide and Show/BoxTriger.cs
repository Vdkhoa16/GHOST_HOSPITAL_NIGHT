using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxTriger : MonoBehaviour
{
    public bool isTriger = false;
    public SafeController safeController;


    public float moveSpeed = 2.0f;
    public float moveDistance = 1.0f; 
    private Vector3 startPosition;// bắt đầu
    private Vector3 targetPosition;// mục tiêu

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        startPosition = transform.position; 
        targetPosition = startPosition + new Vector3(0, 0, moveDistance); // Đặt vị trí mục tiêu trên trục Z
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void setActive()
    {
        gameObject.SetActive(true);
    }


    public bool CheckOnTriger()
    {
       // kiểm tra người dùng có chạm vào saw hay k
        return isTriger;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriger = true;
            //Destroy(gameObject);
            RandomPass();
            // nếu chạm vào thì mật khẩu safe sẽ bị thay đổi 
            // để set sự kiện khi người chơi thua thì mật khẩu safe sẽ bị thay đổi
        }
    }

    public void RandomPass()
    {
        int pass;
        pass = Random.Range(1000, 9999);
        safeController.keyID = pass;

    }
    public void MoveCube()
    {
        // vị trí hiện tại đi đến targetpositon
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Đổi hướng
            if(targetPosition == startPosition)
            {
                targetPosition = startPosition+ new Vector3(0, 0,moveDistance);
            }
            else
            {
                targetPosition = startPosition;
            }
            
        }
    }
    }
