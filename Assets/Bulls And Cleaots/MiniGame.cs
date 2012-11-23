using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using BullsAndCleots.Level;
using BullsAndCleots.Mechanics;

namespace BullsAndCleots {

public class MiniGame : MonoBehaviour, MiniGameAPI.IMiniGame {

	/// <summary>
	/// The tag name of the Solution Length node in the user properties.
	/// </summary>
	public static string SOLUTION_LEN_PROP = "BCSolutionLength";
	public static string DATASET = "dataset";
	public static string ITEM = "item";

	// Use endings.cs?
	public static class ExitEdges {
		public const string IncreaseDifficulty = "BCIncreaseDifficulty";
		public const string IncreaseColors = "BCIncreaseColors";
		public const string IncreaseNumbers = "BCIncreaseNumbers";
	}

	private Dictionary<string, List<Material>> dataSets;
	public GameObject level;
	private XmlNode data;

	public XmlNode Data {
		set {
			data = value;

			loadLevel();
		}
	}

	private bool addDataSet(XmlNode setNode) {
		List<Material> matsList = new List<Material>();
		foreach (Material mat in setNode.GetMaterials()) {
			matsList.Add(mat);
		}

		string setName = setNode.Attributes["id"].Value;

		dataSets[setName] = matsList;
		return true;
	}

	private void loadDataSets() {
		dataSets = new Dictionary<string, List<Material>>();
		foreach (XmlNode child in data.childNodes(DATASET)) {
			addDataSet(child);
		}

	}

	private void loadLevel() {
		GameObject go = Instantiate(level) as GameObject;
		go.transform.parent = transform;
		LevelManager bcLevel = go.GetComponent<LevelManager>();

		int solutionLen = MathData.GetInt(UserProperty.GetPropNode(SOLUTION_LEN_PROP));

		loadDataSets();
		SolutionManager slnManager = new SolutionManager(solutionLen);
		List<List<Material>> choices = new List<List<Material>>();

		foreach (string name in dataSets.Keys) {
			List<Material> mats = dataSets[name];
			List<Material> matChoices =
                mats.OrderBy(x => Random.value).Take(solutionLen).ToList();

			IEnumerable<Material> solution =
                matChoices.OrderBy(x => Random.value).Take(solutionLen);
			choices.Add(matChoices);
			Solution sln = new Solution(name, new ArrayList(solution.ToArray()));
			slnManager.AddSolution(sln);
		}

		bcLevel.InitData = new LevelData(slnManager, choices);
	}

}
}