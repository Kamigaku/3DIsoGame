using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility {
    public class Node {

        public int value;
        public int previous;
        public int shortestDistance;
        public List<int> neighbors;
        public bool fetched;

        public Node(int value) {
            this.value = value;
            this.previous = -1;
            this.shortestDistance = -1;
            this.neighbors = new List<int>();
            this.fetched = false;
        }

        public void addNeighbors(int value) {
            if(!this.neighbors.Contains(value))
                this.neighbors.Add(value);
        }

        public void reset() {
            this.previous = -1;
            this.shortestDistance = -1;
            this.fetched = false;
        }

    }
}
