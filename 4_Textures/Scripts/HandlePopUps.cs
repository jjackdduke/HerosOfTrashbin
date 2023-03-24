using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �˾� â�� ���� �ݴ� ���� �����ϴ� ��ũ��Ʈ
public class HandlePopUps : MonoBehaviour
{
    // Ȱ��ȭ�� �˾� â�� ���� Stack
    public List<GameObject> popUps;

    // Scene�� �ε�� �� �ڽ� �˾� ������Ʈ �� Ȱ��ȭ�� �͸� Push
    private void Awake()
    {
        popUps = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                popUps.Add(child.gameObject);
            }
        }
    }

    // â�� ���� ������Ʈ�� ���ÿ� Push
    public void OpenPopUp(GameObject popUp)
    {
        popUp.SetActive(true);
        popUps.Add(popUp);
    }

    // â�� �ݰ� ���� Pop
    public void ClosePopUp(GameObject popUp)
    {
        popUp.SetActive(false);
        popUps.Remove(popUp);
    }

    // ��� â�� �ݰ� ������ ���� �� ������ Pop
    public void AllClosePopUp()
    {
        foreach (GameObject popUp in popUps)
        {
            popUp.SetActive(false);
        }
        popUps.Clear();
    }
}