#pragma strict
import System.Collections.Generic;
var analyzing = false;
var progress : int = 0;
var navMeshScript : navMesh;
var navMeshMesh : Mesh;
var tempNodeNeighborArray : Array = new Array();
var drawNodes : boolean = false;
var timeStartedAnalyzing : float = 0.0;

@CustomEditor(navMesh)
class navMeshAnalyzer extends Editor {
	function OnInspectorGUI () {
		navMeshScript = target as navMesh;
		navMeshMesh = navMeshScript.gameObject.GetComponent(MeshFilter).sharedMesh;
		EditorUtility.SetDirty(navMeshScript);
		if (GUILayout.Button("Analyze Navmesh")) {
			timeStartedAnalyzing = Time.realtimeSinceStartup;
			progress = 0;
			navMeshScript.nodes = new List.<node>();
			navMeshScript.nodeChanges = new List.<nodeChange>();
			tempNodeNeighborArray = new Array();
			analyzing = true;
		}	
		GUILayout.Label("Nodes: "+navMeshScript.nodes.Count);
		if (analyzing == true) {
			if (progress < (navMeshMesh.triangles.Length/3)) {
				EditorUtility.SetDirty(navMeshScript);
				analyzeSingleTri(progress);
				progress += 1;
				var timeRemaining : int = ((navMeshMesh.triangles.Length/3)-progress)*((Time.realtimeSinceStartup-timeStartedAnalyzing)/progress);
				if (EditorUtility.DisplayCancelableProgressBar("Navmesh", "Analyzing navmesh... Time Remaining: "+timeRemaining+" s", progress/(navMeshMesh.triangles.Length/3.0))) {
					analyzing = false;
					EditorUtility.ClearProgressBar();
				}
			}
			else {
				createNeighbors();
				analyzing = false;
				EditorUtility.ClearProgressBar();
			}
		}
		
		drawNodes = GUILayout.Toggle(drawNodes, "Draw Nodes");
		
		if (drawNodes) {
			draw();
		}
	}
	
}

function draw() {
	for (var i = 0; i<navMeshScript.nodes.Count; i++) {
		for (var j = 0; j<navMeshScript.nodes[i].neighbors.length; j++) {
			Debug.DrawLine(navMeshScript.nodes[i].position, navMeshScript.nodes[navMeshScript.nodes[i].neighbors[j]].position, Color.blue);
		}
	}
}

function analyze() {
	navMeshScript.nodes = new List.<node>();
	navMeshScript.nodeChanges = new List.<nodeChange>();
	for (var i = 0; i<navMeshMesh.triangles.Length/3; i++) {
		navMeshScript.nodes.Add(new node());
		navMeshScript.nodes[i].position = getTrianglePosition(i);
		navMeshScript.nodes[i].neighbors = getAdjacentTris(i);
		
		navMeshScript.nodeChanges.Add(new nodeChange());
		
	}
	for (var j = 0; j<navMeshScript.nodes.Count; j++) {
		navMeshScript.nodes[j].distanceToNeighbors = getDistanceToNeighbors(j);
	}
}

function analyze2() {
	progress = 0;
	navMeshScript.nodes = new List.<node>();
	navMeshScript.nodeChanges = new List.<nodeChange>();
	tempNodeNeighborArray = new Array();
	analyzing = true;
}

class tempNodeNeighbor {
	var nonSharedEdges : Vector3[];
}
	
function analyzeSingleTri(i : int) {
	var currentTriEdgeMidpoints :  Vector3[] = getTriangleEdgeMidpoints(i);
	var currentTriNeighbors : int[] = getAdjacentTris(i);	
	//Loop through the triangle's edges
	for (var j = 0; j<3; j++) {
		//If node doesn't already exist
		if (checkIfNodeExists(currentTriEdgeMidpoints[j]) == -1) {
			var nodeHasNeighbor : boolean = false;
			var neighborEdgeMidpoints : Vector3[] = new Vector3[3];
			var neighborNonSharedEdgeMidpoints : Array = new Array();
			var nodeNeighborTri : int = 0;
			//Loop through the triangle's neighbors
			for (var k = 0; k<currentTriNeighbors.length; k++) {
				neighborEdgeMidpoints = getTriangleEdgeMidpoints(currentTriNeighbors[k]);
				//Loop through the neighbor's edges
				for (var l = 0; l<3; l++) {
					//If the neighbor shares an edge with the current triangle's edge
					if (neighborEdgeMidpoints[l] == currentTriEdgeMidpoints[j]) {
						nodeHasNeighbor = true;
					}
				}
				if (nodeHasNeighbor == true) {
					//Loop through neighbor's edges
					for (var q = 0; q<3; q++) {
						if (neighborEdgeMidpoints[q] != currentTriEdgeMidpoints[j]) {
							neighborNonSharedEdgeMidpoints.Push(neighborEdgeMidpoints[q]);
						}
					}
					//Loop through node's triangle's edges
					for (var p = 0; p<3; p++) {
						//If the edge is not the "node"
						if (currentTriEdgeMidpoints[p] != currentTriEdgeMidpoints[j]) {
							neighborNonSharedEdgeMidpoints.Push(currentTriEdgeMidpoints[p]);
						}
					}
					//Create the node
					navMeshScript.nodes.Add(new node());
					navMeshScript.nodes[navMeshScript.nodes.Count-1].position = currentTriEdgeMidpoints[j];
					//Create a temporary neighbor
					var newTempNodeNeighbor : tempNodeNeighbor = new tempNodeNeighbor();
					newTempNodeNeighbor.nonSharedEdges = neighborNonSharedEdgeMidpoints;
					tempNodeNeighborArray.Push(newTempNodeNeighbor);
					break;
				}
			}
		}
	}
}

function createNeighbors() {
	//Loop through created nodes
	for(var m = 0; m<navMeshScript.nodes.Count; m++) {
		var nodeTempNodeNeighbor : tempNodeNeighbor = tempNodeNeighborArray[m] as tempNodeNeighbor;
		var tempNodeNeighbors : Array = new Array();
		//Loop through the node's possible neighbors
		for (var n = 0; n<nodeTempNodeNeighbor.nonSharedEdges.length; n++) {
			var neighborNodeIndex : int = checkIfNodeExists(nodeTempNodeNeighbor.nonSharedEdges[n]);
			if (neighborNodeIndex != -1) {
				tempNodeNeighbors.Push(neighborNodeIndex);
			}
		}
		navMeshScript.nodes[m].neighbors = tempNodeNeighbors;
		navMeshScript.nodes[m].distanceToNeighbors = getDistanceToNeighbors(m);
	}
}
	
function getTrianglePosition(triangle : int) {
	var vertices : Vector3[] = getTriangleVertices(triangle);
	var trianglePosition : Vector3 = (vertices[0]+vertices[1]+vertices[2])/3;
	return trianglePosition;
}
function getTriangleVertices(triangle : int) {
	var vertices : Vector3[] = new Vector3[3];
	vertices[0] = navMeshScript.gameObject.transform.TransformPoint(navMeshMesh.vertices[navMeshMesh.triangles[triangle*3]]);
	vertices[1] = navMeshScript.gameObject.transform.TransformPoint(navMeshMesh.vertices[navMeshMesh.triangles[triangle*3+1]]);
	vertices[2] = navMeshScript.gameObject.transform.TransformPoint(navMeshMesh.vertices[navMeshMesh.triangles[triangle*3+2]]);
	return vertices;
}

function getTriangleEdgeMidpoints(triangle : int) {
	var vertices : Vector3[] = getTriangleVertices(triangle);
	var edgeMidpoints : Vector3[] = new Vector3[3];
	edgeMidpoints[0] = (vertices[0]+vertices[1])/2;
	edgeMidpoints[1] = (vertices[1]+vertices[2])/2;
	edgeMidpoints[2] = (vertices[2]+vertices[0])/2;
	return edgeMidpoints;
}

function getAdjacentTris(triangle : int) {
	var vertices : Vector3[] = getTriangleVertices(triangle);
	var adjacentTris : Array = new Array();
	var ret : int[];
	for (var i = 0; i<navMeshMesh.triangles.Length/3; i++) {
		var commonVertices : int = 0;
		var currentTriVertices : Vector3[] = getTriangleVertices(i);
		for (var j = 0; j < 3; j++) {
			if (currentTriVertices[j] == vertices[0] ||
			currentTriVertices[j] == vertices[1] ||
			currentTriVertices[j] == vertices[2]) {
				commonVertices += 1;
			}
		}
		if (commonVertices == 2) {
			adjacentTris.Push(i);
		}
	}
	ret = adjacentTris;
	return ret;
}

function getDistanceToNeighbors(nodeIndex : int) {
	var distanceToNeighbors : float[] = new float[navMeshScript.nodes[nodeIndex].neighbors.length];
	for (var i = 0; i < navMeshScript.nodes[nodeIndex].neighbors.length; i++) {
		var neighbor : int = navMeshScript.nodes[nodeIndex].neighbors[i];
		distanceToNeighbors[i] = Vector3.Distance(navMeshScript.nodes[nodeIndex].position, navMeshScript.nodes[neighbor].position);
	}
	return distanceToNeighbors;
}

function checkIfNodeExists(position : Vector3) {
	for (var i = 0; i<navMeshScript.nodes.Count; i++) {
		if (position == navMeshScript.nodes[i].position) {
			return i;
		}
	}
	return -1;
}