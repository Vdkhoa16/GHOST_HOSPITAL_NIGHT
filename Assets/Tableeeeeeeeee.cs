using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject monster; // Gán quái vật từ trong Unity Editor

    void Start()
    {
        if (monster != null)
        {
            monster.SetActive(false); // Ẩn quái vật khi bắt đầu
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng vào vùng là Player để hiện quái
        if (other.CompareTag("Player") && monster != null)
        {
            monster.SetActive(true); // Hiện quái vật khi người chơi lại gần
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra nếu đối tượng ra khỏi vùng là Player để ẩn quái
        if (other.CompareTag("Player") && monster != null)
        {
            monster.SetActive(false); // Ẩn quái vật khi người chơi ra xa
        }
    }
}
