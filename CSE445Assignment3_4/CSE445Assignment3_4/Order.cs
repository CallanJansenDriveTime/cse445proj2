using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Order
    {
        private int senderId;          // ummmmmmmmm
        private int creditCardNumber; // between 5-7k?
        private int amount;         // #tix to order
        private double unitPrice;   // price received from Airline Event

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

        public void getUnitPrice(double price)
        {
            unitPrice = price;
        }

    }
}
