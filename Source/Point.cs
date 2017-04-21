using System;
using System.Collections.Generic;
using System.Text;

namespace RouteSearch
{
    public class Point
    {
        private string nameInner;
        private string aliasInner;

        public string name
        {
            get
            {
                return this.nameInner;
            }
        }

        public string alias
        {
            get
            {
                return this.aliasInner;
            }
        }

        public Point(string name)
        {
            this.nameInner = name;
            this.aliasInner = "";
        }

        public Point(string name,string alias)
        {
            this.nameInner = name;
            this.aliasInner = alias;
        }

        public string convertToString()
        {
            return this.alias;
        }

        //
        //one step forward
        public List<Point> march()
        {
            List<Route> routeCollection = RouteDataManager.getInstance().getRouteCollectionThroughPoint(this);
            List<Point> marchPointCollection = new List<Point>();
            foreach(Route routeInstance in routeCollection){
                marchPointCollection.Add(routeInstance.getAnotherPoint(this));
            }
            return marchPointCollection;
        }
    }
}
