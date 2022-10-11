using Connect4.Server;
using System;
using Xamarin.Forms;

namespace Connect4
{
    internal class GameGrid
    {
        //GameGrid is used to store each Connect 4 Piece

        public const int PlayerState = 1;

        public int width;
        public int height;


        public GamePiece[,] gridEntries;
        public GameGrid(int width, int height)
        {

            this.width = width;
            this.height = height;

            //Array.Clear(gridEntries, 0, gridEntries.Length);
            gridEntries = new GamePiece[this.width, this.height];

            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    gridEntries[x, y] = new GamePiece(0, x, OnPieceTapped); //Initialise each entry with a GamePiece of state 0 (Empty)

                }


        }

        async void OnPieceTapped(object sender, EventArgs args)
        {
            //If there has been a win, return
            if (ServerComm.win != 0)
                return;

            GamePiece gamePiece = sender as GamePiece;


            int column = gamePiece.column;

            //Check for lowest entry in column
            int lowest = 0;
            for (int y = 0; y < height; y++)
            {
                int rowState = gridEntries[column, y].state;
                if (rowState == 0)
                    lowest = y;
                else
                    break;
            }


            if (lowest < height)
            {
                //Set the lowest entry to the player state
                GamePiece entry = gridEntries[column, lowest];

                //Fade Animation
                entry.Opacity = 0;
                entry.ChangeState(PlayerState);
                await entry.FadeTo(1, 400);

                //Server expects Coordinates to be reversed
                int[] lastMove = new int[] { lowest, column };

                //Creating an UpdateGame Task
                var updateTask = ServerComm.UpdateGame(lastMove);

                //When UpdateGame is done, Parse the response and update the grid
                await updateTask.ContinueWith(task =>
                {
                    ServerComm.ParseUpdateResponse(task.Result)
                    .ContinueWith(
                    async response =>
                    {
                        //Update the grid
                        if (ServerComm.win != 1)
                        {
                            //The AIs move is taken from the response
                            int[] AIlastMove = response.Result;

                            var aiEntry = gridEntries[AIlastMove[1], AIlastMove[0]];

                            //Small fade animation
                            aiEntry.Opacity = 0;
                            aiEntry.ChangeState(-PlayerState);
                            await aiEntry.FadeTo(1, 400);
                        }
                        response.Wait();
                    }
                    );
                });
            }
        }

        public void Reset()
        {

            for (int x = 0; x < this.width; x++)
                for (int y = 0; y < this.height; y++)
                {
                    gridEntries[x, y].ChangeState(0);

                }
        }


    }

}
