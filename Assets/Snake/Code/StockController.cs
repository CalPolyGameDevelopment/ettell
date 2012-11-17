using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StockController : MonoBehaviour {
	
	private const int stockPlaceTries = 10;
	#region 1000 random stock ticker symbols
	// Generated by: print "\n".join([''.join([chr(ord('A') + random.randint(0,25)) for x in xrange(random.randint(1,4))]) for i in xrange(1000)])
	public string[] symbolNames = new string[] {
"FG",
"U",
"NDO",
"LHB",
"VBQM",
"GPHW",
"WMXA",
"LMJS",
"VM",
"C",
"OIS",
"FV",
"DGW",
"O",
"RJ",
"INMC",
"LPA",
"BAPP",
"VO",
"I",
"SXHE",
"IVAN",
"VA",
"DSOD",
"PVZ",
"OSIU",
"YCL",
"MEUI",
"SLSM",
"FBEX",
"QIP",
"RRF",
"MNEG",
"AZ",
"SZW",
"QZ",
"CPQX",
"KDQQ",
"UG",
"Q",
"GYNH",
"T",
"OO",
"K",
"SOM",
"OJH",
"KCD",
"TQGU",
"WAVE",
"YPZ",
"WUFC",
"ROR",
"VYNT",
"ST",
"KBJ",
"S",
"YYNI",
"AMZF",
"HS",
"QJ",
"IF",
"IDRE",
"EB",
"M",
"PQZA",
"UH",
"LS",
"PZ",
"ZYGP",
"BXU",
"TA",
"NW",
"KMB",
"BN",
"J",
"XQV",
"WX",
"PRC",
"G",
"ISR",
"TTW",
"UAN",
"TMB",
"JD",
"GUFP",
"PQSI",
"KYY",
"JB",
"E",
"IW",
"Y",
"KXOV",
"YYI",
"A",
"YDC",
"GSOY",
"XAUY",
"ME",
"WWR",
"TF",
"UCRB",
"J",
"S",
"BAQO",
"HC",
"OM",
"Z",
"L",
"XO",
"SVI",
"JLBW",
"CZ",
"NM",
"RWVK",
"PQA",
"UF",
"CV",
"SBNG",
"E",
"LYWA",
"BU",
"YKG",
"B",
"DZA",
"P",
"PQEQ",
"L",
"HQG",
"NXV",
"G",
"R",
"OPF",
"AEJZ",
"KEQ",
"ZN",
"ZI",
"XWW",
"D",
"O",
"TX",
"QXLR",
"RRP",
"IJKG",
"XMV",
"ZL",
"IY",
"AQI",
"W",
"TWP",
"OAYT",
"Q",
"NG",
"JR",
"PWTC",
"EG",
"XD",
"CMDK",
"HKIF",
"QM",
"NJGP",
"T",
"NWVA",
"GWP",
"VNFR",
"QP",
"KJWN",
"OZSZ",
"PL",
"CBW",
"CMU",
"N",
"SYSW",
"JH",
"O",
"D",
"R",
"P",
"CU",
"Z",
"Y",
"ANZ",
"OUS",
"MQ",
"GO",
"O",
"VOVU",
"X",
"F",
"FIK",
"OS",
"QTM",
"BXB",
"U",
"HJ",
"QV",
"GTWQ",
"VAU",
"G",
"HINL",
"XCUM",
"SWWW",
"YMUK",
"Y",
"OO",
"YUKT",
"C",
"VN",
"LGO",
"A",
"G",
"U",
"N",
"E",
"AS",
"QU",
"XJP",
"B",
"WLE",
"FZE",
"D",
"GKDE",
"FVEH",
"TTX",
"Q",
"O",
"XR",
"A",
"MKTL",
"Q",
"TVPH",
"GKTD",
"UKT",
"SHZ",
"Y",
"LW",
"Q",
"EOW",
"GDC",
"VXEI",
"HAM",
"G",
"RZ",
"NVY",
"RR",
"JK",
"OZRL",
"ZLO",
"J",
"ZVH",
"IV",
"VWXJ",
"R",
"L",
"QJ",
"YD",
"VPNK",
"FUY",
"K",
"E",
"VTR",
"HZ",
"IJGU",
"LJKS",
"D",
"F",
"WQ",
"US",
"IG",
"NK",
"AFGN",
"GKOH",
"JJUL",
"K",
"LJML",
"SF",
"YUNV",
"HZSX",
"WRHU",
"B",
"IIN",
"BL",
"UT",
"L",
"F",
"IPMB",
"F",
"AN",
"VM",
"LJY",
"NRTV",
"H",
"V",
"ZTTF",
"K",
"CDGX",
"RF",
"F",
"LX",
"VF",
"VRE",
"YC",
"IIX",
"CXQ",
"WNWB",
"Q",
"ZJB",
"GV",
"C",
"AAY",
"ISZ",
"ARGV",
"OO",
"W",
"SE",
"SRX",
"FV",
"UVVD",
"Y",
"XR",
"Z",
"DMTE",
"I",
"QGQT",
"TB",
"E",
"OC",
"BFA",
"BSZ",
"Z",
"LV",
"U",
"U",
"H",
"OS",
"YEF",
"NOO",
"PYW",
"JDH",
"UMA",
"XDL",
"VF",
"EOS",
"Q",
"KO",
"GX",
"XHZ",
"AQHQ",
"LVB",
"VN",
"EL",
"WCBA",
"JZB",
"VRI",
"PU",
"DKJT",
"SR",
"DIP",
"BYPV",
"GD",
"PQKZ",
"TF",
"KH",
"JRIL",
"QY",
"F",
"MO",
"F",
"UCU",
"KU",
"XLT",
"CSX",
"KAN",
"LO",
"GOL",
"W",
"T",
"VWE",
"NQHX",
"QC",
"T",
"ON",
"CS",
"FYKC",
"ATYL",
"XWZE",
"A",
"QJE",
"CK",
"C",
"MF",
"LBX",
"Q",
"OA",
"FGG",
"LBX",
"L",
"MQ",
"OMY",
"EGLW",
"GY",
"MJV",
"MXLU",
"NWGL",
"SL",
"PT",
"F",
"A",
"VPBJ",
"WUMD",
"CH",
"UBF",
"TLQ",
"T",
"SMYU",
"VZNU",
"QKHI",
"KYH",
"X",
"L",
"V",
"P",
"TAUZ",
"KXJA",
"AIO",
"UCA",
"R",
"PQWC",
"MH",
"TX",
"J",
"GUJ",
"WIBZ",
"Y",
"XPR",
"BV",
"VJAD",
"RY",
"RDR",
"JZOY",
"PDR",
"WZGY",
"KYA",
"XAUH",
"JEV",
"QMF",
"ZZ",
"NK",
"V",
"H",
"U",
"XQG",
"K",
"C",
"IUW",
"UKPL",
"YTY",
"W",
"V",
"C",
"KGO",
"VAZ",
"ZOD",
"Y",
"SAKC",
"KLOF",
"W",
"EMCV",
"P",
"K",
"BESP",
"D",
"P",
"LKM",
"PUW",
"QYX",
"Q",
"VM",
"GRD",
"HCPN",
"MUFI",
"S",
"WX",
"NAKG",
"H",
"TX",
"B",
"VM",
"J",
"APGC",
"N",
"NBCU",
"AJ",
"Z",
"SYXT",
"K",
"F",
"C",
"QE",
"BBDI",
"A",
"CP",
"D",
"TE",
"FTWC",
"WKTP",
"OEKJ",
"P",
"XF",
"KHK",
"YX",
"JG",
"RAM",
"M",
"MK",
"KPEE",
"LPW",
"XA",
"CY",
"UENC",
"EKTZ",
"BEC",
"Y",
"JG",
"EFP",
"DPU",
"E",
"RC",
"L",
"JVBG",
"TFO",
"FZHK",
"IJL",
"YLP",
"NA",
"IVXC",
"QG",
"W",
"LJZ",
"NX",
"F",
"NVCJ",
"DX",
"BD",
"UGC",
"SQ",
"SXHJ",
"YW",
"M",
"L",
"JRQV",
"YLNA",
"CEXW",
"JMJQ",
"B",
"LTMF",
"NC",
"ZSY",
"ORC",
"W",
"IU",
"W",
"DGQ",
"HC",
"O",
"AS",
"RJ",
"ZW",
"UI",
"ZPH",
"AQK",
"XZAL",
"QPOR",
"B",
"HA",
"KXJG",
"KOTU",
"QW",
"ZFFP",
"L",
"PCVX",
"GJA",
"KMIQ",
"Q",
"P",
"QA",
"IX",
"DAHR",
"NP",
"WIHA",
"HSY",
"B",
"ISZ",
"YE",
"F",
"YD",
"OCLA",
"N",
"AEG",
"DSSF",
"BNYR",
"YL",
"NESK",
"RLHJ",
"ZGW",
"LSN",
"M",
"DTDP",
"YNPF",
"OFSX",
"A",
"CVJG",
"UTLL",
"GRL",
"Y",
"Z",
"XA",
"ST",
"SH",
"NHH",
"KXKA",
"RBV",
"SSU",
"UR",
"TS",
"ZD",
"HJV",
"WTDF",
"X",
"NE",
"K",
"RYH",
"OI",
"LD",
"FUCL",
"CMBS",
"FNE",
"D",
"RHA",
"V",
"ZZS",
"EEQ",
"MPG",
"VW",
"TT",
"LJQG",
"VN",
"F",
"T",
"CQRO",
"JB",
"F",
"RDA",
"SO",
"YCK",
"UB",
"P",
"TVQL",
"UAEY",
"Z",
"SN",
"S",
"J",
"NPG",
"VNHM",
"YSTM",
"M",
"L",
"PZDW",
"LG",
"YEF",
"R",
"N",
"HKKY",
"CD",
"HSQX",
"IT",
"MNR",
"BS",
"QAGK",
"OIFE",
"DRJ",
"X",
"YSBA",
"FJO",
"YIFR",
"ECK",
"G",
"GRTV",
"I",
"RQ",
"S",
"B",
"CE",
"J",
"RN",
"QEHS",
"AXQ",
"UP",
"SMW",
"WMF",
"OC",
"EU",
"V",
"C",
"U",
"BGOO",
"BGQV",
"OO",
"GUB",
"VR",
"U",
"KZJ",
"GSE",
"DAC",
"SX",
"DKHE",
"RM",
"ULSV",
"OL",
"BQC",
"L",
"MZJN",
"MSOC",
"B",
"MLZE",
"UC",
"C",
"HTDM",
"EII",
"UN",
"EJLK",
"LMRN",
"JJ",
"L",
"D",
"V",
"W",
"FVBL",
"H",
"CNVD",
"IPK",
"IKO",
"GVT",
"O",
"YVX",
"LTAI",
"TJRI",
"PFYQ",
"K",
"SS",
"A",
"KPUT",
"K",
"C",
"MH",
"PJEY",
"VUD",
"PD",
"QJ",
"CZ",
"GA",
"ID",
"QPFA",
"JHOW",
"ONV",
"GPOU",
"NOUH",
"MHV",
"RT",
"DK",
"O",
"UN",
"JLPP",
"RY",
"WD",
"LCGU",
"RP",
"KGPG",
"SKBY",
"Z",
"IDQ",
"FLB",
"YLR",
"NT",
"Z",
"P",
"Z",
"EHDF",
"ZZ",
"M",
"PRO",
"DWDL",
"KRK",
"KX",
"QWP",
"WM",
"FNZ",
"U",
"U",
"HXI",
"F",
"XCDV",
"VBYA",
"SKF",
"CBW",
"BQVQ",
"DNN",
"EJ",
"EG",
"KZOO",
"XNL",
"M",
"OSUS",
"NW",
"BZJV",
"EV",
"AR",
"CDF",
"X",
"M",
"M",
"VH",
"A",
"OL",
"XYF",
"QDXS",
"XD",
"GO",
"T",
"ZR",
"JZ",
"N",
"Y",
"FXN",
"Y",
"KPPE",
"BGD",
"QNX",
"NQ",
"UFZS",
"CS",
"WCTF",
"Y",
"DCO",
"BTH",
"IAS",
"LTU",
"LT",
"U",
"GG",
"GTS",
"B",
"KP",
"O",
"UMT",
"JPWQ",
"G",
"LLRX",
"M",
"QT",
"PB",
"WGD",
"UL",
"RYG",
"IHN",
"ZW",
"H",
"NBU",
"GA",
"GAD",
"AA",
"TCHH",
"WC",
"TF",
"KSJ",
"X",
"CV",
"AHWK",
"QJRS",
"TM",
"OPF",
"RHJM",
"BNE",
"GHQ",
"MH",
"MWSW",
"JHTF",
"MX",
"VOFQ",
"SZ",
"W",
"OC",
"B",
"CTNW",
"MCO",
"G",
"Q",
"IQX",
"NT",
"OWWC",
"SK",
"XUIG",
"Y",
"HS",
"BJWS",
"UOB",
"AYKX",
"JR",
"NJ",
"KFX",
"DCLQ",
"B",
"CC",
"T",
"MIQB",
"XZA",
"PNVB",
"UZF",
"Q",
"O",
"Y",
"WGDG",
"A",
"GR",
"TI",
"MPW",
"TRP",
"Z",
"XRGZ",
"E",
"ZH",
"DM",
"IGW",
"L",
"MIE",
"DA",
"FKE",
"NKIZ",
"MG",
"E",
"WM",
"ANFL",
"WVWC",
"CS",
"U",
"CYO",
"MCGT",
"RZ",
"VT",
"FN",
"ZV",
"K",
"R",
"AG",
"VD",
"H",
"U",
"RM",
"SHWV",
"W",
"CPZT",
"ZZTE",
"DUA",
"ZPO",
"O",
"UZDA",
"EU",
"L",
"F",
"PNI",
"TDPS",
"TD",
"EHW",
"UGXM",
"FKJ",
"CYX",
"SQ",
"SI",
"G",
"LJF",
"KNU",
"CO",
"CK",
"TM",
"T",
"MOLZ",
"RDZC",
"KEHD",
"DPD",
"ZE",
"O",
"WFQI",
"MQ",
"W",
"Q",
"RKX",
"UX",
"ZR",
"UN",
"GZT"
	};
	#endregion
	
	public float spawnTimer;
	public float targetStockDensity;
	public float badStockDiffusionChance;
	
	private struct Symbol {
		public int x;
		public int y;
		public string name;
		public float changePct;
	}
	
	private List<Symbol> symbols;
	private float t;
	
	void Start() {
		symbols = new List<Symbol>();
		t = 0f;
	}
	
	void checkSymbols() {
		foreach (Symbol s in symbols) {
			if (Random.Range(0f, 1f) < badStockDiffusionChance) {
				SnakeGame.Singleton[s.x, s.y] = SnakeGame.EMPTY_COLOR;
			}
		}
		// This will remove symbols that have been eaten or diffused from the ticker
		symbols = symbols.Where(s => 
			SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.BadStock || 
			SnakeGame.Singleton[s.x, s.y] == SnakeGame.Singleton.GoodStock
		).OrderBy<Symbol, float>(s => s.changePct).ToList();
	}
	
	void spawnSymbol() {
		Symbol s = new Symbol();
		int x = 0, y = 0;
		for (int i = 0; i < stockPlaceTries; i++) {
			x = Random.Range(1, SnakeGame.Width - 1);
			y = Random.Range(1, SnakeGame.Height - 1);
			if (SnakeGame.Singleton[x, y] == SnakeGame.EMPTY_COLOR) {
				break;
			}
			if (i == stockPlaceTries - 1) {
				Debug.Log("Couldn't place a stock");
				return;
			}
		}
		s.x = x;
		s.y = y;
		s.name = symbolNames[Random.Range(0, symbolNames.Length)];
		s.changePct = Random.Range(-5f, 5f);
		symbols.Add(s);
		SnakeGame.Singleton[s.x, s.y] = s.changePct > 0f ? SnakeGame.Singleton.GoodStock : SnakeGame.Singleton.BadStock;
	}
	
	void Update() {
		t += Time.deltaTime;
		while (t > spawnTimer) {
			t -= spawnTimer;
			checkSymbols();
			if (symbols.Count < targetStockDensity * SnakeGame.Width * SnakeGame.Height) {
				spawnSymbol();
			}
		}
	}
	
	void OnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width * 0.8f, 0f, Screen.width * 0.2f, Screen.height));
		foreach (Symbol s in symbols) {
			GUILayout.Label(s.name);
		}
		GUILayout.EndArea();
	}
}
