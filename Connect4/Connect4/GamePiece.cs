using System;
using Xamarin.Forms;

namespace Connect4
{
    internal class GamePiece : BoxView
    {

        public int column;
        public int state;


        public TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();


        public GamePiece(int state, int col, EventHandler tapHandler)
        {
            ChangeState(state);

            this.column = col;

            this.CornerRadius = 25;

            this.tapGestureRecognizer.Tapped += tapHandler;

            this.GestureRecognizers.Add(tapGestureRecognizer);
        }


        public void ChangeState(int state)
        {
            this.state = state;

            switch (state)
            {
                case 0:
                    {
                        this.Color = Color.White;
                    }
                    break;
                case -1:
                    {
                        this.Color = Color.Red;
                    }
                    break;
                case 1:
                    {
                        this.Color = Color.Orange;
                    }
                    break;
            }
        }
    }
}
