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
    int aa;//在写入之前的行数
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
        dt = new DataTable("812学习数据");
        //OpenCSV(Application.streamingAssetsPath + "/SourcesFolder/812学习数据.csv");
        ///* MySCV(3, "7", "720", "8587", "789", "7", "88", DateTime.Now.Year+"/"+DateTime.Now.Month + "/" + DateTime.Now.Day + "--" + DateTime.Now.Hour + ":" + DateTime.Now.Min*/ute);
        #region MyRegion
        ////创建表 设置表名
        //dt = new DataTable();
        ////创建列 有三列
        //dt.Columns.Add("姓名");
        //dt.Columns.Add("侧翼展开");
        //dt.Columns.Add("侧板安装");
        //dt.Columns.Add("载荷吊装");
        //dt.Columns.Add("卫星转运");
        //dt.Columns.Add("卫星吊装");
        //dt.Columns.Add("时间");
        //MySCV(1, "7", "720", "8587", "789", "7", "7","88");
        #endregion        //MySCV();
    }

    public void mybb()
    {
        //MySCV(1, "7", "720", "8587", "789", "7", "7");
    }
    /// <summary>
    /// 保存到SNC
    /// </summary>
    /// <param name="num">存几行</param>
    /// <param name="name">姓名</param>
    /// <param name="ceyi">侧翼展开</param>
    /// <param name="ceban">侧板安装</param>
    /// <param name="zaihe">载荷吊装</param>
    /// <param name="zhuanyun">卫星转运</param>
    /// <param name="diaozhuang">卫星吊装</param>
    public void MySCV(int num, string name, string ceyi, string ceban, string zaihe, string zhuanyun, string diaozhuang, string datetime)
    {
        try
        {
            filePath = Application.streamingAssetsPath + "/SourcesFolder/812学习数据.csv";
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
            Debug.Log("存储数据出现错误！！！！！！"+e.Message);
        }
        
    }
    public DataTable OpenCSV(string filePath)//从csv读取数据返回table
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                //记录每次读取的一行记录
                string strLine = "";
                //记录每行记录中的各字段内容
                string[] aryLine = null;//存下面几行
                string[] tableHead = null;//存第一行
                                          //标示列数
                int columnCount = 0;
                //标示是否是读取的第一行
                bool IsFirst = true;
                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    DataRow dr = dt.NewRow();
                    //Debug.Log("<color=red><size=15>" + strLine + "</size></color>");
                    
                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //创建列
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
                                    //Debug.LogError("包含！！！" + columnCount + "   " + num);
                                    dt.Columns.Remove(dc);
                                }
                            }
                            catch (Exception e)
                            {
                                //Debug.Log("表中名字已经出现！！！！！"+e.Message);
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
                //Debug.Log("CSV当前行数是     " + dt.Rows.Count);
                sr.Close();
            }

        }
        return dt;
    }
    /// <summary>
    /// 检查是否与每列的名字
    /// </summary>
    public void checkNmae()
    {
        if (!dt.Columns.Contains("姓名"))
        {
            dt.Columns.Add("姓名");
            dt.Columns.Add("侧翼展开");
            dt.Columns.Add("侧板安装");
            dt.Columns.Add("载荷吊装");
            dt.Columns.Add("卫星转运");
            dt.Columns.Add("卫星吊装");
            dt.Columns.Add("时间");
            btr = false;
            //Debug.Log("<color=yellow><size=15>未包含</size></color>");
        }
        else
        {
            btr = true;
        }
    }
    public void SaveDataToCSV(int num, string name, string ceyi, string ceban, string zaihe, string zhuanyun, string diaozhuang, string datetime)
    {
        //Debug.Log("CSV原本行数是：" + dt.Rows.Count);
        for (int i = 0; i < num; i++)
        {
            dr = dt.NewRow();
            MyData(dr, name, ceyi, ceban, zaihe, zhuanyun, diaozhuang, datetime);
            //dt = new DataTable();
            dt.Rows.Add(dr);
        }
        //Debug.Log("CSV当前行数是：" + dt.Rows.Count);
    }
    public void MyData(DataRow dr, string name, string ceyi, string ceban, string zaihe, string zhuanyun, string diaozhuang,string datetime)
    {
        dr["姓名"] = name;
        dr["侧翼展开"] = ceyi;
        dr["侧板安装"] = ceban;
        dr["载荷吊装"] = zaihe;
        dr["卫星转运"] = zhuanyun;
        dr["卫星吊装"] = diaozhuang;
        dr["时间"] = datetime;
        //dr["时间"] = $"{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}";
    }

    public void SaveCSV(string CSVPath, DataTable mSheet)
    {
        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;
        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;
        //Debug.Log("行数是：" + rowCount+"    初始行数"+aa);
        //Debug.Log("列数是：" + colCount);
        //创建一个StringBuilder存储数据
        StringBuilder stringBuilder = new StringBuilder();
        //读取数据
        if (!btr)
        {
            //rowCount += 1;
            for (int i = 0; i < mSheet.Columns.Count; i++)
            {
                stringBuilder.Append(mSheet.Columns[i].ColumnName + ",");
                Debug.Log(mSheet.Columns[i].ColumnName);//打印每一列的名字
            }
            stringBuilder.Append("\r\n");
        }
        for (int i =firest; i < rowCount; i++)//aa-1的目的是除去第一行  
        {
            for (int j = 0; j < colCount; j++)
            {
                //使用","分割每一个数值
                stringBuilder.Append(mSheet.Rows[i][j] + ",");
                mSheet.Rows[i][j] = "";
            }
            //Debug.Log(stringBuilder.ToString() + "    " + i);
            //使用换行符分割每一行
            stringBuilder.Append("\r\n");
        }
        //Debug.Log(stringBuilder.ToString() + "最后行数是：" + mSheet.Rows.Count);
        using (StreamWriter textWriter = new StreamWriter(CSVPath, true, Encoding.UTF8))
        {
            textWriter.Write(stringBuilder.ToString());
            textWriter.Flush();
        }

    }
}
