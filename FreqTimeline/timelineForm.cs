using System.Diagnostics;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace FreqTimeline
{
    public partial class timelineForm : Form
    {
        private static int FontSize = 10;

        private static int secondsLabelStep = 5;

        private static int channelHeight = 100;
        private static int channelWidth = 200;

        private static int timeHeight = 50;
        private static int timeWidth = 50;

        private bool PlayTimeline = false;
        private int workspaceWidth = 0;
        private int workspaceHeight = 0;

        private int loopSeconds = 0;

        private int secondsHeight = 10;
        private double secondsWidth = 0;

        private int labelHeight = 20;
        private int labelWidth = 0;
        List<Effect> effects;

        Bitmap bitmap;
        Graphics graphic;
        Pen darkgrayPen = new Pen(Color.DarkGray);
        Pen whitePen = new Pen(Color.White);
        Pen bluePen = new Pen(Color.Blue);
        Pen redPen = new Pen(Color.Red);
        Brush darkgrayBrush = new SolidBrush(Color.DarkGray);
        Brush greenBrush = new SolidBrush(Color.FromArgb(200, 6, 176, 37));
        Font font = new Font("Segoe UI", FontSize, FontStyle.Regular);

        public timelineForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            effects = new() {
                new () { Id = "fx01", Name = "Simple Red Vumeter" },
                new () { Id = "fx02", Name = "Simple Green Vumeter" },
                new () { Id = "fx03", Name = "Simple Blue Vumeter" },
                new () { Id = "fx04", Name = "Simple RGB Vumeter" },
                new () { Id = "fx05", Name = "Simple Classic Vumeter" },
                new () { Id = "fx06", Name = "Faded Classic Vumeter" },
                new () { Id = "fx07", Name = "Centered Classic Vumeter" },
                new () { Id = "fx08", Name = "Centered Color Vumeter" },
                new () { Id = "fx09", Name = "Flat Color Vumeter" },
                new () { Id = "fx10", Name = "Bouncing Ball" },
                new () { Id = "fx11", Name = "Flashing Lights" },
                new () { Id = "fx12", Name = "Reactive Flashing Lights" },
            };

            workspaceWidth = pictureBox1.Width;
            workspaceHeight = pictureBox1.Height;

            loopSeconds = Convert.ToInt32(textBox1.Text);

            secondsHeight = 10;
            secondsWidth = (workspaceWidth - channelWidth - 30.0) / loopSeconds;

            labelHeight = 20;
            labelWidth = workspaceWidth / loopSeconds / secondsLabelStep;

            InitTimeline();
        }

        private void playPauseButton_Click(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
            else
            {
                sw.Start();
            }

            playPauseButton.Text = sw.IsRunning ? "Pause" : "Play";
        }

        Stopwatch sw = new Stopwatch();
        private void timelineTimer_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                if (sw.ElapsedMilliseconds > loopSeconds * 1000)
                {
                    sw.Restart();
                }

                InitTimeline();

                int x = workspaceWidth - channelWidth - 30;
                int v = (int)((sw.ElapsedMilliseconds / 1000.0) * x / loopSeconds);

                graphic.DrawLine(redPen, channelWidth + v, timeHeight, channelWidth + v, workspaceHeight);
                pictureBox1.Image = bitmap;
            }
        }

        private void InitTimeline()
        {
            bitmap = new Bitmap(workspaceWidth, workspaceHeight);
            graphic = Graphics.FromImage(bitmap);
            graphic.Clear(Color.White);

            var channels = new List<Channel>
            {
                new Channel {Name = "Channel 1", Color = Color.FromArgb(230,255,255) },
                new Channel {Name = "Channel 2", Color = Color.FromArgb(255, 230, 230) },
            };


            for (int i = 0; i < channels.Count(); i++)
            {
                graphic.FillRectangle(new SolidBrush(channels[i].Color), 0, timeHeight + (channelHeight * i), workspaceWidth, channelHeight);
                graphic.DrawString(channels[i].Name, font, Brushes.Black, 10, timeHeight + (channelHeight * i) + 10);
            }

            for (int i = 0; i < channels.Count(); i++)
            {
                graphic.DrawLine(redPen, 0, timeHeight + (channelHeight * (i + 1)), workspaceWidth, timeHeight + (channelHeight * (i + 1)));
            }

            for (int i = 1; i <= loopSeconds; i++)
            {
                if (i % secondsLabelStep == 0)
                {
                    var time = new TimeOnly(0, 0, i);
                    graphic.DrawString(time.ToString("mm:ss"), font, Brushes.Black, channelWidth + (int)(secondsWidth * i) - (FontSize * 2), timeHeight - labelHeight - (FontSize * 2));
                    graphic.DrawLine(redPen, channelWidth + (int)(secondsWidth * i), timeHeight - labelHeight, channelWidth + (int)(secondsWidth * i), timeHeight);
                }
                else
                {
                    graphic.DrawLine(redPen, channelWidth + (int)(secondsWidth * i), timeHeight - secondsHeight, channelWidth + (int)(secondsWidth * i), timeHeight);
                }
            }

            graphic.DrawLine(redPen, 0, timeHeight, workspaceWidth, timeHeight);
            graphic.DrawLine(redPen, channelWidth, 0, channelWidth, workspaceHeight);


            pictureBox1.Image = bitmap;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Reset();
                sw.Stop();
                playPauseButton.Text = "Play";
                InitTimeline();
            }
        }
    }

    public class Channel
    {
        public string Name { get; set; }

        public Color Color { get; set; }
    }

    public class Effect
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ChannelEffect
    {
        public required Effect Effect { get; set; }

        public int Red { get; set; }

        public int Green { get; set; }

        public int Blue { get; set; }

        public int Aux1 { get; set; }

        public int Aux2 { get; set; }

        public int Aux3 { get; set; }
    }

}


