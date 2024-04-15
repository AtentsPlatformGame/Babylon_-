using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaData : MonoBehaviour
{
    // ĳ���� ������ ������ ������
    public string characterName;
    public int health = 10;
    public int attack = 1;
    public Equipment equipment;
    public Animations animations;

    // ĳ������ ��� ������ ������ Ŭ����
    [System.Serializable]
    public class Equipment
    {
        public string weapon;
        public string amor;
        public string item;
        public string acces;
    }

    // ĳ������ �ִϸ��̼� ������ ������ Ŭ����
    [System.Serializable]
    public class Animations
    {
        public string idle;
        public string walk;
        public string attack;
        public string hurt;
        public string die;
    }
}
