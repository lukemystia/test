using System;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;


namespace DigitalClock2
{
	public partial class Form1 : Form
	{
		//マウスクリック位置記憶
		private Point mousePoint;
		//日本表記
		private CultureInfo jpCluture = new CultureInfo("ja-JP");
		//表示フォント
		Font f = new Font("MS ゴシック", 16);
		//曜日表示位置
		Rectangle weekDayPosition = new Rectangle(140, 37, 300, 35);

		public Form1()
		{
			InitializeComponent();

			Screen[] allScreens = Screen.AllScreens;

			int screenWidthAll = 0;
			foreach (var items in allScreens)
			{
				screenWidthAll += items.Bounds.Width;
			}

			//魏同時の位置設定
			//起動時前回の位置に表示 ただし画面外の時は修正
			if (Properties.Settings.Default.MainForm_Left < 0 ||
				Properties.Settings.Default.MainForm_Left > screenWidthAll)
			{
				this.Left = 500;
			}
			else
			{
				this.Left = Properties.Settings.Default.MainForm_Left;
			}

			if (Properties.Settings.Default.MainForm_Top < 0 ||
				Properties.Settings.Default.MainForm_Top > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
			{
				this.Top = 500;
			}
			else
			{
				this.Top = Properties.Settings.Default.MainForm_Top;
			}

			//日本表記用
			jpCluture.DateTimeFormat.Calendar = new JapaneseCalendar();

			this.Refresh();
		}

		//フォーム上でマウスクリックされた時の処理
		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				//位置記憶
				mousePoint = new Point(e.X, e.Y);
			}
		}

		//マウスが動いた時の処理
		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				//位置移動
				this.Left += e.X - mousePoint.X;
				this.Top += e.Y - mousePoint.Y;
			}
		}

		//ダブルクリックで終了
		private void Form1_DoubleClick(object sender, EventArgs e)
		{
			this.Close();
		}

		//timer_Tickイベント(1000ミリ秒ごとに発生)
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			this.Refresh();	//再描写
			timer1.Start();
		}

		//終了時位置記憶
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//ウィンドウの値を Settings に格納
			Properties.Settings.Default.MainForm_Left = this.Left;
			Properties.Settings.Default.MainForm_Top = this.Top;
			//ファイルに保存
			Properties.Settings.Default.Save();

			Application.Exit();
		}

		//時刻等描写
		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			DateTime dt = DateTime.Now;

			e.Graphics.DrawString(dt.ToString("yyyy/MM dd HH:mm:ss"),
								  f,
								  Brushes.White,
								  new Rectangle(12, 9, 300, 35));

			e.Graphics.DrawString(dt.ToString("ggyy年", jpCluture),
								  f,
								  Brushes.White,
								  new Rectangle(12, 37, 110, 35));

			switch ((int)dt.DayOfWeek)
			{
				case 0:
					e.Graphics.DrawString(dt.ToString("dddd", jpCluture),
								  f,
								  Brushes.IndianRed,
								  weekDayPosition);
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					e.Graphics.DrawString(dt.ToString("dddd", jpCluture),
								  f,
								  Brushes.White,
								  weekDayPosition);
					break;
				case 6:
					e.Graphics.DrawString(dt.ToString("dddd", jpCluture),
								  f,
								  Brushes.LightBlue,
								  weekDayPosition);
					break;
				default:
					e.Graphics.DrawString("エラー",
								  f,
								  Brushes.Red,
								  weekDayPosition);
					break;
			}

		}
	}
}
