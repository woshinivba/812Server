using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using UnityEngine;


public class ReadCSV : MonoBehaviour
{
    public static ReadCSV Instance;
    private bool IsExixt = false;
    private Dictionary<string, string> account_Pass = new Dictionary<string, string>();
    private Dictionary<string, string> account_Name = new Dictionary<string, string>();
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }
    public void InitInfo(string account,string password,int num)
    {
        Debug.Log("������룡����");
        FileStream stream = File.Open(Application.streamingAssetsPath + "/SourcesFolder/ѧԱ��Ϣ.xlsx", FileMode.Open, FileAccess.Read);
        Debug.Log(stream.Name);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        Debug.Log(excelReader.Name);
        DataSet result = excelReader.AsDataSet();
        Debug.Log(result.ToString());
        int columns = result.Tables[0].Columns.Count;//��ȡ����ж�����
        int rows = result.Tables[0].Rows.Count;//��ȡ����ж�����
        Debug.Log(rows + "    " + columns);//4��   3��
        for (int i = 1; i < rows; i++)
        {
            string Myaccount = result.Tables[0].Rows[i][1].ToString();//��ȡ�����ָ���е�����
            string Mypassword = result.Tables[0].Rows[i][2].ToString();
            Debug.Log("�˻�������ֱ�Ϊ��" + Myaccount + "   " + Mypassword);
            try
            {
                account_Pass.Add(Myaccount, Mypassword);
            }
            catch (System.Exception)
            {
                Debug.Log("�Ѿ����ֹ�");
            }
        }
        for (int i = 1; i < rows; i++)
        {
            string Myaccount = result.Tables[0].Rows[i][1].ToString();//��ȡ�����ָ���е�����
            string Myname = result.Tables[0].Rows[i][0].ToString();
            try
            {
                account_Name.Add(Myaccount, Myname);
            }
            catch (System.Exception)
            {
                Debug.Log("�Ѿ����ֹ�");
            }
        }
        foreach (string item in account_Pass.Keys)
        {
            if (item == account)
            {
                IsExixt=true;
                Debug.Log($"<color=red><size=15>���ڵ�ǰ�˻�  {item}</size></color>");
                if (password == account_Pass[account])
                {
                    //���������͵�½�ɹ�����
                    Debug.Log("<color=red><size=15>�˺ź�������ȷ</size></color>"+ account_Name[item]);
                    SocketServer.Instance.BackLoginMsg(num, true, account_Name[item]);
                }
                else
                {
                    Debug.Log("<color=red><size=15>���벻��ȷ</size></color>");
                    SocketServer.Instance.BackLoginMsg(num, false,"");
                    //���͵�¼ʧ������
                }
                return;
            }
        }
        if (IsExixt)
        {
            //���͵�¼ʧ������
            SocketServer.Instance.BackLoginMsg(num, false, "");
        }
    }
}
