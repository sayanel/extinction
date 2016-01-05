#pragma strict
import System.Collections.Generic;

var nodes : List.<node> = new List.<node>();
var nodeChanges : List.<nodeChange> = new List.<nodeChange>();
var maxQueueSize : int = 0;
var queueSize : int = 0;
var activeQueue : int = 0;

public class node {
	var position : Vector3;
	var neighbors : int[];
	var distanceToNeighbors : float[];
}

public class nodeChange {
	var f : float = 0;
	var g : float = 0;
	var h : float = 0;
	var closed : boolean = false;
	var parent : int = -1;
}

function LateUpdate() {
	if (queueSize > maxQueueSize) {
		maxQueueSize = queueSize;
	}
	if (queueSize > 0) {
		activeQueue += 1;
		queueSize -= 1;	
	}
	else {
		activeQueue = 0;
		queueSize = 0;
	}
}

function newQueue() {
	queueSize += 1;
	return queueSize+activeQueue;
}

function getClosestNode(position : Vector3) {
	var closestDistance : float = Mathf.Infinity;
	var distance : float;
	var closestNode : int;
	for (var i = 0; i < nodes.Count; i++) {
		distance = Vector3.Distance(position, nodes[i].position);
		if (distance < closestDistance) {
			closestDistance = distance;
			closestNode = i;
		}
	}
	return closestNode;
}

function heuristic (startPos : Vector3, endPos : Vector3) {
	var distanceX = Mathf.Abs(startPos.x - endPos.x);
	var distanceY = Mathf.Abs(startPos.y - endPos.y);
	var distanceZ = Mathf.Abs(startPos.z - endPos.z);
	return distanceX+distanceY+distanceZ;
}

function findPath(startPosition : Vector3, endPosition : Vector3) {
	var nodesInstance : List.<nodeChange> = new List.<nodeChange>();
	for (var l = 0; l < nodes.Count; l++) {
		nodesInstance.Add(new nodeChange());
	}
	var startNode : int = getClosestNode(startPosition);
	var endNode : int = getClosestNode(endPosition); 
	
	var openArray : List.<int> = new List.<int>();
	//var closedArray : List.<int> = new List.<int>();
	
	var lowInd : int = 0;
	var currentNode : int;
	var path : List.<Vector3> = new List.<Vector3>();
	var gScore : float;
	var gScoreIsBest : boolean;
	
	openArray.Add(startNode);
	
	while(openArray.Count > 0) {
		lowInd = 0;
		for (var i = 0; i < openArray.Count; i++) {
			if (nodesInstance[openArray[i]].f < nodesInstance[openArray[lowInd]].f) {
				lowInd = i;
			}
		}
		
		currentNode = openArray[lowInd];
		
		if (currentNode == endNode) {
			while (nodesInstance[currentNode].parent != -1) {
				path.Add(nodes[currentNode].position);
				currentNode = nodesInstance[currentNode].parent;
			}
			if (path.Count > 0) {
				path.Reverse();
				return path;
			}
			else {
				//Debug.Log("Already at destination");
				return null;
			}
		}
		
		openArray.Remove(currentNode);
		//closedArray.Add(currentNode);
		nodesInstance[currentNode].closed = true;
		
		for (var j = 0; j < nodes[currentNode].neighbors.length; j++) {
			if (nodesInstance[nodes[currentNode].neighbors[j]].closed == true) {
				continue;
			}
			gScore = nodesInstance[currentNode].g + nodes[currentNode].distanceToNeighbors[j];
			gScoreIsBest = false;
			
			if (openArray.Contains(nodes[currentNode].neighbors[j]) == false) {
				gScoreIsBest = true;
				nodesInstance[nodes[currentNode].neighbors[j]].h = heuristic(nodes[nodes[currentNode].neighbors[j]].position, nodes[endNode].position);
				openArray.Add(nodes[currentNode].neighbors[j]);
			}
			else if (gScore < nodesInstance[nodes[currentNode].neighbors[j]].g) {
				gScoreIsBest = true;
			}
			if (gScoreIsBest == true) {
				nodesInstance[nodes[currentNode].neighbors[j]].parent = currentNode;
				nodesInstance[nodes[currentNode].neighbors[j]].g = gScore;
				nodesInstance[nodes[currentNode].neighbors[j]].f = gScore + nodesInstance[nodes[currentNode].neighbors[j]].h;
			}
		}
	}
	//Debug.Log("Couldn't find path");
	return null;
}

function findRandomPath(startPosition : Vector3) {
	return findPath(startPosition, nodes[Random.Range(0, nodes.Count-1)].position);
}

function findRandomPathOfDistance(startPosition : Vector3, minDistance : float, maxDistance : float) {
	var multiplier1 : int = 0;
	var multiplier2 : int = 0;
	if (Random.value < 0.5) {
		multiplier1 = 1;
	}
	else {
		multiplier1 = -1;
	}
	if (Random.value < 0.5) {
		multiplier2 = 1;
	}
	else {
		multiplier2 = -1;
	}
	var endPosition : Vector3 = startPosition+Vector3(multiplier1*Random.Range(minDistance, maxDistance), 0.0, multiplier2*Random.Range(minDistance, maxDistance));
	return findPath(startPosition, endPosition);
}

function drawPath(path : List.<Vector3>, color : Color) { 
	for (var i = 0; i < path.Count-1; i++) {
		var drawPoint1 : Vector3 = path[i];
		var drawPoint2 : Vector3 = path[i+1];
		Debug.DrawLine(drawPoint1, drawPoint2, color);
	}
}