using System;
using System.Collections;
using System.Collections.Generic;

using Frontend1;


namespace seng301_asgn1 {
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
                dummyVm.deliveryChute.Add(coin);
                Console.WriteLine("Wrong coin inserted! Dispensing to the delivery chute!");        
            }
            //insert the coin **FINISH LATER**
            else
            {
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
            int change = pricePayed % price;
            //if customer didnt pay enough
            if (pricePayed < price)
            {
                //do nothing
            }
            
            else
            {
                //first give customer the pop
                Pop pop = dummyVm.popBank[value][0];
                dummyVm.deliveryChute.Add(pop);
                dummyVm.popBank[value].Remove(pop);
                //creating a copy of customerBank so we can iterate through it and delete items in the real one at the same time
                Dictionary<int, List<Coin>> coinBankCopy = new Dictionary<int, List<Coin>>();
                for(int i = 0; i < dummyVm.coinBank.Count;i++)
                {
                    coinBankCopy.Add(i, new List<Coin> (dummyVm.coinBank[i]));
                }
                //returns change in reverse order - from biggest coin to smallest coin

                for (int i = coinBankCopy.Count-1; i>=0;i--)
                {

                    for (int j=0;j<coinBankCopy.Count;j++)
                    {
                        if(change > 0)
                        {
                            if (change >= coinBankCopy[i][j].Value)
                            {
                                change -= coinBankCopy[i][j].Value;
                                Coin tempCoin =coinBankCopy[i][j];
                                //adding to delivery chute
                                dummyVm.deliveryChute.Add(tempCoin);
                                //deleting from the actual list
                                dummyVm.coinBank[i].Remove(tempCoin);
                            }
                               
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            vendingMachines[vmIndex] = dummyVm;
        }
        public List<Deliverable> extractFromDeliveryChute(int vmIndex) {
            // TODO: Implement
            List<Deliverable> tempDeliveryChute = new List<Deliverable>();
            dummyVm = vendingMachines[vmIndex];
            for(int i =0; i < dummyVm.deliveryChute.Count; i++)
            {
                tempDeliveryChute.Add(dummyVm.deliveryChute[i]);
            }
            dummyVm.deliveryChute.Clear();
            vendingMachines[vmIndex] = dummyVm;

            return tempDeliveryChute;
        }
        public List<IList> unloadVendingMachine(int vmIndex) {
            // TODO: Implement
            //coin bank copy
            dummyVm = vendingMachines[vmIndex];
            List<Coin> changeChute = new List<Coin>();
            List<Pop> popChute = new List<Pop>();
            List<Coin> madeChute = new List<Coin>();


            Dictionary<int, List<Coin>> coinBankCopy = new Dictionary<int, List<Coin>>();
            for (int i = 0; i < dummyVm.coinBank.Count; i++)
            {
                coinBankCopy.Add(i, new List<Coin>(dummyVm.coinBank[i]));
            }
            dummyVm.coinBank.Clear();

            for (int i = 0; i < coinBankCopy.Count; i++)
            {
                for(int j=0; j < coinBankCopy[i].Count; j++)
                {
                    changeChute.Add(coinBankCopy[i][j]);
                }
            }
                //pop bank copy
                Dictionary<int, List<Pop>> popBankCopy = new Dictionary<int, List<Pop>>();
            for (int i = 0; i < dummyVm.popBank.Count; i++)
            {
                popBankCopy.Add(i, new List<Pop>(dummyVm.popBank[i]));
            }
            dummyVm.popBank.Clear();
            for (int i = 0; i < popBankCopy.Count; i++)
            {
                for (int j = 0; j < popBankCopy[i].Count; j++)
                {
                    popChute.Add(popBankCopy[i][j]);
                }
            }

            //customerCoinBank copy
            for (int i = 0; i < dummyVm.customerCoinBank.Count; i++)
            {
                madeChute.Add(dummyVm.customerCoinBank[i]);
            }
            dummyVm.customerCoinBank.Clear();
            vendingMachines[vmIndex] = dummyVm;

            return new List<IList>() {
                //money in the change maker
                changeChute,
                //money we made
                madeChute,
                //unsold pops
                popChute };
            }
    }

 
}
