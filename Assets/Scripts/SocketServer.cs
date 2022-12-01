using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SocketServer : MonoBehaviour
{
    public Text text;
    private Socket Socket_server;//服务器端
    private IPEndPoint iPEndPoint;
    public Dictionary<int, ClientSocket> clientDic = new Dictionary<int, ClientSocket>();
    public int listen=5;//监听数量
    private bool IsConnect = true;
    public static SocketServer Instance;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        ReadConfig.Instance.ReadXml();
        Socket_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        iPEndPoint = new IPEndPoint(IPAddress.Parse(ReadConfig.Instance.IP), ReadConfig.Instance.Port);
        try
        {
            Socket_server.Bind(iPEndPoint);
            Socket_server.Listen(ReadConfig.Instance.listenNum);
            ThreadPool.QueueUserWorkItem(CanAccept);
            //ThreadPool.QueueUserWorkItem(CanReceive);
        }
        catch (System.Exception e)
        {
            Debug.Log("绑定失败！！！！"+e.Message);
        }
    }
    /// <summary>
    /// 链接成功后可以进行接收
    /// </summary>
    private void CanAccept(object o)
    {
        while (IsConnect)//持续接收
        {
            try
            {
                Socket clientSocket = Socket_server.Accept();
                ClientSocket client = new ClientSocket(clientSocket);
                client.btr = true;
                lock (clientDic)
                {
                    clientDic.Add(client.clientID, client);
                }
                Debug.Log($"客户端{clientSocket.RemoteEndPoint}链接成功！！！");
                Debug.Log("连入的客户端的ID为"+ ClientSocket.CLIENT_BEGIN_ID);
                Debug.Log("当前连接服务器的客户端数量" + clientDic.Values.Count);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("客户端连入错误：    " + e.Message);
            }
        }
    }
    private void Update()
    {
        text.text =  clientDic.Values.Count.ToString();
    }
    /// <summary>
    /// 开始接收消息
    /// </summary>
    /// <param name="o"></param>
    private void CanReceive(object o)
    {
        while (IsConnect)
        {
            if (clientDic.Count > 0)
            {
                lock (clientDic)
                {
                    foreach (ClientSocket item in clientDic.Values)
                    {
                        //item.ReceiveMsg();
                    }
                }
            }
            else
            {
                Debug.Log("当前没有客户端链接到服务端！！");
            }
        }
    }

    public void BackLoginMsg(int num,bool btr,string str)
    {
        try
        {
            if (btr) 
            {
                Debug.Log("<color=red><size=15>登陆成功，开始发送命令消息！！！！</size></color>" );
                int index = 0;
                byte[] bytes = new byte[502];
                BitConverter.GetBytes(btr).CopyTo(bytes,index);
                index += 1;
                Debug.Log(BitConverter.ToBoolean(bytes,0));
                int strNum = Encoding.UTF8.GetByteCount(str);
                BitConverter.GetBytes(strNum).CopyTo(bytes,index);
                index += 4;
                Encoding.UTF8.GetBytes(str).CopyTo(bytes, index);
                Debug.Log(Encoding.UTF8.GetString(bytes, index,strNum));
                clientDic[num].socket.Send(bytes);
            }
            else
            {
                Debug.Log("<color=red><size=15>登陆失败，开始发送命令消息！！！！</size></color>");
                int index = 0;
                byte[] bytes = new byte[502];
                BitConverter.GetBytes(btr).CopyTo(bytes, index);
                index += 1;
                Debug.Log(BitConverter.ToBoolean(bytes, 0));
                int strNum = Encoding.UTF8.GetByteCount(str);
                BitConverter.GetBytes(strNum).CopyTo(bytes, index);
                index += 4;
                Encoding.UTF8.GetBytes(str).CopyTo(bytes, index);
                Debug.Log(Encoding.UTF8.GetString(bytes, index, strNum));
                clientDic[num].socket.Send(bytes);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("发送失败！！！！" + e.Message);
        }
    }


    /// <summary>
    /// 关闭socket
    /// </summary>
    /// <param name="num"></param>
    public void RemoveClient(int num) 
    {
        clientDic[num].socket.Shutdown(SocketShutdown.Both);
        clientDic[num].socket.Close();
        clientDic.Remove(num);
        Debug.Log($"当前客户端数量为   {clientDic.Count}");
    }

    private void OnApplicationQuit()
    {
        IsConnect = false;
        //Socket_server.Shutdown(SocketShutdown.Both);
        Socket_server.Dispose();
        Socket_server.Close();
        Socket_server = null;
        BreakAllClientSocket();
    }
    /// <summary>
    /// 断开所有客户端连接
    /// </summary>
    private void BreakAllClientSocket()
    {
        if (clientDic.Count!=0)
        {
            for (int i = 0; i < clientDic.Count; i++)
            {
                clientDic.ElementAt(i).Value.socket.Shutdown(SocketShutdown.Both);
                clientDic.ElementAt(i).Value.socket.Dispose();
                clientDic.ElementAt(i).Value.socket.Close();
            }
            clientDic.Clear();
        }
    }
}
