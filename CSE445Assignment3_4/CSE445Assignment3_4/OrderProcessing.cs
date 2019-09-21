using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class OrderProcessing
    {
        private double tax = 1.075;                     // assume sales tax rate of 7.5%
        private Random rng = new Random();              // generates random location charge

        public void StartThread(Order newOrder)
        {
            if(newOrder.getCreditCardNumber() > 8000 || newOrder.getCreditCardNumber() < 3000)  // validate cc # (arbitrary range 3000-8000)
            {
                Console.WriteLine("Invalid CC #. Order not processed.");
                return;
            }

            CalculateTotal(newOrder);
        }

        public void CalculateTotal(Order newOrder)
        {
            double locationCharge = rng.Next(50, 100);  // random location charge
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
