using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LGH
{
    public class Shop : MonoBehaviour
    {
        public static int gold { get; private set; }
        public static void Spend(int money)
        {
            if (money > gold)
            {
                Debug.LogError("���� �����մϴ�.");
                return;
            }
            gold -= money;
        }
        public static void Earn(int income)
        {
            gold += income;
        }

        // Start is called before the first frame update
        void Start() => this.gameObject.SetActive(false);

        // Update is called once per frame
        void Update()
        {

        }
    }
}

