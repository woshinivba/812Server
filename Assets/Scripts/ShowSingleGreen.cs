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
    const int GWL_STYLE = 16;  //边框用的
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;

    int _posX = 0;
    int _posY = 0;
    // 在这里设置你想要的窗口宽
    public int _Txtwith = 800;
    // 在这里设置你想要的窗口高
    public int _Txtheight = 600;
    void Start()
    {
        //Cursor.visible = false; // 鼠标隐藏
        Debug.Log(Screen.width+"       "+ Screen.currentResolution.width);//Screen.currentResolution.width 当前屏幕的分辨率
        Screen.SetResolution(_Txtwith, _Txtheight, false);
        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            //StartCoroutine("Setposition");
            SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP); //无边框的话GWL_STYLE 为负数
            bool result = SetWindowPos(GetForegroundWindow(), 0, Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);       //设置屏幕大小和位置
            Debug.Log("打包出来了");
        }
        else
        {
            Debug.Log("WINDOWS EDITOR");
        }
        
        //Screen.SetResolution(_Txtwith, _Txtheight, false);		//这个是Unity里的设置屏幕大小，
    }
    IEnumerator Setposition()
    {
        yield return new WaitForSeconds(0.1f);		//不知道为什么发布于行后，设置位置的不会生效，我延迟0.1秒就可以
       
    }
}
