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

namespace filecomp
{
    public partial class FileCompForm : Form
    {
        public string FileName1 { get; set; }
        public string FileName2 { get; set; }
        private List<int> RowDatas = new List<int>();//差分の行を記憶する
        Encoding SJIS = Encoding.GetEncoding("Shift_JIS");

        public FileCompForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 指定されたファイル名の取得をして内容の比較・表示を行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            //DataGridView1のはじめの列のテキストを変更する
            dataGridView1.Columns[0].HeaderText = "行番号";
            dataGridView1.Columns[1].HeaderText = FileName1;
            dataGridView1.Columns[2].HeaderText = "行番号";
            dataGridView1.Columns[3].HeaderText = FileName2;
            FileComp(FileName1, FileName2);
        }

        /// <summary>
        /// ファイルの内容を比較する
        /// </summary>
        /// <param name="FileName1">比較ファイル名1</param>
        /// <param name="FileName2">比較ファイル名2</param>
        private void FileComp(string FileName1,string FileName2)
        {
            List<FileData> Change_file_list1 = new List<FileData>();
            List<FileData> Change_file_list2 = new List<FileData>();
            int Read_file_list_offset1 = 0;
            int Read_file_list_offset2 = 0;
            int diff_location1 = 0; //差分の位置
            int diff_location2 = 0; //差分の位置
            int index2, index3;
            int Read_file_list_offset_position1 = 0;//検索の位置　行番号
            int Read_file_list_offset_position2 = 0;//検索の位置　行番号

            try
            {
                if ((string.IsNullOrEmpty(FileName1)) || (string.IsNullOrEmpty(FileName2)))
                {
                    MessageBox.Show("ファイル名が入力されていません");
                    return;
                }

                List<FileData> Read_file_list1 = new List<FileData>();
                List<FileData> Read_file_list2 = new List<FileData>();

                //ファイルから読み込んだテキストと行番号を代入する
                foreach(var q3data in File.ReadLines(FileName1, SJIS).ToList().Select((d3,index) => new {d3 ,index}))
                {
                    Read_file_list1.Add(new FileData(q3data.index+1, q3data.d3));
                }
                foreach (var q4data in File.ReadLines(FileName2, SJIS).ToList().Select((d4, index) => new { d4, index }))
                {
                    Read_file_list2.Add(new FileData(q4data.index+1, q4data.d4));
                }


                int Read_file_list_count1 = Read_file_list1.Count();
                int Read_file_list_count2 = Read_file_list2.Count();

                //countが少ないシーケンスを大きいシーケンスにcountを合わせる
                if (Read_file_list_count1 > Read_file_list_count2)
                {
                    for (int i = 0; i < Read_file_list_count1 - Read_file_list_count2; i++)
                    {
                        Read_file_list2.Add(new FileData(-1, " "));
                    }

                }
                else
                {
                    for (int i = 0; i < Read_file_list_count2 - Read_file_list_count1; i++)
                    {
                        Read_file_list1.Add(new FileData(-1, " "));
                    }
                }

                foreach (var sdata in Read_file_list1.Select((v3, index) => new { v3, index }))
                {
                    Read_file_list_offset_position1 = sdata.index + Read_file_list_offset1;
                    Read_file_list_offset_position2 = sdata.index + Read_file_list_offset2;

                    if ((Read_file_list_offset_position1 < Read_file_list_count1) && (Read_file_list_offset_position2 < Read_file_list_count2))
                    {
                        if (Read_file_list1[Read_file_list_offset_position1].Filetext == Read_file_list2[Read_file_list_offset_position2].Filetext) //テキスト一致
                        {
                            Change_file_list1.Add(Read_file_list1[Read_file_list_offset_position1]);
                            Change_file_list2.Add(Read_file_list2[Read_file_list_offset_position2]);
                        }
                        else //テキスト不一致　
                        {
                            diff_location1 = 0;
                            diff_location2 = 0;

                            //skipで飛ばしてそれ以降のデータをListにする
                            var results1 = Read_file_list2.Select(s => s.Filetext).Skip(Read_file_list_offset_position2).ToList();
                            //resultsの中で検索する　検索するデータがnullでないこと resultsから見つからない場合nullを返す
                            if (!(String.IsNullOrEmpty(Read_file_list1[Read_file_list_offset_position1].Filetext)) && (results1.FirstOrDefault(c => c == Read_file_list1[Read_file_list_offset_position1].Filetext) != null))
                            {
                                index2 = results1.FindIndex(c => c == Read_file_list1[Read_file_list_offset_position1].Filetext);//query4から見つかった
                            }
                            else
                                index2 = 0;

                            if (index2 > 0)//Read_file_list2から見つかった
                            {
                                if (index2 < 100)//100以上なら差分が大きすぎて別の物と判断する
                                    diff_location2 = index2;
                            }
                            else //Read_file_list2から見つかっていないのでRead_file_list1から検索を行う
                            {
                                //skipで飛ばしてそれ以降のデータをListにする
                                var results2 = Read_file_list1.Select(s => s.Filetext).Skip(Read_file_list_offset_position1).ToList();
                                //results2の中で検索する　検索するデータがnullでないこと results2から見つからない場合nullを返す
                                if (!(String.IsNullOrEmpty(Read_file_list2[Read_file_list_offset_position2].Filetext)) && (results2.FirstOrDefault(c => c == Read_file_list2[Read_file_list_offset_position2].Filetext) != null))
                                {
                                    index3 = results2.FindIndex(c => c == Read_file_list2[Read_file_list_offset_position2].Filetext);
                                }
                                else
                                    index3 = 0;

                                if (index3 < 100) //100以上なら差分が大きすぎて別の物と判断する
                                    diff_location1 = index3;
                            }


                            if ((diff_location1 == 0) && (diff_location2 > 0))
                            {

                                foreach (var item in Read_file_list2.Skip(Read_file_list_offset_position2).Take(diff_location2))//skipでとばして差分の数だけ取り出している
                                {
                                    Change_file_list1.Add(new FileData(-1, " "));
                                    Change_file_list2.Add(item);
                                }
                                Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1));
                                Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2 + diff_location2));
                                Read_file_list_offset2 = Read_file_list_offset2 + diff_location2;

                            }
                            else
                            if ((diff_location2 == 0) && (diff_location1 > 0))
                            {
                                foreach (var item in Read_file_list1.Skip(Read_file_list_offset_position1).Take(diff_location1))//skipでとばして差分の数だけ取り出している
                                {
                                    Change_file_list1.Add(item);
                                    Change_file_list2.Add(new FileData(-1, " "));
                                }

                                Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1 + diff_location1));
                                Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2));
                                Read_file_list_offset1 = Read_file_list_offset1 + diff_location1;
                            }
                            else 
                            if ((diff_location2 == 0) && (diff_location1 == 0))
                            {
                                if (Read_file_list1.Count() > 1){
                                   if (Read_file_list1[Read_file_list_offset_position1 + 1].Filetext == Read_file_list2[Read_file_list_offset_position2].Filetext)
                                   {
                                       Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1));
                                       Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1 + 1));
                                       Change_file_list2.Add(new FileData(-1, " ")); 
                                       Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2));
                                       Read_file_list_offset1 = Read_file_list_offset1 + 1;
                                   }
                                   else
                                   if (Read_file_list2[Read_file_list_offset_position2 + 1].Filetext == Read_file_list1[Read_file_list_offset_position1].Filetext)
                                   {
                                       Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2));
                                       Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2 + 1));
                                       Change_file_list1.Add(new FileData(-1, " "));
                                       Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1));
                                       Read_file_list_offset2 = Read_file_list_offset2 + 1;
                                   }
                                   else
                                   {
                                       Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1));
                                       Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2));
                                   }
                                }
                                else  //1行しか存在しない場合
                                {
                                   Change_file_list1.Add(Read_file_list1.ElementAtOrDefault(Read_file_list_offset_position1));
                                   Change_file_list2.Add(Read_file_list2.ElementAtOrDefault(Read_file_list_offset_position2));
                                }
                            }
                        }
                    }
                }

                dataGridView1.RowCount = Change_file_list1.Count();
                //並び替えができないようにする
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;

                var query17 = from data in Change_file_list1.Zip(Change_file_list2, (data5, data6) => new { data5, data6 }) select data;
                foreach(var pair in query17.Select((v4, index4) => new { v4, index4 }))
                {
                    var data5 = pair.v4.data5;
                    var data6 = pair.v4.data6;

                    dataGridView1[0, pair.index4].Value = data5.Filerowno;
                    dataGridView1[1, pair.index4].Value = data5.Filetext;
                    dataGridView1[2, pair.index4].Value = data6.Filerowno;
                    dataGridView1[3, pair.index4].Value = data6.Filetext;

                    if (data5.Filetext != data6.Filetext) //差分の箇所に色を塗る
                    {
                        if (data5.Filetext == " ")
                        {
                            dataGridView1[0, pair.index4].Style.BackColor = Color.LightGray;
                            dataGridView1[1, pair.index4].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            dataGridView1[0, pair.index4].Style.BackColor = Color.Yellow;
                            dataGridView1[1, pair.index4].Style.BackColor = Color.Yellow;
                        }
                        if (data6.Filetext == " ")
                        {
                            dataGridView1[2, pair.index4].Style.BackColor = Color.LightGray;
                            dataGridView1[3, pair.index4].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            dataGridView1[2, pair.index4].Style.BackColor = Color.Yellow;
                            dataGridView1[3, pair.index4].Style.BackColor = Color.Yellow;
                        }
                        RowDatas.Add(pair.index4 - 1); //差分1行前
                        RowDatas.Add(pair.index4);     //差分行番号
                        RowDatas.Add(pair.index4 + 1); //差分1行後 
                    }
                }
                
            }
            catch
            {
                ;//エラーがでてもスルーする
            }
            
        }
        
        /// <summary>
        /// 差分の行のデータをファイに書き込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int strindex=0;
            string strFileName;
            string separate = textBox2.Text;

            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(FileName1);
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
                string FileName = FileName1 + "と" + FileName2 + "の比較" + "\r\n";
                sw.Write(FileName);

                //現在の日付・時刻
                DateTime dtNow = DateTime.Now;
                sw.Write(dtNow);
                sw.Write("\r\n");

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
                        header = header + separate;
                    }
                    sw.Write(header);//ヘッダ
                }
                sw.Write("\r\n");
                sw.Write(separatecreate());
                sw.Write("\r\n");

                foreach (var index in RowDatas.Where(s => s < dataGridView1.RowCount).OrderBy(s => s).Distinct())//ソートして重複した要素を取り除く
                {
                    if ((index > strindex + 1) && (strindex != 0))
                    {
                        sw.Write(separatecreate()); //違う差分要素なら1行入れる
                        sw.Write("\r\n");
                    }
                    strindex = index;

                    for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                    {
                        // DataGridViewのセルのデータ取得
                        String str = "";
                        string dt = "";
                        if (dataGridView1.Rows[index].Cells[c].Value != null)
                        {
                            dt = "\"" + dataGridView1.Rows[index].Cells[c].Value.ToString() + "\"";
                        }
                        if (c < dataGridView1.Columns.Count - 1)
                        {
                            str = str + dt + separate;
                        }
                        else
                        {
                            str = str + dt;
                        }
                        // CSVファイル書込
                        sw.Write(str);
                    }
                    sw.Write("\r\n");
                }
                sw.Write(separatecreate());
                sw.Write("\r\n");
            }
        }

       /// <summary>
       /// セパレート文を作成する
       /// </summary>
       /// <returns>セパレート文</returns>
        private string separatecreate()
        {
            string separater1 = "";
            IEnumerable<string> strings =
               Enumerable.Repeat("-", 350);//同じ値("-")を指定分繰り返す

            foreach (String str in strings)
            {
                separater1 = separater1 + str;
            }
            return separater1+textBox2.Text+separater1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

  
    }
    class FileData
    {
        public int Filerowno { get; private set; }
        public string Filetext { get; private set; }
      
        public FileData() { }
        public FileData(int filerowno,string filetext)
        {
            Filerowno = filerowno;
            Filetext = filetext;
        }
    }
    
}
