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

        public static void RunTravelAgency(int id)
        {
            while (!Airline.isTerminated)
            {
                MainClass._eventPool.WaitOne();         // wait for a sale to happen

                MainClass._pool.WaitOne();              // sale has happened, now need to wait for buffer spot to open       
                Console.WriteLine("Thread {0} enters the semaphore.", id);
                Thread.Sleep(rng.Next(50, 350));

                Monitor.Enter(MainClass.bufferCellRef);
                try
                {
                    MainClass.bufferCellRef.setOneCell(MainClass.bufferCellRef.getCurrentPrice());
                    Console.WriteLine("Thread {0} sets the buffer cell.", id);
                    Monitor.PulseAll(MainClass.bufferCellRef);
                    Monitor.Wait(MainClass.bufferCellRef);
                }
                finally
                {
                    Monitor.Exit(MainClass.bufferCellRef);
                    MainClass._pool.Release();
                }
            }
        }

        public void ProcessOrder(double saleValue)
        {
            lock (MainClass.bufferCellRef)
            {
                MainClass.bufferCellRef.setCurrentPrice(saleValue);
            }
            MainClass._eventPool.Release(5);

            int numTix = (int)saleValue / 10;
            switch (numTix)
            {
                case 5:     // major sale $50-70
                case 6:
                case 7:
                    numTix = 30;
                    Console.WriteLine("Order 30 tickets at: " + saleValue);
                    break;
                case 8:
                case 9:
                case 10:
                    numTix = 20;
                    Console.WriteLine("Order 20 tickets at: " + saleValue);
                    break;
                case 11:
                case 12:
                    numTix = 10;
                    Console.WriteLine("Order 10 tickets at: " + saleValue);
                    break;
                default:
                    Console.WriteLine("Not a big enough sale. Ticket price too high.");
                    break;
            }

            Order newOrder = new Order();
            // SendOrder(newOrder);



            // then terminate thread

            // calculate # tix to order based on sale $$
            // specific ID of thread? -> Thread.CurrentThread.ManagedThreadId is an int
            // create OrderClass object, fill properties
            // SendOrder(newOrder);
        }

        private void SendOrder(Order newOrder)
        {
            // send order to multi cell buffer
            // buffer object -> MainClass.bufferCellRef.setBUffer
        }
    }
}
