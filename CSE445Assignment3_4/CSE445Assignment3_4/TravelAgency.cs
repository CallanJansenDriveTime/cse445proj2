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
        // private int totalThreads = 5;

        //public List<Thread> CreateThreads()
        //{
        //    for(int i = 0; i < totalThreads; i++)
        //    {
        //        //Thread thread = new Thread(() => ProcessOrder(saleValue));
        //        Thread thread = new Thread(new ThreadStart(RunTravelAgency));
        //        thread.Name = (i + 1).ToString();
        //        runningThreads.Add(thread);
        //    }

        //    return runningThreads;
        //}

        //public void StartThreads()
        //{
        //    foreach (Thread thread in runningThreads)
        //    {
        //        thread.Start();
        //        thread.Join();
        //    }
        //}
    
        public static void RunTravelAgency(int id)
        {
            while(!Airline.isTerminated)
            {
                Console.WriteLine("Thread {0} begins " + "and waits for the semaphore.", id);
                MainClass._pool.WaitOne();// Requesting a resource        

                MainClass.padding = MainClass.padding + 100; // Define padding interval        
                Console.WriteLine("Thread {0} enters the semaphore.", id);
                Thread.Sleep(4000 + MainClass.padding);// Sleep about 1 secondplus        
                Console.WriteLine("Thread {0} releases the semaphore.", id);
                Console.WriteLine("Thread {0} previous semaphore count: {1}",
                id, MainClass._pool.Release());// Release one resource
            }
        }

        //public void RunTravelAgency()
        //{
        //    while (!Airline.isTerminated)
        //    {
        //        Thread.Sleep(new Random().Next(500, 1000));
        //    }
        //    Console.WriteLine("Terminate travel agency");
        //}

        public void ProcessOrder(double saleValue)
        {
            //while (!Airline.isTerminated)
            //{
            //    Thread.Sleep(1000);
            //}
            //Console.WriteLine("Terminate travel agency");

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
            SendOrder(newOrder);
            


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
