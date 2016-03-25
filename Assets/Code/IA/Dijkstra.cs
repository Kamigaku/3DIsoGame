using Maps;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace IA {
    public abstract class Dijkstra {

        public static List<Vector2> shortestPath(int x_origine, int y_origine, int x_target, int y_target, Node[] nodes, int[,] map) {
            int valueOrigine = Map.XYValue(x_origine, y_origine);
            int valueTarget = Map.XYValue(x_target, y_target);

            resetDijkstra(nodes);

            Node currentNode = nodes[valueOrigine];
            currentNode.shortestDistance = 0;
            currentNode.previous = valueOrigine;
            bool found = false;

            List<Node> ordereredDistance = new List<Node>();
            while (!found) {
                int shortDistance = currentNode.shortestDistance + 1; // La valeur la plus petite possible
                for (int i = 0; i < currentNode.neighbors.Count; i++) { // Je parcours tous mes voisins
                    Node neighbor = nodes[currentNode.neighbors[i]];
                    if (!neighbor.fetched) { // Est-ce qu'il n'a jamais été parcouru ?
                        if (neighbor.shortestDistance > 0) { // Est-ce que le noeud voisin a déjà été parcouru ?
							if (neighbor.shortestDistance > shortDistance) { // Le noeud voisin possède un parcours qui était plus long
                                neighbor.shortestDistance = shortDistance;
                                neighbor.previous = currentNode.value;
                            }
                        }
                        else { // Le noeud voisin n'a jamais été parcouru, j'initialise sa valeur et son précédent
							neighbor.shortestDistance = shortDistance;
                            neighbor.previous = currentNode.value;
                            ordereredDistance.Add(neighbor);
                        }
                    }
                }

                nodes[currentNode.value].fetched = true;
                ordereredDistance.Sort((n1, n2) => n1.shortestDistance - n2.shortestDistance);
                currentNode = ordereredDistance[0];
                ordereredDistance.RemoveAt(0);
                if (currentNode.value == valueTarget) {
                    found = true;
                }
            }

            // Parcours inversé
            List<Vector2> reversePath = new List<Vector2>();
            while (currentNode.value != valueOrigine) {
                int yPrevious = Map.YValue(currentNode.value);
                int xPrevious = Map.XValue(currentNode.value);
                reversePath.Add(new Vector2(xPrevious, yPrevious));
                currentNode = nodes[currentNode.previous];
            }
            return reversePath;
        }

		public static List<Vector2> allPathAtRange(int x_origine, int y_origine, int distance, Node[] nodes) {
            int valueOrigine = Map.XYValue(x_origine, y_origine);

            resetDijkstra(nodes);

            Node currentNode = nodes[valueOrigine];
            currentNode.shortestDistance = 0;
            currentNode.previous = valueOrigine;

            List<Node> ordereredDistance = new List<Node>();
            do {
                int shortDistance = currentNode.shortestDistance + 1; // La valeur la plus petite possible
                for(int i = 0; i < currentNode.neighbors.Count; i++) { // Je parcours tous mes voisins
                    Node neighbor = nodes[currentNode.neighbors[i]];
                    if(!neighbor.fetched) { // Est-ce qu'il n'a jamais été parcouru ?
                        if(neighbor.shortestDistance > 0) { // Est-ce que le noeud voisin a déjà été parcouru ?
                            if(neighbor.shortestDistance > shortDistance) { // Le noeud voisin possède un parcours qui était plus long
                                neighbor.shortestDistance = shortDistance;
                                neighbor.previous = currentNode.value;
                            }
                        }
                        else { // Le noeud voisin n'a jamais été parcouru, j'initialise sa valeur et son précédent
                            neighbor.shortestDistance = shortDistance;
                            neighbor.previous = currentNode.value;
                            ordereredDistance.Add(neighbor);
                        }
                    }
                }

                nodes[currentNode.value].fetched = true;
                ordereredDistance.Sort((n1, n2) => n1.shortestDistance - n2.shortestDistance);
                if(ordereredDistance.Count > 0) {
                    currentNode = ordereredDistance[0];
                    ordereredDistance.RemoveAt(0);
                }
            } while(ordereredDistance.Count > 0);

            // Parcours inversé
            List<Vector2> allPossiblePath = new List<Vector2>();
            for(int i = 0; i < nodes.Length; i++) {
                if(nodes[i].shortestDistance <= distance && nodes[i].shortestDistance > 0) {
                    allPossiblePath.Add(new Vector2(Map.XValue(nodes[i].value), Map.YValue(nodes[i].value)));
                }
            }
            return allPossiblePath;
		}

        private static void resetDijkstra(Node[] nodes) {
            for(int i = 0; i < nodes.Length; i++) {
                if(nodes[i] != null)
                    nodes[i].reset();
            }
        }

    }
}
