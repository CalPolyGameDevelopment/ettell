using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using BullsAndCleots.Level;
using BullsAndCleots.Mechanics;

namespace BullsAndCleots
{

// Aliases to shorten the verbose generic collection typing.
	public class DataSetsDict : Dictionary<string, List<Material>>
	{
	};

	public class MatrixRow : Dictionary<string,int>
	{
	};

	public class SolutionMixMatrix : Dictionary<int,MatrixRow>
	{
	};

/// <summary>
/// Hacky terribly stupid thing that shows nothing but reckless abandon of
/// everything that makes me a decent programmer.
///
/// Temp thing until we can discuss UserProps.
/// </summary>
	public class TempDataStore
	{
		private static TempDataStore singleton = null;
		private SolutionMixMatrix slnMixMatrix;

		public static TempDataStore instance {
			get {
				if (singleton == null) {
					singleton = new TempDataStore ();
				}
				return singleton;
			}
		}

		public static SolutionMixMatrix MixMatrix {
			get{ return instance.slnMixMatrix; }
		}

		public TempDataStore ()
		{
			slnMixMatrix = new SolutionMixMatrix ();
		}
	}

	public class MiniGame : MonoBehaviour, MiniGameAPI.IMiniGame
	{

		/// <summary>
		/// The tag name of the Solution Length node in the user properties.
		/// </summary>
		public static string SOLUTION_LEN_PROP = "BCSolutionLength";
		public static string SLN_MIX_MATRIX_PROP = "BCSolutionMixMatrix";
		public static string DATASET = "dataset";
		public static string ITEM = "item";

		// Use endings.cs?
		public static class ExitEdges
		{
			public const string IncreaseDifficulty = "BCIncreaseDifficulty";
			public const string IncreaseColors = "BCIncreaseColors";
			public const string IncreaseNumbers = "BCIncreaseNumbers";
		}

		private DataSetsDict dataSets;
		private SolutionMixMatrix slnMixMatrix;
		private SolutionManager slnManager;
		private List<List<Material>> matChoices;
		public GameObject level;
		private XmlNode data;

		public XmlNode Data {
			set {
				data = value;
				loadLevel ();
			}
		}

		private void addDataSet (XmlNode setNode)
		{
			List<Material> matsList = new List<Material> ();
			foreach (Material mat in setNode.GetMaterials()) {
				matsList.Add (mat);
			}

			string setName = setNode.Attributes ["id"].Value;

			dataSets [setName] = matsList;
		}

		private void loadDataSets ()
		{
			dataSets = new DataSetsDict ();
			foreach (XmlNode child in data.childNodes(DATASET)) {
				addDataSet (child);
			}

		}

		private void addMatrixRow (XmlNode rowNode)
		{
			MatrixRow row = new MatrixRow ();
			int slnLength = rowNode.childNode ("value").GetInt ();

			foreach (XmlNode itemNode in rowNode.childNodes(ITEM)) {
				string name = itemNode.childNode ("name").getString ();
				int data = itemNode.childNode ("value").GetInt ();
				row [name] = data;
			}
			slnMixMatrix [slnLength] = row;
		}

		private void loadMixMatrix ()
		{
			slnMixMatrix = TempDataStore.MixMatrix;
			if (slnMixMatrix.Count > 0) {
				Debug.Log ("already loaded solution mix datastore.");
				return;
			}
			foreach (XmlNode rowNode in UserProperty.GetPropNode(SLN_MIX_MATRIX_PROP).childNodes("row")) {
				addMatrixRow (rowNode);
			}


		}

		private Solution loadSolution ()
		{
			return null;
		}

		// Calculate difficulty
		private void calculateDifficulty(){
			int slnLength = UserProperty.GetPropNode(SOLUTION_LEN_PROP).GetInt();

			MatrixRow slnRow = new MatrixRow();

			if (slnMixMatrix.ContainsKey(slnLength)) {
				slnRow = slnMixMatrix[slnLength];
			}
			else {
				slnMixMatrix[slnLength] = slnRow;
			}


			int slnTypesCount = 0;
			int maxedChoiceCount = 0;

			foreach(KeyValuePair<string,int> item in slnRow.ToArray()){
				string dsName = item.Key;
				int timesUsed = item.Value;

				if (timesUsed > 0){
					slnTypesCount++;
				}

				// if the current choice count for pure solution type > len(choices of type)
				//   - increament the solution length
				if (dataSets[dsName].Count < timesUsed){
					maxedChoiceCount++;
					slnRow[dsName] = dataSets[dsName].Count;
				}


			}

			if (slnTypesCount > 1 || maxedChoiceCount > 1){
				slnLength++;
				UserProperty.setProp(SOLUTION_LEN_PROP, slnLength.ToString());
				MatrixRow nextRow = new MatrixRow();
				foreach(KeyValuePair<string, int> item in slnRow.ToArray()){
					if (item.Value >= slnLength)
						nextRow[item.Key] = item.Value;
					else
						nextRow[item.Key] = slnLength;
				}
				slnMixMatrix[slnLength] = nextRow;
			}

		}

		private SolutionManager loadSolutionManager ()
		{
			int solutionLen = MathData.GetInt (UserProperty.GetPropNode (SOLUTION_LEN_PROP));

			slnManager = new SolutionManager (solutionLen);
			matChoices = new List<List<Material>> ();

			MatrixRow slnRow;
			if(slnMixMatrix.ContainsKey(solutionLen)){
				slnRow = slnMixMatrix[solutionLen];
			}
			else{
				slnRow = new MatrixRow();
				slnMixMatrix[solutionLen] = slnRow;
			}


			foreach (string name in dataSets.Keys) {
				if (!slnRow.ContainsKey(name)){
					slnRow[name] = 0;
				}

				List<Material> mats = dataSets [name];


				int choiceCount = slnRow[name] == 0 ?
					 solutionLen : slnRow[name];

				List<Material> slnChoices =
                mats.OrderBy (x => Random.value).Take (choiceCount).ToList ();

				IEnumerable<Material> solution =
               slnChoices.OrderBy (x => Random.value).Take (solutionLen);
				matChoices.Add (slnChoices);

				Solution sln = new Solution (name, new ArrayList (solution.ToArray ()));
				slnManager.AddSolution (sln);
			}
			return slnManager;
		}

		private void loadLevel ()
		{
			GameObject go = Instantiate (level) as GameObject;
			go.transform.parent = transform;
			LevelManager bcLevel = go.GetComponent<LevelManager> ();

			loadMixMatrix ();
			loadDataSets ();
			calculateDifficulty();
			loadSolutionManager ();

			Ending end = Ending.findEndings (data).ToArray () [0];
			bcLevel.InitData = new LevelData (slnManager, matChoices, end);
		}

	}
}