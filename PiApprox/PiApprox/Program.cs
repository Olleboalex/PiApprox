﻿using Fractions;
using System.Numerics;
using System.Diagnostics;
using ClosedXML.Excel;

namespace PiApprox
{
    internal class Program
    {
        public static Fraction Approx(int n)
        {
            Fraction result = new Fraction(1, 1);
            for (int i = 1; i <= n; i++)
            {
                result += i % 2 == 0 ? new Fraction(1, 2 * i + 1) : new Fraction(-1, 2 * i + 1);
            }
            return new Fraction(4, 1) * result;
        }
        public static Fraction LeibnizApprox(int k)
        {
            int i = k;
            if(i <= 0)
            {
                return (4, 1);
            }
            return i % 2 == 0 ? new Fraction(4, 2 * i + 1) : new Fraction(-4, 2 * i + 1);
        }
        public static Fraction BetterApprox(int n)
        {
            Fraction sqrt12 = (BigInteger.Parse("2361884621859093944424922583935647542436774565268000651212692535795840310603318657942311035831699375859782443869472701468345963893510551577942858717646305960908338050131552256583071900247836267846089971404454566365817515288559790438512594780498882596927166880522373508088684203563180215446887894295934440879096770627402486850806652075366125287038565343886661389071308861952705717106035709402238044209645303094553866793781270755330115697899221958373888893626146711615861323391324707819971073837627846255498558710993962380962928954736309126424782515719869712443608110852912638598783696897"), BigInteger.Parse("681817361112592673006216284302327640982818960193896391676415341431013215947810422286674152535599303079185437256348762881281144978639443606787360473878512400762880499660029189130128408645080193820361591594354362268513003312119270972578772998936601221192160258252982633326911166778277165834790354497150588134500199602023067944823326457969055009801951122594582450881493104787783918874127868907960441150895809062948519124646242770969886395906378491635916654574399788068277255992428794911534311885852123904734915610352395983520458265547670242724232318346588365946902690870909831509060656128"));
            if (n <= 0) return sqrt12;
            return n % 2 == 0 ? (1, (1 + 2 * n) * BigInteger.Pow(3, n)) * sqrt12 : (-1, (1 + 2 * n) * BigInteger.Pow(3, n)) * sqrt12;
        }
        public static Fraction ThreadedApprox(Func<int, Fraction> f, int n)
        {
            const int ThreadCount = 4;
            int N = n - n % ThreadCount;
            Fraction[] results = new Fraction[ThreadCount];
            Parallel.For(0, ThreadCount, i =>
            {
                int Start = 1 + (N / ThreadCount) * i;
                int Stop = N / ThreadCount * (i + 1);
                Fraction temp = (0, 1);
                for(int j = Start; j <= Stop; j++)
                {
                    temp += f(j);
                }
                results[i] = temp;
            });
            Fraction result = f(0);
            if(n > N)
            {
                for (int i = N + 1; i <= n; i++)
                {
                    result += f(i);
                }
            }
            foreach (var term in results)
            {
                result += term;
            }
            return result;
        }
        public static int CompareToVal(Fraction a, string comparisonValue)
        {
            BigInteger remainder = a.top % a.bot;
            if (a.top/a.bot != 3)
            {
                return 0;
            }
            int result = 1;
            while (remainder != 0)
            {
                if(result >= comparisonValue.Length)
                {
                    Console.WriteLine("The fraction is too good!");
                    return result;
                }
                remainder *= 10;
                if (remainder /a.bot != comparisonValue[result] - '0')
                {
                    return result;
                }
                remainder %= a.bot;
                result++;
            }
            return result;
        }

        public static Fraction Rootapproximation(Fraction a, Fraction b, int precision, string comparedroot)
        {
            
            while (CompareToVal(b, comparedroot) < precision)
            {
                b = (b + a / b) / (2, 1);
            }
            return b;
        }
        static void Main(string[] args)
        {
            /*Fraction res = Rootapproximation((12, 1), (3, 1), 1000, "3464101615137754587054892683011744733885610507620761256111613958903866033817600074162292373514497151351252282830813406059939890189997904957623311024188747297056186463804611164135949640202169349846530030624686533806645773301344509337843675942454094263320735723176038099973074759718778935300695013152101513236696259212201895204374380650166291659047919665995579649016577428927665834694448327969175710795335916127636707332221686347561788756632204176611049803340047041422288577391981912731594174336996145798986592968566041572817207977477395075164634635662791985966015677405754107826739126624207414528038498213536462398576751282282844033485504204745988541662119796918951975328577795592295675916780457709705807152067705616128763944689322119379445745730528307645329396840042390968310556882362573069014070383300033378588830961692142554287999525853669259154876723790220254297277493953091964903577101950758027761329923823924445914221110485847446384395476525123262937684065707433365877299223834099477672790991876291515343706747266251821799310849249669574395210471995538384647140440610605680771830829942144859118413412405019040351926371745532719950567326862160301331707421294657077251844521164441020807360540595009597456158923316200834105363880038191466924351887787340498640845382068739624927440222370522168537820599440622404200127014352749164810407695110394559867595229812215789971088446652008037702607263122289773694563178576326490374530133290769755198325153285744222481684136033527034200205886361431030381923284921814078816258433807034985922728008279340862082507264654061845154655920584753195491074190938231484280848461563984655234803812849024909755033725392210667388432427210789208491308280257066015627266899713472813407954684459623922085851069003202811880959094309069096814543475312524733098333280466012026531488140215673716936904626320935508961000804481279823940724437205840477734301422034338800593737519327000081790632428466850455913668134026940371804056721433524295486986899127191616164260885172938937045221816526706017513225206921344390808111968257825951989620001544114880460095346517600183028742978950889758314382589318167141747923031075595280524136741696092145938765439171379519519252208318305315155581564669961135680045803064104277870747551073132854093653748579926882791487332147488891166172955786425970604296394790682956341033229905103526583987399131489045278225038187082773978733634861876452849473852404145981935662308263892968755830919831847856575429390298548052818427291308083289162980403891498838610538005227945292162130142879206412155021188375596569587972390499928330426279430587198843794833294150374471577258932217120340857739211596788105812814861623666777355763125271734312016793520490698459887877734119508630885886861914516941976430926222521533548135729143156121294894999500709089118626573098379698673145495252594829476471373829675662726722567255807649680327613343214359697457111685862698452186481131915107302273509288775668566626933289108360781643797966589252690032342244033859238920338641242");
            Console.WriteLine($"{res.top} / {res.bot}");
            res.Simplify();
            Console.WriteLine($"{res.top} / {res.bot}"); */
            using(var workbook = new XLWorkbook())
            {
                var workSheet = workbook.Worksheets.Add("DATA");
                workSheet.Cell("A1").Value = "n";
                workSheet.Cell("B1").Value = "Decimals";
                for (int i = 1; i <= 500; i++)
                {
                    if(i % 10 == 0) 
                    {
                        Console.WriteLine(i.ToString());
                    }
                    workSheet.Cell("A" + (i + 1).ToString()).Value = i;
                    workSheet.Cell("B" + (i + 1).ToString()).Value = CompareToVal(ThreadedApprox(BetterApprox, i), "3141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141273724587006606315588174881520920962829254091715364367892590360011330530548820466521384146951941511609433057270365759591953092186117381932611793105118548074462379962749567351885752724891227938183011949129833673362440656643086021394946395224737190702179860943702770539217176293176752384674818467669405132000568127145263560827785771342757789609173637178721468440901224953430146549585371050792279689258923542019956112129021960864034418159813629774771309960518707211349999998372978049951059731732816096318595024459455346908302642522308253344685035261931188171010003137838752886587533208381420617177669147303598253490428755468731159562863882353787593751957781857780532171226806613001927876611195909216420199");
                }
                workbook.SaveAs("resultsBetter.xlsx");
            }
        }
    }
}