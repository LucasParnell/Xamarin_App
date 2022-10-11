using System.Collections.Generic;

namespace Connect4.Server
{
    internal class UpdateResponse
    {
        //Responds with win and grid
        public List<int> newGrid { get; set; }
        public int win { get; set; }
    }
}
