using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSocket : MonoBehaviour
{
    [HideInInspector]
    public bool btr=false;
    public Socket socket;

    [HideInInspector]
    public int clientID;
    //private byte[] cacheBytes = new byte[1024 * 1024];
    private int cacheNum = 0;//���յ����ֽ��� 
    private long frontTime = -1;
    //dynamic cishu;
    private Dictionary<string, string> cishu = new Dictionary<string, string>();
    private string Studentname;
    public static int CLIENT_BEGIN_ID = 1;
    private static int TIME_OUT_TIME = 10;//��ʱʱ��
    public ClientSocket(Socket socket)
    {
        cishu.Add("zaihediaozhuang", "0");
        cishu.Add("cebananzhuang", "0");
        cishu.Add("weixingdiaozhuang", "0");
        cishu.Add("weixingzhuanyun", "0");
        cishu.Add("ceyizhankai", "0");
        cishu.Add("datatime", "0");
        //cishu = new { ceban=0,zaihe= 0, zhuanyun= 0, diaozhuang= 0, ceyi= 0 };
        this.clientID = CLIENT_BEGIN_ID;
        this.socket = socket;
        CLIENT_BEGIN_ID++;
        btr = true;
        ThreadPool.QueueUserWorkItem(ReceiveMsg);
        Task.Run(CheckTimeOut);
    }
   
    /// <summary>
    /// ÿһ��ʱ����������Ϣ
    /// </summary>
    private void CheckTimeOut()
    {
        Debug.Log("��ʼ������������飡����");
        while (btr)
        {
            if (frontTime != -1 && DateTime.Now.Ticks / TimeSpan.TicksPerSecond - frontTime >= TIME_OUT_TIME)
            {
                Debug.Log("������Ϣȡ�����ͻ����쳣�Ͽ�����������");
                btr = false;
                SocketServer.Instance.RemoveClient(clientID);
                break;
            }
            Thread.Sleep(500);//ÿ������һ��
        }
    }
    //�ر�
    public void Close()
    {
        if (socket != null)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void ReceiveMsg(object p)
    {
        while (btr)
        {
            try
            {
                if (socket.Available > 0)
                {
                    byte[] result = new byte[socket.Available];
                    int ReciveNum = socket.Receive(result);
                    HandleMsg(result, ReciveNum);
                }
            }
            catch (SocketException e)
            {
                Debug.Log("������Ϣ�������⣡��"+e.Message);
            }
        }
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void HandleMsg(byte[] bytes,int num)
    {
        byte[] cacheBytes = new byte[num];
        int msgID = 0;
        int nowIndex = 0;
        bytes.CopyTo(cacheBytes, nowIndex);
        msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
        nowIndex += 4;
        if (num < 4)//��Ϣ����
        {
            Debug.Log("�����ְ�������");
            return;
        }
        Debug.Log(cacheBytes.Length+"     "+ num+"     "+ nowIndex+"      "+ (cacheBytes.Length - 4));
        switch (msgID)
        {
            case 1001:
                string my = Encoding.UTF8.GetString(cacheBytes, nowIndex, cacheBytes.Length-4);
                Debug.Log($"�յ���Ϣ{my}");
                Operass(my);
                break;
            case 1002:
                //Debug.Log("�յ�������Ϣ������");
                frontTime= DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                break;
            case 1003:
                Debug.Log("�ͻ��������Ͽ�������");
                try
                {
                    int strindex = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    string time = Encoding.UTF8.GetString(cacheBytes, nowIndex, strindex);
                    Debug.Log(strindex + "     " + time);
                    Operass(time);
                    WriteJointCSV.Instance.MySCV(1, Studentname, cishu["ceyizhankai"], cishu["cebananzhuang"], cishu["zaihediaozhuang"],
                  cishu["weixingzhuanyun"], cishu["weixingdiaozhuang"],cishu["datatime"]);
                    SocketServer.Instance.RemoveClient(clientID);
                }
                catch (Exception e)
                {
                    Debug.Log("���ݴ洢ʧ�ܣ�����"+e.Message);
                }
                break;
            case 1004:
                Debug.Log("�յ���¼���������  ��ʼ����˺ź������Ƿ���ȷ������");
                HandleAccountPass(bytes,ref nowIndex);
                break;
        }
    }
    /// <summary>
    /// �����˻�������
    /// </summary>
    public void HandleAccountPass(byte[] bytes,ref int num)
    {
        int accountByteLength = BitConverter.ToInt32(bytes,num);
        num += 4;
        string account = Encoding.UTF8.GetString(bytes,num, accountByteLength);
        Debug.Log($"<color=yellow><size=15>�˺���    {account}</size></color>");
        num += accountByteLength;
        int password = BitConverter.ToInt32(bytes,num);
        num += 4;
        string password_ = Encoding.UTF8.GetString(bytes,num, password);
        Debug.Log($"<color=yellow><size=15>������    {password_}</size></color>");
        try
        {
            ReadCSV.Instance.InitInfo(account, password_, clientID);
        }
        catch (Exception e)
        {
            Debug.Log($"<color=yellow><size=15>    {e.Message}</size></color>");
        }
    }

   


    /// <summary>
    /// ��������������
    /// </summary>
    public void Operass(string str) 
    {
        string[] a = str.Split(',');
        switch (a[0])
        {
            case "xingming":
                Debug.Log(a[0] + ":" + a[1]);
                Studentname = a[1];
                break;
            case "zaihediaozhuang":
                hjhj(a[0], a[1]);
                Debug.Log(a[0] + ":" + a[1]);
                break;
            case "cebananzhuang":
                hjhj(a[0], a[1]);
                Debug.Log(a[0] + ":" + a[1]);
                break;
            case "weixingdiaozhuang":
                hjhj(a[0], a[1]);
                Debug.Log(a[0] + ":" + a[1]);
                break;
            case "weixingzhuanyun":
                hjhj(a[0], a[1]);
                Debug.Log(a[0] + ":" + a[1]);
                break;
            case "ceyizhankai":
                hjhj(a[0], a[1]);
                Debug.Log(a[0] + ":" + a[1]);
                break;
            case "datatime":
                hjhj(a[0], a[1]);
                Debug.Log(a[0]+":"+ a[1]);
                break;
        }
    }
    public void hjhj(string key,string valus)
    {
        if (!cishu.ContainsKey(key))
        {
            cishu.Add(key, valus);
            //Debug.Log("<color=red><size=15>������</size></color>");
        }
        else
        {
            cishu[key] = valus;
            //Debug.Log("<color=red><size=15>����</size></color>");
        }
    }

    
}
