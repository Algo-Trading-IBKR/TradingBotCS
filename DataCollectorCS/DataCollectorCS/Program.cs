﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataCollectorCS;
using IBApi;
using TradingBotCS.HelperClasses;
using TradingBotCS.IBApi_OverRide;

namespace TradingBotCS
{
    class Program
    {
        public static float TradeCash = 100;
        //static string Ip = "jorenvangoethem.duckdns.org";
        //static string Ip = "192.168.50.107";
        static string Ip = "127.0.0.1";
        static int Port = 4001;
        static int ApiId = 1;
        public static WrapperOverride IbClient = new WrapperOverride();
        static EReader IbReader;

        public static List<Symbol> SymbolObjects;
        //public static List<string> SymbolList = new List<string>() { "TSLA", "AAPL", "NTLA", "ABUS", "XBIOW", "CHD", "APPF", "GRBK", "ADVM", "UNM", "RLAY", "RDFN", "XOM", "PLD", "STAY", "NERV", "LPLA", "MCHI", "MPWR", "BGNE", "CRBP", "C", "GTHX", "CLDX" };

        #region list
        public static List<string> SymbolList = new List<string>() { "TXG", "YI", "PIH", "PIHPP", "TURN", "FLWS", "BCOW", "ONEM", "FCCY", "SRCE", "VNET", "TWOU", "QFIN", "KRKR", "JOBS", "ETNB", "JFK", "JFKKR", "JFKKU", "JFKKW", "EGHT", "NMTR", "JFU", "AAON", "ABEO", "ABMD", "AXAS", "ACIU", "ACIA", "ACTG", "ACHC", "ACAD", "ACAM", "ACAMU", "ACAMW", "ACST", "AXDX", "ACCP", "XLRN", "ACCD", "ARAY", "ACEVU", "ACLL", "ACRX", "ACER", "ACHV", "ACIW", "ACRS", "ACMR", "ACNB", "ACOR", "ATVI", "AFIB", "ADMS", "ADMP", "AHCO", "ADAP", "ADPT", "ADXN", "ADUS", "AEY", "ADIL", "ADILW", "ADTX", "ADMA", "ADBE", "ADTN", "ADRO", "ADES", "AEIS", "AMD", "ADXS", "ADVM", "DWEQ", "DWAW", "DWUS", "DWMC", "DWSH", "ACT", "AEGN", "AGLE", "AEHR", "AMTX", "AERI", "AVAV", "ARPO", "AIH", "AEZS", "AEMD", "AFMD", "AFYA", "AGBA", "AGBAR", "AGBAU", "AGBAW", "AGEN", "AGRX", "AGYS", "AGIO", "AGMH", "AGNC", "AGNCM", "AGNCN", "AGNCO", "AGNCP", "API", "AGFS", "AIKI", "ALRN", "AIMT", "AIRT", "AIRTP", "AIRTW", "ATSG", "AIRG", "ANTE", "AKAM", "AKTX", "AKCA", "AKBA", "KERN", "KERNW", "AKRO", "AKER", "AKUS", "AKTS", "AKU", "ALRM", "ALSK", "ALAC", "ALACR", "ALACU", "ALACW", "ALBO", "ALDX", "ALEC", "ALRS", "ALXN", "ALCO", "ALIT", "ALGN", "ALIM", "ALYA", "ALJJ", "ALKS", "ALLK", "ABTX", "ALGT", "ALNA", "ARLP", "LNT", "AESE", "AHPI", "AMOT", "ALLO", "ALLT", "ALVR", "MDRX", "ALNY", "AOSL", "GOOG", "GOOGL", "SMCP", "ATEC", "ALPN", "ALTA", "ALTR", "ATHE", "ALT", "ASPS", "AIMC", "ALTM", "ALXO", "AMAG", "AMAL", "AMRN", "AMRK", "AMZN", "AMBC", "AMBA", "AMCX", "AMCI", "AMCIU", "AMCIW", "DOX", "AMED", "AMTB", "AMTBB", "UHAL", "AMRH", "AMRHW", "ATAX", "AMOV", "AAL", "AFIN", "AFINP", "AMNB", "ANAT", "AOUT", "APEI", "AREC", "AMRB", "AMSWA", "AMSC", "AVCT", "AVCTW", "AMWD", "CRMT", "ABCB", "AMSF", "ASRV", "ASRVP", "ATLO", "AMGN", "FOLD", "AMKR", "AMPH", "IBUY", "AMHC", "AMHCU", "AMHCW", "AMYT", "ASYS", "AMRS", "ADI", "ANAB", "AVXL", "ANCN", "ANDA", "ANDAR", "ANDAU", "ANDAW", "ANGI", "ANGO", "ANIP", "ANIK", "ANIX", "ANNX", "ANPC", "ANSS", "ATRS", "ATEX", "APA", "APLS", "APEX", "APXT", "APXTU", "APXTW", "APHA", "APOG", "APEN", "AINV", "AMEH", "APPF", "APPN", "AAPL", "APDN", "AGTC", "AMAT", "AMTI", "AAOI", "APLT", "AUVI", "APRE", "APVO", "APTX", "APM", "APTO", "APYX", "AQMS", "AQB", "AQST", "ARAV", "ABUS", "ABIO", "RKDA", "ARCB", "ACGL", "ACGLO", "ACGLP", "FUV", "ARCE", "ARCT", "ARQT", "ARDX", "ARNA", "ARCC", "ARGX", "ARDS", "ARKR", "DWCR", "DWAT", "AROW", "ARWR", "ARTL", "ARTLW", "ARTNA", "AACQ", "AACQU", "AACQW", "ARTW", "ARVN", "ARYB", "ARYBU", "ARYBW", "ARYA", "ASND", "APWC", "ASLN", "ASML", "ASPU", "AZPN", "AWH", "ASMB", "ASRT", "ASFI", "ASTE", "ATRO", "ALOT", "ASTC", "ASUR", "AACG", "ATRA", "ATNX", "ATHX", "ATIF", "AAME", "ACBI", "AUB", "AUBAP", "AY", "ATLC", "AAWW", "ATCX", "ATCXW", "TEAM", "ATNI", "ATOM", "ATOS", "BCEL", "ATRC", "ATRI", "LIFE", "AUBN", "AUDC", "AEYE", "AUPH", "EARS", "JG", "ADSK", "AUTL", "ADP", "AUTO", "AVDL", "AHI", "AVCO", "ATXI", "AVEO", "AVNW", "CDMO", "CDMOP", "AVID", "RNA", "AVGR", "CAR", "RCEL", "AVT", "AVRO", "AWRE", "ACLS", "AXLA", "AXGN", "AAXN", "AXNX", "AXGT", "AXSM", "AXTI", "AYLA", "AYRO", "AYTU", "AZRX", "RILY", "RILYG", "RILYH", "RILYI", "RILYL", "RILYM", "RILYN", "RILYO", "RILYP", "RILYZ", "BOSC", "BIDU", "BCPC", "BLDP", "BANF", "BANFP", "BAND", "BFC", "BOCH", "BMRC", "BMLP", "BKSC", "BOTJ", "OZK", "BSVN", "BFIN", "BWFG", "BANR", "BZUN", "DFVL", "DFVS", "TAPR", "BBSI", "GOLD", "BSET", "ZTEST", "BXRX", "BCML", "BBQ", "BCBP", "BCTG", "BECN", "BEAM", "BBGI", "BBBY", "BGNE", "BELFA", "BELFB", "BLPH", "BLCM", "BLU", "BNFT", "BNTC", "BLI", "BRY", "BWMX", "XAIR", "BYND", "BYSI", "BGCP", "BCYC", "BGFV", "BRPA", "BRPAR", "BRPAU", "BRPAW", "BIGC", "BILI", "BASI", "BCDA", "BCDAW", "BIOC", "BCRX", "BDSI", "BFRA", "BIIB", "BHTG", "BKYI", "BIOL", "BLFS", "BLRX", "BMRN", "BMRA", "BNGO", "BNGOW", "BVXV", "BNTX", "BPTH", "BSGM", "BSTC", "TECH", "BEAT", "BIVI", "BTAI", "BJRI", "BDTX", "BLKB", "BL", "BKCC", "TCPC", "BLNK", "BLNKW", "BLMN", "BCOR", "BLBD", "BHAT", "BLUE", "BLCT", "BKEP", "BKEPP", "BPMC", "ITEQ", "BMCH", "BSBK", "WIFI", "BOKF", "BOKFL", "BNSO", "BKNG", "BIMI", "BRQS", "BOMN", "BPFH", "EPAY", "BOWXU", "BOXL", "BBRX", "BCLI", "BWAY", "BCTX", "BBI", "BDGE", "BBIO", "BLIN          ", "BWB", "BRID", "BCOV", "BHF", "BHFAL", "BHFAO", "BHFAP", "BRLI", "BRLIR", "BRLIU", "BRLIW", "AVGO", "AVGOP", "BYFC", "BWEN", "BROG", "BROGW", "BPY", "BPYPN", "BPYPO", "BPYPP", "BPYU", "BPYUP", "BRKL", "BCAC", "BCACU", "BRKS", "BRP", "DOOO", "BRKR", "BMTC", "BSQR", "BLDR", "BTAQU", "BNR", "BFST", "CFFI", "CHRW", "CABA", "CCMP", "CDNS", "CDZI", "CZR", "CSTE", "CLBS", "CHY", "CHI", "CCD", "CHW", "CGO", "CPZ", "CSQ", "CAMP", "CVGW", "CALB", "CALA", "CALT", "CALM", "CLMT", "CLXT", "CMBM", "CATC", "CAC", "CAMT", "CAN", "CSIQ", "CGIX", "CPHC", "CBNK", "CCBG", "CPLP", "CSWC", "CSWCL", "CPTA", "CPTAG", "CPTAL", "CFFN", "CAPR", "CSTR", "CPST", "CARA", "CRDF", "CSII", "CDLX", "CATM", "CDNA", "CTRE", "CARG", "PRTS", "TAST", "CARE", "CARV", "CASA", "CWST", "CASY", "CASI", "CASS", "SAVA", "CSTL", "CTRM", "CATB", "CBIO", "CPRX", "CATY", "CVCO", "CBFV", "CBAT", "CBMB", "CBOE", "CBTX", "CDK", "CDW", "CECE", "CELC", "CLDX", "APOP", "APOPW", "CLRB", "CLRBZ", "CLLS", "CBMG", "CLSN", "CELH", "CYAD", "CETX", "CETXP", "CETXW", "CDEV", "CNTG", "CETV", "CENT", "CENTA", "CVCY", "CNTX", "CENX", "CNBKA", "CNTY", "CRNT", "CERC", "CRNC", "CERN", "CERS", "CEVA", "CFBK", "CFFA", "CFFAU", "CFFAW", "CFIIU", "CSBR", "CHNG", "CHNGU", "CTHR", "GTLS", "CHTR", "CHKP", "CHEK", "CHEKZ", "CMPI", "CKPT", "CEMI", "CCXI", "CHMG", "CHFS", "CHMA", "CSSE", "CSSEN", "CSSEP", "PLCE", "CMRX", "CAAS", "CBPO", "CCCL", "CCRC", "JRJC", "HGSH", "CIH", "CJJD", "CLEU", "CHNR", "CREG", "SXTC", "CXDC", "PLIN", "CNET", "IMOS", "COFS", "CHPM", "CHPMU", "CHPMW", "CDXC", "CHSCL", "CHSCM", "CHSCN", "CHSCO", "CHSCP", "CHDN", "CHUY", "CDTX", "CIIC", "CIICU", "CIICW", "CMCT", "CMCTP", "CMPR", "CNNB", "CINF", "CIDM", "CTAS", "CRUS", "CSCO", "CTRN", "CTXR", "CTXRW", "CZNC", "CZWI", "CIZN", "CTXS", "CHCO", "CIVB", "CLAR", "CLNE", "CLSK", "CACG", "YLDE", "LRGE", "CLFD", "CLRO", "CLPT", "CLSD", "CLIR", "CBLI", "CLVS", "CLPS", "CMLFU", "CME", "CCNE", "CCNEP", "CNSP", "CCB", "COKE", "COCP", "CODA", "CCNC", "CDXS", "CODX", "CVLY", "JVA", "CCOI", "CGNX", "CTSH", "CWBR", "COHR", "CHRS", "COHU", "CGRO", "CGROU", "CGROW", "CLCT", "COLL", "CIGI", "CLGN", "CBAN", "HHT", "COLB", "CLBK", "COLM", "CMCO", "CMCSA", "CBSH", "CVGI", "COMM", "JCS", "ESXB", "CFBI", "CTBI", "CWBC", "CVLT", "CGEN", "CPSI", "CTG", "SCOR", "CHCI", "CMTL", "CNCE", "BBCP", "CDOR", "CNDT", "CFMS", "CNFR", "CNFRL", "CNMD", "CNOB", "CONN", "CNSL", "CWCO", "CNST", "ROAD", "CPSS", "CFRX", "CPTI", "CPAA", "CPAAU", "CPAAW", "CPRT", "CRBP", "CORT", "CORE", "CSOD", "CRTX", "CLDB", "CRVL", "KOR", "CRVS", "CSGP", "COST", "CPAH", "ICBK", "COUP", "CVLG", "CVET", "COWN", "COWNL", "COWNZ", "CPSH", "CRAI", "CBRL", "BREW", "CRTD", "CRTDW", "CREX", "CREXW", "CACC", "GLDI", "SLVO", "USOI", "CREE", "CRSA", "CRSAU", "CRSAW", "CCAP", "CRESY", "CXDO", "CRNX", "CRSP", "CRTO", "CROX", "CRON", "CCRN", "CFB", "CRWD", "CRWS", "CYRX", "CSGS", "CCLP", "CSPI", "CSWI", "CSX", "CTIC", "CUE", "CPIX", "CMLS", "CVAC", "CRIS", "CUTR", "CVBF", "CVV", "CYAN", "CYBR", "CYBE", "CYCC", "CYCCP", "CYCN", "CBAY", "CYRN", "CONE", "CYTK", "CTMX", "CTSO", "DADA", "DJCO", "DAKT", "DARE", "DRIO", "DRIOW", "DSKE", "DSKEW", "DAIO", "DDOG", "DTSS", "PLAY", "DTEA", "DFNL", "DINT", "DUSA", "DWLD", "DWSN", "DBVT", "DCPH", "DFHTU", "IBBJ", "TACO", "DCTH", "DNLI", "DENN", "XRAY", "DRMT", "DMTK", "DXLG", "DSWL", "DXCM", "DFPH", "DFPHU", "DFPHW", "DMAC", "DHIL", "FANG", "DPHC", "DPHCU", "DPHCW", "DRNA", "DFFN", "DGII", "DMRC", "DRAD", "DRADP", "DGLY", "APPS", "DCOM", "DCOMP", "DIOD", "DRTT", "DISCA", "DISCB", "DISCK", "DISH", "DHC", "DHCNI", "DHCNL", "DLHC", "BOOM", "DOCU", "DOGZ", "DLTR", "DLPN", "DLPNW", "DOMO", "DGICA", "DGICB", "DMLP", "DORM", "DDI", "DOYU", "DKNG", "LYL", "DBX", "DSPG", "DCT", "DLTH", "DNKN", "DUOT", "DRRX", "DXPE", "DYAI", "DYNT", "DVAX", "DZSI", "ETFC", "ETACU", "SSP", "EBMT", "EGBN", "EGLE", "EGRX", "IGLE", "ERESU", "ESSC", "ESSCR", "ESSCU", "ESSCW", "EWBC", "EML", "EAST", "EVGBC", "EVSTC", "EVLMC", "EBON", "EBAY", "EBAYL", "EBIX", "ECHO", "SATS", "MOHO", "EDAP", "EDSA", "EDNT", "EDIT", "EDUC", "EGAN", "EH", "EHTH", "EIDX", "EIGR", "EKSO", "LOCO", "ESLT", "SOLO", "SOLOW", "ECOR", "EA", "ELSE", "ESBK", "ELOX", "ELTK", "EMCF", "EMKR", "ENTA", "ECPG", "WIRE", "ENDP", "NDRA", "NDRAW", "EIGI", "WATT", "EFOI", "ERII", "ENG", "ENLV", "ENOB", "ENPH", "ESGR", "ESGRO", "ESGRP", "ETTX", "ENTG", "ENTX", "ENTXW", "EBTC", "EFSC", "EVSI", "EVSIW", "EPZM", "PLUS", "EPSN", "EQ", "EQIX", "EQBK", "ERIC", "ERIE", "ERYP", "ESCA", "ESPR", "GMBL", "GMBLW", "ESQ", "ESSA", "EPIX", "ESTA", "VBND", "VUSE", "VIDI", "ETON", "ETSY", "CLWT", "EDRY", "EEFT", "ESEA", "EVLO", "EVBG", "EVK", "EVER", "MRAM", "EVOP", "EVFM", "EVGN", "EVOK", "EOLS", "EVOL", "EXAS", "XGN", "ROBO", "XELA", "EXEL", "EXC", "EXFO", "XCUR", "EXLS", "EXPI", "EXPE", "EXPD", "EXPC", "EXPCU", "EXPCW", "EXPO", "STAY", "EXTR", "EYEG", "EYEN", "EYPT", "EZPW", "FLRZ", "FFIV", "FB", "FLMN", "FLMNW", "DUO", "FANH", "FARM", "FMAO", "FMNB", "FAMI", "FARO", "FAST", "FAT", "FATBP", "FATBW", "FATE", "FTHM", "FBSS", "FNHC", "FENC", "GSM", "FFBW", "FGEN", "FDBC", "ONEQ", "FDUS", "FDUSG", "FDUSL", "FDUSZ", "FRGI", "FITB", "FITBI", "FITBO", "FITBP", "FISI", "FSRV", "FSRVU", "FSRVW", "FTAC", "FTACU", "FTACW", "FEYE", "FBNC", "FNLC", "FRBA", "BUSE", "FBIZ", "FCAP", "FCBP", "FCNCA", "FCNCP", "FCBC", "FCCO", "FCRD", "FFBC", "FFIN", "THFF", "FFNW", "FFWM", "FGBI", "FHB", "INBK", "INBKL", "INBKZ", "FIBK", "FRME", "FMBH", "FMBI", "FMBIO", "FMBIP", "FXNC", "FNWB", "FSFG", "FSEA", "FSLR", "FAAR", "FPA", "BICK", "FBZ", "FTHI", "FCAL", "FCAN", "FTCS", "FCEF", "FCA", "SKYY", "RNDM", "FDT", "FDTS", "FVC", "FV", "IFV", "DDIV", "DVOL", "DVLU", "DWPP", "DALI", "FDNI", "FEM", "RNEM", "FEMB", "FEMS", "FTSM", "FEP", "FEUZ", "FGM", "FTGC", "FTLB", "HYLS", "FHK", "NFTY", "FTAG", "FTRI", "LEGR", "NXTG", "FPXI", "FPXE", "FJP", "FEX", "FTC", "RNLC", "FTA", "FLN", "LMBS", "LDSF", "FMB", "FMK", "FNX", "FNY", "RNMC", "FNK", "FAD", "FAB", "MDIV", "MCEF", "FMHI", "QABA", "ROBT", "FTXO", "QCLN", "GRID", "CIBR", "FTXG", "CARZ", "FTXN", "FTXH", "FTXD", "FTXL", "TDIV", "FTXR", "QQEW", "QQXT", "QTEC", "AIRR", "RDVY", "RFAP", "RFDI", "RFEM", "RFEU", "FID", "FTSL", "FYX", "FYC", "RNSC", "FYT", "SDVY", "FKO", "FCVT", "FDIV", "FSZ", "FIXD", "TUSA", "FKU", "RNDV", "FUNC", "FUSB", "MYFW", "FCFS", "SVVC", "FSV", "FISV", "FIVE", "FPRX", "FVE", "FIVN", "FLEX", "FLXN", "SKOR", "ASET", "ESG", "ESGG", "LKOR", "QLC", "MBSD", "FPAY", "FLXS", "FLIR", "FLWR", "FLNT", "FLDM", "FFIC", "FLUX", "FNCB", "FOCS", "FONR", "FRSX", "FMTX", "FORM", "FORTY", "FORR", "FBRX", "FRTA", "FTNT", "FBIO", "FBIOP", "FMCI", "FMCIU", "FMCIW", "FIIIU", "FWRD", "FORD", "FWP", "FOSL", "FOX", "FOXA", "FOXF", "FRAN", "FRG", "FELE", "FRAF", "FRHC", "FRLN", "RAIL", "FEIM", "FREQ", "FRPT", "FTDR", "FRPH", "FSBW", "FSDC", "HUGE", "FTOCU", "FTEK", "FCEL", "FULC", "FLGT", "FORK", "FLL", "FMAX", "FULT", "FNKO", "FUSN", "FUTU", "FTFT", "FFHL", "FVCB", "WILC", "GTHX", "GAIA", "GLPG", "GALT", "GRTX", "GLMD", "GMDA", "GLPI", "GAN", "GRMN", "GARS", "GLIBA", "GLIBP", "GDS", "GNSS", "GENC", "GFN", "GFNCP", "GFNSL", "GBIO", "GENE", "GTH", "GNFT", "GNUS", "GMAB", "GNMK", "GNCA", "GNPX", "GNTX", "THRM", "GEOS", "GABC", "GERN", "GEVO", "ROCK", "GIGM", "GIII", "GILT", "GILD", "GBCI", "GLAD", "GLADD", "GLADL", "GOOD", "GOODM", "GOODN", "GAIN", "GAINL", "GAINM", "LAND", "LANDP", "GLBZ", "GBT", "GBLI", "GBLIL", "SELF", "GWRS", "AIQ", "DRIV", "POTX", "CLOU", "KRMA", "BUG", "DAX", "EBIZ", "EDUT", "FINX", "CHIC", "GNOM", "BFIT", "SNSR", "LNGR", "MILN", "EFAS", "QYLD", "BOTZ", "CATH", "CEFA", "SOCL", "ALTY", "SRET", "EDOC", "GXTG", "HERO", "YLCO", "GLBS", "GSMG", "GSMGW", "GLUU", "GLYC", "GOGO", "GOCO", "GLNG", "GMLP", "GMLPP", "BTBT", "GDEN", "GOGL", "GBDC", "GTIM", "GBLK", "GSHD", "GPRO", "GHIV", "GHIVU", "GHIVW", "GRSVU", "GMHI", "GMHIU", "GMHIW", "GOSS", "LOPE", "GRVY", "GECC", "GECCL", "GECCM", "GECCN", "GEC", "GLDD", "GSBC", "GRBK", "GPP", "GPRE", "GRCY", "GRCYU", "GRCYW", "GCBC", "GTEC", "GNLN", "GLRE", "GP", "GRNQ", "GNRS", "GNRSU", "GNRSW", "GSKY", "GRNV", "GRNVR", "GRNVU", "GRNVW", "GDYN", "GDYNW", "GSUM", "GRIF", "GRFS", "GRIN", "GRTS", "GO", "GRPN", "GRWG", "OMAB", "GGAL", "GVP", "GSIT", "GTYH", "GNTY", "GFED", "GH", "GHSI", "GIFI", "GURE", "GPOR", "GWPH", "GWGH", "GXGX", "GXGXU", "GXGXW", "GYRO", "HEES", "HLG", "HOFV", "HOFVW", "HNRG", "HALL", "HALO", "HLNE", "HJLI", "HJLIW", "HWC", "HWCPL", "HWCPZ", "HAFC", "HAPP", "HCDI", "HONE", "HLIT", "HRMY", "HARP", "HROW", "HBIO", "HCAP", "HCAPZ", "HAS", "HA", "HWKN", "HWBK", "HYAC", "HYACU", "HYACW", "HAYN", "HBT", "HDS", "HHR", "HCAT", "HSAQ", "HCCO", "HCCOU", "HCCOW", "HCSG", "HTIA", "HQY", "HSTM", "HTLD", "HTLF", "HTLFP", "HTBX", "HEBT", "HSII", "HELE", "HLIO", "HSDT", "HMTV", "HNNA", "HCAC", "HCACU", "HCACW", "HSIC", "HEPA", "HTBK", "HFWA", "HGBL", "HCCI", "MLHR", "HRTX", "HSKA", "HX", "HFEN", "HFFG", "HIBB", "CAPAU", "SNLN", "HPK", "HPKEW", "HIHO", "HIMX", "HIFS", "HQI", "HSTO", "HKIT", "HCCH", "HCCHR", "HCCHU", "HCCHW", "HMNF", "HMSY", "HOLUU", "HOLI", "HOLX", "HBCP", "HOMB", "HFBL", "HMST", "HTBI", "FIXX", "HOFT", "HOOK", "HOPE", "HBNC", "HRZN", "HZNP", "TWNK", "TWNKW", "HOTH", "HMHC", "HWCC", "HOVNP", "HBMD", "HTGM", "HTHT", "HUBG", "HUSN", "HEC", "HECCU", "HECCW", "HSON", "HDSN", "HUIZ", "HBAN", "HBANN", "HBANO", "HURC", "HURN", "HCM", "HBP", "HVBC", "HYMC", "HYMCW", "HYMCZ", "HYRE", "IIIV", "IAC", "IBEX", "ICAD", "IEP", "ICCH", "ICFI", "ICHR", "ICLK", "ICLR", "ICON", "ICUI", "IPWR", "IDEX", "IDYA", "INVE", "IDRA", "IDXX", "IEC", "IESC", "IROQ", "IFMK", "IGMS", "IHRT", "INFO", "IIVI", "IIVIP", "IKNX", "ILMN", "IMAB", "IMAC", "IMACW", "ISNS", "IMRA", "IMBI", "IMTX", "IMTXW", "IMMR", "ICCC", "IMUX", "IMGN", "IMMU", "IMVT", "IMRN", "IMRNW", "IMMP", "PI", "IMV", "NARI", "INCY", "INDB", "IBCP", "IBTX", "ILPT", "ITACU", "INFN", "INFI", "IFRX", "III", "IEA", "IEAWW", "IMKTA", "INBX", "INMD", "INMB", "IPHA", "INWK", "INOD", "IOSP", "ISSC", "INVA", "INGN", "INOV", "INO", "INZY", "INPX", "INSG", "NSIT", "ISIG", "INSM", "INSE", "IIIN", "INAQU", "PODD", "INSU", "INSUU", "INSUW", "NTEC", "IART", "IMTE", "INTC", "NTLA", "IDN", "IPAR", "IBKR", "ICPT", "IDCC", "TILE", "IBOC", "IGIC", "IGICW", "IMXI", "IDXG", "XENT", "IPLDP", "IVAC", "ITCI", "IIN", "INTU", "ISRG", "IVA", "PLW", "ADRE", "BSCK", "BSJK", "BSCL", "BSJL", "BSML", "BSAE", "BSCM", "BSJM", "BSMM", "BSBE", "BSCN", "BSJN", "BSMN", "BSCE", "BSCO", "BSJO", "BSMO", "BSDE", "BSCP", "BSJP", "BSMP", "BSCQ", "BSJQ", "BSMQ", "BSCR", "BSJR", "BSMR", "BSCS", "BSMS", "BSCT", "BSMT", "PKW", "PFM", "PYZ", "PEZ", "PSL", "PIZ", "PIE", "PXI", "PFI", "PTH", "PRN9", "PDP", "DWAS", "PTF", "PUI", "IDLB", "PRFZ", "PIO", "PGJ", "PEY", "IPKW", "PID", "KBWB", "KBWD", "KBWY", "KBWP", "KBWR", "PNQI", "PDBC", "QQQ", "ISDX", "ISEM", "IUS", "IUSS", "USLB", "PSCD", "PSCC", "PSCE", "PSCF", "PSCH", "PSCI", "PSCT", "PSCM", "PSCU", "VRIG", "PHO", "ISTR", "CMFNL", "ICMB", "ISBC", "ITIC", "NVIV", "IONS", "IOVA", "IPGP", "CLRG", "CSML", "IQ", "IRMD", "IRTC", "IRIX", "IRDM", "IRBT", "IRWD", "IRCP", "SLQD", "ISHG", "SHY", "TLT", "IEI", "IEF", "AIA", "USIG", "COMT", "ISTB", "IXUS", "IUSG", "IUSV", "IUSB", "HEWG", "DMXF", "USXF", "SUSB", "ESGD", "ESGE", "ESGU", "SUSC", "LDEM", "SUSL", "XT", "FALN", "IFGL", "BGRN", "IGF", "GNMA", "IBTA", "IBTB", "IBTD", "IBTE", "IBTF", "IBTG", "IBTH", "IBTI", "IBTJ", "IBTK", "HYXE", "IGIB", "IGOV", "EMB", "MBB", "JKI", "ACWX", "ACWI", "AAXJ", "EWZS", "MCHI", "SCZ", "EEMA", "EMXC", "EUFN", "IEUS", "RING", "SDG", "EWJE", "EWJV", "ENZL", "QAT", "TUR", "UAE", "IBB", "SOXX", "PFF", "AMCA", "EMIF", "ICLN", "WOOD", "INDY", "IJT", "DVY", "SHV", "IGSB", "ITMR", "ITOS", "ITI", "ITRM", "ITRI", "ITRN", "ISEE", "IZEA", "JJSF", "MAYS", "JBHT", "JCOM", "JKHY", "JACK", "JAGX", "JAKK", "JRVR", "JAMF", "JAN", "JSML", "JSMD", "JAZZ", "JD", "JRSH", "JBLU", "JCTCF", "JFIN", "JMPNL", "JMPNZ", "JBSS", "JOUT", "JNCE", "YY", "JAQC", "JAQCU", "KALU", "KXIN", "KALA", "KLDO", "KALV", "KMDA", "KNDI", "KRTX", "KPTI", "KSPN", "KZIA", "KBLM", "KBLMR", "KBLMU", "KBLMW", "KBSF", "KRNY", "KELYA", "KELYB", "KFFB", "KROS", "KEQU", "KTCC", "KZR", "KFRC", "KE", "KBAL", "KIN", "KC", "KINS", "KNSA", "KNSL", "KTRA", "KIRK", "KSMTU", "KTOV", "KTOVW", "KLAC", "KLXE", "KOD", "KOPN", "KRNT", "KOSS", "KWEB", "KTOS", "KRYS", "KBNT", "KBNTW", "KLIC", "KURA", "KRUS", "KVHI", "KYMR", "FSTR", "LJPC", "LSBK", "LBAI", "LKFN", "LAKE", "LRCX", "LAMR", "LANC", "LCA", "LCAHU", "LCAHW", "LNDC", "LARK", "LMRK", "LMRKN", "LMRKO", "LMRKP", "LE", "LSTR", "LTRN", "LNTH", "LTRX", "LRMR", "LSCC", "LAUR", "LAWS", "LAZY", "LCNB", "LPTX", "LEGH", "LEGN", "INFR", "LVHD", "SQLV", "LACQ", "LACQU", "LACQW", "LMAT", "TREE", "LEVL", "LEVLP", "LXRX", "LX", "LFAC", "LFACU", "LFACW", "LGIH", "LHCG", "LI", "LLIT", "LBRDA", "LBRDK", "LBTYA", "LBTYB", "LBTYK", "LILA", "LILAK", "LILAR", "BATRA", "BATRK", "FWONA", "FWONK", "LSXMA", "LSXMB", "LSXMK", "LTRPA", "LTRPB", "LSAC", "LSACU", "LSACW", "LCUT", "LFVN", "LWAY", "LGND", "LTBR", "LPTH", "LMB", "LLNW", "LMST", "LMNL", "LMNR", "LINC", "LECO", "LIND", "LGHL", "LGHLW", "LCAPU", "LPCN", "LIQT", "YVR", "LQDA", "LQDT", "LFUS", "LIVK", "LIVKU", "LIVKW", "LIVN", "LOB", "LIVE", "LPSN", "LIVX", "LVGO", "LIZI", "LKQ", "LMFA", "LMFAW", "LMPX", "LOGC", "LOGI", "CNCR", "CHNA", "LONE", "LOAC", "LOACR", "LOACU", "LOACW", "LOOP", "LORL", "LPLA", "LYTS", "LULU", "LITE", "LMNX", "LUMO", "LUNA", "LKCO", "LBC", "LYFT", "LYRA", "MCBC", "MFNC", "MTSI", "MGNX", "MDGL", "MAGS", "MGLN", "MGTA", "MGIC", "MGNI", "MGYR", "MHLD", "MNSB", "MJCO", "MMYT", "MLAC", "MLACU", "MLACW", "MBUU", "MLVF", "TUSK", "MANH", "LOAN", "MNTX", "MTEX", "MNKD", "MANT", "MARA", "MCHX", "MRIN", "MARPS", "MRNS", "MRKR", "MKTX", "MRLN", "MAR", "MBII", "MRTN", "MMLP", "MRVL", "MASI", "MCFT", "MTCH", "MTLS", "MTRX", "MAT", "MATW", "MAXN", "MXIM", "MGRC", "MDCA", "MDJH", "MDRR", "MDRRP", "MBNKP", "MFIN", "MFINL", "MDIA", "MDNA", "MNOV", "MDGS", "MDGSW", "MDWD", "MEDP", "MEIP", "MGTX", "MLCO", "MTSL", "MELI", "MBWM", "MERC", "MBIN", "MBINO", "MBINP", "MFH", "MRCY", "MREO", "MCMJ", "MCMJW", "EBSB", "VIVO", "MRBK", "MMSI", "SNUG", "MACK", "MRSN", "MRUS", "MESA", "MLAB", "MESO", "CASH", "METX", "METXW", "MEOH", "MCBS", "MGEE", "MGPI", "MBOT", "MCHP", "MU", "MSFT", "MSTR", "MVIS", "MICT", "MPB", "MTP", "MCEP", "MBCN", "MSEX", "MSBI", "MSVB", "MOFG", "MIST", "MLND", "TIGO", "MIME", "MNDO", "MIND", "MINDP", "NERV", "MGEN", "MRTX", "MIRM", "MSON", "MITK", "MKSI", "MMAC", "MTC", "MOBL", "MRNA", "MOGO", "MWK", "MKD", "MTEM", "MBRX", "MNTA", "MOMO", "MKGI", "MCRI", "MDLZ", "MGI", "MDB", "MNCL", "MNCLU", "MNCLW", "MPWR", "MNPR", "MNRO", "MRCC", "MRCCL", "MNST", "MORN", "MORF", "MOR", "MOSY", "MPAA", "MOTS", "MCAC", "MCACR", "MCACU", "MOXC", "COOP", "MTBC", "MTBCP", "MTSC", "GRIL", "MBIO", "MVBF", "MYSZ", "MYL", "MYOK", "MYOS", "MYRG", "MYGN", "NBRV", "NAKD", "NNDM", "NSTG", "NAOV", "NNOX", "NH", "NK", "NSSC", "NDAQ", "NTRA", "NATH", "NKSH", "FIZZ", "NCMI", "NESR", "NESRW", "NGHC", "NGHCN", "NGHCO", "NGHCP", "NGHCZ", "NHLD", "NHLDW", "NATI", "NRC", "NSEC", "EYE", "NWLI", "NAII", "NHTC", "NATR", "NTUS", "JSM", "NAVI", "NMCI", "NBTB", "NCNO", "NCSM", "NKTR", "NMRD", "NEOG", "NEO", "NLTX", "NEON", "NEOS", "NVCN", "NEPH", "NEPT", "UEPS", "NETE", "NTAP", "NTES", "NFIN", "NFINU", "NFINW", "NFLX", "NTGR", "NTCT", "NTWK", "NBSE", "NRBO", "NBIX", "NURO", "STIM", "NTRP", "NFE", "NPA", "NPAUU", "NPAWW", "NYMT", "NYMTM", "NYMTN", "NYMTO", "NYMTP", "NBEV", "NEWA", "NBAC", "NBACR", "NBACU", "NBACW", "NWL", "NWGI", "NHICU", "NMRK", "NWS", "NWSA", "NEWT", "NEWTI", "NEWTL", "NXMD", "NXST", "NXTC", "NEXT", "NXGN", "NGM", "NODK", "EGOV", "NICE", "NICK", "NCBS", "NKLA", "NIU", "NKTX", "LASR", "NMIH", "NNBR", "NBL", "NBLX", "NDLS", "NDSN", "NSYS", "NBN", "NTIC", "NTRS", "NTRSO", "NFBK", "NRIM", "NWBI", "NWPX", "NLOK", "NCLH", "NWFL", "NVFY", "NVMI", "NOVN", "NOVT", "NVAX", "NVCR", "NOVS", "NOVSU", "NOVSW", "NVUS", "NUAN", "NCNA", "NRIX", "NTNX", "NUVA", "QQQX", "NUZE", "NVEE", "NVEC", "NVDA", "NXPI", "NXTD", "NYMX", "OIIM", "OVLY", "OCSL", "OCSI", "OMP", "OAS", "OBLN", "OBSV", "OBCI", "OPTT", "OCFC", "OCFCP", "OFED", "OCGN", "OCUL", "ODT", "OMEX", "OPI", "OPINI", "OPINL", "OFS", "OFSSI", "OFSSL", "OFSSZ", "OCCI", "OCCIP", "OVBC", "OKTA", "ODFL", "ONB", "OPOF", "OSBC", "OLLI", "ZEUS", "OFLX", "OMER", "OMCL", "ON", "ONCY", "ONTX", "ONTXW", "ONCS", "ONCT", "OSS", "OSPN", "OSW", "ONEW", "OTRK", "OTRKP", "OPBK", "LPRO", "OTEX", "OPRA", "OPES", "OPESU", "OPESW", "OPGN", "OPNT", "OPK", "OPRT", "OBAS", "OCC", "OPRX", "OPHC", "OPTN", "OPCH", "ORMP", "OSUR", "ORBC", "OEG", "ORTX", "ORLY", "OGI", "ORGO", "ONVO", "ORGS", "ORIC", "SEED", "OBNK", "OESX", "ORSN", "ORSNR", "ORSNU", "ORSNW", "ORRF", "OFIX", "KIDS", "OSIS", "OSMT", "OSN", "OTEL", "OTG", "OTIC", "OTTR", "OTLK", "OTLKW", "OSTK", "OVID", "OXBR", "OXBRW", "OXFD", "OXLC", "OXLCM", "OXLCO", "OXLCP", "OXSQ", "OXSQL", "OXSQZ", "OYST", "PFIN", "PTSI", "PCAR", "HERD", "ECOW", "VETS", "PACB", "PEIX", "PMBC", "PPBI", "PCRX", "PACW", "PAE", "PAEWW", "PRFX", "PLMR", "PAAS", "PAND", "PANL", "PZZA", "PRTK", "TEUM", "PCYG", "PKBK", "PKOH", "PTNR", "PTRS", "PASG", "PBHC", "PATK", "PNBK", "PATI", "PDCO", "PTEN", "PAVM", "PAVMW", "PAVMZ", "PAYX", "PCTY", "PYPL", "PAYS", "CNXN", "PCB", "PCIM", "PCSB", "PCTI", "PDCE", "PDFS", "PDLI", "PDLB", "PDSB", "PGC", "PEGA", "PTON", "PENN", "PVAC", "PFLT", "PNNT", "PNNTG", "PWOD", "PEBO", "PEBK", "PFIS", "PBCT", "PBCTP", "PEP", "PRCP", "PRDO", "PRFT", "PSHG", "PFMT", "PERI", "PESI", "PPIH", "PSNL", "PETQ", "PETS", "PAIC", "PFSW", "PGTI", "PHAS", "PHAT", "PAHC", "PHIO", "PHIOW", "PLAB", "PHUN", "PHUNW", "PICO", "PLL", "PIRS", "PPC", "PDD", "PME", "PNFP", "PNFPP", "PT", "PBFS", "PPSI", "PXLW", "PLYA", "PLXS", "PLRX", "PLUG", "PLBC", "PS", "PSTI", "PSTV", "PLXP", "PCOM", "POLA", "PTE", "PYPD", "POOL", "BPOP", "BPOPM", "BPOPN", "KCAPL", "PTMN", "PSTX", "PBPB", "PCH", "POWL", "POWI", "PBTS", "PWFL", "PPD", "PRAA", "PRAH", "PGEN", "PRPO", "DTIL", "POAI", "PFBC", "PLPC", "PFBI", "PFC", "PINC", "SQFT", "PBIO", "PRVL", "PRGX", "PSMT", "PNRG", "PRMW", "PRIM", "PFG", "BTEC", "PDEV", "GENY", "PSET", "PY", "PLC", "USMC", "PSC", "PSM", "PRNB", "PRTH", "UFO", "PDEX", "IPDN", "PFHD", "PFIE", "PROF", "PROG", "PRGS", "PGNY", "LUNG", "PFPT", "PSAC", "PSACU", "PSACW", "PRPH", "PTAC", "PTACU", "PTACW", "PRQR", "EQRR", "BIB", "TQQQ", "SQQQ", "BIS", "PSEC", "PTGX", "TARA", "PTVCA", "PTVCB", "PTI", "PRTA", "PRVB", "PVBC", "PROV", "PBIP", "PMD", "PTC", "PTCT", "PHCF", "PULM", "PLSE", "PBYI", "PCYO", "PRPL", "PRPLW", "PUYI", "PXS", "QK", "QADA", "QADB", "QCRH", "QGEN", "QIWI", "QRVO", "QCOM", "QLGN", "QLYS", "QTRX", "QMCO", "QRHC", "QH", "QUIK", "QDEL", "QNST", "QUMU", "QTNT", "QRTAV", "QRTBV", "QRTEA", "QRTEB", "QRTEV", "QTT", "RRD", "RCM", "RXT", "RADA", "RDCM", "RDUS", "RDNT", "RDWR", "METC", "RMBS", "RAND", "RNDB", "RPD", "RAPT", "RTLR", "RAVE", "RAVN", "RBB", "ROLL", "RICK", "RCMT", "RDI", "RDIB", "BLCN", "RNWK", "RP", "RETA", "RCON", "REPH", "RRBI", "RRGB", "RRR", "RDVT", "RDFN", "RDHL", "REED", "REG", "REGN", "RGNX", "RGLS", "REKR", "RLAY", "RBNC", "RELV", "RLMD", "MARK", "RNLX", "RNST", "REGI", "RCII", "RPTX", "RPAY", "RGEN", "REPL", "KRMD", "RBCAA", "FRBK", "REFR", "RSSS", "RESN", "RGP", "TORC", "ROIC", "RETO", "RTRX", "RVNC", "RVMD", "RWLK", "REXN", "REYN", "RFIL", "RGCO", "RBKB", "RYTM", "RBBN", "RIBT", "RELL", "RMBI", "RIGL", "RNET", "RMNI", "RIOT", "REDU", "RVSB", "RIVE", "RMRM", "RCKT", "RMTI", "RCKY", "RMCF", "ROKU", "ROST", "ROCH", "ROCHU", "ROCHW", "RGLD", "RPRX", "RBCN", "RUBY", "RUHN", "RMBL", "RUSHA", "RUSHB", "RUTH", "RYAAY", "STBA", "SANW", "SBRA", "SABR", "SFET", "SAFT", "SGA", "SAGE", "SAIA", "SLRX", "SALM", "SAL", "SAFM", "SASR", "SGMO", "SANM", "SNY", "SPNS", "SRPT", "STSA", "SVRA", "SBFG", "SBAC", "SCSC", "SMIT", "SCHN", "SRRK", "SCHL", "SDGR", "SAMA", "SAMAU", "SAMAW", "SJ", "SGMS", "SCPL", "SCPH", "WORX", "SCYX", "SEAC", "SBCF", "STX", "SHIP", "SHIPW", "SHIPZ", "SPNE", "SGEN", "EYES", "EYESW", "SECO", "SCWX", "SNFCA", "SEEL", "SEIC", "SLCT", "SIC", "SELB", "SIGI", "SLS", "LEDS", "SMTC", "SNCA", "SENEA", "SENEB", "SNES", "AIHS", "SRTS", "SQBG", "MCRB", "SVC", "SREV", "SFBS", "SESN", "SVBI", "SGBX", "SGOC", "SMED", "SHSP", "SHEN", "PIXY", "SCCI", "TYHT", "SWAV", "SCVL", "SHBI", "SSTI", "SIBN", "SIEB", "SIEN", "BSRR", "SRRA", "SWIR", "SIFY", "SIGA", "SGLB", "SGLBW", "SGMA", "SBNY", "SLN", "SLGN", "SILC", "SLAB", "SIMO", "SILK", "SSPK", "SSPKU", "SSPKW", "SAMG", "SSNT", "SFNC", "SLP", "SINA", "SBGI", "SINO", "SVA", "SINT", "SPQQ", "SG", "SIRI", "SRVA", "SITM", "EDTK", "SKYS", "SKYW", "SWKS", "SNBR", "SLM", "SLMBP", "SGH", "SND", "SMBK", "SDC", "SWBI", "SMSI", "SMTX", "SCKT", "GIGE", "SAQN", "SAQNU", "SAQNW", "SOHU", "SLRC", "SUNS", "SEDG", "SLNO", "SLGL", "SLDB", "SNGX", "SNGXW", "SOLY", "SONM", "SONN", "SNOA", "SONO", "SRNE", "SOHO", "SOHOB", "SOHON", "SOHOO", "SFBC", "SMMC", "SMMCU", "SMMCW", "SPFI", "SSB", "SFST", "SMBC", "SONA", "SBSI", "SY", "SP", "SGRP", "SPKE", "SPKEP", "SPTN", "DWFI", "SPPI", "SPRO", "ANY", "SPI", "SAVE", "STXB", "SPLK", "SPOK", "SPWH", "SBPH", "SWTX", "FUND", "SPT", "SFM", "SPSC", "SRAX", "SSNC", "SSRM", "STAA", "SRAC", "SRACU", "SRACW", "STAF", "STMP", "STND", "SBLK", "SBLKZ", "SVACU", "SBUX", "STFC", "MITO", "GASS", "STCN", "STLD", "SRCL", "SBT", "STRL", "SHOO", "SFIX", "SYBT", "STOK", "BANX", "STNE", "SNEX", "SSKN", "SSYS", "STRA", "HNDL", "STRT", "STRS", "STRM", "SBBP", "SUMR", "SMMF", "SSBI", "SMMT", "WISA", "SNBP", "SNDE", "SNDL", "SNSS", "STKL", "SPWR", "RUN", "SUNW", "SLGG", "SMCI", "SPCB", "SCON", "SGC", "SUPN", "SPRT", "SURF", "SRGA", "SGRY", "SRDX", "SSSS", "STRO", "SIVB", "SIVBP", "SVMK", "SWKH", "SYKE", "SYNC", "SYNL", "SYNA", "SNCR", "SNDX", "SYNH", "SYBX", "SNPS", "SYPR", "SYRS", "TROW", "TTOO", "TRHC", "TCMD", "TAIT", "TLC", "TTWO", "TLND", "TNDM", "TANH", "TAOP", "TAPM", "TEDU", "TH", "THWWW", "TATT", "TAYD", "TCF", "TCFCP", "CGBD", "TCRR", "AMTD", "GLG", "PETZ", "TCCO", "TTGT", "TGLS", "TECTP", "TELA", "TNAV", "TLGT", "TELL", "TENB", "TENX", "TZAC", "TZACU", "TZACW", "TER", "TBNK", "TSLA", "TESS", "TTEK", "TCBI", "TCBIL", "TCBIP", "TXN", "TXRH", "TFFP", "TFSL", "TGTX", "WTER", "ANDE", "TBBK", "BPRN", "CG", "CAKE", "CHEF", "TCFC", "DSGX", "DXYN", "ENSG", "XONE", "FBMS", "FLIC", "GT", "HCKT", "HAIN", "CUBA", "INTG", "JYNT", "KHC", "OLD", "LOVE", "MIK", "MIDD", "ODP", "OLB", "STKS", "PECK", "PNTG", "PRSC", "REAL", "RMR", "SHYF", "SMPL", "TTD", "YORW", "NCTY", "RACA", "TXMD", "THTX", "TBPH", "THMO", "THBR", "THBRU", "THBRW", "TLRY", "TSBK", "TIPT", "TITN", "TMDI", "TTNP", "TVTY", "TLSA", "TMUS", "TNXP", "TOPS", "TRCH", "TRMD", "TOTA", "TOTAR", "TOTAU", "TOTAW", "TBLT", "TBLTW", "TSEM", "CLUB", "TOWN", "TPIC", "TCON", "TSCO", "TW", "TACT", "TRNS", "TGA", "TBIO", "TMDX", "TA", "TANNI", "TANNL", "TANNZ", "TZOO", "TIG", "TRMT", "TRVN", "TRVI", "TPCO", "TCDA", "TCBK", "TDAC", "TDACU", "TDACW", "TRIL", "TRS", "TRMB", "TRIB", "TCOM", "TRIP", "TSC", "TSCAP", "TSCBP", "TBK", "TBKCP", "TRVG", "TRUE", "TRUP", "TRST", "TRMK", "MEDS", "TSRI", "TTEC", "TTMI", "TC", "TCX", "TOUR", "TPTX", "HEAR", "THCB", "THCBU", "THCBW", "THCA", "THCAU", "THCAW", "TWCT", "TWCTU", "TWIN", "TWST", "TYME", "USCR", "USEG", "GROW", "USAU", "USWS", "USWSW", "UCL", "UFPI", "UFPT", "ULTA", "UCTT", "RARE", "ULBI", "UMBF", "UMPQ", "UNAM", "LATN", "LATNU", "LATNW", "UNB", "QURE", "UAL", "UBCP", "UBOH", "UBSI", "UCBI", "UCBIO", "UFCS", "UIHC", "UNFI", "UBFO", "USLM", "UTHR", "UG", "UNIT", "UNTY", "UBX", "OLED", "UEIC", "ULH", "USAP", "UVSP", "UMRX", "TTTN", "TIGR", "UPLD", "UPWK", "UONE", "UONEK", "URBN", "MYT", "URGN", "UROV", "ECOL", "ECOLW", "USAK", "USIO", "UTMD", "UTSI", "UXIN", "VCNX", "VLY", "VLYPO", "VLYPP", "VTEC", "VALU", "VNDA", "BBH", "ANGL", "BJK", "PPH", "RTH", "SMH", "ESPO", "VWOB", "VNQI", "VCIT", "VGIT", "VIGI", "VYMI", "VCLT", "VGLT", "VMBS", "VONE", "VONG", "VONV", "VTWO", "VTWG", "VTWV", "VTHR", "VCSH", "VTIP", "VGSH", "BND", "VTC", "BNDX", "VXUS", "BNDW", "VREX", "VRNS", "VBLT", "VSTA", "VXRT", "PCVX", "VBIV", "VECO", "VERO", "VEON", "VRA", "VCYT", "VSTM", "VERB", "VERBW", "VCEL", "VERY", "VRME", "VRMEW", "VRNT", "VRSN", "VRSK", "VBTX", "VERI", "VRNA", "VRRM", "VRCA", "VTNR", "VRTX", "VERX", "VERU", "VIAC", "VIACA", "VSAT", "VIAV", "VICR", "VCTR", "CIZ", "VSDA", "CEY", "CEZ", "CID", "CIL", "QQQN", "CFO", "CFA", "CSF", "CDC", "CDL", "VSMV", "CSB", "CSA", "VIE", "VMD", "VRAY", "VKTX", "VKTXW", "VBFC", "VFF", "VLGEA", "VIOT", "VNOM", "VIR", "VIRC", "VTSI", "VIRT", "VRTS", "BBC", "BBP", "VRTU", "VISL", "VTGN", "VMAC", "VMACU", "VMACW", "VC", "VITL", "VIVE", "VVPR", "VOD", "VG", "VJET", "VOXX", "VYGR", "VRM", "VSEC", "VTVT", "VUZI", "VYNE", "WAFU", "HLAL", "WTRH", "WBA", "WSG", "WMG", "WAFD", "WASH", "WSBF", "WTRE", "WTREP", "WVE", "WNFM", "WSTG", "WDFC", "WB", "WEN", "WERN", "WSBC", "WSBCP", "WTBA", "WABC", "WSTL", "WINC", "WBND", "WDC", "WNEB", "WPRT", "WWR", "WEYS", "WHLR", "WHLRD", "WHLRP", "WHF", "WHFBZ", "FREE", "FREEW", "WHLM", "WVVI", "WVVIP", "WLDN", "WLFC", "WLTW", "WSC", "WIMI", "WINT", "WING", "WINA", "WINS", "WTFC", "WTFCM", "WTFCP", "CXSE", "WCLD", "EMCB", "DGRE", "DXGE", "HYZD", "AGZD", "WETF", "DXJS", "DGRW", "DGRS", "WKEY", "WIX", "WWD", "WDAY", "WKHS", "WRLD", "WRTC", "WMGI", "WSFS", "WVFC", "WW", "WYNN", "XFOR", "XBIT", "XELB", "XEL", "XNCR", "XBIO", "XBIOW", "XENE", "XERS", "XLNX", "XOMA", "XP", "XPEL", "XPER", "XSPA", "XTLB", "XNET", "YNDX", "YTRA", "YTEN", "YIN", "YMAB", "YGYI", "YGYIP", "YRCW", "CTIB", "ZGYH", "ZGYHR", "ZGYHU", "ZGYHW", "YJ", "ZAGG", "ZLAB", "ZEAL", "ZBRA", "ZNTL", "ZCMD", "Z", "ZG", "ZION", "ZIONL", "ZIONN", "ZIONO", "ZIONP", "ZIOP", "ZIXI", "ZKIN", "ZGNX", "ZM", "ZI", "ZSAN", "ZVO", "ZS", "ZUMZ", "ZYNE", "ZYXI", "ZNGA", "JWN", "ADS", "HOG", "MMM", "ABT", "ABBV", "ACN", "AAP", "AES", "AFL", "A", "APD", "ALK", "ALB", "ARE", "ALLE", "ALL", "MO", "AMCR", "AEE", "AEP", "AXP", "AIG", "AMT", "AWK", "AMP", "ABC", "AME", "APH", "ANTM", "AON", "AOS", "AIV", "APTV", "ADM", "ANET", "AJG", "AIZ", "T", "ATO", "AZO", "AVB", "AVY", "BKR", "BLL", "BAC", "BK", "BAX", "BDX", "BRK.B", "BBY", "BIO", "BLK", "BA", "BWA", "BXP", "BSX", "BMY", "BR", "BF.B", "COG", "CPB", "COF", "CAH", "KMX", "CCL", "CARR", "CAT", "CBRE", "CE", "CNC", "CNP", "CTL", "CF", "SCHW", "CVX", "CMG", "CB", "CHD", "CI", "C", "CFG", "CLX", "CMS", "KO", "CL", "CMA", "CAG", "CXO", "COP", "ED", "STZ", "COO", "GLW", "CTVA", "COTY", "CCI", "CMI", "CVS", "DHI", "DHR", "DRI", "DVA", "DE", "DAL", "DVN", "DLR", "DFS", "DG", "D", "DPZ", "DOV", "DOW", "DTE", "DUK", "DRE", "DD", "DXC", "EMN", "ETN", "ECL", "EIX", "EW", "EMR", "ETR", "EOG", "EFX", "EQR", "ESS", "EL", "EVRG", "ES", "RE", "EXR", "XOM", "FRT", "FDX", "FIS", "FE", "FRC", "FLT", "FLS", "FMC", "F", "FTV", "FBHS", "BEN", "FCX", "GPS", "IT", "GD", "GE", "GIS", "GM", "GPC", "GL", "GPN", "GS", "GWW", "HRB", "HAL", "HBI", "HIG", "HCA", "PEAK", "HSY", "HES", "HPE", "HLT", "HFC", "HD", "HON", "HRL", "HST", "HWM", "HPQ", "HUM", "HII", "IEX", "ITW", "IR", "ICE", "IBM", "IP", "IPG", "IFF", "IVZ", "IQV", "IRM", "J", "SJM", "JNJ", "JCI", "JPM", "JNPR", "KSU", "K", "KEY", "KEYS", "KMB", "KIM", "KMI", "KSS", "KR", "LB", "LHX", "LH", "LW", "LVS", "LEG", "LDOS", "LEN", "LLY", "LNC", "LIN", "LYV", "LMT", "L", "LOW", "LYB", "MTB", "MRO", "MPC", "MMC", "MLM", "MAS", "MA", "MKC", "MCD", "MCK", "MDT", "MRK", "MET", "MTD", "MGM", "MAA", "MHK", "TAP", "MCO", "MS", "MOS", "MSI", "MSCI", "NOV", "NEM", "NEE", "NLSN", "NKE", "NI", "NSC", "NOC", "NRG", "NUE", "NVR", "OXY", "OMC", "OKE", "ORCL", "OTIS", "PKG", "PH", "PAYC", "PNR", "PKI", "PRGO", "PFE", "PM", "PSX", "PNW", "PXD", "PNC", "PPG", "PPL", "PG", "PGR", "PLD", "PRU", "PEG", "PSA", "PHM", "PVH", "PWR", "DGX", "RL", "RJF", "RTX", "O", "RF", "RSG", "RMD", "RHI", "ROK", "ROL", "ROP", "RCL", "SPGI", "CRM", "SLB", "SEE", "SRE", "NOW", "SHW", "SPG", "SLG", "SNA", "SO", "LUV", "SWK", "STT", "STE", "SYK", "SYF", "SYY", "TPR", "TGT", "TEL", "FTI", "TDY", "TFX", "TXT", "TMO", "TIF", "TJX", "TT", "TDG", "TRV", "TFC", "TWTR", "TYL", "TSN", "UDR", "USB", "UAA", "UA", "UNP", "UNH", "UPS", "URI", "UHS", "UNM", "VFC", "VLO", "VAR", "VTR", "VZ", "V", "VNO", "VMC", "WRB", "WAB", "WMT", "DIS", "WM", "WAT", "WEC", "WFC", "WELL", "WST", "WU", "WRK", "WY", "WHR", "WMB", "XRX", "XYL", "YUM", "ZBH", "ZTS" };

        #endregion

        private static string Name = "Program";

        public static bool ScannerReady = true;
        public static int GettingData = 0;

        public static uint year = 2020;
        public static uint month = 1;
        public static uint day = 1;
        public static string bartype = "min";
        public static int barsize = 15;

        public static string stepSize;

        public static List<string> TimeList = new List<string>();

        public static List<HistoricalRequest> Requests = new List<HistoricalRequest>();

        static async Task Main(string[] args)
        {
            Logger.SetLogLevel(Logger.LogLevel.LogLevelInfo);
            Logger.Verbose(Name, "Start");

            Connect();

            Thread.Sleep(100);
            await reqParameters();
            await calculateStepSizes();

            //get Symbol List

            SymbolObjects = await CreateSymbolObjects(SymbolList);

            await RequestSymbolContracts();

            await GetData();

            //Thread.Sleep(10);

            //Logger.Info(Name, "Started");
            while (true) Console.ReadKey();
        }

        public static async Task reqParameters()
        {
            year = Convert.ToUInt16(Ask("starting year: "));
            month = Convert.ToUInt16(Ask("starting month: "));
            day = Convert.ToUInt16(Ask("starting day: "));
            Console.WriteLine("1 secs  5 secs  10 secs 15 secs 30 secs \n"
                               + "1 min   2 mins  3 mins  5 mins  10 mins 15 mins 20 mins 30 mins\n"
                               + "1 hour  2 hours 3 hours 4 hours 8 hours\n"
                               + "1 day\n"
                               + "1 week\n"
                               + "1 month\n");
            bartype = Ask("bar type: ");
            barsize = Convert.ToUInt16(Ask("Bar size: "));
        }

        public static async Task calculateStepSizes()
        {
            DateTime Time = Convert.ToDateTime(year + "-" + month + "-" + day + " 00:00:00");
            switch (bartype)
            {
                case "secs":
                    switch (barsize)
                    {
                        case 1:
                            stepSize = "1800 S";
                            while(Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddSeconds(1800);
                            }
                            break;
                        case 5:
                            stepSize = "3600 S";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddSeconds(3600);
                            }
                            break;
                        case 10:
                            stepSize = "14400 S";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddSeconds(14400);
                            }
                            break;
                        case 30:
                            stepSize = "28800 S";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddSeconds(28800);
                            }
                            break;
                        default:
                            stepSize = "1800 S";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddSeconds(1800);
                            }
                            break;
                    }
                    break;

                case "min":
                    stepSize = "1 D";
                    while (Time <= DateTime.Now.AddDays(-1))
                    {
                        string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                        TimeList.Add(queryTime);
                        Time = Time.AddDays(1);
                    }
                    break;

                case "mins":
                    switch (barsize)
                    {
                        case 2:
                            stepSize = "2 D";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddDays(2);
                            }
                            break;
                        case 3:
                            stepSize = "1 W";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddDays(7);
                            }
                            break;
                        case 15:
                            stepSize = "1 W";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddDays(7);
                            }
                            break;
                        case 30:
                            stepSize = "1 M";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddMonths(1);
                            }
                            break;
                        default:
                            stepSize = "1 D";
                            while (Time <= DateTime.Now.AddDays(-1))
                            {
                                string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                                TimeList.Add(queryTime);
                                Time = Time.AddDays(1);
                            }
                            break;
                    }
                    break;

                default:
                    stepSize = "1 Y";
                    while (Time <= DateTime.Now.AddDays(-1))
                    {
                        string queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
                        TimeList.Add(queryTime);
                        Time = Time.AddYears(1);
                    }
                    break;
            }
            
        }

        public static string Ask(string question) 
        {
            Console.WriteLine(question);
            string answer = Console.ReadLine();
            return answer;
        }

        static async Task Connect()
        {
            Logger.Info(Name, "Connecting To API");

            IbClient.ClientSocket.eConnect(Ip, Port, ApiId);
            IbReader = new EReader(IbClient.ClientSocket, IbClient.Signal);
            IbReader.Start();

            new Thread(() =>
            {
                while (IbClient.ClientSocket.IsConnected())
                {
                    IbClient.Signal.waitForSignal();
                    IbReader.processMsgs();
                }
            })
            { IsBackground = true }.Start();
        }

        static async Task<List<Symbol>> CreateSymbolObjects(List<string> symbolList)
        {
            Logger.Info(Name, "Creating Symbol Objects");

            List<Symbol> Result = new List<Symbol>();
            for (int i = 0; i < symbolList.Count; i++)
            {
                Symbol s = new Symbol(symbolList[i], i);
                Result.Add(s);
            }
            return Result;
        }

        static async Task RequestSymbolContracts()
        {
            Logger.Info(Name, "Creating Symbol Contracts");

            foreach (Symbol S in SymbolObjects)
            {
                Contract Contract = await CreateContract(S.Ticker);
                S.Contract = Contract;
                IbClient.ClientSocket.reqContractDetails(S.Id, Contract);
                Thread.Sleep(20);
                //GetMarketData(Contract, i);
            }
        }

        static async Task CheckRequests()
        {
            
            foreach(HistoricalRequest H in Requests)
            {
                if(H.DateTime < DateTime.Now.AddMinutes(-5))
                {
                    Logger.Error(Name, "Order Cancelled");
                    IbClient.ClientSocket.cancelHistoricalData(H.Id);
                    //Requests.Remove(H);
                }
            }

        }

        static async Task GetData()
        {
            //DateTime Time = Convert.ToDateTime(year+"-"+month+"-"+day+" 00:00:00");
            //String queryTime = Time.ToString("yyyyMMdd HH:mm:ss");
            bool CheckerEnabled = false;

            foreach(string queryTime in TimeList)
            {
                Console.WriteLine(queryTime);
                foreach (Symbol S in SymbolObjects)
                {
                    try
                    { 
                        if (GettingData < 49 && S.Enabled == true)
                        {
                            while (GettingData >= 48)
                            {
                                Thread.Sleep(1);
                                if (S == SymbolObjects.Last()) break;
                                CheckRequests();
                            };
                            S.Ids.Insert(0, S.Ids[0] + 10000);
                            IbClient.ClientSocket.reqHistoricalData(S.Ids[0], S.Contract, queryTime, stepSize, $"{barsize} {bartype}", "MIDPOINT", 1, 1, false, null);
                            Requests.Add(new HistoricalRequest(S.Ids[0]));
                            GettingData += 1;
                            Thread.Sleep(25);
                            CheckRequests();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(Name, $"{S.Ticker} Failed: \n{ex}");
                    }
                }
            }
            foreach (Symbol S in SymbolObjects)
            {
                await Csv.WriteToCsv(S.Ticker, S.RawDataList);
            }
            Logger.Info(Name, "Historical Data Done");
        }

        public static async Task<Contract> CreateContract(string symbol, string secType = "STK", string exchange = "SMART", string currency = "USD")
        {
            Logger.Verbose(Name, "Creating Contract");
            Contract Contract = new Contract();
            Contract.Symbol = symbol;
            Contract.SecType = secType;
            Contract.Exchange = exchange;
            Contract.PrimaryExch = "ISLAND";
            Contract.Currency = currency;

            return Contract;
        }
    }
}
