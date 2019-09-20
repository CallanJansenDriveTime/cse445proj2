using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Buffer
    {
        // n=2 data cells -> references to Order Object? 
        public int numberOfCells = 2;
        //public Order[] dataCells = new Order[2];
        public double cell = -1;
        public double[] cells = new double[2];
        public static double currentPrice = -1;

        public Buffer()
        {
            for(int i = 0; i < numberOfCells; i++)
            {
                cells[i] = -1;
                //Order dataCell = new Order();
                //dataCells[i] = dataCell;
            }
        }

        //public void setOneCell(Order newOrder)  // synchronized
        //{

        //}

        //public Order getOneCell() // synchronized
        //{
        //    return null;
        //}

        public double getCurrentPrice()
        {
            return currentPrice;
        }

        public void setCurrentPrice(double price)
        {
            currentPrice = price;
        }

        public double getOneCell() // synchronized
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                if (cells[i] != -1)
                {
                    double temp = cells[i];
                    cells[i] = -1;
                    Console.WriteLine("gc index: " + i);
                    return temp;
                }
            }
            return -1;
        }

        public void setOneCell(double newCell)  // synchronized
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                if(cells[i] == -1)
                {
                    cells[i] = newCell;
                    Console.WriteLine("sc index: " + i);
                    return;
                }
            }
            Console.WriteLine("this shouldnt print, set one cell");
        }
    }
}
