using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShowSingleGreen : MonoBehaviour
{

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = 16;  //�߿��õ�
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;

    int _posX = 0;
    int _posY = 0;
    // ��������������Ҫ�Ĵ��ڿ�
    public int _Txtwith = 800;
    // ��������������Ҫ�Ĵ��ڸ�
    public int _Txtheight = 600;
    void Start()
    {
        //Cursor.visible = false; // �������
        Debug.Log(Screen.width+"       "+ Screen.currentResolution.width);//Screen.currentResolution.width ��ǰ��Ļ�ķֱ���
        Screen.SetResolution(_Txtwith, _Txtheight, false);
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            //StartCoroutine("Setposition");
            SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP); //�ޱ߿�Ļ�GWL_STYLE Ϊ����
            bool result = SetWindowPos(GetForegroundWindow(), 0, Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);       //������Ļ��С��λ��
            Debug.Log("���������");
        }
        else
        {
            Debug.Log("WINDOWS EDITOR");
        }
        
        //Screen.SetResolution(_Txtwith, _Txtheight, false);		//�����Unity���������Ļ��С��
    }
    IEnumerator Setposition()
    {
        yield return new WaitForSeconds(0.1f);		//��֪��Ϊʲô�������к�����λ�õĲ�����Ч�����ӳ�0.1��Ϳ���
       
    }
}
