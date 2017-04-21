using System;
using System.Collections.Generic;
using System.Text;

namespace RouteSearch
{
    public class RouteDataManager
    {
        private static RouteDataManager routeDataManager;
        private static List<Route> routeCollection = new List<Route>();
        private static List<Point> pointCollection = new List<Point>();
        private static Dictionary<Point, List<Route>> routeCollectionGroupByPoint = new Dictionary<Point, List<Route>>();

        public static RouteDataManager getInstance()
        {
            if (RouteDataManager.routeDataManager == null)
            {
                RouteDataManager.routeDataManager = new RouteDataManager();
            }
            return RouteDataManager.routeDataManager;
        }

        private RouteDataManager()
        {

        }

        //
        //put a route into Dictionary
        public void addRoute(Route route)
        {
            //
            //check if the route already exists
            if (this.routeExists(route))
            {
                return;
            }
            //
            //put route into routeCollection
            routeCollection.Add(route);
            Point pointA = route.pointA;
            Point pointB = route.pointB;
            //
            //put PointA into routeCollectionGroupByPoint
            this.addRouteByRouteAndPoint(route, pointA);
            //
            //put PointB into routeCollectionGroupByPoint
            this.addRouteByRouteAndPoint(route, pointB);
        }

        private void addRouteByRouteAndPoint(Route route,Point point)
        {
            //
            //check if the point is in the route
            if(route.pointA!=point && route.pointB != point)
            {
                throw new NullReferenceException("point is not in the route");
            }
            //
            //put Point into routeCollectionGroupByPoint
            if (RouteDataManager.routeCollectionGroupByPoint.ContainsKey(point))
            {
                List<Route> pointVectorCollection = RouteDataManager.routeCollectionGroupByPoint[point];
                pointVectorCollection.Add(route);
            }
            else
            {
                List<Route> pointVectorCollection = new List<Route>();
                pointVectorCollection.Add(route);
                RouteDataManager.routeCollectionGroupByPoint.Add(point, pointVectorCollection);
                RouteDataManager.pointCollection.Add(point);
            }

        }

        public void printRouteData()
        {
            foreach(KeyValuePair<Point, List<Route>> keyValuePairInstance in RouteDataManager.routeCollectionGroupByPoint)
            {
                Point point = keyValuePairInstance.Key;
                List<Route> routeCollection = keyValuePairInstance.Value;
                Console.Write(String.Concat("point="+point.convertToString()+" route="));
                foreach(Route routeInstance in routeCollection)
                {
                    Console.Write(String.Concat(routeInstance.convertToString(point)+" "));
                }
                Console.WriteLine();
            }
        }

        public bool routeExists(Route route)
        {
            bool exists = false;
            foreach(Route routeInstace in RouteDataManager.routeCollection)
            {
                if (route.isSameRoute(routeInstace))
                {
                    exists = true;
                }
            }
            return exists;
        }

        public List<Route> getRouteCollectionThroughPoint(Point point)
        {
            return RouteDataManager.routeCollectionGroupByPoint[point];
        }

        public List<Point> getPointCollection()
        {
            return RouteDataManager.pointCollection;
        }
    }
}
