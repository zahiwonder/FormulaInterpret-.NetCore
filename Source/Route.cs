using System;
using System.Collections.Generic;
using System.Text;

namespace RouteSearch
{
    public class Route
    {
        private Point pointAInner;
        private Point pointBInner;

        public Point pointA
        {
            get
            {
                return this.pointAInner;
            }
        }

        public Point pointB
        {
            get
            {
                return this.pointBInner;
            }
        }

        public Route(Point pointA,Point pointB)
        {
            this.pointAInner = pointA;
            this.pointBInner = pointB;
        }

        public String convertToString()
        {
            return String.Concat(this.pointA.convertToString(),"-",this.pointB.convertToString());
        }
        
        public String convertToString(Point point)
        {
            Point displayPoint;
            if(this.pointA!=point && this.pointB != point)
            {
                throw new NullReferenceException("point is not in the route");
            }
            if (this.pointA == point)
            {
                displayPoint = this.pointB;
            }
            else
            {
                displayPoint = this.pointA;
            }
            return String.Concat("-",displayPoint.convertToString());
        }

        public Point getAnotherPoint(Point point)
        {
            if (this.pointA == point)
            {
                return this.pointB;
            }
            else
            {
                return this.pointA;
            }
        }

        public bool isSameRoute(Route anotherRoute)
        {
            if(this.pointA==anotherRoute.pointA && this.pointB == anotherRoute.pointB)
            {
                return true;
            }else if(this.pointA==anotherRoute.pointB && this.pointB == anotherRoute.pointA)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
