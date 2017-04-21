using System;
using System.Collections.Generic;
using System.Text;

namespace RouteSearch
{
    public class Explore
    {
        private List<Point> bannedPointCollection = new List<Point>();
        private Dictionary<Point, VectorChain> vectorChainMappedByPoint = new Dictionary<Point, VectorChain>();
        VectorChain finalVetorChain = null;

        public Explore(List<Point> transitablePointCollection,Point startPoint,Point endPoint)
        {
            //
            //any point not in the transitablePointCollection will be add to the bannedPointCollection
            foreach(Point pointInstance in RouteDataManager.getInstance().getPointCollection())
            {
                if (transitablePointCollection.Contains(pointInstance) == false)
                {
                    this.bannedPointCollection.Add(pointInstance);
                }
            }
            //
            VectorChain initialVectorChain = new VectorChain();
            initialVectorChain.addPoint(startPoint);
            this.vectorChainMappedByPoint.Add(startPoint,initialVectorChain);
            while (this.finalVetorChain == null)
            {
                Dictionary<Point, VectorChain> marchVectorChainMappedByPoint = new Dictionary<Point, VectorChain>();
                //
                //update bannedPointCollection
                foreach(Point nowPointInstance in this.vectorChainMappedByPoint.Keys)
                {
                    this.bannedPointCollection.Add(nowPointInstance);
                }
                //
                //one step forward
                bool allDeadWay = true;
                foreach(Point nowPointInstance in this.vectorChainMappedByPoint.Keys)
                {
                    foreach(Point targetPointInstance in nowPointInstance.march())
                    {
                        if (this.bannedPointCollection.Contains(targetPointInstance))
                        {
                            //
                            //deadWay
                        }
                        else
                        {
                            allDeadWay = false;
                            if (marchVectorChainMappedByPoint.ContainsKey(targetPointInstance))
                            {
                                //
                                //nothing to do
                            }
                            else
                            {
                                VectorChain vectorChainBeforeMarch = this.vectorChainMappedByPoint[nowPointInstance];
                                VectorChain vectorChainAfterMarch = new VectorChain(vectorChainBeforeMarch);
                                vectorChainAfterMarch.addPoint(targetPointInstance);
                                marchVectorChainMappedByPoint.Add(targetPointInstance, vectorChainAfterMarch);
                                if (targetPointInstance == endPoint)
                                {
                                    this.finalVetorChain = vectorChainAfterMarch;
                                    break;
                                }
                            }
                        }
                    }
                }
                vectorChainMappedByPoint = marchVectorChainMappedByPoint;
                if (allDeadWay == true)
                {
                    break;
                }
            }
        }

        public void printBannedPointCollection()
        {
            Console.Write("bannedPointCollection:");
            foreach(Point pointInstance in this.bannedPointCollection)
            {
                Console.Write(String.Concat(pointInstance.convertToString()," "));
            }
            Console.WriteLine();
        }

        public void printFinalVectorChain()
        {
            if (this.finalVetorChain != null)
            {
                Console.WriteLine(this.finalVetorChain.convertToString());
            }
            else
            {
                Console.WriteLine("unreachable");
            }
        }

        //
        //get the minimum distance
        public int getMinDistance()
        {
            if (this.finalVetorChain != null)
            {
                return this.finalVetorChain.size - 1;
            }
            else
            {
                return -1;
            }
        }

        public void printVectorChainMappedByPoint()
        {
            Console.Write("vectorChainMappedByPoint:");
            foreach(KeyValuePair<Point,VectorChain> keyValuePairInstance in this.vectorChainMappedByPoint)
            {
                Point point = keyValuePairInstance.Key;
                VectorChain vectorChain = keyValuePairInstance.Value;
                Console.Write(String.Concat("point=",point.convertToString()," "));
                Console.Write(String.Concat("vectorChain=",vectorChain.convertToString()));
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }


}
