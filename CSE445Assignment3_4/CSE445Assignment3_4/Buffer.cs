using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Buffer
    {
        public int numberOfCells = 2;
        public double cell = -1;
        public double[] cells = new double[2];
        public static double currentPrice = -1;


        public Order[] dataCells = new Order[2];
        public Order currentOrder = new Order();

        public Buffer()
        {
            for(int i = 0; i < numberOfCells; i++)
            {
                cells[i] = -1;

                currentOrder.isAvailable = true;
                Order dataCell = new Order();
                dataCell.isAvailable = true;
                dataCells[i] = dataCell;
            }
        }

        //public void setOneCell(Order newOrder)  // synchronized
        //{

        //}

        //public Order getOneCell() // synchronized
        //{
        //    return null;
        //}

        public Order getCurrentOrder()
        {
            return currentOrder;
        }

        public void setCurrentOrder(Order newOrder)
        {
            currentOrder = newOrder;
        }

        public Order getOneCell(int delete) // DELETE PARAM
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                if (!dataCells[i].isAvailable)
                {
                    //Console.WriteLine("gc index: " + i);
                    dataCells[i].isAvailable = true;
                    return dataCells[i];
                }
            }
            return null;
        }

        public void setOneCell(Order newOrder)  // synchronized
        {
            for (int i = 0; i < numberOfCells; i++)
            {
                if (dataCells[i].isAvailable)
                {
                    dataCells[i] = newOrder;
                    //Console.WriteLine("sc index: " + i);
                    return;
                }
            }
            Console.WriteLine("this shouldnt print, set one cell");
        }





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
                    //Console.WriteLine("gc index: " + i);
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
                    //Console.WriteLine("sc index: " + i);
                    return;
                }
            }
            Console.WriteLine("this shouldnt print, set one cell");
        }
    }
}
