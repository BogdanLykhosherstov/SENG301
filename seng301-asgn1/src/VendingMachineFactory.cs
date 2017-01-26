using System;
using System.Collections;
using System.Collections.Generic;

using Frontend1;


namespace seng301_asgn1 {
    /// <summary>
    /// Represents the concrete virtual vending machine factory that you will implement.
    /// This implements the IVendingMachineFactory interface, and so all the functions
    /// are already stubbed out for you.
    /// 
    /// Your task will be to replace the TODO statements with actual code.
    /// 
    /// Pay particular attention to extractFromDeliveryChute and unloadVendingMachine:
    /// 
    /// 1. These are different: extractFromDeliveryChute means that you take out the stuff
    /// that has already been dispensed by the machine (e.g. pops, money) -- sometimes
    /// nothing will be dispensed yet; unloadVendingMachine is when you (virtually) open
    /// the thing up, and extract all of the stuff -- the money we've made, the money that's
    /// left over, and the unsold pops.
    /// 
    /// 2. Their return signatures are very particular. You need to adhere to this return
    /// signature to enable good integration with the other piece of code (remember:
    /// this was written by your boss). Right now, they return "empty" things, which is
    /// something you will ultimately need to modify.
    /// 
    /// 3. Each of these return signatures returns typed collections. For a quick primer
    /// on typed collections: https://www.youtube.com/watch?v=WtpoaacjLtI -- if it does not
    /// make sense, you can look up "Generic Collection" tutorials for C#.
    /// </summary>
    public class VendingMachineFactory : IVendingMachineFactory {
        //Variables
        VM dummyVm;
        Dictionary<int, List<Coin>> coinBank = new Dictionary<int, List<Coin>>();
        Dictionary<int, List<Pop>> popBank = new Dictionary<int, List<Pop>>();

        public Dictionary<int, VM> vendingMachines { get; set; } = new Dictionary<int, VM>();
        int id =-1;
        int vmIndex;

        public VendingMachineFactory() {
             // TODO: Implement
        }

        public int createVendingMachine(List<int> coinKinds, int selectionButtonCount) {
            // TODO: Implement
            //goin through the list of coins, making sure they arent repeated
            //Keeps track of objects created
            //coin is already in the list
            int currentIndex = 0;
            foreach (int coin in coinKinds)
            {
                for(int i = 0; i < currentIndex; i++)
                {
                    if(coinKinds[i] == coin)
                    {
                        throw new Exception("The coin already exists, please try again. ");
                    }
                    
                }
                currentIndex++;
                //coin is negative
                if (coin <= 0)
                {
                    throw new Exception("The coin is negative or zero, please try again. ");

                }
            }
       
            vmIndex = ++id;
            vendingMachines.Add(vmIndex, new VM(coinKinds, selectionButtonCount));
            return vmIndex;
        }

        public void configureVendingMachine(int vmIndex, List<string> popNames, List<int> popCosts) {
            // TODO: Implement
            //First we check for errors:
            if(popNames.Count != popCosts.Count)
            {
                throw new Exception("The number of names doesn't match the number of costs for the pops. Please try again");
            }
            else
            {
                foreach(int pop in popCosts)
                {
                    if (pop <= 0)
                    {
                        throw new Exception("One of the costs is zero or less. Please try again");
                    }
                }
            }
            if (vendingMachines[vmIndex].Equals(null))
            {
                throw new Exception("Tried to configure a non-existent vending machine. Please create one THEN configure it.");

            }
            //Then we assign appropirate values to the dummy vending machine and replace the indexed vending machine with it
            dummyVm = vendingMachines[vmIndex];
            dummyVm.popNames = popNames;
            dummyVm.popCosts = popCosts;
            vendingMachines[vmIndex] = dummyVm; 
        }

        public void loadCoins(int vmIndex, int coinKindIndex, List<Coin> coins) {
            dummyVm = vendingMachines[vmIndex];
            //checking if its the right type of coin
            if(dummyVm.coinKinds[coinKindIndex] != coins[0].Value)
            {
                throw new Exception ("Inserting the wrong kind of coin into the coin chute. Please try again with the right coin!");
            }
            //else proceed with the operation
            else
            {
                dummyVm.coinBank.Add(coinKindIndex, coins);
                vendingMachines[vmIndex] = dummyVm;
            }

        }

        public void loadPops(int vmIndex, int popKindIndex, List<Pop> pops) {
            // TODO: Implement
            dummyVm = vendingMachines[vmIndex];
            if(!(dummyVm.popNames[popKindIndex].Equals(pops[0].Name)))
            {
                throw new Exception("Inserting the wrong kind of pop into the pop chute. Please try again with the right coin!");

            }
            else
            {
                dummyVm.popBank.Add(popKindIndex, pops);
                vendingMachines[vmIndex] = dummyVm;
            }
        }

        public void insertCoin(int vmIndex, Coin coin) {
            dummyVm = vendingMachines[vmIndex];
            //checking if this type of the coin matches any of the coin chutes
            if (!(dummyVm.coinKinds.Contains(coin.Value)))
            {
                Console.WriteLine("Wrong coin inserted! Dispensing to the delivery chute!");
                dummyVm.deliveryChute.Add(coin);
            }
            //insert the coin **FINISH LATER**
            else
            {

                //adding to already existing chutes

                //bool flag = false;
                //for(int i = 0; i< dummyVm.coinBank.Count; i++)
                //{
                //    List<Coin> coinList = dummyVm.coinBank[i];
                //    for(int j = 0; j < coinList.Count; j++)
                //    {
                //        Coin singleCoin = coinList[j];
                //        if (coin.Value == singleCoin.Value)
                //        {
                //            dummyVm.coinBank[i].Add(coin);
                //            flag = true;
                //            break;
                //        }
                //    }
                //}  
                //adding new coin shute if it has not already been loaded by default, making sure to not double add checking the flag
                //if ((dummyVm.coinKinds.Contains(coin.Value))&&(!flag))
                //{
                //    List<Coin> additionalCoin = new List<Coin>();
                //    additionalCoin.Add(coin);
                //    int index = dummyVm.coinKinds.IndexOf(coin.Value);
                //    dummyVm.coinBank.Add(index, additionalCoin);
                //}
                dummyVm.customerCoinBank.Add(coin);
                vendingMachines[vmIndex] = dummyVm;
            }
        }
        public void pressButton(int vmIndex, int value) {
            dummyVm = vendingMachines[vmIndex];
            //checking for errors
            if((0 > value) || (dummyVm.selectionButtonCount < value))
            {
                throw new Exception("The button index is greater than the number of buttons or is negative!");
            }
            //actual code
            int price = dummyVm.popCosts[value];
            int pricePayed = 0;

            foreach (Coin coin in dummyVm.customerCoinBank)
            {
                pricePayed += coin.Value;
            }
            //if customer didnt pay enough
            if (pricePayed < price)
            {
                Console.WriteLine("Not enough money inserted. Keeping da bling suckaaa");
            }
            
            else
            {
                //first give customer the pop
                Pop pop = dummyVm.popBank[value][0];
                dummyVm.deliveryChute.Add(pop);
                dummyVm.popBank[value].Remove(pop);
                //then calculate the change and consume the money from customer bank
                int iterator = -1;
                foreach (Coin item in dummyVm.customerCoinBank)
                {
                    if ((pricePayed > 0)&&(pricePayed>=item.Value))
                    {
                        pricePayed -= item.Value;
                     
                        iterator++;
                    }
                    //if the remaining change is less than whats in the insertion chute, we move into the machine bank
                    else if ((pricePayed < item.Value))
                    {
                        if (pricePayed > 0)
                        {
                            int indexIterator = 0;
                            foreach (List<Coin> coins in dummyVm.coinBank.Values)
                            {
                                foreach (Coin coin in coins)
                                {
                                    if (pricePayed > 0)
                                    {
                                        pricePayed -= coin.Value;
                                        dummyVm.coinBank[indexIterator].Remove(coin);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                indexIterator++;
                            }
                        }
                    }
                }
                //deleting those items from the custBank
                for(int i = 0; i <= iterator; i++)
                {
                    dummyVm.customerCoinBank.RemoveAt(0);
                }
                //checking if even after dispensing everything from customerBank we still need change
                


            }
            vendingMachines[vmIndex] = dummyVm;
        }
        public List<Deliverable> extractFromDeliveryChute(int vmIndex) {
            // TODO: Implement
            return new List<Deliverable>();
        }
        public List<IList> unloadVendingMachine(int vmIndex) {
            // TODO: Implement
            return new List<IList>() {
                new List<Coin>(),
                new List<Coin>(),
                new List<Pop>() };
            }
    }

 
}