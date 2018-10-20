using System;
using System.Collections.Generic;
using System.Linq;

namespace WebServiceCaller
{
    public class ExchangeHolidayByYearWebServiceGateway
    {
        private readonly Random _random;

        public ExchangeHolidayByYearWebServiceGateway()
        {
            _random = new Random();
        }

        public DateTime[] GetHolidays(
            string exchangeIsoCode,
            int startYear,
            int endYear)
        {
            if (ExchangeCodeIsValid(exchangeIsoCode))
                return new DateTime[0];

            var holidays = new List<DateTime>();

            for (int yearCounter = startYear; yearCounter <= endYear; yearCounter++)
            {
                var holidaysForYear = CreateRandomNonWeekendDaysInYear(yearCounter);

                holidays.AddRange(holidaysForYear);
            }

            return holidays.ToArray();
        }

        private bool ExchangeCodeIsValid(string exchangeIsoCode)
        {
            var validExchangeIsoCodes = GetValidExchangeIsoCodes();

            return validExchangeIsoCodes.All(c => c != exchangeIsoCode);
        }

        private DateTime[] CreateRandomNonWeekendDaysInYear(
            int yearCounter)
        {
            var holidayCount = _random.Next(5, 7);

            var holidays = new DateTime[holidayCount];

            for (int holidayCounter = 0; holidayCounter < holidayCount; holidayCounter++)
            {
                holidays[holidayCounter] = GetRandomNonWeekendDateInYear(yearCounter);
            }

            return holidays;
        }

        private DateTime GetRandomNonWeekendDateInYear(
            int year)
        {
            var nonWeekendDate = CreateRandomDayInYear(year);

            while (IsWeekend(nonWeekendDate))
            {
                nonWeekendDate = CreateRandomDayInYear(year);
            }

            return nonWeekendDate;
        }

        private DateTime CreateRandomDayInYear(
            int year)
        {
            var dayOffset = GetRandomDayCountInYear();

            return new DateTime(year, 1, 1)
                .AddDays(dayOffset);
        }

        private int GetRandomDayCountInYear()
        {
            return _random.Next(0, 364);
        }

        private static bool IsWeekend(
            DateTime d)
        {
            return d.DayOfWeek == DayOfWeek.Saturday
                   || d.DayOfWeek == DayOfWeek.Sunday;
        }

        private string[] GetValidExchangeIsoCodes()
        {
            return new string[]
            {
                "GLPX",
                "ADVT",
                "AQUA",
                "ARCD",
                "PIPE",
                "AATS",
                "CORE",
                "XAQS",
                "ATDF",
                "BAMX",
                "BAML",
                "MLCO",
                "MLVX",
                "BARX",
                "BCDX",
                "BARD",
                "BARL",
                "BHSF",
                "BGCD",
                "BGCF",
                "BIDS",
                "BTNL",
                "BPOL",
                "BBSF",
                "BLTD",
                "VABD",
                "BVUS",
                "XBOX",
                "XBRT",
                "C2OX",
                "XCFF",
                "CAVE",
                "BATY",
                "BYXD",
                "BATO",
                "BATS",
                "BZXD",
                "EDGA",
                "EDGD",
                "EDGO",
                "EDGX",
                "EDDP",
                "XCBF",
                "XCBO",
                "CBSX",
                "XCBT",
                "FCBT",
                "XCME",
                "FCME",
                "XCHI",
                "CIOI",
                "CDED",
                "CICX",
                "LQFI",
                "CBLC",
                "CGMI",
                "CMSF",
                "GLBX",
                "CBTS",
                "CMES",
                "CECS",
                "NYMS",
                "PDQX",
                "XCEC",
                "BNYC",
                "CRED",
                "CAES",
                "CSLP",
                "IMCG",
                "IMCC",
                "CURX",
                "XCUR",
                "SHAW",
                "SHAD",
                "TSBX",
                "DEAL",
                "DBSX",
                "DWSF",
                "SSTX",
                "EGMT",
                "XELX",
                "ERIS",
                "GLPS",
                "FAST",
                "FNCS",
                "NFSA",
                "XSTM",
                "NFSD",
                "FICO",
                "XFDA",
                "XFCI",
                "FINR",
                "XADF",
                "FINO",
                "FINN",
                "FINC",
                "FINY",
                "FSEF",
                "FXAL",
                "FXCM",
                "G1XX",
                "GLLC",
                "GSEF",
                "GLMX",
                "GOTC",
                "XGMX",
                "SGMA",
                "GSCO",
                "GOVX",
                "GTSX",
                "GTXS",
                "HSFX",
                "HRTX",
                "HRTF",
                "BTEC",
                "ICSU",
                "IFUS",
                "IFED",
                "IMAG",
                "IMBD",
                "IMCR",
                "IMEN",
                "IMFX",
                "IMIR",
                "ICES",
                "IMCS",
                "XIOM",
                "XINS",
                "BLKX",
                "ICBX",
                "INCA",
                "ICRO",
                "INCR",
                "IATS",
                "IEPA",
                "XIMM",
                "XISX",
                "XISA",
                "XISE",
                "IEXG",
                "IEXD",
                "ISDA",
                "GMNI",
                "MCRY",
                "ICEL",
                "ITGI",
                "JSES",
                "JSJX",
                "JEFX",
                "JPBX",
                "JPMX",
                "JLQD",
                "XKBT",
                "ACKF",
                "GTCO",
                "KNIG",
                "KNCM",
                "KNEM",
                "KNLI",
                "KNMX",
                "LASF",
                "LEDG",
                "LEVL",
                "LQED",
                "LIUS",
                "LIFI",
                "LIUH",
                "LTAA",
                "LMNX",
                "MAGM",
                "MTXA",
                "MTXX",
                "MTXM",
                "MTXC",
                "MTXS",
                "XMER",
                "MIHI",
                "XMIO",
                "EMLD",
                "MPRL",
                "NYFX",
                "XMGE",
                "MOCX",
                "MSCO",
                "MSPL",
                "MSRP",
                "MSTX",
                "MTSB",
                "MTUS",
                "HEGX",
                "XNAS",
                "XNCM",
                "XNIM",
                "MELO",
                "XBOS",
                "BOSD",
                "XBXO",
                "ESPD",
                "XPBT",
                "XPHL",
                "XPSX",
                "XNDQ",
                "XNGS",
                "XNMS",
                "NFSC",
                "XCIS",
                "XNYM",
                "XNYL",
                "XNYE",
                "NYPC",
                "XNYS",
                "NBLX",
                "NODX",
                "NMRA",
                "NASD",
                "NXUS",
                "ALDP",
                "AMXO",
                "ARCX",
                "ARCO",
                "NYSD",
                "XNLI",
                "XASE",
                "XOCH",
                "OPRA",
                "PSGM",
                "OTCM",
                "PINC",
                "PINL",
                "PINX",
                "PINI",
                "XOTC",
                "OLLC",
                "OTCB",
                "OTCQ",
                "OOTC",
                "XPHO",
                "XPOR",
                "HPPO",
                "PRSE",
                "RICX",
                "RICD",
                "SCXO",
                "SCXM",
                "SCXA",
                "SCXF",
                "SCXS",
                "SGMT",
                "SUNT",
                "XSEF",
                "TERA",
                "TFSU",
                "GREE",
                "THEM",
                "THRE",
                "TPSE",
                "TRCK",
                "BNDD",
                "TRWB",
                "TSAD",
                "TSEF",
                "TRUX",
                "TRU1",
                "TRU2",
                "TMID",
                "TWSF",
                "SOHO",
                "UBSA",
                "UBSP",
                "UBSS",
                "VERT",
                "VIRT",
                "VFCM",
                "VTEX",
                "WSAG",
                "WEED",
                "XWEE",
                "WELS"
            };
        }
    }
}