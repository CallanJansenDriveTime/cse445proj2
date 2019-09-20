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
        public delegate void priceCutEvent(double saleValue);       // delegate-> so that priceCutEvent points to a method: void "" (double x)

        private double oldPrice = 0;
        public static event priceCutEvent priceCut;                 // actual event object being emitted
        public static bool isTerminated = false;
        private static Random rng = new Random();                   // random number generator
        private int priceCutCounter = 0;                            // to terminate thread after n price cuts
        // private OrderProcessing processor = new OrderProcessing();

        public Thread CreateThread()
        {
            Thread thread = new Thread(new ThreadStart(RunAirline));
            return thread;
        }

        public void StartThread(Thread newThread)
        {
            newThread.Start();
        }

        public void ReadOrder()
        {
            bool success;

            lock(MainClass.bufferCellRef)
            {
                Monitor.Wait(MainClass.bufferCellRef, 1000);
                do
                {
                    success = Monitor.Wait(MainClass.bufferCellRef, 1000);
                    double yo = MainClass.bufferCellRef.getOneCell();

                    Console.WriteLine("we got the goods, yo- " + yo);
                    Monitor.PulseAll(MainClass.bufferCellRef);

                } while (success);
            }
        }

        public void RunAirline()
        {
            while (priceCutCounter < 5)
            {
                PricingModel();
                Thread.Sleep(3000);
            }

            isTerminated = true;
            Console.WriteLine("Terminating Airline thread");
        }

        private void PricingModel()   // returns ticket value with RNG from 50-200 , price cut $30?
        {
            double randomPrice = rng.Next(5000, 20000) / 100.00D;
            Console.WriteLine(randomPrice);
            if (oldPrice > randomPrice)      // sale when ticket cheaper
            {
                Console.WriteLine("HUGE SALE AT: " + randomPrice);
                priceCut?.Invoke(randomPrice);                              // emit event
                priceCutCounter++;
                ReadOrder();
            }
            else
            {
                lock(MainClass.bufferCellRef)
                {
                    MainClass.bufferCellRef.setCurrentPrice(-1);
                }
            }
            oldPrice = randomPrice;
        }

        public void SubmitOrder(Order newOrder)      // ORDER OBJECT AS PARAM-> BUFFER SENDS IT
        {
                                // processor.StartThread(newOrder);
        }
    }
}
