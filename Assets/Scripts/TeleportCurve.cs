using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCircleUI; //�ڷ���Ʈ�� ǥ���� UI
    LineRenderer lr;//���� �׸� ���η�����
    Vector3 originScale = Vector3.one * 0.02f; //���� �ڷ���Ʈ UIũ��
    public int lineSmooth = 40; //Ŀ���� �ε巯�� ����
    public float curveLength = 50; //Ŀ���� ����
    public float gravity = -60; // Ŀ���� �߷�
    public float simulateTime = 0.02f;//��� �ùķ��̼��� ���� �� �ð�
    List<Vector3> lines = new List<Vector3>();//��� �̷�� ������ ����� ����Ʈ
    // Start is called before the first frame update
    void Start()
    {
        //������ �� ��Ȱ��ȭ �Ѵ�.
        teleportCircleUI.gameObject.SetActive(false);
        //���� ������ ������Ʈ ������
        lr = GetComponent<LineRenderer>();
        //���η������� �� �ʺ� ����
        lr.startWidth = 0.0f;
        lr.endWidth = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true; //������ Ȱ��ȭ
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false; //������ ��Ȱ��ȭ
            if (teleportCircleUI.gameObject.activeSelf)
            {
                GetComponent<CharacterController>().enabled = false;
                transform.position = teleportCircleUI.position + Vector3.up;
                GetComponent<CharacterController>().enabled = true;
            }
            teleportCircleUI.gameObject.SetActive(false);
        }
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            MakeLines();
        }
    }
    void MakeLines() //���η������� �̿��� ���� ����� ���� �׸���.
    {
        lines.RemoveRange(0, lines.Count); //����Ʈ�� ��� ��ġ �������� ����ش�.
        Vector3 dir = ARAVRInput.LHandDirection * curveLength; //���� ����� ���� ����
        Vector3 pos = ARAVRInput.LHandPosition; //���� �׷��� ��ġ �ʱⰪ ����
        lines.Add(pos); //���� ��ġ�� ����Ʈ�� ���

        for(int i=0; i<lineSmooth; i++) {
            Vector3 lastPos = pos; //���� ��ġ ���
            //v = v0+at
            dir.y += gravity * simulateTime; // ��� ����� ���� ��ġ ���
            pos += dir * simulateTime;
            if (CheckHitRay(lastPos, ref pos)) //Ray �浹üũ�� �Ͼ����..
            {
                lines.Add(pos); //�浹 ������ ����ϰ� ����
                break;
            }
            else
            {
                //�ڷ���Ʈ UI ��Ȱ��ȭ
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
        //���� �������� ǥ���� ���� ������ ��ϵ� ������ ũ��� �Ҵ�
        lr.positionCount = lines.Count;
        //���� �������� ������ ���� ������ ����
        lr.SetPositions(lines.ToArray());
    }
    private bool CheckHitRay(Vector3 lastPos,  ref Vector3 pos)
    {
        //�� ���� ��ġ�� ���� ���� ��ġ�� �޾� ������ �浹�� üũ
        Vector3 rayDir = pos - lastPos;
        Ray ray = new Ray(lastPos, rayDir);
        RaycastHit hitInfo;
        //RayCast�� �� ������ ũ�⸦ �� ���� ���� �� ������ �Ÿ��� �����Ѵ�.
        if (Physics.Raycast(ray, out hitInfo, rayDir.magnitude))
        {
            pos = hitInfo.point;
            int layer = LayerMask.NameToLayer("Terrain");
            //Terrain ���̾�� �浹���� ��쿡�� �ڷ���Ʈ UI�� ǥ�õǵ��� �Ѵ�.
            if (hitInfo.transform.gameObject.layer == layer)
            {
                //�ڷ���Ʈ UIȰ��ȭ
                teleportCircleUI.gameObject.SetActive(true);
                //�ڷ���Ʈ UI�� ��ġ ����
                teleportCircleUI.position = pos;
                //�ڷ���Ʈ UI�� ���� ����
                teleportCircleUI.forward = hitInfo.normal;
                float distance = (pos-ARAVRInput.LHandPosition).magnitude;
                //�ڷ���Ʈ UI�� ���� ũ�⸦ ����
                teleportCircleUI.localScale = originScale * Mathf.Max(1, distance);
            }
            return true;
        }
        return false;
    }
}
