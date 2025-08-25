using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Database_Link
{
    public partial class SettingDatabase : Form
    {
        public List<string> DataBase_Connection = new List<string>();
        public List<List<ColumnSet>> DataBase_Column = new List<List<ColumnSet>>();
        public List<string> Table_Name = new List<string>();
        XmlDocument doc = new XmlDocument();
        List<GroupBox> Table = new List<GroupBox>();
        List<TextBox> Table_Name_Textbox = new List<TextBox>();
        List<TextBox> Table_Column_Textbox = new List<TextBox>();
        List<ComboBox> Table_Column_ComboBox = new List<ComboBox>();
        List<RichTextBox> ConnectString_RichTextBox = new List<RichTextBox>();
        string SettingFilePath;
        public SettingDatabase(string settingfilepath)
        {
            InitializeComponent();
            SettingFilePath = settingfilepath;
            settingfilepath.LastIndexOf(@"\");
            string DirectoryPath = settingfilepath.Remove(settingfilepath.LastIndexOf(@"\"));
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            if (!File.Exists(SettingFilePath))
            {

            }
            else
            {
                doc.InnerXml  = String_Password.Encrypt.DecryptText(SettingFilePath);
              //  doc.Load(@"C:\image\Database");
               // XmlNodeList NodeLists = doc.SelectNodes("Table");
                XmlNode xnC = doc.SelectSingleNode("DataBase/Conntection");
                XmlNodeList xnCl = xnC.ChildNodes;

                foreach (XmlNode OneNode in xnCl)
                {

                    
                        DataBase_Connection.Add(OneNode.InnerText);
                    



                }
                XmlNode xn = doc.SelectSingleNode("DataBase/Table");
                    XmlNodeList xnl=xn.ChildNodes;
                foreach (XmlNode OneNode in xnl)
                {
                    
                        Table_Name.Add(OneNode.Name);
                    
                    

                   
                }

                 TableList = new List<ColumnSet>[Table_Name.Count];
                for (int i = 0; i < TableList.Length; i++)
                {
                    TableList[i] = new List<ColumnSet>();
                }
                int j = 0;
                foreach (string TableName in Table_Name)
                {
                 
                    //XmlNode mainColumn = doc.SelectNode("Table/" + TableName);
                    XmlNode xnColumn = doc.SelectSingleNode("DataBase/Table/" + TableName);
                    XmlNodeList xnlColumn = xnColumn.ChildNodes;
                    foreach (XmlNode OneNode in xnlColumn)
                    {
                       // XmlNodeList nodeList = doc.SelectNodes("Table/" + TableName );
                       // foreach (XmlNode onenode in nodeList)
                        {
                            if (OneNode.Attributes["Setting"].Value == "Column")
                            {
                                ColumnSet columnset = new ColumnSet();
                                switch (OneNode.Attributes["Type"].Value)
                                {
                                    case "String":
                                        columnset.Type = DatabaseType.Str;
                                        break;
                                    case "Bool":
                                        columnset.Type = DatabaseType.Bool;
                                        break;
                                    case "DateTime":
                                        columnset.Type = DatabaseType.DateTime;
                                        break;
                                    case "Int":
                                        columnset.Type = DatabaseType.Int;
                                        break;
                                    case "Double":
                                        columnset.Type = DatabaseType.Double;
                                        break;
                                    case "Date":
                                        columnset.Type = DatabaseType.Date;
                                        break;
                                    default:
                                        columnset.Type = DatabaseType.Str;
                                        break;
                                }
                                columnset.Name = OneNode.InnerText;

                                TableList[j].Add(columnset);
                            }
                        }
                     



                    }
      
                    j++;
                   
                }
              

                for (int i = 0; i < TableList.Length;i++ )
                {
                   DataBase_Column.Add( TableList[i]);
                }
            }
                    
          
        }
        List<ColumnSet>[] TableList;

        private void SettingDatabase_Load(object sender, EventArgs e)
        {
          Table .Clear();
           Table_Name_Textbox.Clear(); 
           Table_Column_Textbox.Clear();
            Table_Column_ComboBox.Clear();
            ConnectString_RichTextBox.Clear();
            foreach (string DataBaseString in DataBase_Connection)
            {
                RichTextBox richtextbox = new RichTextBox();
                Label label1 = new Label();
                ConnectString_RichTextBox.Add(richtextbox);
                ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Location = new Point(200, (ConnectString_RichTextBox.Count - 1) * 40 + 50);
                ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Size = new Size(225, 38);
                ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].WordWrap = false;
                ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Multiline = true;
                ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].ScrollBars = RichTextBoxScrollBars.Horizontal;
                label1.Text = "資料庫連接" + (ConnectString_RichTextBox.Count - 1).ToString();

                label1.Size = new Size(50, 30);
                label1.Location = new Point(150, (ConnectString_RichTextBox.Count - 1) * 40 + 50);
                Button Remove_Conntect = new Button();
                Remove_Conntect.Name = "RemoveConntect" + (Table.Count).ToString();
                Remove_Conntect.Location = new Point(425, (ConnectString_RichTextBox.Count - 1) * 40 + 50);
                Remove_Conntect.Size = new System.Drawing.Size(23, 23);
                Remove_Conntect.Click += new EventHandler(RemoveConntect_Click);
                Remove_Conntect.Text = "X";
                this.Controls.Add(Remove_Conntect);
                this.Controls.Add(ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1]);
                ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Text = DataBaseString;
                this.Controls.Add(label1);

            }
            int j=0;
                  foreach (string TableName in Table_Name)
                {
                    GroupBox GroupBox1 = new GroupBox();
                    TextBox Table_Textbox = new TextBox();
                    Button Button_Add = new Button();
                    Table_Textbox.Text = TableName;
                   
                    GroupBox1.Controls.Add(Button_Add);
                    GroupBox1.Controls.Add(Table_Textbox);
                    Button Remove_Table = new Button();

                    Table.Add(GroupBox1);
                    //Panel1.Size = new Size(500, 500);
                    this.Controls.Add(Remove_Table);
                    this.Controls.Add(Table[Table.Count - 1]);
                    Remove_Table.Name = "RemoveTable" + (Table.Count).ToString();
                    GroupBox1.Name = " GroupBox1" + (Table.Count).ToString();


                    GroupBox1.Text = "資料表" + (Table.Count - 1).ToString();
                    Table_Textbox.Location = new Point(0, 15);
                    Table_Textbox.Size = new System.Drawing.Size(50, Table_Textbox.Size.Height);
                    Button_Add.Text = "新增欄位";
                    Button_Add.Location = new Point(Table_Textbox.Size.Width, Table_Textbox.Location.Y);
                    Button_Add.Size = new Size(65, Table_Textbox.Size.Height);
                    Button_Add.Name = "Button" + (Table.Count).ToString();
                    Remove_Table.Location = new Point(475 + GroupBox1.Size.Width + (Table.Count - 1) * 200, 30);
                    Remove_Table.Text = "X";
                    Remove_Table.Size = new System.Drawing.Size(23, 23);
                    Remove_Table.Click += new EventHandler(RemoveTable_Click);
                    Button_Add.Click += new EventHandler(AddColumn_Click);
                    GroupBox1.Location = new Point(500 + (Table.Count - 1) * 200, 40);
                    Table_Name_Textbox.Add(Table_Textbox);
                    foreach (ColumnSet Columnset in TableList[j])
                    {
                      //  string aa = ((Button)sender).Name.Remove(0, 6);

                        int Table_Item = j + 1;
                        Label label1 = new Label();
                        ComboBox combobox1 = new ComboBox();
                        combobox1.Items.Add("String");
                        combobox1.Items.Add("Bool");
                        combobox1.Items.Add("Int");
                        combobox1.Items.Add("Double");
                        combobox1.Items.Add("DateTime");
                        combobox1.Items.Add("Date");
                        combobox1.Text = "String";
                        combobox1.Tag = "Table" + Table_Item;
                   
                        TextBox Column_Textbox = new TextBox();
                        Button Remove_Button = new Button();
                        Remove_Button.Text = "X";
                        Remove_Button.Name = "Remove" + Column_Item;
                        Remove_Button.Tag = "Table" + Table_Item;
                        Column_Textbox.Name = "Column" + Column_Item;
                        label1.Name = label1.Name = "Label" + Column_Item;
                        Column_Textbox.Tag = "Table" + Table_Item;
                        int Textbox_Item = 0;
                        foreach (TextBox textbox in Table_Column_Textbox)
                        {
                            if (textbox.Tag.ToString() == "Table" + Table_Item)
                                Textbox_Item++;
                        }
                        label1.Text = "欄位" + Textbox_Item.ToString() + "名稱";

                        label1.Size = new Size(60, label1.Size.Height);
                        label1.Location = new Point(0, 60 + (Textbox_Item - 1) * 23);
                        combobox1.Size = new System.Drawing.Size(50, combobox1.Size.Height);
                        Column_Textbox.Size = new System.Drawing.Size(60, Column_Textbox.Size.Height);
                        Column_Textbox.Location = new Point(label1.Size.Width, label1.Location.Y);
                        combobox1.Location = new Point(label1.Size.Width + Column_Textbox.Size.Width, label1.Location.Y);
                        Remove_Button.Location = new Point(label1.Size.Width + Column_Textbox.Size.Width + combobox1.Size.Width, label1.Location.Y);
                        Remove_Button.Size = new System.Drawing.Size(23, 23);
                        Remove_Button.Click += new EventHandler(RemoveColumn_Click);
                        Table_Column_Textbox.Add(Column_Textbox);
                        Table_Column_ComboBox.Add(combobox1);
                        Table[Table_Item - 1].Controls.Add(combobox1);
                        Table[Table_Item - 1].Controls.Add(Remove_Button);
                        Table[Table_Item - 1].Controls.Add(label1);
                        Table[Table_Item - 1].Controls.Add(Table_Column_Textbox[Table_Column_Textbox.Count - 1]);
                        Table[Table_Item - 1].Size = new Size(Table[Table_Item - 1].Size.Width, Table[Table_Item - 1].Size.Height + 23);
                        Column_Item++;
                        Column_Textbox.Text = Columnset.Name;
                        switch (Columnset.Type)
                        {
                            case DatabaseType.Str:
                                combobox1.Text = "String";

                                break;
                            case DatabaseType.Bool:
                                combobox1.Text = "Bool";
                                break;

                            case DatabaseType.DateTime:
                                combobox1.Text = "DateTime";
                                break;

                            case DatabaseType.Int:
                                combobox1.Text = "Int";
                                break;
                            case DatabaseType.Double:
                                combobox1.Text = "Double";

                                break;
                            case DatabaseType.Date:
                                combobox1.Text = "Date";
                                break;
                            default:
                                combobox1.Text = "String";
                                break;

                        }
                    }
                    j++;

                }
               
            
        }

        private void AddTable_Click(object sender, EventArgs e)
        {
            GroupBox GroupBox1 = new GroupBox();
             TextBox Table_Textbox=new TextBox();
            Button Button_Add = new Button();
            
            GroupBox1.Controls.Add(Button_Add);
            GroupBox1.Controls.Add(Table_Textbox);
            Button Remove_Table = new Button();
           
            Table.Add(GroupBox1);
            //Panel1.Size = new Size(500, 500);
            this.Controls.Add(Remove_Table);
            this.Controls.Add(Table[Table.Count-1]);
            
            Remove_Table.Name = "RemoveTable" + (Table.Count ).ToString();
            GroupBox1.Name = " GroupBox1" + (Table.Count).ToString();

            GroupBox1.Text = "資料表" + (Table.Count - 1).ToString();
            Table_Textbox.Location = new Point(0, 15);
            Table_Textbox.Size = new System.Drawing.Size(50, Table_Textbox.Size.Height);
            Button_Add.Text = "新增欄位";
            Button_Add.Location = new Point(Table_Textbox.Size.Width, Table_Textbox.Location.Y);
            Button_Add.Size = new Size(65,Table_Textbox.Size.Height);
            Button_Add.Name = "Button" + (Table.Count).ToString();
            Remove_Table.Location = new Point(475 +GroupBox1.Size.Width+ (Table.Count - 1)* 200, 30);
            Remove_Table.Text = "X";
            Remove_Table.Size = new System.Drawing.Size(23, 23);
            Remove_Table.Click += new EventHandler(RemoveTable_Click);
            Button_Add.Click += new EventHandler(AddColumn_Click);     
            GroupBox1.Location = new Point(500+(Table.Count-1)*200, 40);
            Table_Name_Textbox.Add(Table_Textbox);
         
         
        }
        private void RemoveTable_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "X")
            {
                string aa = ((Button)sender).Name.Remove(0, 11);

                int Table_Item = Convert.ToInt32(aa)-1;
                Table[Table_Item].Enabled = false;
                Table_Name_Textbox[Table_Item].Enabled = false;
                ((Button)sender).Text = "O";
            }
            else
            {
                string aa = ((Button)sender).Name.Remove(0, 11);

                int Table_Item = Convert.ToInt32(aa)-1;
                Table[Table_Item].Enabled = true;
                Table_Name_Textbox[Table_Item].Enabled = true;

                ((Button)sender).Text = "X";
            }
        }
        int Column_Item = 0;
        private void AddColumn_Click(object sender, EventArgs e)
        {
            string aa = ((Button)sender).Name.Remove(0,6);

           int Table_Item=Convert.ToInt32( aa);
           Label label1 = new Label();
           ComboBox combobox1 = new ComboBox();
           combobox1.Items.Add("String");
           combobox1.Items.Add("Bool");
           combobox1.Items.Add("Int");
           combobox1.Items.Add("Double");
           combobox1.Items.Add("DateTime");
           combobox1.Items.Add("Date");
           combobox1.Text = "String";
           combobox1.Tag = "Table" + Table_Item;
          
           TextBox Column_Textbox = new TextBox();
           Button Remove_Button = new Button();
           Remove_Button.Text = "X";
           Remove_Button.Name = "Remove" + Column_Item;
           Remove_Button.Tag = "Table" + Table_Item;
           Column_Textbox.Name = "Column" + Column_Item;
           label1.Name = label1.Name = "Label" + Column_Item;
           Column_Textbox.Tag = "Table"+Table_Item;
           int Textbox_Item = 0;
            foreach(TextBox textbox in Table_Column_Textbox)
            {
                if (textbox.Tag.ToString() == "Table" + Table_Item)
                    Textbox_Item++;
            }
            label1.Text = "欄位" + Textbox_Item.ToString() + "名稱";

            label1.Size=new Size (60,label1.Size.Height);
            label1.Location = new Point(0, 60 + (Textbox_Item - 1) * 23);
            combobox1.Size = new System.Drawing.Size(50, combobox1.Size.Height);
            Column_Textbox.Size = new System.Drawing.Size(60, Column_Textbox.Size.Height);
           Column_Textbox.Location = new Point(label1.Size.Width,label1.Location.Y);
           combobox1.Location = new Point(label1.Size.Width + Column_Textbox.Size.Width, label1.Location.Y);
           Remove_Button.Location = new Point(label1.Size.Width + Column_Textbox.Size.Width + combobox1.Size.Width, label1.Location.Y);
            Remove_Button.Size = new System.Drawing.Size(23, 23);
            Remove_Button.Click += new EventHandler(RemoveColumn_Click);
           Table_Column_Textbox.Add(Column_Textbox);
           Table_Column_ComboBox.Add(combobox1);
           Table[Table_Item - 1].Controls.Add(combobox1);
           Table[Table_Item - 1].Controls.Add(Remove_Button);
           Table[Table_Item-1].Controls.Add(label1);
           Table[Table_Item - 1].Controls.Add(Table_Column_Textbox[Table_Column_Textbox.Count-1]);
           Table[Table_Item - 1].Size = new Size(Table[Table_Item - 1].Size.Width, Table[Table_Item - 1].Size.Height + 23);
           Column_Item++;
          
        
        }
        private void RemoveColumn_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "X")
            {
                string aa = ((Button)sender).Name.Remove(0, 6);

                int Button_Item = Convert.ToInt32(aa);
                Table_Column_Textbox[Button_Item].Enabled = false;
                Table_Column_ComboBox[Button_Item].Enabled = false;
                ((Button)sender).Text = "O";
            }
            else
            {
                string aa = ((Button)sender).Name.Remove(0, 6);

                int Button_Item = Convert.ToInt32(aa);
                Table_Column_Textbox[Button_Item].Enabled = true ;
                Table_Column_ComboBox[Button_Item].Enabled = true;
                ((Button)sender).Text = "X";
            }
        }
        private void Save_Click(object sender, EventArgs e)
        {
            Table_Name.Clear();
            DataBase_Column.Clear();
            DataBase_Connection.Clear();

            foreach (RichTextBox RichTextBox in ConnectString_RichTextBox)
            {
                if (RichTextBox.Enabled == true)
                    DataBase_Connection.Add(RichTextBox.Text);
            }
            foreach (TextBox Textbox in Table_Name_Textbox)
            {
                if (Textbox.Enabled == true)
                Table_Name.Add(Textbox.Text);
            }
            List<ColumnSet>[] Table = new   List<ColumnSet>[Table_Name_Textbox.Count];
            for (int i = 0; i < Table.Length; i++)
            {
                Table[i] = new List<ColumnSet>();
            }
            for (int i = 0; i < Table_Column_Textbox.Count;i++ )
            {

                int Table_Item = Convert.ToInt32(Table_Column_Textbox[i].Tag.ToString().Remove(0, 5)) - 1;
                if (Table_Column_Textbox[i].Enabled == true)
                {
                    ColumnSet columnset = new ColumnSet();
                    columnset.Name = Table_Column_Textbox[i].Text;
                   switch(Table_Column_ComboBox[i].Text)
                   {
                       case "String":
                           columnset.Type = DatabaseType.Str;
                           break;
                       case "Bool":
                           columnset.Type = DatabaseType.Bool;
                           break;
                       case "DateTime":
                           columnset.Type = DatabaseType.DateTime;
                           break;
                       case "Int":
                           columnset.Type = DatabaseType.Int;
                           break;
                       case "Double":
                           columnset.Type = DatabaseType.Double;
                           break;
                       case "Date":
                           columnset.Type = DatabaseType.Date;
                           break;
                       default:
                               columnset.Type = DatabaseType.Str;
                           break;
                   }
                    

                    Table[Table_Item].Add(columnset);
                }
            }
            for(int i=0;i<Table.Length;i++)
            {
                if(Table_Name_Textbox[i].Enabled ==true)
                
                DataBase_Column.Add(Table[i]);
            }
            doc = new XmlDocument();
            XmlElement Database = doc.CreateElement("DataBase");
            doc.AppendChild(Database);
           XmlElement DatabaseConntection = doc.CreateElement("Conntection");
           Database.AppendChild(DatabaseConntection);
            foreach(string Conntection in DataBase_Connection)
            {
                XmlElement Conntection_Element = doc.CreateElement("ConntectionString");
                Conntection_Element.SetAttribute("Setting", "Conntection");
                Conntection_Element.InnerText = Conntection;
                DatabaseConntection.AppendChild(Conntection_Element);
            }
            XmlElement DatabseTable = doc.CreateElement("Table");
            Database.AppendChild(DatabseTable);
            int j=0;
            foreach (List<ColumnSet> Column in DataBase_Column)
            {

                XmlElement Table_Element = doc.CreateElement(Table_Name[j]);
                    Table_Element.SetAttribute("Setting", "Table");
                    Table_Element.SetAttribute("Name", Table_Name[j]);
                  //  Table_Element.InnerText = Table_Name[j];5 
                    foreach (ColumnSet Column_Name in Column)
                    {
                        XmlElement info = doc.CreateElement("ColumnName");
                        info.SetAttribute("Setting", "Column");
                        switch(Column_Name.Type)
                        {
                            case DatabaseType.Str:
                                info.SetAttribute("Type", "String");
                                
                                break;
                            case DatabaseType.Bool:
                                info.SetAttribute("Type", "Bool");
                                break;

                            case  DatabaseType.DateTime:
                                info.SetAttribute("Type", "DateTime");
                                break;
                                
                            case  DatabaseType.Int:
                                info.SetAttribute("Type", "Int");
                                break;
                            case DatabaseType.Double:
                                info.SetAttribute("Type", "Double");
                                break;
                            case DatabaseType.Date:
                                info.SetAttribute("Type", "Date");
                                break;
                            default:
                                info.SetAttribute("Type", "String");
                                break;

                        }
                        
                        info.InnerText = Column_Name.Name;
                        Table_Element.AppendChild(info);
                    }


                    DatabseTable.AppendChild(Table_Element);
                    j++;
                
            }
           
            String_Password.Encrypt.EncryptedText(SettingFilePath, doc.InnerXml);
            this.Close();
            
           

      



        }

        private void AddConnection_Click(object sender, EventArgs e)
        {
            RichTextBox richtextbox = new RichTextBox();
            Label label1 = new Label();
          
            ConnectString_RichTextBox.Add(richtextbox);
            ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Location = new Point(200, (ConnectString_RichTextBox.Count - 1) * 40 + 50);
            ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Size = new Size(225, 38);
            ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].WordWrap = false;
            ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Multiline = true;
            ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].ScrollBars = RichTextBoxScrollBars.Horizontal;
            ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1].Tag = "Conntect" + (ConnectString_RichTextBox.Count - 1).ToString();
            label1.Text = "資料庫連接" + (ConnectString_RichTextBox.Count - 1 ).ToString();

            label1.Size = new Size(50, 30);
            label1.Location = new Point(150, (ConnectString_RichTextBox.Count - 1) * 40 + 50);
            Button Remove_Conntect = new Button();
            Remove_Conntect.Name = "RemoveConntect" + (Table.Count).ToString();
            Remove_Conntect.Location = new Point(425, (ConnectString_RichTextBox.Count - 1) * 40 + 50);
            Remove_Conntect.Size = new System.Drawing.Size(23, 23);
            Remove_Conntect.Click += new EventHandler(RemoveConntect_Click);
            Remove_Conntect.Text = "X";
            this.Controls.Add(Remove_Conntect);
            this.Controls.Add(ConnectString_RichTextBox[ConnectString_RichTextBox.Count - 1]);
            this.Controls.Add(label1);
        }
        private void RemoveConntect_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "X")
            {
                string aa = ((Button)sender).Name.Remove(0, 14);

                int Button_Item = Convert.ToInt32(aa);
                ConnectString_RichTextBox[Button_Item].Enabled = false;
   
                ((Button)sender).Text = "O";
            }
            else
            {
                string aa = ((Button)sender).Name.Remove(0, 14);

                int Button_Item = Convert.ToInt32(aa);
                ConnectString_RichTextBox[Button_Item].Enabled = true;
              
                ((Button)sender).Text = "X";
            }
        }
    }
    public class ColumnSet
    {
        public string Name { get; set; }
        public DatabaseType Type { get; set; }
    }
}
