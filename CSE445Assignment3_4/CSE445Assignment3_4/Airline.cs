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
        private OrderProcessing processor = new OrderProcessing();

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
                // Monitor.Wait(MainClass.bufferCellRef, 1000);
                do
                {
                    success = Monitor.Wait(MainClass.bufferCellRef, 500);


                    Order toBeSubmitted = MainClass.bufferCellRef.getOneCell(1);
                    if(toBeSubmitted == null)
                    {
                        Monitor.PulseAll(MainClass.bufferCellRef);
                        return;
                    }
                    SubmitOrder(toBeSubmitted);

                    //double yo = MainClass.bufferCellRef.getOneCell();
                    //if(yo == -1)
                    //{
                    //    Monitor.PulseAll(MainClass.bufferCellRef);
                    //    return;
                    //}
                    //Console.WriteLine("we got the goods, yo- " + yo);


                    Monitor.PulseAll(MainClass.bufferCellRef);

                } while (success);
            }
        }

        public void RunAirline()
        {
            while (priceCutCounter < 5)
            {
                PricingModel();
                Thread.Sleep(2000);
            }

            isTerminated = true;
            Console.WriteLine("Terminating Airline thread");
        }

        private void PricingModel() 
        {
            double randomPrice = rng.Next(5000, 20000) / 100.00D;       // create ticket value from $50.00 - $200.00
            Console.WriteLine("Airline ticket price: {0:0.00}", randomPrice);
            if (oldPrice > randomPrice)                                 // priceCut event when ticket value is cheaper than before
            {
                Console.WriteLine("PriceCutEvent emitted: {0:0.00}", randomPrice);
                priceCut?.Invoke(randomPrice);                              // emit event
                priceCutCounter++;
                ReadOrder();
            }
            else
            {
                if(MainClass.bufferCellRef.currentOrder != null)    // if current order is outdated
                {
                    lock (MainClass.bufferCellRef.currentOrder)
                    {
                        // MainClass.bufferCellRef.setCurrentPrice(-1);
                        MainClass.bufferCellRef.setCurrentOrder(null);
                    }
                }
            }
            oldPrice = randomPrice;
        }

        public void SubmitOrder(Order toBeSubmitted)      // ORDER OBJECT AS PARAM-> BUFFER SENDS IT
        {

            Thread orderProcessingThread = new Thread(() => processor.StartThread(toBeSubmitted));
            orderProcessingThread.Start();
        }
    }
}
