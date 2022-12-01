using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

public class WriteJointCSV : MonoBehaviour
{
    string filePath;
    private DataTable dt;
    int num;
    int aa;//��д��֮ǰ������
    DataRow dr;
    public static WriteJointCSV Instance;
    bool btr = false;
    int firest = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        dt = new DataTable("812ѧϰ����");
        //OpenCSV(Application.streamingAssetsPath + "/SourcesFolder/812ѧϰ����.csv");
        ///* MySCV(3, "7", "720", "8587", "789", "7", "88", DateTime.Now.Year+"/"+DateTime.Now.Month + "/" + DateTime.Now.Day + "--" + DateTime.Now.Hour + ":" + DateTime.Now.Min*/ute);
        #region MyRegion
        ////������ ���ñ���
        //dt = new DataTable();
        ////������ ������
        //dt.Columns.Add("����");
        //dt.Columns.Add("����չ��");
        //dt.Columns.Add("��尲װ");
        //dt.Columns.Add("�غɵ�װ");
        //dt.Columns.Add("����ת��");
        //dt.Columns.Add("���ǵ�װ");
        //dt.Columns.Add("ʱ��");
        //MySCV(1, "7", "720", "8587", "789", "7", "7","88");
        #endregion        //MySCV();
    }

    public void mybb()
    {
        //MySCV(1, "7", "720", "8587", "789", "7", "7");
    }
    /// <summary>
    /// ���浽SNC
    /// </summary>
    /// <param name="num">�漸��</param>
    /// <param name="name">����</param>
    /// <param name="ceyi">����չ��</param>
    /// <param name="ceban">��尲װ</param>
    /// <param name="zaihe">�غɵ�װ</param>
    /// <param name="zhuanyun">����ת��</param>
    /// <param name="diaozhuang">���ǵ�װ</param>
    public void MySCV(int num, string name, string ceyi, string ceban, string zaihe, string zhuanyun, string diaozhuang, string datetime)
    {
        try
        {
            filePath = Application.streamingAssetsPath + "/SourcesFolder/812ѧϰ����.csv";
            OpenCSV(filePath);
            if (dt.Rows.Count == 0)
            {
                aa = 1;
                firest = aa - 1;
            }
            else
            {
                aa = dt.Rows.Count;
                firest = aa;
            }
            checkNmae();
            SaveDataToCSV(num, name, ceyi, ceban, zaihe, zhuanyun, diaozhuang, datetime);
            SaveCSV(filePath, dt);
        }
        catch (Exception e)
        {
            Debug.Log("�洢���ݳ��ִ��󣡣���������"+e.Message);
        }
        
    }
    public DataTable OpenCSV(string filePath)//��csv��ȡ���ݷ���table
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                //��¼ÿ�ζ�ȡ��һ�м�¼
                string strLine = "";
                //��¼ÿ�м�¼�еĸ��ֶ�����
                string[] aryLine = null;//�����漸��
                string[] tableHead = null;//���һ��
                                          //��ʾ����
                int columnCount = 0;
                //��ʾ�Ƿ��Ƕ�ȡ�ĵ�һ��
                bool IsFirst = true;
                //���ж�ȡCSV�е�����
                while ((strLine = sr.ReadLine()) != null)
                {
                    DataRow dr = dt.NewRow();
                    //Debug.Log("<color=red><size=15>" + strLine + "</size></color>");
                    
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //������
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            //Debug.Log(dc);
                            try
                            {
                                dt.Columns.Add(dc);
                                if (dc.ColumnName.Contains("Column"))
                                {
                                    num++;
                                    //Debug.LogError("����������" + columnCount + "   " + num);
                                    dt.Columns.Remove(dc);
                                }
                            }
                            catch (Exception e)
                            {
                                //Debug.Log("���������Ѿ����֣���������"+e.Message);
                            }
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');

                        for (int j = 0; j < columnCount - num; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        //Debug.Log(dr);
                    }
                    dt.Rows.Add(dr);
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }
                //Debug.Log("CSV��ǰ������     " + dt.Rows.Count);
                sr.Close();
            }

        }
        return dt;
    }
    /// <summary>
    /// ����Ƿ���ÿ�е�����
    /// </summary>
    public void checkNmae()
    {
        if (!dt.Columns.Contains("����"))
        {
            dt.Columns.Add("����");
            dt.Columns.Add("����չ��");
            dt.Columns.Add("��尲װ");
            dt.Columns.Add("�غɵ�װ");
            dt.Columns.Add("����ת��");
            dt.Columns.Add("���ǵ�װ");
            dt.Columns.Add("ʱ��");
            btr = false;
            //Debug.Log("<color=yellow><size=15>δ����</size></color>");
        }
        else
        {
            btr = true;
        }
    }
    public void SaveDataToCSV(int num, string name, string ceyi, string ceban, string zaihe, string zhuanyun, string diaozhuang, string datetime)
    {
        //Debug.Log("CSVԭ�������ǣ�" + dt.Rows.Count);
        for (int i = 0; i < num; i++)
        {
            dr = dt.NewRow();
            MyData(dr, name, ceyi, ceban, zaihe, zhuanyun, diaozhuang, datetime);
            //dt = new DataTable();
            dt.Rows.Add(dr);
        }
        //Debug.Log("CSV��ǰ�����ǣ�" + dt.Rows.Count);
    }
    public void MyData(DataRow dr, string name, string ceyi, string ceban, string zaihe, string zhuanyun, string diaozhuang,string datetime)
    {
        dr["����"] = name;
        dr["����չ��"] = ceyi;
        dr["��尲װ"] = ceban;
        dr["�غɵ�װ"] = zaihe;
        dr["����ת��"] = zhuanyun;
        dr["���ǵ�װ"] = diaozhuang;
        dr["ʱ��"] = datetime;
        //dr["ʱ��"] = $"{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}";
    }

    public void SaveCSV(string CSVPath, DataTable mSheet)
    {
        //�ж����ݱ����Ƿ��������
        if (mSheet.Rows.Count < 1)
            return;
        //��ȡ���ݱ�����������
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;
        //Debug.Log("�����ǣ�" + rowCount+"    ��ʼ����"+aa);
        //Debug.Log("�����ǣ�" + colCount);
        //����һ��StringBuilder�洢����
        StringBuilder stringBuilder = new StringBuilder();
        //��ȡ����
        if (!btr)
        {
            //rowCount += 1;
            for (int i = 0; i < mSheet.Columns.Count; i++)
            {
                stringBuilder.Append(mSheet.Columns[i].ColumnName + ",");
                Debug.Log(mSheet.Columns[i].ColumnName);//��ӡÿһ�е�����
            }
            stringBuilder.Append("\r\n");
        }
        for (int i =firest; i < rowCount; i++)//aa-1��Ŀ���ǳ�ȥ��һ��  
        {
            for (int j = 0; j < colCount; j++)
            {
                //ʹ��","�ָ�ÿһ����ֵ
                stringBuilder.Append(mSheet.Rows[i][j] + ",");
                mSheet.Rows[i][j] = "";
            }
            //Debug.Log(stringBuilder.ToString() + "    " + i);
            //ʹ�û��з��ָ�ÿһ��
            stringBuilder.Append("\r\n");
        }
        //Debug.Log(stringBuilder.ToString() + "��������ǣ�" + mSheet.Rows.Count);
        using (StreamWriter textWriter = new StreamWriter(CSVPath, true, Encoding.UTF8))
        {
            textWriter.Write(stringBuilder.ToString());
            textWriter.Flush();
        }

    }
}
