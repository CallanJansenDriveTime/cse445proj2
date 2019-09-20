using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Order
    {
        private int senderId;           // travel agency thread Id
        private int creditCardNumber;   // between 3000, 8000 as an example
        private int amount;             // number of tickets to order
        private double unitPrice;       // price received from Airline Event
        public bool isAvailable;        // used for buffer

        public int getSenderId()
        {
            return senderId;
        }

        public void setSenderId(int id)
        {
            senderId = id;
        }

        public int getCreditCardNumber()
        {
            return creditCardNumber;
        }

        public void setCreditCardNumber(int ccNumber)
        {
            creditCardNumber = ccNumber;
        }

        public int getAmount()
        {
            return amount;
        }

        public void setAmount(int amt)
        {
            amount = amt;
        }

        public double getUnitPrice()
        {
            return unitPrice;
        }

        public void setUnitPrice(double price)
        {
            unitPrice = price;
        }
    }
}
