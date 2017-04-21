using System;
using System.Collections.Generic;
using System.Text;

namespace RouteSearch
{
    public class VectorChain
    {
        private List<Point> pointCollection = new List<Point>();

        public int size
        {
            get
            {
                return this.pointCollection.Count;
            }
        }

        public VectorChain()
        {

        }

        public VectorChain(VectorChain vectorChain)
        {
            foreach(Point pointInstance in vectorChain.pointCollection)
            {
                this.addPoint(pointInstance);
            }
        }

        public void addPoint(Point point)
        {
            this.pointCollection.Add(point);
        }

        public Point getStartPoint()
        {
            if (this.pointCollection.Count > 0)
            {
                return this.pointCollection[0];
            }
            else
            {
                return null;
            }
        }

        public Point getEndPoint()
        {
            if (this.pointCollection.Count > 0)
            {
                return this.pointCollection[this.pointCollection.Count-1];
            }
            else
            {
                return null;
            }
        }

        public string convertToString()
        {
            string returnString = "";
            for(int forInt_1 = 0; forInt_1 < this.pointCollection.Count; forInt_1++)
            {
                returnString += this.pointCollection[forInt_1].convertToString();
                if (forInt_1 != pointCollection.Count - 1)
                {
                    returnString += "-";
                }
            }
            return returnString;
        }
    }
}
