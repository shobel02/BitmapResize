using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BitmapResize
{
    public partial class Form1 : Form
    {        
        /// <summary>
        /// 画像保存リスト
        /// </summary>
        List<System.Drawing.Imaging.ImageFormat> imageFormats = new List<System.Drawing.Imaging.ImageFormat>();
        List<System.Drawing.Drawing2D.InterpolationMode> interpolationModes = new List<System.Drawing.Drawing2D.InterpolationMode>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /// 画像の保存形式セット
            imageFormats.Add(System.Drawing.Imaging.ImageFormat.Jpeg);
            imageFormats.Add(System.Drawing.Imaging.ImageFormat.Png);
            imageFormats.Add(System.Drawing.Imaging.ImageFormat.Bmp);
            imageFormats.Add(System.Drawing.Imaging.ImageFormat.Tiff);
            imageFormats.Add(System.Drawing.Imaging.ImageFormat.Icon);

            int cnt = 0;
            /// コンボボックスに画像保存形式をセット
            foreach (var item in imageFormats)
            {
                comboBox1.Items.Add(imageFormats[cnt]);
                cnt++;
            }

            /// 初期値セット
            comboBox1.SelectedIndex = 0;

            /// 画像の変換形式セット
            interpolationModes.Add(System.Drawing.Drawing2D.InterpolationMode.Default);
            interpolationModes.Add(System.Drawing.Drawing2D.InterpolationMode.High);
            interpolationModes.Add(System.Drawing.Drawing2D.InterpolationMode.Low);
            interpolationModes.Add(System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);
            interpolationModes.Add(System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear);
            interpolationModes.Add(System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);

            cnt = 0;
            /// コンボボックスに画像変換形式をセット
            foreach (var item in imageFormats)
            {
                comboBox2.Items.Add(interpolationModes[cnt]);
                cnt++;
            }

            /// 初期値セット
            comboBox2.SelectedIndex = 0;
        }

        /// <summary>
        /// 取得画像表示イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                /// 画像取得
                Bitmap bmp = new Bitmap(textBox1.Text);
                /// 画像から横幅取得
                textBox2.Text = bmp.Width.ToString();
                /// 画像から縦幅取得
                textBox3.Text = bmp.Height.ToString();
                /// 画像表示
                pictureBox1.Image = bmp;                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// 画像変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                /// 描画先とするImageオブジェクトを作成する
                Bitmap canvas = new Bitmap(int.Parse(textBox2.Text), int.Parse(textBox3.Text));

                /// ImageオブジェクトのGraphicsオブジェクトを作成する
                Graphics g = Graphics.FromImage(canvas);

                /// Bitmapオブジェクトの作成
                Bitmap image = new Bitmap(pictureBox1.Image);
                /// 補間方法を指定する
                g.InterpolationMode = (System.Drawing.Drawing2D.InterpolationMode)comboBox2.SelectedItem;
                /// 画像を変換して描画する
                g.DrawImage(image, 0, 0, int.Parse(textBox2.Text), int.Parse(textBox3.Text));

                /// BitmapとGraphicsオブジェクトを破棄
                image.Dispose();
                g.Dispose();

                /// PictureBox1に表示する
                pictureBox1.Image = canvas;
            }
            catch (Exception)
            {
                MessageBox.Show("画像取り込みエラーまたはその他のエラー");
            }
        }

        /// <summary>
        /// チェックボックス状態変化イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            /// チェックボックスがONの時
            if (checkBox1.Checked == true)
            {
                /// 原寸大で表示
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            else
            {
                /// ピクチャーボックスのサイズに合わせて表示
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        /// <summary>
        /// 画像保存イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>C
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                /// 表示している画像からビットマップ生成
                Bitmap bmp = new Bitmap(pictureBox1.Image);

                /// 出力パス取得
                string path = "C:\\" + textBox4.Text;

                /// 出力先のディレクトリがなかった場合は作成
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                /// 画像保存
                bmp.Save("C:\\" + textBox4.Text + "\\" + textBox5.Text + "." + comboBox1.SelectedItem.ToString(), (System.Drawing.Imaging.ImageFormat)comboBox1.SelectedItem);
                bmp.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("未入力の項目またはその他のエラー");
            }
        }

        /// <summary>
        /// 画像取り消し処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                /// 初期化
                pictureBox1.Image = null;
                textBox1.Text = "";
            }
            catch (Exception)
            {

            }
        }


        #region ドラック＆ドロップ処理
        //ListBox1のDragEnterイベントハンドラ
        private void ListBox1_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            else
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
        }

        //ListBox1のDragDropイベントハンドラ
        private void ListBox1_DragDrop(object sender, DragEventArgs e)
        {
            //コントロール内にドロップされたとき実行される
            //ドロップされたすべてのファイル名を取得する
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //textBoxに追加する
            textBox1.Text = fileName[0];
        }
        #endregion
    }
}
