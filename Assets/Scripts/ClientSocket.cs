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
    private int cacheNum = 0;//接收到的字节数 
    private long frontTime = -1;
    //dynamic cishu;
    private Dictionary<string, string> cishu = new Dictionary<string, string>();
    private string Studentname;
    public static int CLIENT_BEGIN_ID = 1;
    private static int TIME_OUT_TIME = 10;//超时时间
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
    /// 每一段时间检测心跳消息
    /// </summary>
    private void CheckTimeOut()
    {
        Debug.Log("开始进行心跳检测检查！！！");
        while (btr)
        {
            if (frontTime != -1 && DateTime.Now.Ticks / TimeSpan.TicksPerSecond - frontTime >= TIME_OUT_TIME)
            {
                Debug.Log("心跳消息取消，客户端异常断开！！！！！");
                btr = false;
                SocketServer.Instance.RemoveClient(clientID);
                break;
            }
            Thread.Sleep(500);//每半秒检测一次
        }
    }
    //关闭
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
    /// 接收消息
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
                Debug.Log("接收消息出现问题！！"+e.Message);
            }
        }
    }
    /// <summary>
    /// 处理消息
    /// </summary>
    public void HandleMsg(byte[] bytes,int num)
    {
        byte[] cacheBytes = new byte[num];
        int msgID = 0;
        int nowIndex = 0;
        bytes.CopyTo(cacheBytes, nowIndex);
        msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
        nowIndex += 4;
        if (num < 4)//消息正常
        {
            Debug.Log("产生分包！！！");
            return;
        }
        Debug.Log(cacheBytes.Length+"     "+ num+"     "+ nowIndex+"      "+ (cacheBytes.Length - 4));
        switch (msgID)
        {
            case 1001:
                string my = Encoding.UTF8.GetString(cacheBytes, nowIndex, cacheBytes.Length-4);
                Debug.Log($"收到消息{my}");
                Operass(my);
                break;
            case 1002:
                //Debug.Log("收到心跳消息！！！");
                frontTime= DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                break;
            case 1003:
                Debug.Log("客户端主动断开！！！");
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
                    Debug.Log("数据存储失败！！！"+e.Message);
                }
                break;
            case 1004:
                Debug.Log("收到登录请求命令！！  开始检测账号和密码是否正确！！！");
                HandleAccountPass(bytes,ref nowIndex);
                break;
        }
    }
    /// <summary>
    /// 处理账户和密码
    /// </summary>
    public void HandleAccountPass(byte[] bytes,ref int num)
    {
        int accountByteLength = BitConverter.ToInt32(bytes,num);
        num += 4;
        string account = Encoding.UTF8.GetString(bytes,num, accountByteLength);
        Debug.Log($"<color=yellow><size=15>账号是    {account}</size></color>");
        num += accountByteLength;
        int password = BitConverter.ToInt32(bytes,num);
        num += 4;
        string password_ = Encoding.UTF8.GetString(bytes,num, password);
        Debug.Log($"<color=yellow><size=15>密码是    {password_}</size></color>");
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
    /// 处理传过来的数据
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
            //Debug.Log("<color=red><size=15>不存在</size></color>");
        }
        else
        {
            cishu[key] = valus;
            //Debug.Log("<color=red><size=15>存在</size></color>");
        }
    }

    
}
