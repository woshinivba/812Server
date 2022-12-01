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
    /// ����һ��xml�ĵ�
    /// </summary>
    public void WriteSml()
    {
        //����  ����xml

        /*ʵ����xmlDocument��*/
        XmlDocument xDoc = new XmlDocument();
        //����һ������xml�ĵ�����Ҫ���﷨�ı���
        XmlDeclaration declaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        xDoc.AppendChild(declaration);//��ӵ���һ��

        //һ��xml�ĵ�  ����Ҫ��һ����Ԫ��
        //�������ڵ�students
        XmlElement elem = xDoc.CreateElement("PIN_Ao");
        //�Ѹ��ڵ���ӵ�xml�ĵ���
        xDoc.AppendChild(elem);
        //����ӽڵ���ӽڵ�
        XmlElement elem1 = xDoc.CreateElement("IP��ַ");
        elem.AppendChild(elem1);
        //���ڵ��������
        elem1.SetAttribute("IP", "127.0.0.1");
        elem1.SetAttribute("�˿�", "8888");
        
        xDoc.Save(Application.streamingAssetsPath+"/Config.xml");//��xml���浽ָ�����ļ�
    }

    //��ȡ��xml�ĵ�
    public void ReadXml()
    {
        /*ʵ����xmlDocument��*/
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(Application.streamingAssetsPath + "/Config.xml");
        //��ȡ���ڵ�
        XmlNode noode = xDoc.SelectSingleNode("PIN_Ao");//��ȡ�ڵ� ȡ��students�ڵ�
        XmlNodeList nodelist = noode.ChildNodes;
        foreach (XmlNode xn in nodelist)
        {
            //string name = xn.Name;//�ڵ������
            //������ʽת��
            XmlElement xmle = (XmlElement)xn;
            IP = xmle.GetAttribute("IP");//ȡ����
            Port = int.Parse(xmle.GetAttribute("�˿�"));//ȡ����
            listenNum = int.Parse(xmle.GetAttribute("����"));//ȡ����
        }
    }
}
