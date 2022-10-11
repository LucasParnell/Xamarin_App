using Connect4.Server;
using System;
using Xamarin.Forms;

namespace Connect4
{
    public partial class MainPage : ContentPage
    {
        const int GridSpacing = 9;
        const int GridWidth = 7;
        const int GridHeight = 6;

        GameGrid gameGrid;


        double horizontalOffset;
        double cellSize;

        void OnLayoutSizeChanged(object sender, EventArgs args)
        {
            Layout layout = sender as Layout;

            cellSize = layout.Width / GridWidth;

            horizontalOffset = layout.Width / 2 - (cellSize * GridWidth) / 2 - GridSpacing;

            UpdateGridView(gameGrid);

        }

        public MainPage()
        {
            InitializeComponent();

            gameGrid = new GameGrid(GridWidth, GridHeight); // A Conventional Connect 4 Grid is 6x7
            UpdateGridView(gameGrid);

            ServerComm.Win += OnWin;

            var startTask = ServerComm.StartGame();
            //When the task is done, grab the UUID from response
            startTask.ContinueWith(task => { ServerComm.ParseInitResponse(task.Result); });



        }


        private void UpdateTitle(int win)
        {
            string winString = "Connect 4";
            if (win == -1)
                winString = "AI Wins";
            else if (win == 1)
                winString = "Player Wins";

            TitleLabel.Text = winString;
        }


        public void OnRestartButton(object sender, EventArgs e)
        {
            ServerComm.Win -= OnWin;

            ServerComm.Reset();
            gameGrid.Reset();
            Device.BeginInvokeOnMainThread(() => { UpdateTitle(0); });
            ServerComm.Win += OnWin;

            var startTask = ServerComm.StartGame();
            //When the task is done, grab the UUID from response
            startTask.ContinueWith(task => { ServerComm.ParseInitResponse(task.Result); });
        }


        public void OnWin(object sender, OnWinEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => { UpdateTitle(e.player); });
        }



        private void UpdateGridView(GameGrid gameGrid)
        {
            //Increments through Game Pieces
            //Adds each piece to AbsoluteLayout

            GridLayout.Children.Clear();


            for (int x = 0; x < gameGrid.width; x++)
                for (int y = 0; y < gameGrid.height; y++)
                {
                    var entry = gameGrid.gridEntries[x, y];
                    Rectangle rect = new Rectangle(x * cellSize + GridSpacing + horizontalOffset / 2,
                               y * cellSize + GridSpacing / 2,
                                cellSize - GridSpacing,
                               cellSize - GridSpacing);


                    GridLayout.Children.Add(entry);
                    AbsoluteLayout.SetLayoutBounds(entry, rect);
                }


        }
    }
}
