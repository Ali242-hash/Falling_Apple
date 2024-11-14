using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Falling_Apple
{
    public partial class Form1 : Form
    {
        Timer AppleFallTimer = new Timer();
        Timer HandMoveTimer = new Timer();
        Timer AppleSpwanTimer = new Timer();
        Timer PersonMoveTimer = new Timer();

        List<PictureBox> Apples = new List<PictureBox>();
        PictureBox AppleHeldByHand = null;

        bool MovingLeft = true;
        bool MovingBasket = false;
        bool IncreaseSpeed = false;
        bool IncreaseBask = false;

        int HitByHand =0;
        int BasketCapa = 50;

        int point = 0;
        Random rnd = new Random();
   
      

        public Form1()
        {
            InitializeComponent();
            Start();
            
        }

        void Start()
        {
            SetUpTime();
            AddApple();
            UpdatePoint();
            label1.Font = new Font(label1.Font.FontFamily, 20,FontStyle.Bold);
            
        }

        void SetUpTime()
        {
            AppleFallTimer.Interval = 100;
            HandMoveTimer.Interval = 20;
            AppleSpwanTimer.Interval = RandomSpeed();
         

            AppleFallTimer.Start();
            HandMoveTimer.Start();
            AppleSpwanTimer.Start();
            

            AppleFallTimer.Tick += AppleFallEvent;
            HandMoveTimer.Tick += HandMoveEvent;
            AppleSpwanTimer.Tick += (s, e) => AddApple();
           

        

        }

        void AppleFallEvent(object s,EventArgs e)
        {
          if(AppleHeldByHand == null)
            {
                for(int i = 0; i < Apples.Count; i++)
                {
                    PictureBox OneApple = Apples[i];

                    if (OneApple.Visible)
                        OneApple.Top += 2;

                    if (i > 0 && Apples[i - 1].Bottom + 20 <= OneApple.Top)
                        Apples[i - 1].Top += 2;

                    if (OneApple.Bounds.IntersectsWith(Hand.Bounds))
                    {
                        OneApple.Hide();
                        AppleHeldByHand = OneApple;
                        MovingBasket = true;
                        return;
                    }

                    if (OneApple.Top > this.ClientSize.Height)
                    {
                        this.Controls.Remove(OneApple);
                        Apples.Remove(OneApple);
                        i--;

                    }
                }
            }

        
        }


        void HandMoveEvent(object s,EventArgs e)
        {

                if (Hand.Bounds.IntersectsWith(Tree1.Bounds))
                {
                    HitByHand++;
                    if (HitByHand >= 10)
                    {
                        HitByHand = 0;
                        AddApple();
                    }
                }

          
           
            if (MovingLeft)
            {
                Hand.Left -= 3;
                Head.Left -= 3;
                Body.Left -= 3;

                if(Hand.Left < 0)
                {
                    MovingLeft = false;
                }
            }
          
            else
            {
                Hand.Left += 3;
                Head.Left += 3;
                Body.Left += 3;

                int screen = 70;

                if(Hand.Left > this.ClientSize.Width -Hand.Width - screen)
                {
                    MovingLeft = true;
                }

                else
                {
                    DropeAppleToBasket();
                    MovingBasket = false;
                }
            }
            

           


        }

        void AddApple()
        {
            PictureBox OneApple = new PictureBox();
            OneApple.Width = 15;
            OneApple.Height = 15;
            OneApple.BackColor = Color.Red;
            OneApple.Visible = true;
            OneApple.Top = (Tree.Height + 30);
            OneApple.Left = (Tree.Width + 30);
            this.Controls.Add(OneApple);
            Apples.Add(OneApple);

        }

        void DropeAppleToBasket()
        {
            if (AppleHeldByHand != null)
            {
                this.Controls.Remove(AppleHeldByHand);
                AppleHeldByHand = null;
                Apples.Remove(AppleHeldByHand);
                point++;

                if(point >=5 && point %5==0 && !IncreaseBask)
                {
                    BasketCapa += 5;
                    Basket.Width += 5;
                    point += 2;
                    IncreaseBask = true;
                }

                if(point>=30 && !IncreaseSpeed)
                {
                    if (AppleFallTimer.Interval > 1) AppleFallTimer.Interval--;
                    if (HandMoveTimer.Interval > 1) HandMoveTimer.Interval--;
                    IncreaseSpeed = true;
                }

                UpdatePoint();
                   

            }

        }

        int RandomSpeed()
        {
            return rnd.Next(1500, 2501);
        }


        void UpdatePoint()
        {
            label1.Text = $"Your point {point}";
        }

     
    }
}
