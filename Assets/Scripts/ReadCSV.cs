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
        Debug.Log("程序进入！！！");
        FileStream stream = File.Open(Application.streamingAssetsPath + "/SourcesFolder/学员信息.xlsx", FileMode.Open, FileAccess.Read);
        Debug.Log(stream.Name);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        Debug.Log(excelReader.Name);
        DataSet result = excelReader.AsDataSet();
        Debug.Log(result.ToString());
        int columns = result.Tables[0].Columns.Count;//获取表格有多少列
        int rows = result.Tables[0].Rows.Count;//获取表格有多少行
        Debug.Log(rows + "    " + columns);//4行   3列
        for (int i = 1; i < rows; i++)
        {
            string Myaccount = result.Tables[0].Rows[i][1].ToString();//获取表格中指定列的数据
            string Mypassword = result.Tables[0].Rows[i][2].ToString();
            Debug.Log("账户和密码分别为：" + Myaccount + "   " + Mypassword);
            try
            {
                account_Pass.Add(Myaccount, Mypassword);
            }
            catch (System.Exception)
            {
                Debug.Log("已经出现过");
            }
        }
        for (int i = 1; i < rows; i++)
        {
            string Myaccount = result.Tables[0].Rows[i][1].ToString();//获取表格中指定列的数据
            string Myname = result.Tables[0].Rows[i][0].ToString();
            try
            {
                account_Name.Add(Myaccount, Myname);
            }
            catch (System.Exception)
            {
                Debug.Log("已经出现过");
            }
        }
        foreach (string item in account_Pass.Keys)
        {
            if (item == account)
            {
                IsExixt=true;
                Debug.Log($"<color=red><size=15>存在当前账户  {item}</size></color>");
                if (password == account_Pass[account])
                {
                    //发送姓名和登陆成功命令
                    Debug.Log("<color=red><size=15>账号和密码正确</size></color>"+ account_Name[item]);
                    SocketServer.Instance.BackLoginMsg(num, true, account_Name[item]);
                }
                else
                {
                    Debug.Log("<color=red><size=15>密码不正确</size></color>");
                    SocketServer.Instance.BackLoginMsg(num, false,"");
                    //发送登录失败命令
                }
                return;
            }
        }
        if (IsExixt)
        {
            //发送登录失败命令
            SocketServer.Instance.BackLoginMsg(num, false, "");
        }
    }
}
