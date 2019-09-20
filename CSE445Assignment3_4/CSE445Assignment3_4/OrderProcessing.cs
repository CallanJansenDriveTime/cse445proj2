using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class OrderProcessing
    {
        private double tax = 0.075;                     // assume sales tax rate of 7.5%
        private Random rng = new Random();              // used to start async

        public void StartThread(Order newOrder)
        {
            if(newOrder.getCreditCardNumber() > 8000 || newOrder.getCreditCardNumber() < 3000)  // validate cc #
            {
                Console.WriteLine("Invalid CC #. Order not processed.");
                return;
            }

            CalculateTotal(newOrder);
        }

        public void CalculateTotal(Order newOrder)
        {
            double locationCharge = rng.Next(50, 100);
            double total = (newOrder.getUnitPrice() * newOrder.getAmount() * tax) + locationCharge;

            Console.Write("Location fee: " + locationCharge);
            Console.Write(" --- # of tickets: " + newOrder.getAmount());
            Console.Write(" --- Unit Price: {0:0.00}", newOrder.getUnitPrice());
            Console.Write(" --- CC#: " + newOrder.getCreditCardNumber());
            Console.Write(" --- SenderId: " + newOrder.getSenderId());
            Console.WriteLine(" --- Total: {0:0.00}", total);
        }
    }
}
