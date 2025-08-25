#define NEW
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database_Link;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
namespace DataBaseTest
{
    public partial class Form1 : Form
    {
        Database database;
        Database database2;
        public Form1()
        {
            //  System.Environment.SetEnvironmentVariable("NLS_LANG", "TRADITIONAL CHINESE_TAIWAN.ZHT16MSWIN950");
            //    System.Environment.SetEnvironmentVariable("NLS_LANG", "TRADITIONAL CHINESE_TAIWAN.ZHT16BIG5");
            //_putenv("NLS_LANG = AMERICAN_AMERICA.US7ASCII");
            InitializeComponent();
            textBox1.Text = /*"Driver={MySQL ODBC 5.1 Driver};*/"Server=192.168.166.35;Port=3306;User=sc;Password=Aa3566528;convert zero datetime=True";
          //   textBox1.Text = "Server = 192.168.166.42; Uid = sa; Pwd = 1qaz2wsx#;database=SCHTgsql;";
         //    textBox1.Text = "Server = 192.168.143.105; Uid = tiger; Pwd =tiger;";
           // textBox1.Text = @"Data Source=.\DB01;Initial Catalog=HSPC;Integrated Security=False;Persist Security Info=True;User ID=SHC;Password=smartcare123;Connection Lifetime=3600;";
            //     textBox1.Text = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.143.109)(PORT=1521))(CONNECT_DATA=(SID = MSD2)));User Id=LAB;Password=LAB000;";
            //  Driver={MySQL ODBC 5.2 ANSI Driver};Server=myServerAddress;Port=3306;Database=myDataBase;User=myUsername;Password=myPassword;
            //   textBox1.Text = "Driver={MySQL ODBC 5.1 Driver}; Data Source = 192.168.166.35; User ID = sc; Password = Aa3566528; Initial Catalog = scdb;";// "Provider=sqloledb;Server=HOWARDH;Database=master;Uid=sa;Pwd=0000;";
            // Provider=MySQL; Data Source=MySQLServerIP; User ID =MyID; Password=MyPassword; Initial Catalog=DatabaseName;
          //  System.Environment.SetEnvironmentVariable("NLS_LANG", "TRADITIONAL CHINESE_TAIWAN.US7ASCII");
            //    System.Environment.SetEnvironmentVariable("NLS_LANG", "TRADITIONAL CHINESE_TAIWAN.ZHT16BIG5");
            /*  database = new SQL();
              string[] Column = { "name", "Idmax" };
              string[] Value = { "asd", "1" };
                 DatabaseType[] type={DatabaseType.Str,DatabaseType.Int};
                 string[] Search_Column = { "Idmax" };
                 string[] Search_Value = { "1" };
                 DatabaseType[] Search_type = { DatabaseType.Int };
                 try
                 {
                  //   database.Insert("Table_1", Column, Value, type);
                 }
              catch(Exception ex)
                 {
                     MessageBox.Show(ex.ToString());
                 }

            database.Updata("Table_1",Search_Column,Search_Value,Search_type, Column, Value, type);
              label1.Text="";
              string aa = string.Empty;
              if (aa == "")
                  label1.Text += "ccc";
               if(aa==null)
                  label1.Text+="null";
               if (aa == string.Empty)
                   label1.Text += "Empty";*/



            //label1.Text = aa.Tables[0].Rows.Count.ToString();


            if (!Directory.Exists(@"C:\LOG"))
            {
                Directory.CreateDirectory(@"C:\LOG");
            }
            using (StreamWriter sw = new StreamWriter(@"C:\LOG\L2H_" + ".log", true))
            {
                string SW = "連接失敗";
                sw.WriteLine(SW);


            }
           

          
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
             
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            string Conntect = @"";
            Conntect += textBox1.Text;
            ((RadioButton)sender).Checked = true;
            try
            {
                switch (((RadioButton)sender).Text)
                {
                    case "SQL":
                        database = new SQL(Conntect);
                        break;
                    case "OLEDB":
                        database = new OLEDB(Conntect);


                        break;
                    case "Oracle":

                        database = new Oracle(Conntect);
                        break;
                    case "ODBC":

                        database = new ODBC(Conntect);
                        break;
                    case "MYSQL":

                        database = new MYSQL(Conntect);
                        break;
                }
                MessageBox.Show("Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            timer1.Enabled = false;
          textBox2.Text = "SELECT * FROM scdb.scds_BP";
           // textBox2.Text = "SELECT * FROM TGSQL_DB_201801101429.dbo.血壓量測結果資料檔;";
            /*     textBox2.Text = "select fin.病歷號碼,fix.姓名 as '醫師姓名',fin.生日 as '出生日期',fin.處置索引1,fin.處置名稱,fin.單價,"+
                                 "fin.報告內容,fin.開單日期,fin.開單時間,fin.報到日期,fin.報到時間,fin.檢查日期,fin.檢查時間,"+
     "fin.完成日期,fin.完成時間,fin.報告師代號,fqx.姓名 as '報告醫師姓名',fin.報告台代碼,"+
     "fvx.代碼內容 as '報告台代碼名稱',fin.處置種類代碼,fux.代碼內容 as '處置種類代碼名稱',"+
     "'1532061065' as 醫事機構碼,'大園敏盛醫院' as 醫事機構名稱,fin.申請流水號,fin.處置檔_counter,fin.同處置_counter"+
     " from "+

     "(select fa.生日, fa.代碼內容, '門診' AS RTYPE, fa.病歷號碼, fa.結束日, fa.就診序號, fa.處置代號, fa.處置索引1,"+
     "fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期,"+
     "fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter,"+
     "fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價 "+
     "from "+
     "(select c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.就診日, a.結束日, a.就診序號, a.醫師代號,"+
           " b.處置代號, b.劑量, b.已執行, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter,"+
           " b.處置檔_counter, b.單價"+
     " from 門診檔 a, 門診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e"+
     " where a.病患檔_Counter = c.Counter and"+
          " b.處置檔_counter = d.Counter AND"+
          " b.門診檔_Counter = a.Counter AND"+
         "  a.科別代碼 = e.代碼 AND"+
         "  b.處置種類代碼 = '2' and"+
         "  d.處置索引1 is not null and"+
         "  b.結果檔_counter > 0 and"+
          " c.病歷號碼 <> '9999999999'"+
     ")fa,"+
     " (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫','確','報') and e.完成日期 between '1060701' and '1060731')fb"+
     " where fa.結果檔_counter = fb.counter" +
     ")fin,"+
     " (SELECT * FROM dbo.人事資料檔 x)fix,"+
     " (SELECT * FROM dbo.人事資料檔 q)fqx,"+
     " (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx," +
     " (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux" +
      " WHERE fin.醫師代號 = fix.人事代號" +
     " and fin.報告師代號 = fqx.人事代號" +
     " and fin.報告台代碼 = fvx.代碼" +
     " and fin.處置種類代碼 = fux.代碼" +
     " order by fin.病歷號碼";*/

            //        textBox2.Text = "SELECT * FROM scdb.scds_BP where userKey not like '%-%' and userKey <> 'IDEMPTY' and userKey <> '' and deviceType <> 'I';";
        }
        [DllImport("msvcrt.dll")]
        private static extern int _putenv(string str);
        DataSet dataset;
        private void button1_Click(object sender, EventArgs e)
        {
            //   _putenv("NLS_LANG = CHINESE_CHINA.ZHS16GBK");
            // database.test();
            //   System.Environment.SetEnvironmentVariable("NLS_LANG", "TRADITIONAL CHINESE_TAIWAN.ZHT16MSWIN950");
            // System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            dataset = database.Custom_DataSet("Table_name", textBox2.Text);
            //  _putenv("NLS_LANG = SIMPLIFIED CHINESE_CHINA.ZHS16GBK");

            // System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
            try
            {
                dataGridView1.DataSource = dataset;
                //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                //  System.Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.US7ASCII");
                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();
                // string temp = Encoding.Default.GetString(Encoding.GetEncoding("gb2312").GetBytes(aa));
                //  for (int i = 0; i < temp.Length; i++)
                //      aa += aa[i] & temp[i];

                //   string bb = aa;
            }
            catch
            {
                ;
            }

            /* foreach (var e1 in Encoding.GetEncodings())
             {
                 foreach (var e2 in Encoding.GetEncodings())
                 {
                     byte[] unknow = Encoding.GetEncoding(e1.CodePage).GetBytes(source);
                     string result = Encoding.GetEncoding(e2.CodePage).GetString(unknow);
                     sb.AppendLine(string.Format("{0} => {1} : {2}", e1.CodePage, e2.CodePage, result));
                 }
             }
             File.WriteAllText("test.txt", sb.ToString());*/

        }
        int aa = 0;
        int item = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;

            string user_no = "";
            string Cmd = "update scdb.scds_BP set userKey='' user_no='" + user_no + "' where userKey='" + user_no + "'";
            string Table = "scdb.scds_BP";
            string[] Column = { "address", "checkDate", "deviceType", "dia", "pul", "sortDate", "sys", "updateDate", "userKey", "user_no" };

            string[] Value = { "testaddress", "2010-01-01 00:00:00", "M", "63", "75", "1262275200000", "121", "2017-12-12 14:23:07", "", "SCtest" };
            DatabaseSQL aa = new DatabaseSQL();

            DatabaseType[] type = { DatabaseType.Str, DatabaseType.DateTime, DatabaseType.Str, DatabaseType.Int, DatabaseType.Int, DatabaseType.Str, DatabaseType.Int, DatabaseType.DateTime, DatabaseType.Str, DatabaseType.Str };
            try
            {
                database.Insert(Table, Column, Value, type);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        DataSet Compare_1;
        DataSet Compare_2;
        private void button3_Click(object sender, EventArgs e)
        {
            List<string[]> BPlistCSV = CSVFileList("C:\\csv\\Insert.csv");
           
            
            string[] column = { "user_no", "userKey", "checkDate", "deviceType", "measureValue", "updateDate", "address" };
         
            DatabaseType[] type = { DatabaseType.Str, DatabaseType.Str, DatabaseType.DateTime, DatabaseType.Str, DatabaseType.Str, DatabaseType.DateTime, DatabaseType.Str };
            /*  string[] Search_value = { BPlistCSV[1][0] };
              string[] Modify_Value = { BPlistCSV[1][12] };
              database.Updata("scdb.scds_BP", Search_Column, Search_value, Search_type, Modify_Column, Modify_Value, Modify_type);*/


            foreach (string[] data in BPlistCSV)
            {
                if (data.Length == 10)
                {
                    DateTime NewDate = DateTime.Parse(data[8]);//, "yyyy/MM/dd HH:mm:ss", null);
                    string AA = NewDate.ToString("yyyyMMddhhmmss");
                    string[] value = {data[0], data[1],
                 DateTime.Parse(data[8]).ToString("yyyy-MM-dd HH:mm:ss")
                       , data[2],data[6],
                     DateTime.Parse(data[9]).ToString("yyyy-MM-dd HH:mm:ss") 
                        ,data[7] };
                    database.Insert("scdb.scds_BT",column,value,type );
                        }
            }
        }
        static List<string[]> CSVFileList(string CSVfile)
        {
            List<string[]> list = new List<string[]>();
            using (StreamReader SR = new StreamReader(CSVfile, Encoding.UTF8, true))
            {
                string srl;

                while ((srl = SR.ReadLine()) != null)
                {
                    string[] CSVarray = srl.Split(',');
                    list.Add(CSVarray);
                }

            }
            list.RemoveAt(0);
            return list;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string 院所檔SQL = "select a.counter as '院所檔_counter',a.院所名稱,a.醫院等級代碼,a.院所科別代碼,a.院所負責人,a.院所代號," +
"a.院所地址,a.院所電話,a.醫院類別代碼,a.居家照護代號,a.居家照護負責人,a.院所郵遞區號,a.傳真號碼,a.電子信箱," +
"a.分機號碼,a.英文院所名稱,a.英文院所地址" +
" from dbo.院所資料檔 a";// +
                    //" where a.院所代號 = '1532061065'";




            dataset = database.Custom_DataSet("Table_name", 院所檔SQL);



            try
            {
                dataGridView1.DataSource = dataset;

                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();

            }
            catch
            {
                ;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string 病患檔SQL = "select a.counter as '病患檔_counter',a.病歷號碼,a.身份證字號,a.姓名 as '病患姓名',a.生日,a.婚姻代碼," +
"a.性別代碼,a.血型代碼,a.身高,a.體重,a.電話H,a.電話O,a.過敏,a.地址,a.初診日期,a.郵地區號,a.死亡日期," +
"a.重大傷病證號,a.重大傷病效期,a.重病卡生效日,a.重病卡備註,a.國籍,a.緊急聯絡人姓名,a.緊急聯絡人電話," +
"a.安寧代碼,'1532061065' as 醫事機構碼,'大園敏盛醫院' as 醫事機構名稱" +
" from dbo.病患檔 a" +
" where a.counter = '2568'";// +
                            //" and a.病歷號碼 = '0000005107'";




            dataset = database.Custom_DataSet("Table_name", 病患檔SQL);



            try
            {
                dataGridView1.DataSource = dataset;

                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();

            }
            catch
            {
                ;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
#if NEW
            string 放射科報告名單SQL_門 = "select distinct fib.counter,fib.病歷號碼,fib.身份證字號,fib.病患姓名,fib.生日,'1532061065' as 醫事機構碼,'大園敏盛醫院' as 醫事機構名稱" +
" from" +
" (select  fin.counter, fin.病歷號碼, fin.身份證字號, fin.病患姓名, fin.生日, fin.報告師代號" +
" from" +
" (select fa.counter, fa.生日, fa.代碼內容, fa.病歷號碼, fa.結束日, fa.就診序號, fa.處置代號, fa.處置索引1," +
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期," +
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter," +
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.姓名 as '病患姓名', fa.身份證字號," +
" CASE" +
" WHEN RTRIM(fb.來源代碼) IN(0) THEN '門急診'" +
" WHEN RTRIM(fb.來源代碼) IN(1) THEN '門診'" +
" WHEN RTRIM(fb.來源代碼) IN(2) THEN '急診'" +
" WHEN RTRIM(fb.來源代碼) IN(3) THEN '住診'" +
" WHEN RTRIM(fb.來源代碼) IN(4) THEN '預約'" +
" WHEN RTRIM(fb.來源代碼) IN(5) THEN '體檢'" +
" WHEN RTRIM(fb.來源代碼) IN(6) THEN '門診洗腎'" +
" WHEN RTRIM(fb.來源代碼) IN(7) THEN '外來代檢'" +
" ELSE 'NA'" +
" END AS  '來源'" +
" from" +
" (select c.counter, c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.就診日, a.結束日, a.就診序號, a.醫師代號," +
      " b.處置代號, b.劑量, b.已執行, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter, c.身份證字號," +
     "  b.處置檔_counter, b.單價" +
" from 門診檔 a, 門診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e" +
" where a.病患檔_Counter = c.Counter and" +
"      b.處置檔_counter = d.Counter AND" +
"      b.門診檔_Counter = a.Counter AND" +
"      a.科別代碼 = e.代碼 AND" +
"      b.處置種類代碼 = '2' and" +
"      d.處置索引1 is not null and" +
"      b.結果檔_counter > 0 and" +
"      c.病歷號碼 <> '9999999999'" +
")fa," +
" (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb" +
"  where fa.結果檔_counter = fb.counter" +
")fin," +
" (SELECT * FROM dbo.人事資料檔 x)fix," +
" (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx," +
" (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux" +
"  WHERE fin.醫師代號 = fix.人事代號" +
" and fin.報告台代碼 = fvx.代碼" +
" and fin.處置種類代碼 = fux.代碼 )fib" +
"  left join dbo.人事資料檔 q on fib.報告師代號 = q.人事代號" +
//"  --where fib.counter = '5094'"+
" order by fib.病歷號碼";
#else
            string 放射科報告名單SQL_門 = "select distinct fib.counter,fib.病歷號碼,fib.身份證字號,fib.病患姓名,fib.生日"+
                                           " from "+
                                          "(select  fin.counter, fin.病歷號碼, fin.身份證字號, fin.病患姓名, fin.生日, fin.報告師代號"+
                                           " from "+
                                         " (select fa.counter, fa.生日, fa.代碼內容, fa.病歷號碼, fa.結束日, fa.就診序號, fa.處置代號, fa.處置索引1,"+
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期,"+
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter,"+
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.姓名 as '病患姓名', fa.身份證字號,"+
" CASE" +
" WHEN RTRIM(fb.來源代碼) IN(0) THEN '門急診'"+
" WHEN RTRIM(fb.來源代碼) IN(1) THEN '門診'"+
" WHEN RTRIM(fb.來源代碼) IN(2) THEN '急診'"+
" WHEN RTRIM(fb.來源代碼) IN(3) THEN '住診'"+
" WHEN RTRIM(fb.來源代碼) IN(4) THEN '預約'"+
" WHEN RTRIM(fb.來源代碼) IN(5) THEN '體檢'"+
" WHEN RTRIM(fb.來源代碼) IN(6) THEN '門診洗腎'"+
" WHEN RTRIM(fb.來源代碼) IN(7) THEN '外來代檢'"+
" ELSE 'NA'"+
" END AS  '來源'"+
" from "+
"(select c.counter, c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.就診日, a.結束日, a.就診序號, a.醫師代號,"+
       "b.處置代號, b.劑量, b.已執行, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter, c.身份證字號,"+
      "b.處置檔_counter, b.單價"+
 " from 門診檔 a, 門診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e"+
" where a.病患檔_Counter = c.Counter and "+
     " b.處置檔_counter = d.Counter AND "+
     " b.門診檔_Counter = a.Counter AND"+
   "   a.科別代碼 = e.代碼 AND"+
    "  b.處置種類代碼 = '2' and"+
   "   d.處置索引1 is not null and"+
    "  b.結果檔_counter > 0 and"+
     " c.病歷號碼 <> '9999999999'"+
")fa,"+
"(SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb"+
"  where fa.結果檔_counter = fb.counter"+
")fin,"+
"(SELECT * FROM dbo.人事資料檔 x)fix,"+
"(SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx,"+
"(SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux"+
 " WHERE fin.醫師代號 = fix.人事代號"+
" and fin.報告台代碼 = fvx.代碼"+
" and fin.處置種類代碼 = fux.代碼 )fib"+
 " left join dbo.人事資料檔 q on fib.報告師代號 = q.人事代號"+
//  " where fib.counter = '5094'"+
" order by fib.病歷號碼";
#endif



            dataset = database.Custom_DataSet("Table_name", 放射科報告名單SQL_門);



            try
            {
                dataGridView1.DataSource = dataset;

                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();

            }
            catch
            {
                ;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
#if NEW
            string 放射科報告資料SQL_門SQL = "select fib.病歷號碼,fib.身份證字號,fib.醫師姓名,fib.生日,fib.處置索引1,fib.處置名稱," +
"fib.報告內容,fib.開單日期,fib.開單時間,fib.報到日期,fib.報到時間,fib.檢查日期,fib.檢查時間," +
"fib.完成日期,fib.完成時間,fib.報告師代號," +
"fqx.姓名 as '報告醫師姓名'," +
"fib.報告台代碼," +
"fib.報告台代碼名稱,fib.處置種類代碼,fib.處置種類代碼名稱,fib.醫事機構碼,fib.醫事機構名稱," +
"fib.申請流水號,fib.處置檔_counter,fib.同處置_counter,fib.來源,fib.counter AS '結果檔_counter'" +
" from" +
" (select fin.病歷號碼, fix.姓名 as '醫師姓名', fin.生日, fin.處置索引1, fin.處置名稱, fin.單價," +
"fin.報告內容, fin.開單日期, fin.開單時間, fin.報到日期, fin.報到時間, fin.檢查日期, fin.檢查時間," +
"fin.完成日期, fin.完成時間, fin.報告師代號, fin.報告台代碼," +
"fvx.代碼內容 as '報告台代碼名稱', fin.處置種類代碼, fux.代碼內容 as '處置種類代碼名稱'," +
"'1532061065' as 醫事機構碼, '大園敏盛醫院' as 醫事機構名稱, fin.申請流水號, fin.處置檔_counter, fin.同處置_counter," +
"fin.來源, fin.身份證字號, fin.counter from" +
" (select fa.生日, fa.代碼內容, fa.病歷號碼, fa.結束日, fa.就診序號, fa.處置代號, fa.處置索引1," +
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期," +
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter," +
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.身份證字號, '門診' AS  '來源', fb.counter" +
" from" +
" (select c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.就診日, a.結束日, a.就診序號, a.醫師代號," +
"       b.處置代號, b.劑量, b.已執行, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter," +
"       b.處置檔_counter, b.單價, c.身份證字號, b.counter, b.申請流水號" +
" from 門診檔 a, 門診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e" +
" where a.病患檔_Counter = c.Counter and" +
"      b.處置檔_counter = d.Counter AND" +
"      b.門診檔_Counter = a.Counter AND" +
"      a.科別代碼 = e.代碼 AND" +
"      b.處置種類代碼 = '2' and" +
"      d.處置索引1 is not null and" +
"      b.結果檔_counter > 0 and" +
"      c.病歷號碼 <> '9999999999'" +
")fa," +
" (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb" +
"  where fa.結果檔_counter = fb.counter" +
" and fa.counter = fb.來源檔_counter" +
")fin," +
" (SELECT * FROM dbo.人事資料檔 x)fix," +
" (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx," +
" (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux" +
"  WHERE fin.醫師代號 = fix.人事代號" +
" and fin.報告台代碼 = fvx.代碼" +
" and fin.處置種類代碼 = fux.代碼)fib" +
"  left join dbo.人事資料檔 fqx on fib.報告師代號 = fqx.人事代號" +
" order by fib.病歷號碼";
#else

            string 放射科報告資料SQL_門SQL = "select fib.病歷號碼,fib.醫師姓名,fib.生日,fib.處置索引1,fib.處置名稱,fib.單價," +
"fib.報告內容,fib.開單日期,fib.開單時間,fib.報到日期,fib.報到時間,fib.檢查日期,fib.檢查時間," +
"fib.完成日期,fib.完成時間,fib.報告師代號," +
"fqx.姓名 as '報告醫師姓名'," +
"fib.報告台代碼," +
"fib.報告台代碼名稱,fib.處置種類代碼,fib.處置種類代碼名稱,fib.醫事機構碼,fib.醫事機構名稱," +
"fib.申請流水號,fib.處置檔_counter,fib.同處置_counter,fib.來源" +
" from " +
"(select fin.病歷號碼, fix.姓名 as '醫師姓名', fin.生日, fin.處置索引1, fin.處置名稱, fin.單價," +
"fin.報告內容, fin.開單日期, fin.開單時間, fin.報到日期, fin.報到時間, fin.檢查日期, fin.檢查時間," +
"fin.完成日期, fin.完成時間, fin.報告師代號, fin.報告台代碼," +
"fvx.代碼內容 as '報告台代碼名稱', fin.處置種類代碼, fux.代碼內容 as '處置種類代碼名稱'," +
"'1532061065' as 醫事機構碼, '大園敏盛醫院' as 醫事機構名稱, fin.申請流水號, fin.處置檔_counter, fin.同處置_counter," +
"fin.來源 from " +
"(select fa.生日, fa.代碼內容, fa.病歷號碼, fa.結束日, fa.就診序號, fa.處置代號, fa.處置索引1," +
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期," +
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter," +
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價," +
" CASE " +
" WHEN RTRIM(fb.來源代碼) IN(0) THEN '門急診'" +
" WHEN RTRIM(fb.來源代碼) IN(1) THEN '門診'" +
" WHEN RTRIM(fb.來源代碼) IN(2) THEN '急診'" +
" WHEN RTRIM(fb.來源代碼) IN(3) THEN '住診'" +
" WHEN RTRIM(fb.來源代碼) IN(4) THEN '預約'" +
" WHEN RTRIM(fb.來源代碼) IN(5) THEN '體檢'" +
" WHEN RTRIM(fb.來源代碼) IN(6) THEN '門診洗腎'" +
" WHEN RTRIM(fb.來源代碼) IN(7) THEN '外來代檢'" +
" ELSE 'NA'" +
" END AS  '來源'" +
" from " +
"(select c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.就診日, a.結束日, a.就診序號, a.醫師代號," +
"       b.處置代號, b.劑量, b.已執行, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter," +
"         b.處置檔_counter, b.單價" +
" from 門診檔 a, 門診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e" +
" where a.病患檔_Counter = c.Counter and" +
"      b.處置檔_counter = d.Counter AND" +
 "     b.門診檔_Counter = a.Counter AND" +
"      a.科別代碼 = e.代碼 AND" +
"      b.處置種類代碼 = '2' and" +
 "     d.處置索引1 is not null and" +
  "    b.結果檔_counter > 0 and" +
  "    c.病歷號碼 <> '9999999999'" +
")fa," +
"(SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb" +
"  where fa.結果檔_counter = fb.counter" +
")fin," +
"(SELECT * FROM dbo.人事資料檔 x)fix," +
"(SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx," +
"(SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux" +
"  WHERE fin.醫師代號 = fix.人事代號" +
" and fin.報告台代碼 = fvx.代碼" +
" and fin.處置種類代碼 = fux.代碼)fib" +
 " left join dbo.人事資料檔 fqx on fib.報告師代號 = fqx.人事代號" +
" order by fib.病歷號碼";





#endif


            dataset = database.Custom_DataSet("Table_name", 放射科報告資料SQL_門SQL);



            try
            {
                dataGridView1.DataSource = dataset;

                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();
                Compare_1 = dataset;

            }
            catch
            {
                ;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
#if NEW
            string 放射科報告名單SQL_住 = "select distinct fib.counter,fib.病歷號碼,fib.身份證字號,fib.病患姓名,fib.生日,'1532061065' as 醫事機構碼,'大園敏盛醫院' as 醫事機構名稱" +
" from" +
" (select fin.counter, fin.病歷號碼, fin.身份證字號, fin.病患姓名, fin.生日, fin.報告師代號" +
" from" +
" (select fa.counter, fa.生日, fa.代碼內容, fa.病歷號碼, fa.處置代號, fa.處置索引1," +
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.主治醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期," +
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter," +
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.姓名 as '病患姓名', fa.身份證字號," +
" CASE" +
" WHEN RTRIM(fb.來源代碼) IN(0) THEN '門急診'" +
" WHEN RTRIM(fb.來源代碼) IN(1) THEN '門診'" +
" WHEN RTRIM(fb.來源代碼) IN(2) THEN '急診'" +
" WHEN RTRIM(fb.來源代碼) IN(3) THEN '住診'" +
" WHEN RTRIM(fb.來源代碼) IN(4) THEN '預約'" +
" WHEN RTRIM(fb.來源代碼) IN(5) THEN '體檢'" +
" WHEN RTRIM(fb.來源代碼) IN(6) THEN '門診洗腎'" +
" WHEN RTRIM(fb.來源代碼) IN(7) THEN '外來代檢'" +
" ELSE 'NA'" +
" END AS  '來源'" +

" from" +
" (select c.counter, c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.入院日期, a.實際出院日, a.主治醫師代號," +
"       b.處置代號, b.總量, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter, c.身份證字號," +
"       b.處置檔_counter, b.單價" +
" from 住診檔 a, 住診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e" +
" where a.病患檔_Counter = c.Counter and" +
"      b.處置檔_counter = d.Counter AND" +
"      b.住診檔_Counter = a.Counter AND" +
"      a.目前科別代碼 = e.代碼 AND" +
"      b.處置種類代碼 = '2' and" +
"      d.處置索引1 is not null and" +
"      b.結果檔_counter > 0 and" +
"      c.病歷號碼 <> '9999999999'" +
")fa," +
" (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb" +
"  where fa.結果檔_counter = fb.counter" +
")fin," +
" (SELECT * FROM dbo.人事資料檔 x)fix," +
" (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx," +
" (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux" +
"  WHERE fin.主治醫師代號 = fix.人事代號" +
" and fin.報告台代碼 = fvx.代碼" +
" and fin.處置種類代碼 = fux.代碼)fib" +
"  left join dbo.人事資料檔 q on fib.報告師代號 = q.人事代號" +
" order by fib.病歷號碼";
#else
          string 放射科報告名單SQL_住 =   "select distinct fib.counter,fib.病歷號碼,fib.身份證字號,fib.病患姓名,fib.生日"+
" from"+
" (select fin.counter, fin.病歷號碼, fin.身份證字號, fin.病患姓名, fin.生日, fin.報告師代號"+
" from"+
" (select fa.counter, fa.生日, fa.代碼內容, fa.病歷號碼, fa.處置代號, fa.處置索引1,"+
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.主治醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期,"+
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter,"+
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.姓名 as '病患姓名', fa.身份證字號,"+
" CASE"+
" WHEN RTRIM(fb.來源代碼) IN(0) THEN '門急診'"+
" WHEN RTRIM(fb.來源代碼) IN(1) THEN '門診'"+
" WHEN RTRIM(fb.來源代碼) IN(2) THEN '急診'"+
" WHEN RTRIM(fb.來源代碼) IN(3) THEN '住診'"+
" WHEN RTRIM(fb.來源代碼) IN(4) THEN '預約'"+
" WHEN RTRIM(fb.來源代碼) IN(5) THEN '體檢'"+
" WHEN RTRIM(fb.來源代碼) IN(6) THEN '門診洗腎'"+
" WHEN RTRIM(fb.來源代碼) IN(7) THEN '外來代檢'"+
" ELSE 'NA'"+
" END AS  '來源'"+

" from"+
" (select c.counter, c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.入院日期, a.實際出院日, a.主治醫師代號,"+
      " b.處置代號, b.總量, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter, c.身份證字號,"+
      " b.處置檔_counter, b.單價"+
" from 住診檔 a, 住診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e"+
" where a.病患檔_Counter = c.Counter and"+
     " b.處置檔_counter = d.Counter AND"+
     " b.住診檔_Counter = a.Counter AND"+
     " a.目前科別代碼 = e.代碼 AND"+
     " b.處置種類代碼 = '2' and"+
     " d.處置索引1 is not null and"+
     " b.結果檔_counter > 0 and"+
     " c.病歷號碼 <> '9999999999'"+
")fa,"+
" (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb"+
 " where fa.結果檔_counter = fb.counter"+
")fin,"+
" (SELECT * FROM dbo.人事資料檔 x)fix,"+
" (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx,"+
" (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux"+
"  WHERE fin.主治醫師代號 = fix.人事代號"+
" and fin.報告台代碼 = fvx.代碼"+
" and fin.處置種類代碼 = fux.代碼)fib"+
 " left join dbo.人事資料檔 q on fib.報告師代號 = q.人事代號"+
" order by fib.病歷號碼";
#endif




            dataset = database.Custom_DataSet("Table_name", 放射科報告名單SQL_住);



            try
            {
                dataGridView1.DataSource = dataset;

                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();

            }
            catch
            {
                ;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
#if NEW
            string 放射科報告資料SQL_住 = "select fib.病歷號碼,fib.身份證字號,fib.醫師姓名,fib.生日,fib.處置索引1,fib.處置名稱," +
"fib.報告內容,fib.開單日期,fib.開單時間,fib.報到日期,fib.報到時間,fib.檢查日期,fib.檢查時間," +
"fib.完成日期,fib.完成時間,fib.報告師代號,fqx.姓名 as '報告醫師姓名',fib.報告台代碼," +
"fib.報告台代碼名稱,fib.處置種類代碼,fib.處置種類代碼名稱," +
"fib.醫事機構碼,fib.醫事機構名稱,fib.申請流水號,fib.處置檔_counter,fib.同處置_counter," +
"fib.來源,fib.counter as '結果檔_counter'" +
" from" +
" (select fin.病歷號碼, fix.姓名 as '醫師姓名', fin.生日, fin.處置索引1, fin.處置名稱, fin.單價," +
"fin.報告內容, fin.開單日期, fin.開單時間, fin.報到日期, fin.報到時間, fin.檢查日期, fin.檢查時間," +
"fin.完成日期, fin.完成時間, fin.報告師代號, fin.報告台代碼," +
"fvx.代碼內容 as '報告台代碼名稱', fin.處置種類代碼, fux.代碼內容 as '處置種類代碼名稱'," +
"'1532061065' as 醫事機構碼, '大園敏盛醫院' as 醫事機構名稱, fin.申請流水號, fin.處置檔_counter, fin.同處置_counter," +
"fin.來源, fin.身份證字號, fin.counter" +
" from" +

" (select fa.生日, fa.代碼內容, fa.病歷號碼, fa.處置代號, fa.處置索引1," +
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.主治醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期," +
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter," +
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.身份證字號, '住診' AS  '來源', fb.counter" +

" from" +
" (select c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.入院日期, a.實際出院日, a.主治醫師代號," +
"       b.處置代號, b.總量, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter," +
"       b.處置檔_counter, b.單價, c.身份證字號, b.counter" +
" from 住診檔 a, 住診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e" +
" where a.病患檔_Counter = c.Counter and" +
"      b.處置檔_counter = d.Counter AND" +
"      b.住診檔_Counter = a.Counter AND" +
"      a.目前科別代碼 = e.代碼 AND" +
"      b.處置種類代碼 = '2' and" +
"      d.處置索引1 is not null and" +
"      b.結果檔_counter > 0 and" +
"      c.病歷號碼 <> '9999999999'" +
")fa," +
" (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb" +
"  where fa.結果檔_counter = fb.counter" +
" and fa.counter = fb.來源檔_counter" +
")fin," +
" (SELECT * FROM dbo.人事資料檔 x)fix," +
" (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx," +
" (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux" +
"  WHERE fin.主治醫師代號 = fix.人事代號" +
" and fin.報告台代碼 = fvx.代碼" +
" and fin.處置種類代碼 = fux.代碼)fib" +
"  left join dbo.人事資料檔 fqx on fib.報告師代號 = fqx.人事代號" +
" order by fib.病歷號碼";

#else
            string 放射科報告資料SQL_住 = "select fib.病歷號碼,fib.身份證字號,fib.醫師姓名,fib.生日,fib.處置索引1,fib.處置名稱,"+
"fib.報告內容,fib.開單日期,fib.開單時間,fib.報到日期,fib.報到時間,fib.檢查日期,fib.檢查時間,"+
"fib.完成日期,fib.完成時間,fib.報告師代號,fqx.姓名 as '報告醫師姓名',fib.報告台代碼,"+
"fib.報告台代碼名稱,fib.處置種類代碼,fib.處置種類代碼名稱,"+
"fib.醫事機構碼,fib.醫事機構名稱,fib.申請流水號,fib.處置檔_counter,fib.同處置_counter,"+
"fib.來源"+
" from "+
"(select fin.病歷號碼, fix.姓名 as '醫師姓名', fin.生日, fin.處置索引1, fin.處置名稱, fin.單價,"+
"fin.報告內容, fin.開單日期, fin.開單時間, fin.報到日期, fin.報到時間, fin.檢查日期, fin.檢查時間,"+
"fin.完成日期, fin.完成時間, fin.報告師代號, fin.報告台代碼,"+
"fvx.代碼內容 as '報告台代碼名稱', fin.處置種類代碼, fux.代碼內容 as '處置種類代碼名稱',"+
"'1532061065' as 醫事機構碼, '大園敏盛醫院' as 醫事機構名稱, fin.申請流水號, fin.處置檔_counter, fin.同處置_counter,"+
"fin.來源, fin.身份證字號"+
" from "+

"(select fa.生日, fa.代碼內容, fa.病歷號碼, fa.處置代號, fa.處置索引1,"+
"fa.處置名稱, fa.處置種類代碼, fa.姓名, fa.主治醫師代號, fa.結果檔_counter, fb.開單日期, fb.開單時間, fb.報到日期,"+
"fb.報到時間, fb.檢查日期, fb.檢查時間, fb.完成日期, fb.完成時間, fb.報告內容, fb.報告師代號, fb.同處置_counter,"+
"fb.申請流水號, fb.報告台代碼, fa.處置檔_counter, fa.單價, fa.身份證字號,"+
" CASE"+
" WHEN RTRIM(fb.來源代碼) IN(0) THEN '門急診'"+
" WHEN RTRIM(fb.來源代碼) IN(1) THEN '門診'"+
" WHEN RTRIM(fb.來源代碼) IN(2) THEN '急診'"+
" WHEN RTRIM(fb.來源代碼) IN(3) THEN '住診'"+
" WHEN RTRIM(fb.來源代碼) IN(4) THEN '預約'"+
" WHEN RTRIM(fb.來源代碼) IN(5) THEN '體檢'"+
" WHEN RTRIM(fb.來源代碼) IN(6) THEN '門診洗腎'"+
" WHEN RTRIM(fb.來源代碼) IN(7) THEN '外來代檢'"+
" ELSE 'NA'"+
" END AS  '來源'"+

" from"+
" (select c.生日, e.代碼內容, a.病患檔_counter, c.病歷號碼, c.姓名, a.入院日期, a.實際出院日, a.主治醫師代號,"+
      " b.處置代號, b.總量, d.處置索引1, d.處置名稱, b.處置種類代碼, b.結果檔_counter,"+
     "  b.處置檔_counter, b.單價, c.身份證字號"+
" from 住診檔 a, 住診處置內容檔 b, 病患檔 c, 處置檔 d, (SELECT e.代碼, e.代碼內容 from dbo.代碼檔 e where e.代碼名稱 = '科別代碼')e"+
" where a.病患檔_Counter = c.Counter and"+
     " b.處置檔_counter = d.Counter AND"+
     " b.住診檔_Counter = a.Counter AND"+
     " a.目前科別代碼 = e.代碼 AND"+
     " b.處置種類代碼 = '2' and"+
     " d.處置索引1 is not null and"+
     " b.結果檔_counter > 0 and"+
     " c.病歷號碼 <> '9999999999'"+
")fa,"+
" (SELECT * FROM dbo.報告台結果檔 e where e.流程旗標 in ('暫', '確', '報') and e.完成日期 between '1060701' and '1060731')fb"+
"  where fa.結果檔_counter = fb.counter"+
")fin,"+
" (SELECT * FROM dbo.人事資料檔 x)fix,"+
" (SELECT * FROM dbo.代碼檔 v where v.代碼名稱 = '報告台代碼')fvx,"+
" (SELECT * FROM dbo.代碼檔 u where u.代碼名稱 = '處置種類代碼')fux"+
"  WHERE fin.主治醫師代號 = fix.人事代號"+
" and fin.報告台代碼 = fvx.代碼"+
" and fin.處置種類代碼 = fux.代碼)fib"+
"  left join dbo.人事資料檔 fqx on fib.報告師代號 = fqx.人事代號"+
" order by fib.病歷號碼";


#endif


            dataset = database.Custom_DataSet("Table_name", 放射科報告資料SQL_住);



            try
            {
                dataGridView1.DataSource = dataset;

                dataGridView1.DataMember = "Table_name";

                button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();
                Compare_2 = dataset;
            }
            catch
            {
                ;
            }
        }
        DataSet New_dataset = new DataSet();
        DataSet Old_dataset = new DataSet();
        private void timer1_Tick(object sender, EventArgs e)
        {
            string Timer_SQL = "SELECT * FROM scdb.scds_BP;";// +
                                                             //" where a.院所代號 = '1532061065'";


            try

            {
                New_dataset = database.Custom_DataSet("Table_name", Timer_SQL);
            }
            catch
            {
                string Conntect = @"";
                Conntect += textBox1.Text;
                database = new MYSQL(Conntect);
                return;
            }
            if (Old_dataset.Tables.Count > 0)
            {

            }
            else
            {
                Old_dataset = New_dataset;

                try
                {
                    dataGridView1.DataSource = Old_dataset;

                    dataGridView1.DataMember = "Table_name";

                    button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();

                }
                catch
                {
                    ;
                }
                if (!Directory.Exists(@"C:\LOG"))
                {
                    Directory.CreateDirectory(@"C:\LOG");
                }
                string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                using (StreamWriter sw = new StreamWriter(@"C:\LOG\test_" + date + ".log", true))
                {
                    string SW = "[";//[date+time] 執行 [109] RPT001Y -> [100]LAB.RPT001 ：aplseq:<value>，diccode:<value> 轉出成功。
                    SW += date + "+" + time;
                    SW += "] 執行 true";
                    sw.WriteLine(SW);
                }
            }
            if (comparedata(New_dataset.Tables[0], Old_dataset.Tables[0]))
            {
                if (!Directory.Exists(@"C:\LOG"))
                {
                    Directory.CreateDirectory(@"C:\LOG");
                }
                string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                using (StreamWriter sw = new StreamWriter(@"C:\LOG\test_" + date + ".log", true))
                {
                    string SW = "[";//[date+time] 執行 [109] RPT001Y -> [100]LAB.RPT001 ：aplseq:<value>，diccode:<value> 轉出成功。
                    SW += date + "+" + time;
                    SW += "] 執行 false";
                    sw.WriteLine(SW);
                }
            }
            else
            {

                Old_dataset = New_dataset;

                try
                {
                    dataGridView1.DataSource = Old_dataset;

                    dataGridView1.DataMember = "Table_name";

                    button1.Text = (string)dataset.Tables[0].Rows.Count.ToString();

                }
                catch
                {
                    ;
                }
                if (!Directory.Exists(@"C:\LOG"))
                {
                    Directory.CreateDirectory(@"C:\LOG");
                }
                string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                using (StreamWriter sw = new StreamWriter(@"C:\LOG\test_" + date + ".log", true))
                {
                    string SW = "[";//[date+time] 執行 [109] RPT001Y -> [100]LAB.RPT001 ：aplseq:<value>，diccode:<value> 轉出成功。
                    SW += date + "+" + time;
                    SW += "] 執行 true";
                    sw.WriteLine(SW);
                }
            }
        }
        private bool comparedata(DataTable datatable1, DataTable datatable2)
        {
            if (datatable1.Rows.Count != datatable2.Rows.Count)
            {
                return false;
            }
            for (int i = 0; i < datatable1.Rows.Count; i++)

            {
                object[] aa = datatable1.Rows[i].ItemArray;
                object[] bb = datatable2.Rows[i].ItemArray;
                for (int j = 0; j < aa.Count(); j++)
                {

                    object aa1 = aa[j];
                    object bb1 = bb[j];
                    if (aa1.Equals(bb1))
                    {

                    }
                    else
                    {
                        return false;
                    }

                }
            }

            return true;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            string data = "";
            StreamWriter wr = new StreamWriter(@"C:\csv\data.csv", false, System.Text.Encoding.Default);
            foreach (DataColumn column in dataset.Tables[0].Columns)
            {
                data += column.ColumnName + ",";
            }
       
            wr.WriteLine(data);
            data = "";

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                foreach (DataColumn column in dataset.Tables[0].Columns)
                {
                    data += row[column].ToString().Replace("\n","n").Replace(",","，").Trim() + ",";
                }
            
                wr.WriteLine(data);
                data = "";
            }
            data += "\n";

            wr.Dispose();
            wr.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
    
        }
    }

    public class Rootobject
    {
        public A_Data[] A_data { get; set; }
    }

    public class A_Data
    {
        public string Medical_Number { get; set; }
        public string Doctor_Name { get; set; }
        public string Birthday { get; set; }
        public string Inspection_Index_4 { get; set; }
        public string Inspection_Code { get; set; }
        public string Inspection_name { get; set; }
        public string Inspection_result_value { get; set; }
        public string Inspection_reference_value { get; set; }
        public string unit { get; set; }
        public string RPTDTE { get; set; }
        public string Treatment_date { get; set; }
        public string Treatment_time { get; set; }
        public string Sign_date { get; set; }
        public string Sign_time { get; set; }
        public string Sign_person { get; set; }
        public string Doctor_Key_Code { get; set; }
        public string Doctor_Key_Name { get; set; }
        public string Sample_category { get; set; }
        public object Sample_category_name { get; set; }
        public string Inspection_category { get; set; }
        public string Inspection_category_name { get; set; }
        public string Serial_Number { get; set; }
        public string Report_person { get; set; }
        public string Inspection_counter { get; set; }
        public string Inspection_agency_ID { get; set; }
        public string Inspection_agency_name { get; set; }
        public string Inspection_agency_cperson { get; set; }
        public string Class { get; set; }
    }

}
