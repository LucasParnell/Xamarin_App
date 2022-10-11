using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4
{
    public class InitResponse
    {
        public List<List<int>> grid { get; set; }
        public int playerSide { get; set; }
        public string uuid { get; set; }
    }
}
