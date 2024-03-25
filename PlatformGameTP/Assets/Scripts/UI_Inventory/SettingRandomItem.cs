using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingRandomItem : MonoBehaviour
{
    public ItemProperty[] itemArray; // �нú�, ���� ��Ÿ ����� ���� �迭
    public ItemProperty[] itemButtons; // ���� ȭ�鿡�� ��Ÿ�� ��ư �迭
    int origin = -1;
    // Start is called before the first frame update
    void OnEnable()
    {
        SetItemButton();
        Debug.Log(itemButtons[1].GetItemStat());
    }

    public void SetItemButton()
    {
        for (int i = 0; i < 3;)
        {
            int idx = Random.Range(0, itemArray.Length);
            if (origin != idx)
            {
                itemButtons[i] = itemArray[idx];
                //
                origin = idx;

                i++;
            }
            else
            {
                continue;
            }
        }
    }
}
