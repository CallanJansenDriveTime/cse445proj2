using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Airline
    {
        public delegate void priceCutEvent(double saleValue);       // delegate-> so that priceCutEvent points to a method
        public static event priceCutEvent priceCut;                 // actual event object being emitted
        private OrderProcessing processor = new OrderProcessing();  // object for creating completed order threads
        private static Random rng = new Random();                   // random number generator for generating ticket prices
        private int priceCutCounter = 0;                            // counter to terminate thread after n price cuts
        private double oldPrice = 0;                                // keeps track of last ticket value for comparison

        public Thread CreateThread()
        {
            Thread thread = new Thread(new ThreadStart(RunAirline));
            return thread;
        }

        public void StartThread(Thread newThread)
        {
            newThread.Start();
        }

        public void RunAirline()                // airline thread runs here. generates new ticket price-> sleeps-> repeat until n price cut events
        {
            while (priceCutCounter < 5)
            {
                PricingModel();
                Thread.Sleep(2000);
            }

            Console.WriteLine("Terminating Airline thread");
        }

        public void ReadOrder()                 // waits for Monitor.Pulse from TravelAgency (when a buffer is set)
        {
            lock(MainClass.bufferCellRef.dataCells)
            {
                for(int i = 0; i < 2; i++)
                {
                    Monitor.Wait(MainClass.bufferCellRef.dataCells, 1000);
                    Monitor.Wait(MainClass.bufferCellRef.dataCells, 1000);
                    Order test1 = MainClass.bufferCellRef.getOneCell(0);
                    Order test2 = MainClass.bufferCellRef.getOneCell(1);
                    SubmitOrder(test1);
                    SubmitOrder(test2);
                    MainClass.bufferCellRef.resetCells();
                    MainClass._bufferPool.Release(2);
                }

                Monitor.Wait(MainClass.bufferCellRef.dataCells, 1000);
                Order test3 = MainClass.bufferCellRef.getOneCell(0);
                SubmitOrder(test3);
                MainClass.bufferCellRef.resetCells();
                MainClass._bufferPool.Release();
            }
        }

        private void PricingModel() 
        {
            double randomPrice = rng.Next(5000, 20000) / 100.00D;       // create ticket value from $50.00 - $200.00
            Console.WriteLine("Airline ticket price: {0:0.00}", randomPrice);

            if (oldPrice > randomPrice)                                 // priceCut event when ticket value is cheaper than before
            {
                Console.WriteLine("PriceCutEvent emitted.", randomPrice);
                priceCut?.Invoke(randomPrice);                          // emit event
                priceCutCounter++;
                ReadOrder();                                            // after event is emitted, need to read results from buffer
            }
            else
            {
                if(MainClass.bufferCellRef.currentOrder != null)        // if current order is outdated
                {
                    lock (MainClass.bufferCellRef.currentOrder)
                    {
                        MainClass.bufferCellRef.setCurrentOrder(null);  // reset it to null
                    }
                }
            }
            oldPrice = randomPrice;                                     // to keep track of last price for comparison
        }

        public void SubmitOrder(Order toBeSubmitted)                    // submit order to OrderProcessing Thread
        {

            Thread orderProcessingThread = new Thread(() => processor.StartThread(toBeSubmitted));
            orderProcessingThread.Start();
        }
    }
}
