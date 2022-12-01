using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ReadConfig 
{
    private static ReadConfig instance = new ReadConfig();
    public static ReadConfig Instance { get { return instance; } }
    public string IP;
    public int Port;
    public int listenNum;
    /// <summary>
    /// 生成一个xml文档
    /// </summary>
    public void WriteSml()
    {
        //首先  创建xml

        /*实例化xmlDocument类*/
        XmlDocument xDoc = new XmlDocument();
        //创建一个声明xml文档所需要的语法的变量
        XmlDeclaration declaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        xDoc.AppendChild(declaration);//添加到第一行

        //一个xml文档  必须要有一个根元素
        //创建根节点students
        XmlElement elem = xDoc.CreateElement("PIN_Ao");
        //把根节点添加到xml文档中
        xDoc.AppendChild(elem);
        //添加子节点的子节点
        XmlElement elem1 = xDoc.CreateElement("IP地址");
        elem.AppendChild(elem1);
        //给节点添加属性
        elem1.SetAttribute("IP", "127.0.0.1");
        elem1.SetAttribute("端口", "8888");
        
        xDoc.Save(Application.streamingAssetsPath+"/Config.xml");//将xml保存到指定的文件
    }

    //读取器xml文档
    public void ReadXml()
    {
        /*实例化xmlDocument类*/
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(Application.streamingAssetsPath + "/Config.xml");
        //获取根节点
        XmlNode noode = xDoc.SelectSingleNode("PIN_Ao");//获取节点 取到students节点
        XmlNodeList nodelist = noode.ChildNodes;
        foreach (XmlNode xn in nodelist)
        {
            //string name = xn.Name;//节点的名字
            //类型显式转换
            XmlElement xmle = (XmlElement)xn;
            IP = xmle.GetAttribute("IP");//取属性
            Port = int.Parse(xmle.GetAttribute("端口"));//取属性
            listenNum = int.Parse(xmle.GetAttribute("监听"));//取属性
        }
    }
}
