using System;
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
        public string FileName1,FileName2;//Form1でデータ設定される
        private List<int> RowDatas = new List<int>();//差分の行を記憶する
        private List<FileData> query23 = new List<FileData>();
        private List<FileData> query24 = new List<FileData>();
        private List<int> query25 = new List<int>();
        private List<int> query26 = new List<int>();
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
            List<FileData> query15 = new List<FileData>();
            List<FileData> query16 = new List<FileData>();
            int aOffset = 0;
            int bOffset = 0;
            int acount = 0;
            int bcount = 0;
            int index2, index3;
            int query3Postion = 0;
            int query4Postion = 0;

            try
            {
                if ((string.IsNullOrEmpty(FileName1)) || (string.IsNullOrEmpty(FileName2)))
                {
                    MessageBox.Show("ファイル名が入力されていません");
                    return;
                }

                var query3 = (File.ReadLines(FileName1, System.Text.Encoding.GetEncoding("Shift_JIS"))).ToList();//左側のファイルを全て読み込んでListにする
                var query4 = (File.ReadLines(FileName2, System.Text.Encoding.GetEncoding("Shift_JIS"))).ToList();//右側のファイルを全て読み込んでListにする
                List<FileData> query13 = new List<FileData>();
                List<FileData> query14 = new List<FileData>();

                //ファイルから読み込んだテキストと行番号を代入する
                foreach(var q3data in query3.Select((d3,index) => new {d3 ,index}))
                {
                    query13.Add(new FileData(q3data.index+1, q3data.d3));
                }
                foreach (var q4data in query4.Select((d4, index) => new { d4, index }))
                {
                    query14.Add(new FileData(q4data.index+1, q4data.d4));
                }


                int q13Count = query13.Count();
                int q14Count = query14.Count();

                //countが少ないシーケンスを大きいシーケンスにcountを合わせる
                if (q13Count > q14Count)
                {
                    for (int i = 0; i < q13Count-q14Count; i++)
                    {
                        query14.Add(new FileData(-1, " "));
                    }
                }
                else
                {
                    for (int i = 0; i < q14Count - q13Count; i++)
                    {
                        query13.Add(new FileData(-1, " "));
                    }
                }

                foreach (var sdata in query13.Select((v3, index) => new { v3, index }))
                {
                    query3Postion = sdata.index + aOffset;
                    query4Postion = sdata.index + bOffset;

                    //if ((query3Postion < query13.Count() - 1) && (query4Postion < query14.Count() - 1))
                    if ((query3Postion < query13.Count()) && (query4Postion < query14.Count()))
                    {
                        if (query13[query3Postion].Filetext == query14[query4Postion].Filetext) //データ一致
                        {
                            query15.Add(query13[query3Postion]);
                            query16.Add(query14[query4Postion]);
                        }
                        else //データ不一致
                        {
                            acount = 0;
                            bcount = 0;

                            //skipで飛ばしてそれ以降のデータをListにする
                            var results1 = query14.Select(s => s.Filetext).Skip(query4Postion).ToList();
                            //resultsの中で検索する　検索するデータがnullでないこと resultsから見つからない場合nullを返す
                            if (!(String.IsNullOrEmpty(query13[query3Postion].Filetext)) && (results1.FirstOrDefault(c => c == query13[query3Postion].Filetext) != null))
                            {
                                index2 = results1.FindIndex(c => c == query13[query3Postion].Filetext);//query4から見つかった
                            }
                            else
                                index2 = 0;

                            if (index2 > 0)//query4から見つかった
                            {
                                if (index2 < 100)//100以上なら差分が大きすぎて別の物と判断する
                                    bcount = index2;
                            }
                            else //query4から見つかっていないのでquery3から検索を行う
                            {
                                //skipで飛ばしてそれ以降のデータをListにする
                                var results2 = query13.Select(s => s.Filetext).Skip(query3Postion).ToList();
                                //results2の中で検索する　検索するデータがnullでないこと results2から見つからない場合nullを返す
                                if (!(String.IsNullOrEmpty(query14[query4Postion].Filetext)) && (results2.FirstOrDefault(c => c == query14[query4Postion].Filetext) != null))
                                {
                                    index3 = results2.FindIndex(c => c == query14[query4Postion].Filetext);
                                }
                                else
                                    index3 = 0;

                                if (index3 < 100) //100以上なら差分が大きすぎて別の物と判断する
                                    acount = index3;
                            }


                            if ((acount == 0) && (bcount > 0))
                            {

                                foreach (var item in query14.Skip(query4Postion).Take(bcount))//skipでとばして差分の数だけ取り出している
                                {
                                    query15.Add(new FileData(-1, " "));
                                    query16.Add(item);
                                }
                                query15.Add(query13.ElementAtOrDefault(query3Postion));
                                query16.Add(query14.ElementAtOrDefault(query4Postion + bcount));
                                bOffset = bOffset + bcount;

                            }
                            else
                            if ((bcount == 0) && (acount > 0))
                            {
                                foreach (var item in query13.Skip(query3Postion).Take(acount))//skipでとばして差分の数だけ取り出している
                                {
                                    query15.Add(item);
                                    query16.Add(new FileData(-1, " "));
                                }

                                query15.Add(query13.ElementAtOrDefault(query3Postion + acount));
                                query16.Add(query14.ElementAtOrDefault(query4Postion));
                                aOffset = aOffset + acount;
                            }
                            else 
                            if ((bcount == 0) && (acount == 0))
                            {
                                if (query13.Count() > 1){
                                   if (query13[query3Postion + 1].Filetext == query14[query4Postion].Filetext)
                                   {
                                       query15.Add(query13.ElementAtOrDefault(query3Postion));
                                       query15.Add(query13.ElementAtOrDefault(query3Postion + 1));
                                       query16.Add(new FileData(-1, " ")); 
                                       query16.Add(query14.ElementAtOrDefault(query4Postion));
                                       aOffset = aOffset + 1;
                                   }
                                   else
                                   if (query14[query4Postion + 1].Filetext == query13[query3Postion].Filetext)
                                   {
                                       query16.Add(query14.ElementAtOrDefault(query4Postion));
                                       query16.Add(query14.ElementAtOrDefault(query4Postion + 1));
                                       query15.Add(new FileData(-1, " "));
                                       query15.Add(query13.ElementAtOrDefault(query3Postion));
                                       bOffset = bOffset + 1;
                                   }
                                   else
                                   {
                                       query15.Add(query13.ElementAtOrDefault(query3Postion));
                                       query16.Add(query14.ElementAtOrDefault(query4Postion));
                                   }
                                }
                                else  //1行しか存在しない場合
                                {
                                   query15.Add(query13.ElementAtOrDefault(query3Postion));
                                   query16.Add(query14.ElementAtOrDefault(query4Postion));
                                }
                            }
                        }
                    }
                }

                dataGridView1.RowCount = query15.Count();
                //並び替えができないようにする
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;

                var query17 = from data in query15.Zip(query16, (data5, data6) => new { data5, data6 }) select data;
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
                query23 = new List<FileData>(query15);//Listのコピー
                query24 = new List<FileData>(query16);//Listのコピー
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

        /// <summary>
        /// 関数の最初の行を検索//implementation
        /// </summary>
        private void procedure_search()
        {
            
            var query27 = new List<int>();

            int ino = query23.FindIndex(s => s.Filetext.Trim() == "implementation");

            //"implementation"以降の関数の行番号を取得
            foreach (var item in query23.Skip(ino).Where(c => (c.Filetext.TrimStart().Length >= 9) && (c.Filetext.TrimStart().Substring(0, 8) == "function" || c.Filetext.TrimStart().Substring(0, 9) == "procedure")))
                query25.Add(item.Filerowno);
            foreach (var item in query24.Skip(ino).Where(c => (c.Filetext.TrimStart().Length >= 9) && (c.Filetext.TrimStart().Substring(0, 8) == "function" || c.Filetext.TrimStart().Substring(0, 9) == "procedure")))
                query26.Add(item.Filerowno);

            foreach(var data in RowDatas)
            {
                
                int fno = query25.FirstOrDefault(c => c > data);//差分の行より大きい
                if(fno != 0)//見つかった
                {
                    int eno = query25.IndexOf(fno)-1;
                    if( eno >= 0  )             //差分の有る関数の先頭行番号 
                        query27.Add(eno);
                    query27.Add(query23.Take(fno).LastOrDefault(c => ((c.Filetext.TrimStart().Length >= 3)&&(c.Filetext.TrimStart().Substring(0, 3) == "end"))).Filerowno);//その関数内の最後の"end"の行番号
                }else
                {
                    query27.Add(query25.Last());//一番最後の関数の先頭行番号
                    query27.Add(query23.Last().Filerowno);//一番最後の行番号
                }
            }
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
