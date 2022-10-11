using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4
{
    internal class UpdateResponse
    {
        //Responds with win and grid
        public List<int> newGrid { get; set; }
        public int win { get; set; }
    }
}
