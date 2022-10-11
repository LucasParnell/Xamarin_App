using System.Collections.Generic;

namespace Connect4.Server
{
    public class InitResponse
    {
        public List<List<int>> grid { get; set; }
        public int playerSide { get; set; }
        public string uuid { get; set; }
    }
}
