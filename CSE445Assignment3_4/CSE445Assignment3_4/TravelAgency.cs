using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CSE445Assignment3_4
{
    public class TravelAgency
    {
        static Random rng = new Random();               // used to start async

        public static void RunTravelAgency(int id)      // 5 TravelAgency threads will run here
        {
            while (true)
            {
                MainClass._saleEventPool.WaitOne();         // wait for a sale to happen
                MainClass._bufferPool.WaitOne();              // sale has happened, now need to wait for buffer spot to open       
                //Console.WriteLine("Thread {0} enters the semaphore.", id);
                Thread.Sleep(rng.Next(50, 350));        // so the two threads don't enter at the same time

                Monitor.Enter(MainClass.bufferCellRef);
                try
                {
                    Order tempOrder = MainClass.bufferCellRef.getCurrentOrder();
                    Order newOrder = new Order();
                    newOrder.setAmount(tempOrder.getAmount());
                    newOrder.setUnitPrice(tempOrder.getUnitPrice());
                    newOrder.setSenderId(id);
                    newOrder.setCreditCardNumber(rng.Next(3000, 8000));
                    newOrder.isAvailable = false;
                    MainClass.bufferCellRef.setOneCell(newOrder);

                    //tempOrder.setSenderId(id);
                    //tempOrder.setCreditCardNumber(rng.Next(3000, 8000));
                    //tempOrder.isAvailable = false;

                    //MainClass.bufferCellRef.setOneCell(MainClass.bufferCellRef.getCurrentPrice());
                    //Console.WriteLine("Thread {0} sets the buffer cell.", id);

                    Monitor.PulseAll(MainClass.bufferCellRef);
                    Monitor.Wait(MainClass.bufferCellRef);
                }
                finally
                {
                    Monitor.Exit(MainClass.bufferCellRef);
                    MainClass._bufferPool.Release();
                }
            }
        }

        public void ProcessOrder(double saleValue)
        {
            int amountOfTickets = (int)saleValue / 10;
            switch (amountOfTickets)
            {
                case 5:     // major sale $50-79
                case 6:
                case 7:
                    amountOfTickets = 30;
                    break;
                case 8:     // normal sale $80-109
                case 9:
                case 10:
                    amountOfTickets = 20;
                    break;
                case 11:    // minor sale $110-139
                case 12:
                case 13:
                    amountOfTickets = 10;
                    break;
                case 14:    // very minor sale $140-159
                case 15:
                    amountOfTickets = 5;
                    break;
                default:    // $160-200 too high of a price
                    Console.WriteLine("$160-200 too high of a price for a sale. Order not placed");
                    return; // return, do not create order
            }

            Order newOrder = new Order();
            newOrder.setAmount(amountOfTickets);
            newOrder.setUnitPrice(saleValue);

            lock (MainClass.bufferCellRef)
            {
                // MainClass.bufferCellRef.setCurrentPrice(saleValue);
                MainClass.bufferCellRef.setCurrentOrder(newOrder);
            }

            MainClass._saleEventPool.Release(5);
        }
    }
}
