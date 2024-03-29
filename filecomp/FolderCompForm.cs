﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;


namespace filecomp
{
    public partial class FolderCompForm : Form
    {
        List<string> FolderList1 = new List<string>();
        List<string> FolderList2 = new List<string>();
        List<FoldetSetdata> FolderSetDatas = new List<FoldetSetdata>();
        //FileList用
        FileListLibrary.FileListClass filelistclass = new FileListLibrary.FileListClass();

        List<FileSetdata.FileSetdata> copyfilelist = new List<FileSetdata.FileSetdata>();//コピー対象ファイル

        //FolderBrowserDialogクラスのインスタンスを作成
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        string folder;
        int startrow = 7;
        Encoding SJIS = Encoding.GetEncoding("Shift_JIS");
        string WorkFolder;
        string Excludefilename;
        string Selectfilename;
        string BeforeFolder;
        string AfterFolder;
        string StartSelectFolder;
        string SetFolderName1;
        string SetFolderName2;

        public FolderCompForm()
        {
            InitializeComponent();
            Initial_processing();
        }

        /// <summary>
        /// XMLFile(setlist.xml)の読み込み・設定
        /// </summary>
        private void XMLread()
        {
            string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "setlist.xml");//実行ファイルと同じフォルダ
            if (File.Exists(fileName))
            {
                //xmlファイルを指定する
                XElement xml = XElement.Load(fileName);
                //メンバー情報のタグ内の情報を取得する
                IEnumerable<XElement> infos = from item in xml.Elements("設定内容") select item;
                WorkFolder = infos.Select(c => c.Element("WorkFolder").Value).FirstOrDefault() ?? @"C:\FileList";
                Excludefilename = infos.Select(c => c.Element("Excludefilename").Value).FirstOrDefault() ?? "exclude.txt";
                Selectfilename = infos.Select(c => c.Element("Selectfilename").Value).FirstOrDefault() ?? "SelectFile.txt";
                BeforeFolder = infos.Select(c => c.Element("BeforeFolder").Value).FirstOrDefault() ?? @"\変更前\";
                AfterFolder = infos.Select(c => c.Element("AfterFolder").Value).FirstOrDefault() ?? @"\変更後\";
                StartSelectFolder = infos.Select(c => c.Element("StartSelectFolder").Value).FirstOrDefault() ?? @"C:\";
            }
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        private void Initial_processing()
        {
            XMLread();

            if (Directory.Exists(WorkFolder))//前回分のフォルダーが存在したら削除する
            {
                Directory.Delete(WorkFolder, true);
            }
            Directory.CreateDirectory(WorkFolder);//作業用フォルダ作成

            //プロパテイに固定値代入
            filelistclass.workfolder = WorkFolder;
            filelistclass.excludefilename = Excludefilename;
            filelistclass.selectfilename = Selectfilename;
            filelistclass.beforeFolder = BeforeFolder;
            filelistclass.afterFolder = AfterFolder;

            filelistclass.fileCopy(WorkFolder, Excludefilename);
            filelistclass.fileCopy(WorkFolder, Selectfilename);
        }

        /// <summary>
        /// フォルダ1を指定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            fbd.SelectedPath = StartSelectFolder;
            SetFolderName1 = FolderSelect();
            if (Directory.Exists(SetFolderName1))
            {
                textBox1.Text = SetFolderName1;
                FolderList1 = checkBox1.Checked ? Directory.EnumerateFiles(@SetFolderName1, Filter.Text, SearchOption.AllDirectories).ToList() : 
                                                  Directory.EnumerateFiles(@SetFolderName1, Filter.Text, SearchOption.TopDirectoryOnly).ToList(); //現在のディレクトリのみ
            }
        }

        /// <summary>
        /// フォルダ2を指定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void button4_Click(object sender, EventArgs e)
        {
            fbd.SelectedPath = folder;
            SetFolderName2 = FolderSelect();
            if (Directory.Exists(SetFolderName2))
            {
                textBox2.Text = SetFolderName2;
                FolderList2 = checkBox1.Checked ? Directory.EnumerateFiles(@SetFolderName2, Filter.Text, SearchOption.AllDirectories).ToList() :
                                                  Directory.EnumerateFiles(@SetFolderName2, Filter.Text, SearchOption.TopDirectoryOnly).ToList(); //現在のディレクトリのみ
            }
        }

        /// <summary>
        /// フォルダを指定する
        /// </summary>
        /// <returns>フォルダ名</returns>
        private string FolderSelect()
        {
            string folderName = "";
            //上部に表示する説明テキストを指定する
            fbd.Description = "フォルダを指定してください。";
            //ルートフォルダを指定する
            //デフォルトでDesktop
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            //最初に選択するフォルダを指定する
            //RootFolder以下にあるフォルダである必要がある
            //fbd.SelectedPath = @"C:\Windows";
            //ユーザーが新しいフォルダを作成できるようにする
            //デフォルトでTrue
            fbd.ShowNewFolderButton = false;
            //ダイアログを表示する
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                folderName = fbd.SelectedPath;
            }
            folder = fbd.SelectedPath;
            return folderName;
        }

        /// <summary>
        /// フォルダの比較　DataSouceを使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //比較に不要なファイルを削除して、ファイル名を同じにするため前のフォルダ名を削除する

            if (String.IsNullOrEmpty(SetFolderName1) || String.IsNullOrEmpty(SetFolderName2))
            {
                MessageBox.Show("フォルダ名が入力されていません");
                return;
            }
                
            var query1 = GetReadyList(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FolderList1).Select(c => c.Substring(SetFolderName1.Length));
            var query2 = GetReadyList(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FolderList2).Select(c => c.Substring(SetFolderName2.Length));
            textBox3.Text = query1.Union(query2).Count().ToString(); //全項目数

            foreach (var sdata in query1.Intersect(query2))//積集合
            {
                int kind = 0;
                string fcomp;
                if ((File.ReadLines(SetFolderName1 + sdata, SJIS)).ToList().SequenceEqual((File.ReadLines(SetFolderName2 + sdata, SJIS)).ToList())) //差分の判定をする
                {
                    fcomp = "ファイルは同一です";
                    kind = 4;
                }
                else
                {
                    fcomp = "ファイルは異なります";
                    kind = 1;
                }

                FileInfo file1 = new FileInfo(SetFolderName1 + sdata);
                long size1 = file1.Length;
                FileInfo file2 = new FileInfo(SetFolderName2 + sdata);
                long size2 = file2.Length;
                FolderSetDatas.Add(new FoldetSetdata(
                   Path.GetFileName(sdata),
                   Path.GetDirectoryName(sdata).Substring(1),
                   fcomp,
                   File.GetLastWriteTime(SetFolderName1 + sdata).ToString().Substring(0,10),
                   File.GetLastWriteTime(SetFolderName1 + sdata).ToString().Substring(11),
                   size1.ToString("D"),
                   File.GetLastWriteTime(SetFolderName2 + sdata).ToString().Substring(0, 10),
                   File.GetLastWriteTime(SetFolderName2 + sdata).ToString().Substring(11),
                   size2.ToString("D"),
                   Path.GetExtension(sdata),
                   kind));
            }

            foreach (var sdata in query1.Except(query2))//差集合　query1のみにある
            {
                FileInfo file1 = new FileInfo(SetFolderName1 + sdata);
                long size1 = file1.Length;
                FolderSetDatas.Add(new FoldetSetdata(
                   Path.GetFileName(sdata),
                   Path.GetDirectoryName(sdata).Substring(1),
                   "左側のみ",
                   File.GetLastWriteTime(SetFolderName1 + sdata).ToString().Substring(0, 10),
                   File.GetLastWriteTime(SetFolderName1 + sdata).ToString().Substring(11),
                   size1.ToString("D"),
                   "－",
                   "－",
                   "－",
                   Path.GetExtension(sdata),
                   2));
            }

            foreach (var sdata in query2.Except(query1))//差集合　query2のみにある
            {
                FileInfo file2 = new FileInfo(SetFolderName2 + sdata);
                long size2 = file2.Length;
                FolderSetDatas.Add(new FoldetSetdata(
                   Path.GetFileName(sdata),
                   Path.GetDirectoryName(sdata).Substring(1),
                   "右側のみ",
                   "－",
                   "－",
                   "－",
                   File.GetLastWriteTime(SetFolderName2 + sdata).ToString().Substring(0, 10),
                   File.GetLastWriteTime(SetFolderName2 + sdata).ToString().Substring(11),
                   size2.ToString("D"), 
                   Path.GetExtension(sdata),
                   3));
            }

            FolderSetDatas = FolderSetDatas.OrderBy(n => n.Kind).ToList();//種類(Kind)でsortする
            dataGridView1.DataSource = FolderSetDatas;//List<>をDataGridViewにバインドする
            Modified_dataGridView();
        }


        /// <summary>
        /// dataGridViewを修飾する
        /// </summary>
        private void Modified_dataGridView()
        {
            Pair[] HeaderData =
            {
               new Pair() { HeaderText =  "ファイル名", HeadetWidth = 200 },
               new Pair() { HeaderText =  "フォルダ名", HeadetWidth = 100 },
               new Pair() { HeaderText =  "比較結果",   HeadetWidth = 100 },
               new Pair() { HeaderText =  "左日付",     HeadetWidth = 100 },
               new Pair() { HeaderText =  "左時刻",     HeadetWidth = 100 },
               new Pair() { HeaderText =  "左サイズ",   HeadetWidth = 100 },
               new Pair() { HeaderText =  "右日付",     HeadetWidth = 100 },
               new Pair() { HeaderText =  "右時刻",     HeadetWidth = 100 },
               new Pair() { HeaderText =  "右サイズ",   HeadetWidth = 100 },
               new Pair() { HeaderText =  "拡張子",     HeadetWidth = 50  },
            };

            foreach (var item in HeaderData.Select((v, index) => new { v, index }))
            {
                dataGridView1.Columns[item.index].HeaderText = item.v.HeaderText;
                dataGridView1.Columns[item.index].Width = item.v.HeadetWidth;
            }

            //並び替えができないようにする
            foreach (DataGridViewColumn c in dataGridView1.Columns)
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[10].Visible = false;//列の非表示
            foreach (var item in FolderSetDatas.Select((data, index) => new { data, index }))//行に色を付ける
            {
                switch (dataGridView1[10, item.index].Value.ToString())
                {
                    case "1":
                        dataGridView1.Rows[item.index].DefaultCellStyle.BackColor = Color.Orange;
                        break;
                    case "2":
                        dataGridView1.Rows[item.index].DefaultCellStyle.BackColor = Color.LimeGreen;
                        break;
                    case "3":
                        dataGridView1.Rows[item.index].DefaultCellStyle.BackColor = Color.Aqua;
                        break;
                    case "4":
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// ファイルに保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            string strFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileName = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            // CSVファイルオープン
            using (StreamWriter sw = new StreamWriter(strFileName, false, SJIS))
            {
                //フォルダ名書き込み
                string FolderName = SetFolderName1 + "と" + SetFolderName2 + "の比較" + "\n";
                sw.Write(FolderName);

                //現在の日付・時刻
                DateTime dtNow = DateTime.Now;
                sw.Write(dtNow);
                sw.Write("\n");

                //dataGridView1の列のヘッダ書き込み

                string header = "";
                for (int i = 0; i <= dataGridView1.Columns.Count - 1; i++)
                {
                    if (dataGridView1.Columns[i].HeaderCell.Value != null)
                    {
                        header = dataGridView1.Columns[i].HeaderCell.Value.ToString();
                    }
                    if (i < dataGridView1.Columns.Count - 1)
                    {
                        header = header + ",";
                    }
                    sw.Write(header);//ヘッダ
                }
                sw.Write("\n");

                for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
                {
                    for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                    {
                        // DataGridViewのセルのデータ取得
                        String dt = "";
                        if (dataGridView1.Rows[r].Cells[c].Value != null)
                        {
                            dt = dataGridView1.Rows[r].Cells[c].Value.
                                ToString();
                        }
                        if (c < dataGridView1.Columns.Count - 1)
                        {
                            dt = dt + ",";
                        }
                        // CSVファイル書込
                        sw.Write(dt);
                    }
                    sw.Write("\n");
                }
            }
        }

        /// <summary>
        /// クリックされた行のファイルの内容を比較して表示させる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // *** クリックしたセルを取得する ***
            DataGridView.HitTestInfo info = ((DataGridView)sender).HitTest(e.X, e.Y);
            int index = info.RowIndex;
            if ((index >= 0) && (index < dataGridView1.RowCount))
            {
                FileCompForm form2 = new FileCompForm();
                string filename = Path.Combine(dataGridView1[1, index].Value.ToString(), dataGridView1[0, index].Value.ToString());

                form2.FileName1 = Path.Combine(SetFolderName1, filename);  //比較するファイル名1
                form2.FileName2 = Path.Combine(SetFolderName2, filename);  //比較するファイル名2
                form2.Show();
            }
        }

        //DataErrorイベントハンドラ
        private void dataGridView1_DataError(object sender,
            DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show(this,
                    string.Format("({0}, {1}) のセルでエラーが発生しました。\n\n説明: {2}",
                    e.ColumnIndex, e.RowIndex, e.Exception.Message),
                    "エラーが発生しました",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// リリース審査用　csvファイル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void release_Click(object sender, EventArgs e)
        {
            string strFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileName = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            FolderSetDatas.RemoveAll(c => c.Kind == 2); //古い日付しかないファイル
            FolderSetDatas.RemoveAll(c => c.Kind == 4); //同じファイル

            using (StreamWriter sw = new StreamWriter(strFileName, false, SJIS))
            {
                sw.WriteLine("プログラムファイル変更リスト（社内・現地）");
                sw.WriteLine("リリースバージョン");
                sw.WriteLine("客先名");
                sw.WriteLine("指番");
                sw.WriteLine("");
                sw.WriteLine("No,変更されたファイル名,更新前日付,時刻,更新後日付,時刻,更新前サイズ,更新後サイズ");
                foreach (var (item, index) in FolderSetDatas.Select((item, index) => (item, index)))
                {
                    string str = "";
                    string no = (index + 1).ToString();
                    string name = textBox4.Text + Path.Combine(item.Foldername, item.Filename);
                    string beforedate = item.Leftdate;
                    string beforetime = item.Lefttime;
                    string afterdate = item.Rightdate;
                    string aftertime = item.Righttime;
                    string beforesize = item.Leftsize;
                    string aftersize = item.Rightsize;
                    str = no + "," + name + "," + beforedate + "," + beforetime + "," + afterdate + "," + aftertime + "," + beforesize + "," + aftersize;
                    sw.WriteLine(str);
                }
            }
        }

        /// <summary>
        /// EXCEL リリース審査用ファイル作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void excel_button_Click(object sender, EventArgs e)
        {
            string strFileName;
           
            //はじめに「ファイル名」で表示される文字列を指定する
            saveFileDialog1.FileName = @"【プログラムファイル変更リスト】";
            // [ファイルの種類] ボックスに表示される選択肢を設定する
            saveFileDialog1.Filter = "Excelブック(*.xlsx)|*.xlsx|すべてのファイル(*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileName = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            try
            {
                FolderSetDatas.RemoveAll(c => c.Kind == 2); //古い日付しかないファイル削除
                FolderSetDatas.RemoveAll(c => c.Kind == 4); //同じファイル削除

                //ブック作成
                var book = CreateNewBook(strFileName);

                //シート無しのexcelファイルは保存は出来るが、開くとエラーが発生する
                book.CreateSheet("newSheet");

                //シート名からシート取得
                var sheet = book.GetSheet("newSheet");
                                                            
                var style = book.CreateCellStyle();
                style.DataFormat = book.CreateDataFormat().GetFormat("yyyy/mm/dd");//日付表示するために書式変更

                var style2 = book.CreateCellStyle();
                style2.DataFormat = book.CreateDataFormat().GetFormat("#,###");//表示するために書式設定

                //枠線設定
                var style3 = book.CreateCellStyle();
                style3.BorderTop    = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft   = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight  = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                //表示位置設定
                var style4 = book.CreateCellStyle();
                style4.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

                //見出し設定
                for (int i = 0; i < 8; i++)
                {
                    WriteStyle(sheet, i, startrow - 1, style4);
                }

                excelset(sheet);//セルの設定変更

                foreach (var (item, index) in FolderSetDatas.Select((item, index) => (item, index)))
                {
                    WriteCell(sheet, 0, index + startrow, index+1);//No
                    WriteCell(sheet, 1, index + startrow, textBox4.Text + item.Foldername + "\\" + item.Filename);//変更されたファイル名
                    WriteCell(sheet, 2, index + startrow, item.Leftdate);//更新前日付
                    WriteCell(sheet, 3, index + startrow, item.Lefttime);//更新前時刻
                    WriteCell(sheet, 4, index + startrow, item.Rightdate);//更新後日付
                    WriteCell(sheet, 5, index + startrow, item.Righttime);//更新後時刻
                    WriteCell(sheet, 6, index + startrow, item.Rightsize); //更新後サイズ


                    WriteStyle(sheet, 2, index + startrow, style);
                    WriteStyle(sheet, 4, index + startrow, style);
                    WriteStyle(sheet, 6, index + startrow, style2);

                    for (int j = 0; j < 8; j++)
                    {
                        WriteStyle(sheet, j, index + startrow, style3);//枠線設定
                    }
                }
                //ブックを保存
                using (var fs = new FileStream(strFileName, FileMode.Create))
                {
                    book.Write(fs);
                }
            }
            finally
            {
                ;//空文
            }
        }

        //ブック作成
        static IWorkbook CreateNewBook(string filePath)
        {
            IWorkbook book;
            var extension = Path.GetExtension(filePath);

            // HSSF => Microsoft Excel(xls形式)(excel 97-2003)
            // XSSF => Office Open XML Workbook形式(xlsx形式)(excel 2007以降)
            if (extension == ".xls")
            {
                book = new HSSFWorkbook();
            }
            else if (extension == ".xlsx")
            {
                book = new XSSFWorkbook();
            }
            else
            {
                throw new ApplicationException("CreateNewBook: invalid extension");
            }

            return book;
        }

        /// <summary>
        /// excel設定用のデータを読み込み
        /// </summary>
        /// <param name="sheet"></param>
        private void excelset(ISheet sheet)
        {
            string fileName = Directory.GetCurrentDirectory() + @"\setlist.xml";
            if (File.Exists(fileName))
            {
                //xmlファイルを指定する
                XElement xml = XElement.Load(fileName);

                //メンバー情報のタグ内の情報を取得する
                IEnumerable<XElement> infos = from item in xml.Elements("設定内容") select item;


                foreach (var (item, index) in infos.Select((item, index) => (item, index)))
                {
                    WriteCell(sheet, index, startrow - 1, item.Element("name").Value);//見出しの設定
                    SetWidth(sheet, index, int.Parse(item.Element("width").Value));//列の幅設定
                }

                //メンバー情報のタグ内の情報を取得する
                IEnumerable<XElement> headings = from item in xml.Elements("見出し") select item;
                foreach (var (item, index) in headings.Select((item, index) => (item, index)))
                {
                    WriteCell(sheet, 0, index, item.Element("name").Value);
                }
            }
        }

        //セル設定(文字列用)
        public static void WriteCell(ISheet sheet, int columnIndex, int rowIndex, string value)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);

            cell.SetCellValue(value);
        }

        //セル設定(数値用)
        public static void WriteCell(ISheet sheet, int columnIndex, int rowIndex, double value)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);

            cell.SetCellValue(value);
        }

        //セル設定(日付用)
        public static void WriteCell(ISheet sheet, int columnIndex, int rowIndex, DateTime value)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);

            cell.SetCellValue(value);
        }

        //書式変更
        public static void WriteStyle(ISheet sheet, int columnIndex, int rowIndex, ICellStyle style)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
            cell.CellStyle = style;
        }

        /// <summary>
        /// セルの幅を設定する
        /// </summary>
        /// <param name="sheet">シート</param>
        /// <param name="rowIndex">行No</param>
        /// <param name="width">行の幅</param>
        public static void SetWidth(ISheet sheet, int rowIndex, int width)
        {
            sheet.SetColumnWidth(rowIndex, 256 * width);
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ファイルコピー用SelectFile.txtを作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyFileCreatrBtn_Click(object sender, EventArgs e)
        {
            string strFileName;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                strFileName = saveFileDialog2.FileName;
            }
            else
            {
                return;
            }
            FolderSetDatas.RemoveAll(c => c.Kind == 2); //古い日付しかないファイル
            FolderSetDatas.RemoveAll(c => c.Kind == 4); //同じファイル
            using (StreamWriter writer = new StreamWriter(strFileName, false, SJIS))
            {
                foreach (var item in FolderSetDatas)
                {
                    writer.WriteLine(Path.Combine(item.Foldername , item.Filename));
                }
            } 
        }

        private void FileCopyBtn_Click(object sender, EventArgs e)
        {
            //変更前
            filelistclass.setfoldername = SetFolderName1;
            if ((!String.IsNullOrEmpty(SetFolderName1)) && (Directory.Exists(SetFolderName1)))
            {
                filelistclass.FolderCopy(
                    filelistclass.CopyFileListCreate(WorkFolder, filelistclass.ListOfFiles()),
                    WorkFolder, true);
            }
            //変更後
            filelistclass.setfoldername = SetFolderName2;
            if ((!String.IsNullOrEmpty(SetFolderName2)) && (Directory.Exists(SetFolderName2)))
            {
                filelistclass.FolderCopy(
                    filelistclass.CopyFileListCreate(WorkFolder, filelistclass.ListOfFiles()),
                    WorkFolder, false);
            }
            MessageBox.Show("コピー完了");
        }

        /// <summary>
        /// 引数のListから比較対象外のファイルを削除する
        /// 設定ファイル exclude.txt
        /// </summary>
        private List<string> GetReadyList(string wfolder, List<String> folderlist)
        {
            if (!(folderlist?.Count > 0))
            {
                return folderlist; //空なら抜ける
            }
            List<string> extension = new List<string>();//管理しない拡張子
            List<string> unnecessary = new List<string>();//管理しないファイル

            string FileName = Path.Combine(wfolder, Excludefilename);//除外設定ファイル
            if (File.Exists(FileName))
            {
                IEnumerable<string> lines = File.ReadLines(FileName, SJIS);
                foreach (var line in lines.Where(c => c.Length > 2).Where(c => c.Substring(0, 2) == "*."))
                {
                    extension.Add(line);
                }

                foreach (var line in lines.Where(c => c.Length > 2).Where(c => c.Substring(0, 2) == "$$"))
                {
                    unnecessary.Add(line.Substring(2));
                }
            }
            else
            {
                return folderlist;
            }

            foreach (var sdat in extension) //指定された拡張子のファイルを削除する
            {
                folderlist.RemoveAll(c => Path.GetExtension(c).ToLower() == sdat.Substring(1).TrimEnd().ToLower());
            }

            foreach (var tdat in unnecessary) //管理しないファイルを削除する
            {
                folderlist.RemoveAll(c => Path.GetFileName(c).ToLower() == tdat.ToLower());
            }
            folderlist.RemoveAll(c => c.IndexOf("workarea", StringComparison.OrdinalIgnoreCase) >= 0);
            return folderlist;
        }
    }

    /// <summary>
    /// 比較したフォルダのデータをdataridviewに設定する為のクラス
    /// </summary>
    /// Kind 1:ファイルは異なります 2:左側のみ　3:右側のみ 4:ファイルは同一です
    class FoldetSetdata
    {
        public string Filename { get; private set; }
        public string Foldername { get; private set; }
        public string Compare { get; private set; }
        public string Leftdate { get; private set; }
        public string Lefttime { get; private set; }
        public string Leftsize { get; private set; }
        public string Rightdate { get; private set; }
        public string Righttime { get; private set; }
        public string Rightsize{ get; private set; }
        public string Extension { get; private set; }
        public int Kind { get; private set; }

        public FoldetSetdata() { }
        public FoldetSetdata(string filename, string foldername, string compare, string leftdate, string lefttime, string leftsize,
                             string rightdate, string righttime, string rightsize, string extension, int kind)
        {
            Filename = filename;
            Foldername = foldername;
            Compare = compare;
            Leftdate = leftdate;
            Lefttime = lefttime;
            Leftsize = leftsize;
            Rightdate = rightdate;
            Righttime = righttime;
            Rightsize = rightsize;
            Extension = extension;
            Kind = kind;
        }
    }
    class Pair
    {
        public string HeaderText;
        public int HeadetWidth;
    }
}
