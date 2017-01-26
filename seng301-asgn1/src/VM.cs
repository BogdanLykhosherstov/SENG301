using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend1;
namespace seng301_asgn1
{
    public class VM 
    {
        //variables
        public List<Deliverable> deliveryChute = new List<Deliverable>();
        public List<Coin> customerCoinBank = new List<Coin>();
        public Dictionary<int, List<Coin>> coinBank = new Dictionary<int, List<Coin>>();
        public Dictionary<int, List<Pop>> popBank = new Dictionary<int, List<Pop>>();
        public List<Coin> coins = new List<Coin>();
        public List<int> coinKinds;  public List<Pop> pops;
        public List<string> popNames;
        public List<int> popCosts;
        public int selectionButtonCount;
        //constructor
        public VM(List<int> coinKinds, int selectionButtonCount)
        {
            this.coinKinds = coinKinds;
            this.selectionButtonCount = selectionButtonCount;
        }
        public VM()
        {

        }
    }
}
