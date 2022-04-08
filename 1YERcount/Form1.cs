using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1YERcount
{

   

    public partial class Form1 : Form
    {
        public static Panel lastpanel = null;

        public static int panelc = 0;

        public static Panel selected = null;

        public static Form2[] fom = new Form2[1000000];
        public static int fomsiz = 0;


        public static Panel[] panelI = new Panel[100000];

        //public static Form2 form2 = new Form2();

        public static string[] hour = { };
        public static string[] date = { };
        public static string[] nameI = { };

        bool moving;
        Point offset;
        Point original;

        void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            panel1.Capture = true;
            offset = MousePosition;
            original = this.Location;
        }

        void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!moving)
                return;

            int x = original.X + MousePosition.X - offset.X;
            int y = original.Y + MousePosition.Y - offset.Y;

            this.Location = new Point(x, y);
        }

        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            panel1.Capture = false;
        }

        public bool IsConnectedToInternet()
        {
            string host = "http://opshop.rf.gd/log.html?i=1";  
           bool result = false;
            WebClient clin = new WebClient();
            try
            {
                string rep = clin.DownloadString("http://opshop.rf.gd/log.html");
                if (rep != String.Empty)
                    return true;
            }
            catch { }
            return result;
        }

        public Form1()
        {
            //MessageBox.Show(Properties.Settings.Default.date_vals.ToString());

          //  Properties.Settings.Default.Reset();
            InitializeComponent();
            panel1.AutoScroll = true;
            if (Properties.Settings.Default.stolen == true) {
                MessageBox.Show("This was stolen or either the dev thought it would be great to fuck up");
                
                this.Close();
            }

            //form2.Show();
            //   panelc = Properties.Settings.Default.panelcount;

            if (IsConnectedToInternet() == true) {
                WebClient clin = new WebClient();
                string k = clin.DownloadString("https://pastebin.com/raw/KLv5DexB");
                if (k.Contains("false") == false) {
                    MessageBox.Show("This was stolen or either the dev thought it would be great to fuck up");
                    Properties.Settings.Default.stolen = true;
                    this.Close();
                }
            }

           // MessageBox.Show(Properties.Settings.Default.panelcount.ToString());

           

            for (int p = 0; p < Properties.Settings.Default.panelcount; p++)
            {
                panelc = panelc + 1;


                vScrollBar1.Maximum = panel1.VerticalScroll.Maximum;
                vScrollBar1.Minimum = panel1.VerticalScroll.Minimum;

                panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
                vScrollBar1.Value = panel1.VerticalScroll.Minimum;
                if (panelc >= 6)
                {
                    vScrollBar1.Enabled = true;
                }
                lastpanel = Properties.Settings.Default.lastpanel;

                Panel panels = new Panel();
                panels.Size = exform.Size;
                panels.Visible = true;
                panels.Cursor = exform.Cursor;


                selected = panels;

                // MessageBox.Show(panelI.Length.ToString());

                panelI.SetValue(panels, panelc - 1);
                //  MessageBox.Show(panelI[panelc-1].ToString());



                panels.BackColor = exform.BackColor;

                panels.BorderStyle = exform.BorderStyle;
                panels.Parent = panel1;
                panels.BackColor = Color.LightCyan;

                for (int c = 0; c < panel1.Controls.Count; c++)
                {
                    if (panel1.Controls[c].BackColor == Color.LightCyan)
                    {
                        panel1.Controls[c].BackColor = Color.White;
                    }
                }

                if (lastpanel != null)
                {
                    panels.Location = new Point(lastpanel.Location.X, lastpanel.Location.Y + 78);
                    lastpanel = panels;
                    Properties.Settings.Default.lastpanel = panels;
                }
                if (lastpanel == null)
                {
                    panels.Location = exform.Location;
                    lastpanel = panels;
                    Properties.Settings.Default.lastpanel = panels;
                }




                Properties.Settings.Default.Save();
                panel4.Location = new Point(lastpanel.Location.X, lastpanel.Location.Y + 78);
                panel1.Controls.Add(panels);
                panels.Name = "panel" + (panel1.Controls.Count + 1).ToString();
                panel1.Controls.SetChildIndex(panels, 0);



                for (int c = 0; c < exform.Controls.Count; c++)
                {
                    var t = exform.Controls[c].GetType();
                    var ctrl = exform.Controls[c];
                    if (t.Name == "Label")
                    {
                        Label lab = new Label();
                        lab.Location = ctrl.Location;
                        lab.Size = ctrl.Size;
                        lab.Text = ctrl.Text;
                        lab.Name = ctrl.Name + "1";
                        lab.BackColor = ctrl.BackColor;
                        lab.Font = ctrl.Font;
                        lab.ForeColor = ctrl.ForeColor;



                        panels.Controls.Add(lab);
                        panels.Controls.SetChildIndex(lab, 0);

                    }

                }

                panels.Click += (s, ev) =>
                {
                    for (int c = 0; c < panel1.Controls.Count; c++)
                    {
                        if (panel1.Controls[c].BackColor == Color.LightCyan)
                        {
                            panel1.Controls[c].BackColor = Color.White;
                        }
                    }
                    panel2.Visible = true;
                    textBox1.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button4.Visible = true;

                    selected = panels;

                    label5.Text = selected.Controls[0].Text;
                    label6.Text = selected.Controls[1].Text;
                    textBox1.Text = selected.Controls[2].Text;
                    panels.BackColor = Color.LightCyan;


                };

                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Start();
                int pog = panelc;
                timer.Tick += (obj, sen) =>
                {

                    // MessageBox.Show(panels.Controls[1].Text.Split('.')[0]);
                    int hrs = int.Parse(panels.Controls[1].Text.Split(':')[0]);
                    int mins = int.Parse(panels.Controls[1].Text.Split(':')[1]);
                    int sec = int.Parse(panels.Controls[1].Text.Split(':')[2]);


                    int day = int.Parse(panels.Controls[0].Text.Split(' ')[0]);

                    sec = sec - 1;

                    if (sec == 0)
                    {
                        sec = 59;
                        mins = mins - 1;
                    }
                    if (mins == 0)
                    {
                        mins = 59;
                        hrs = hrs - 1;
                    }
                    if (hrs == 0)
                    {
                        hrs = 24;
                        day = day - 1;
                    }
                    if (day == 0)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }


                    string finm = mins.ToString();
                    string finh = hrs.ToString();
                    string fins = sec.ToString();
                    string find = day.ToString();


                    if (mins.ToString().ToCharArray().Length == 1)
                    {
                        finm = mins.ToString().Insert(1, mins.ToString());
                        finm = mins.ToString().Insert(0, "0");
                    }
                    if (sec.ToString().ToCharArray().Length == 1)
                    {
                        fins = sec.ToString().Insert(1, sec.ToString());
                        fins = sec.ToString().Insert(0, "0");
                    }
                    if (hrs.ToString().ToCharArray().Length == 1)
                    {
                        finh = hrs.ToString().Insert(1, hrs.ToString());
                        finh = hrs.ToString().Insert(0, "0");
                    }
                    if (day.ToString().ToCharArray().Length == 1)
                    {
                        find = day.ToString().Insert(1, day.ToString());
                        find = day.ToString().Insert(0, "0");
                    }
                    if (day.ToString().ToCharArray().Length == 2)
                    {
                        find = day.ToString().Insert(1, day.ToString());
                        find = day.ToString().Insert(0, "0");
                    }

                    panels.Controls[1].Text = finh + ":" + finm + ":" + fins;

             //       form2.Controls[5].Text = fins;
             //       form2.Controls[4].Text = find + " : " + finh + " : " + finm + " :";

                    panels.Controls[0].Text = day.ToString();


                };
                selected.Controls[0].Click += (ooop, d) =>
                {


                    for (int c = 0; c < panel1.Controls.Count; c++)
                    {
                        if (panel1.Controls[c].BackColor == Color.LightCyan)
                        {
                            panel1.Controls[c].BackColor = Color.White;
                        }
                    }
                    panel2.Visible = true;
                    textBox1.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button4.Visible = true;
                    selected = panels;

                    label5.Text = selected.Controls[0].Text;
                    label6.Text = selected.Controls[1].Text;
                    textBox1.Text = selected.Controls[2].Text;
                    panels.BackColor = Color.LightCyan;



                };
                selected.Controls[1].Click += (ooop, d) =>
                {


                    for (int c = 0; c < panel1.Controls.Count; c++)
                    {
                        if (panel1.Controls[c].BackColor == Color.LightCyan)
                        {
                            panel1.Controls[c].BackColor = Color.White;
                        }
                    }
                    panel2.Visible = true;
                    textBox1.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button4.Visible = true;
                    selected = panels;

                    label5.Text = selected.Controls[0].Text;
                    label6.Text = selected.Controls[1].Text;
                    textBox1.Text = selected.Controls[2].Text;
                    panels.BackColor = Color.LightCyan;



                };
                selected.Controls[2].Click += (ooop, d) =>
                {

                    
                    for (int c = 0; c < panel1.Controls.Count; c++)
                    {
                        if (panel1.Controls[c].BackColor == Color.LightCyan)
                        {
                            panel1.Controls[c].BackColor = Color.White;
                        }
                    }
                    panel2.Visible = true;
                    textBox1.Visible = true;
                    button1.Visible = true;
                    button2.Visible = true;
                    button4.Visible = true;
                    selected = panels;

                    label5.Text = selected.Controls[0].Text;
                    label6.Text = selected.Controls[1].Text;
                    textBox1.Text = selected.Controls[2].Text;
                    panels.BackColor = Color.LightCyan;



                };

                selected.Controls[0].TextChanged += (f, d) =>
                {


                    label5.Text = selected.Controls[0].Text;



                };
                selected.Controls[1].TextChanged += (f, d) =>
                {


                    label6.Text = selected.Controls[1].Text;



                };


                panels.Controls[0].Text = Properties.Settings.Default.date_vals[p];
                panels.Controls[1].Text = Properties.Settings.Default.hour_vals[p];
                panels.Controls[2].Text = Properties.Settings.Default.names[p];


            }






            Properties.Settings.Default.firstlaunc = true;
            Properties.Settings.Default.Save();
        }

        private void Formclose(object sender, EventArgs e)
        {
            // Properties.Settings.Default.panel = panel1;
            Properties.Settings.Default.panelcount = panelc;






            StringCollection str = new StringCollection();

            for (int m = 0; m < panelc; m++)
            {

                str.Insert(m, panelI[m].Controls[0].Text);
            }

            Properties.Settings.Default.date_vals = str;





            StringCollection str1 = new StringCollection();

            for (int m = 0; m < panelc; m++)
            {
                str1.Insert(m, panelI.ElementAt(m).Controls[1].Text);
            }

            Properties.Settings.Default.hour_vals = str1;





            StringCollection names = new StringCollection();

            for (int m = 0; m < panelc; m++)
            {
                names.Insert(m, panelI.ElementAt(m).Controls[2].Text);
            }

            Properties.Settings.Default.names = names;
            Properties.Settings.Default.Save();

            Properties.Settings.Default.Save();

            Properties.Settings.Default.Save();
         //   MessageBox.Show(Properties.Settings.Default.date_vals.ToString());
           //  Properties.Settings.Default.Reset();
            Console.Read();
            //Properties.Settings.Default.Save();
            
        }



        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            label5.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button4.Visible = true;

            panelc = panelc + 1;

            Properties.Settings.Default.panelcount = panelc;

            vScrollBar1.Maximum = panel1.VerticalScroll.Maximum;
            vScrollBar1.Minimum = panel1.VerticalScroll.Minimum;

            panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
            vScrollBar1.Value = panel1.VerticalScroll.Minimum;
            if (panelc >= 6)
            {
                vScrollBar1.Enabled = true;
            }
            lastpanel = Properties.Settings.Default.lastpanel;

            Panel panels = new Panel();
            panels.Size = exform.Size;
            panels.Visible = true;
            panels.Cursor = exform.Cursor;

            //textBox1.Text = panels.Controls[2].Text;
            selected = panels;

            // MessageBox.Show(panelI.Length.ToString());

            panelI.SetValue(panels, panelc - 1);
            //  MessageBox.Show(panelI[panelc-1].ToString());



            panels.BackColor = exform.BackColor;

            panels.BorderStyle = exform.BorderStyle;
            panels.Parent = panel1;

            panels.BackColor = Color.LightCyan;

            for (int c = 0; c < panel1.Controls.Count; c++)
            {
                if (panel1.Controls[c].BackColor == Color.LightCyan)
                {
                    panel1.Controls[c].BackColor = Color.White;
                }
            }
            if (lastpanel != null)
            {
                panels.Location = new Point(lastpanel.Location.X, lastpanel.Location.Y + 78);
                lastpanel = panels;
                Properties.Settings.Default.lastpanel = panels;
            }
            if (lastpanel == null)
            {
                panels.Location = exform.Location;
                lastpanel = panels;
                Properties.Settings.Default.lastpanel = panels;
            }




            Properties.Settings.Default.Save();
            panel4.Location = new Point(lastpanel.Location.X, lastpanel.Location.Y + 78);
            panel1.Controls.Add(panels);
            panels.Name = "panel" + (panel1.Controls.Count + 1).ToString();
            panel1.Controls.SetChildIndex(panels, 0);



            for (int c = 0; c < exform.Controls.Count; c++)
            {
                var t = exform.Controls[c].GetType();
                var ctrl = exform.Controls[c];
                if (t.Name == "Label")
                {
                    Label lab = new Label();
                    lab.Location = ctrl.Location;
                    lab.Size = ctrl.Size;
                    lab.Text = ctrl.Text;
                    lab.Name = ctrl.Name + "1";
                    lab.BackColor = ctrl.BackColor;
                    lab.Font = ctrl.Font;
                    lab.ForeColor = ctrl.ForeColor;



                    panels.Controls.Add(lab);
                    panels.Controls.SetChildIndex(lab, 0);

                }

            }

            panels.Click += (s, ev) =>
            {
                for (int c = 0; c < panel1.Controls.Count; c++)
                {
                    if (panel1.Controls[c].BackColor == Color.LightCyan)
                    {
                        panel1.Controls[c].BackColor = Color.White;
                    }
                }
                label5.Visible = true;
                panel2.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                selected = panels;

                label5.Text = selected.Controls[0].Text;
                label6.Text = selected.Controls[1].Text;
                textBox1.Text = selected.Controls[2].Text;
                panels.BackColor = Color.LightCyan;

                selected.Controls[0].TextChanged += (p, d) =>
                {


                    label5.Text = selected.Controls[0].Text;



                };
                selected.Controls[1].TextChanged += (p, d) =>
                {


                    label6.Text = selected.Controls[1].Text;



                };


            };



            selected.Controls[0].Click += (p, d) =>
            {


                for (int c = 0; c < panel1.Controls.Count; c++)
                {
                    if (panel1.Controls[c].BackColor == Color.LightCyan)
                    {
                        panel1.Controls[c].BackColor = Color.White;
                    }
                }
                label5.Visible = true;
                panel2.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                selected = panels;

                label5.Text = selected.Controls[0].Text;
                label6.Text = selected.Controls[1].Text;
                textBox1.Text = selected.Controls[2].Text;
                panels.BackColor = Color.LightCyan;



            };
            selected.Controls[1].Click += (p, d) =>
            {


                for (int c = 0; c < panel1.Controls.Count; c++)
                {
                    if (panel1.Controls[c].BackColor == Color.LightCyan)
                    {
                        panel1.Controls[c].BackColor = Color.White;
                    }
                }
                label5.Visible = true;
                panel2.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                selected = panels;

                label5.Text = selected.Controls[0].Text;
                label6.Text = selected.Controls[1].Text;
                textBox1.Text = selected.Controls[2].Text;
                panels.BackColor = Color.LightCyan;



            };

            selected.Controls[2].Click += (ooop, d) =>
            {


                for (int c = 0; c < panel1.Controls.Count; c++)
                {
                    if (panel1.Controls[c].BackColor == Color.LightCyan)
                    {
                        panel1.Controls[c].BackColor = Color.White;
                    }
                }
                label5.Visible = true;
                panel2.Visible = true;
                textBox1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                selected = panels;

                label5.Text = selected.Controls[0].Text;
                label6.Text = selected.Controls[1].Text;
                textBox1.Text = selected.Controls[2].Text;
                panels.BackColor = Color.LightCyan;



            };

            selected.Controls[0].TextChanged += (p, d) =>
            {


                label5.Text = selected.Controls[0].Text;



            };
            selected.Controls[1].TextChanged += (p, d) =>
            {


                label6.Text = selected.Controls[1].Text;



            };


            panels.Controls[0].Text = "365";
            panels.Controls[1].Text = "23:59:59";
            selected.Controls[0].TextChanged += (p, d) =>
            {


                label5.Text = selected.Controls[0].Text;



            };
            selected.Controls[1].TextChanged += (p, d) =>
            {


                label6.Text = selected.Controls[1].Text;



            };
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Start();
            int pog = panelc;


            timer.Tick += (obj, sen) =>
            {

                // MessageBox.Show(panels.Controls[1].Text.Split('.')[0]);
                int hrs = int.Parse(panels.Controls[1].Text.Split(':')[0]);
                int mins = int.Parse(panels.Controls[1].Text.Split(':')[1]);
                int sec = int.Parse(panels.Controls[1].Text.Split(':')[2]);


                int day = int.Parse(panels.Controls[0].Text.Split(' ')[0]);

                sec = sec - 1;

                if (sec == 0)
                {
                    sec = 59;
                    mins = mins - 1;
                }
                if (mins == 0)
                {
                    mins = 59;
                    hrs = hrs - 1;
                }
                if (hrs == 0)
                {
                    hrs = 24;
                    day = day - 1;
                }
                if (day == 0)
                {
                    timer.Stop();
                    timer.Dispose();
                }


                string finm = mins.ToString();
                string finh = hrs.ToString();
                string fins = sec.ToString();
                string find = day.ToString();


                if (mins.ToString().ToCharArray().Length == 1)
                {
                    finm = mins.ToString().Insert(1, mins.ToString());
                    finm = mins.ToString().Insert(0, "0");
                }
                if (sec.ToString().ToCharArray().Length == 1)
                {
                    fins = sec.ToString().Insert(1, sec.ToString());
                    fins = sec.ToString().Insert(0, "0");
                }
                if (hrs.ToString().ToCharArray().Length == 1)
                {
                    finh = hrs.ToString().Insert(1, hrs.ToString());
                    finh = hrs.ToString().Insert(0, "0");
                }
                if (day.ToString().ToCharArray().Length == 1)
                {
                    find = day.ToString().Insert(1, day.ToString());
                    find = day.ToString().Insert(0, "0");
                }
                if (day.ToString().ToCharArray().Length == 2)
                {
                    find = day.ToString().Insert(1, day.ToString());
                    find = day.ToString().Insert(0, "0");
                }

                panels.Controls[1].Text = finh + ":" + finm + ":" + fins;

            //    form2.Controls[5].Text = fins;
            //    form2.Controls[4].Text = find + " : " + finh + " : " + finm + " :";

                panels.Controls[0].Text = day.ToString();


            };





            textBox1.Text = selected.Controls[2].Text;



        }




        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel1.VerticalScroll.Value = vScrollBar1.Value;
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {



            vScrollBar1.Maximum = panel1.VerticalScroll.Maximum;
            vScrollBar1.Minimum = panel1.VerticalScroll.Minimum;
            vScrollBar1.Value = panel1.VerticalScroll.Value;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {



            selected.Controls[2].Text = textBox1.Text;


        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected.Controls[0].Text = "365";
            selected.Controls[1].Text = "23:59:59";
            selected.Controls[0].Text = "365";
            selected.Controls[1].Text = "23:59:59";
            selected.Controls[0].Text = "365";
            selected.Controls[1].Text = "23:59:59";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.panelcount = panelc;


            for (int p = 0; p < fomsiz; p++)
            {
                fom[p].Close();
            }



            StringCollection str = new StringCollection();

            for (int m = 0; m < panelc; m++)
            {

                str.Insert(m, panelI[m].Controls[0].Text);
            }

            Properties.Settings.Default.date_vals = str;





            StringCollection str1 = new StringCollection();

            for (int m = 0; m < panelc; m++)
            {
                str1.Insert(m, panelI.ElementAt(m).Controls[1].Text);
            }

            Properties.Settings.Default.hour_vals = str1;





            StringCollection names = new StringCollection();

            for (int m = 0; m < panelc; m++)
            {
                names.Insert(m, panelI.ElementAt(m).Controls[2].Text);
            }

            Properties.Settings.Default.names = names;



            


            lastpanel = null;
            Properties.Settings.Default.lastpanel = null;

            Properties.Settings.Default.Save();




            int index = 0;

            bool ftime = true;

            panel2.Visible = false;
            label5.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button4.Visible = false;




            if (Properties.Settings.Default.panelcount == 1) {

                panel4.Location = new Point(panel4.Location.X, panel4.Location.Y - 78);
            }





            for (int p = 0; p < Properties.Settings.Default.panelcount; p++)
            {
                if (index == 0)
                {
                    for (int ddd = 0; ddd < panelI.Length; ddd++)
                    {
                        if (panelI[ddd] == selected)
                        {
                            index = ddd;
                        }
                    }
                }
                

                    if (ftime == true)
                    {

                        for (int ddd = 0; ddd < panelc; ddd++)
                        {
                            panelI[ddd].Parent = null;
                            panelI[ddd] = null;
                        }
                    ftime = false;
                    panelc = 0;
                }
                    




                if (p != index)
                {
                    panelc = panelc + 1;
                    if (panelc <= 6)
                    {
                        vScrollBar1.Enabled = false;
                    }

                    vScrollBar1.Maximum = panel1.VerticalScroll.Maximum;
                    vScrollBar1.Minimum = panel1.VerticalScroll.Minimum;

                    panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
                    vScrollBar1.Value = panel1.VerticalScroll.Minimum;
                    if (panelc >= 6)
                    {
                        vScrollBar1.Enabled = true;
                    }
                    lastpanel = Properties.Settings.Default.lastpanel;

                    Panel panels = new Panel();
                    panels.Size = exform.Size;
                    panels.Visible = true;
                    panels.Cursor = exform.Cursor;


                    selected = panels;

                    // MessageBox.Show(panelI.Length.ToString());

                    panelI.SetValue(panels, panelc - 1);
                    //  MessageBox.Show(panelI[panelc-1].ToString());



                    panels.BackColor = exform.BackColor;
                    panels.BackColor = Color.LightCyan;

                    for (int c = 0; c < panel1.Controls.Count; c++)
                    {
                        if (panel1.Controls[c].BackColor == Color.LightCyan)
                        {
                            panel1.Controls[c].BackColor = Color.White;
                        }
                    }

                    panels.BorderStyle = exform.BorderStyle;
                    panels.Parent = panel1;


                    if (lastpanel != null)
                    {
                        panels.Location = new Point(lastpanel.Location.X, lastpanel.Location.Y + 78);
                        lastpanel = panels;
                        Properties.Settings.Default.lastpanel = panels;
                    }
                    if (lastpanel == null)
                    {
                        panels.Location = exform.Location;
                        lastpanel = panels;
                        Properties.Settings.Default.lastpanel = panels;
                    }




                    Properties.Settings.Default.Save();
                    panel4.Location = new Point(lastpanel.Location.X, lastpanel.Location.Y + 78);
                    panel1.Controls.Add(panels);
                    panels.Name = "panel" + (panel1.Controls.Count + 1).ToString();
                    panel1.Controls.SetChildIndex(panels, 0);



                    for (int c = 0; c < exform.Controls.Count; c++)
                    {
                        var t = exform.Controls[c].GetType();
                        var ctrl = exform.Controls[c];
                        if (t.Name == "Label")
                        {
                            Label lab = new Label();
                            lab.Location = ctrl.Location;
                            lab.Size = ctrl.Size;
                            lab.Text = ctrl.Text;
                            lab.Name = ctrl.Name + "1";
                            lab.BackColor = ctrl.BackColor;
                            lab.Font = ctrl.Font;
                            lab.ForeColor = ctrl.ForeColor;



                            panels.Controls.Add(lab);
                            panels.Controls.SetChildIndex(lab, 0);

                        }

                    }

                    panels.Click += (s, ev) =>
                    {
                        for (int c = 0; c < panel1.Controls.Count; c++)
                        {
                            if (panel1.Controls[c].BackColor == Color.LightCyan)
                            {
                                panel1.Controls[c].BackColor = Color.White;
                            }
                        }
                        panel2.Visible = true;
                        textBox1.Visible = true;
                        button1.Visible = true;
                        button2.Visible = true;
                        button4.Visible = true;

                        selected = panels;

                        label5.Text = selected.Controls[0].Text;
                        label6.Text = selected.Controls[1].Text;
                        textBox1.Text = selected.Controls[2].Text;
                        panels.BackColor = Color.LightCyan;


                    };

                    Timer timer = new Timer();
                    timer.Interval = 1000;
                    timer.Start();
                    int pog = panelc;
                    timer.Tick += (obj, sen) =>
                    {

                        // MessageBox.Show(panels.Controls[1].Text.Split('.')[0]);
                        int hrs = int.Parse(panels.Controls[1].Text.Split(':')[0]);
                        int mins = int.Parse(panels.Controls[1].Text.Split(':')[1]);
                        int sec = int.Parse(panels.Controls[1].Text.Split(':')[2]);


                        int day = int.Parse(panels.Controls[0].Text.Split(' ')[0]);

                        sec = sec - 1;

                        if (sec == 0)
                        {
                            sec = 59;
                            mins = mins - 1;
                        }
                        if (mins == 0)
                        {
                            mins = 59;
                            hrs = hrs - 1;
                        }
                        if (hrs == 0)
                        {
                            hrs = 24;
                            day = day - 1;
                        }
                        if (day == 0)
                        {
                            timer.Stop();
                            timer.Dispose();
                        }


                        string finm = mins.ToString();
                        string finh = hrs.ToString();
                        string fins = sec.ToString();
                        string find = day.ToString();


                        if (mins.ToString().ToCharArray().Length == 1)
                        {
                            finm = mins.ToString().Insert(1, mins.ToString());
                            finm = mins.ToString().Insert(0, "0");
                        }
                        if (sec.ToString().ToCharArray().Length == 1)
                        {
                            fins = sec.ToString().Insert(1, sec.ToString());
                            fins = sec.ToString().Insert(0, "0");
                        }
                        if (hrs.ToString().ToCharArray().Length == 1)
                        {
                            finh = hrs.ToString().Insert(1, hrs.ToString());
                            finh = hrs.ToString().Insert(0, "0");
                        }
                        if (day.ToString().ToCharArray().Length == 1)
                        {
                            find = day.ToString().Insert(1, day.ToString());
                            find = day.ToString().Insert(0, "0");
                        }
                        if (day.ToString().ToCharArray().Length == 2)
                        {
                            find = day.ToString().Insert(1, day.ToString());
                            find = day.ToString().Insert(0, "0");
                        }

                        panels.Controls[1].Text = finh + ":" + finm + ":" + fins;

                      //  form2.Controls[5].Text = fins;
                     //   form2.Controls[4].Text = find + " : " + finh + " : " + finm + " :";

                        panels.Controls[0].Text = day.ToString();


                    };

                    selected.Controls[1].Click += (ooop, d) =>
                    {


                        for (int c = 0; c < panel1.Controls.Count; c++)
                        {
                            if (panel1.Controls[c].BackColor == Color.LightCyan)
                            {
                                panel1.Controls[c].BackColor = Color.White;
                            }
                        }
                        panel2.Visible = true;
                        textBox1.Visible = true;
                        button1.Visible = true;
                        button2.Visible = true;
                        button4.Visible = true;
                        selected = panels;

                        label5.Text = selected.Controls[0].Text;
                        label6.Text = selected.Controls[1].Text;
                        textBox1.Text = selected.Controls[2].Text;
                        panels.BackColor = Color.LightCyan;



                    };
                    selected.Controls[2].Click += (ooop, d) =>
                    {


                        for (int c = 0; c < panel1.Controls.Count; c++)
                        {
                            if (panel1.Controls[c].BackColor == Color.LightCyan)
                            {
                                panel1.Controls[c].BackColor = Color.White;
                            }
                        }
                        panel2.Visible = true;
                        textBox1.Visible = true;
                        button1.Visible = true;
                        button2.Visible = true;
                        button4.Visible = true;
                        selected = panels;

                        label5.Text = selected.Controls[0].Text;
                        label6.Text = selected.Controls[1].Text;
                        textBox1.Text = selected.Controls[2].Text;
                        panels.BackColor = Color.LightCyan;



                    };

                    selected.Controls[0].TextChanged += (f, d) =>
                    {


                        label5.Text = selected.Controls[0].Text;



                    };
                    selected.Controls[1].TextChanged += (f, d) =>
                    {


                        label6.Text = selected.Controls[1].Text;



                    };


                    panels.Controls[0].Text = Properties.Settings.Default.date_vals[p];
                    panels.Controls[1].Text = Properties.Settings.Default.hour_vals[p];
                    panels.Controls[2].Text = Properties.Settings.Default.names[p];


                }



            }

            //panelI[i].Parent = null;
            //selected = panelI[1];
            //panelc = panelc - 1;
            for (int c = 0; c < panel1.Controls.Count; c++)
            {
                if (panel1.Controls[c].BackColor == Color.LightCyan)
                {
                    panel1.Controls[c].BackColor = Color.White;
                }
            }






        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 kk = new Form2();

            Panel loc = selected;
            fomsiz = fomsiz + 1;
            kk.Controls[2].Controls[0].Text = loc.Controls[2].Text;
            fom[fomsiz - 1] = kk;
            loc.Controls[2].TextChanged += (l, p) =>
            {
                if (kk.Controls.Count >= 8)
                {
                    kk.Controls[2].Controls[0].Text = loc.Controls[2].Text;
                    int x = (kk.Controls[2].Size.Width - kk.Controls[2].Controls[0].Size.Width) / 2;
                    kk.Controls[2].Controls[0].Location = new Point(x, kk.Controls[2].Controls[0].Location.Y);
                }
            };
            loc.Controls[1].TextChanged += (o, d) =>
            {
                if (kk.Controls.Count >= 8)
                {
                    int hrs = int.Parse(loc.Controls[1].Text.Split(':')[0]);
                    int mins = int.Parse(loc.Controls[1].Text.Split(':')[1]);
                    int sec = int.Parse(loc.Controls[1].Text.Split(':')[2]);


                    int day = int.Parse(loc.Controls[0].Text.Split(' ')[0]);








                    string finm = mins.ToString();
                    string finh = hrs.ToString();
                    string fins = sec.ToString();
                    string find = day.ToString();


                    if (mins.ToString().ToCharArray().Length == 1)
                    {
                        finm = mins.ToString().Insert(1, mins.ToString());
                        finm = mins.ToString().Insert(0, "0");
                    }
                    if (sec.ToString().ToCharArray().Length == 1)
                    {
                        fins = sec.ToString().Insert(1, sec.ToString());
                        fins = sec.ToString().Insert(0, "0");
                    }
                    if (hrs.ToString().ToCharArray().Length == 1)
                    {
                        finh = hrs.ToString().Insert(1, hrs.ToString());
                        finh = hrs.ToString().Insert(0, "0");
                    }
                    if (day.ToString().ToCharArray().Length == 1)
                    {
                        find = day.ToString().Insert(1, day.ToString());
                        find = day.ToString().Insert(0, "0");
                    }
                    if (day.ToString().ToCharArray().Length == 2)
                    {
                        find = day.ToString().Insert(1, day.ToString());
                        find = day.ToString().Insert(0, "0");
                    }


                    kk.Controls[8].Text = fins;
                    kk.Controls[7].Text = find + " : " + finh + " : " + finm + " :";
                }
            };

            
            kk.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;

            WindowState = FormWindowState.Normal;
        }

        private void X_Click(object sender, EventArgs e)
        {
            for (int p = 0; p < fomsiz; p++)
            {
                fom[p].Close();
            }
            this.Close();
            
            
        }
    }
}
