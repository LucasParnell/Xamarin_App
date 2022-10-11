using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connect4.Server
{
    public class ServerComm
    {
        //Basic Connect4 API i have written previously using Flask, NGINX and Azure
        const string serverUrl = "https://zenpanda.uk/";

        static HttpClient client = new HttpClient();



        const string initPost = "{\"uuid\": 0, \"playerSide\": 1}";


        //Current Game Data
        static string uuid;
        public static int win;



        public static void Reset()
        {
            uuid = "";
            win = 0;
            client = new HttpClient();

        }



        public static async Task<HttpResponseMessage> StartGame()
        {
            //To start the game, post a JSON string containing {'uuid': 0, 'playerSide': 1} (UUID is used on the web version when refreshing the page / session
            client.BaseAddress = new Uri(serverUrl);
            HttpContent content = new StringContent(initPost, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("api/initPlayer", content);
            return result;
        }


        public static async Task<HttpResponseMessage> UpdateGame(int[] lastMove)
        {
            //To update, post a JSON string containing the UUID and the last move made

            GameTurn gameTurn = new GameTurn();
            gameTurn.uuid = uuid;
            gameTurn.lastMove = lastMove;



            var serial = JsonSerializer.Serialize(gameTurn);


            HttpContent content = new StringContent(serial, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("api/gameTurn", content);


            return result;
        }


        //Could pass a template to the function
        public static async void ParseInitResponse(HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                //If the request was a success, Grab the UUID
                var data = JsonSerializer.Deserialize<InitResponse>(content);
                uuid = data.uuid;
            }
        }

        public static async Task<int[]> ParseUpdateResponse(HttpResponseMessage response)
        {
            var move = new int[2] { -1, -1 };
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                //If the request was a success, Grab the UUID
                var data = JsonSerializer.Deserialize<UpdateResponse>(content);
                win = data.win;
                OnWinEventArgs args = new OnWinEventArgs();
                args.player = win;
                if (win != 0)
                    Win?.Invoke(typeof(ServerComm), args);
                return data.newGrid.ToArray();
            }

            return move;
        }




        public static event EventHandler<OnWinEventArgs> Win;

    }
    public class OnWinEventArgs : EventArgs
    {
        public int player;
    }








}
