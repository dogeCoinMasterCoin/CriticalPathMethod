using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Odbc;
using Xceed.Wpf.Toolkit;

namespace WebApplication1.Pages.SearchPage
{
    [Authorize]
    public class SearchPageModel : PageModel
    {
        [BindProperty]
        public int productID { get; set; }
        [BindProperty]
        public int finalizationTime { get; set; }
        [BindProperty]
        public int Counter { get; set; }

        public List<int> idFazaProiectListGlobal = new List<int>();
        public List<int> durataStandardFazaListGlobal = new List<int>();
        public List<int> durataMinimaFazaListGlobal = new List<int>();
        public List<int> durataFazaListGlobal = new List<int>();
        public List<int> costStandardFazaListGlobal = new List<int>();
        public List<int> scurtarePosibilaListGLobal = new List<int>();
        public List<int> rataCrestereCostFazaListGlobal = new List<int>();
        public List<int> costFazaListGlobal = new List<int>();
        public List<int> npFazaProiectCmdListGlobal = new List<int>();
        public List<int> npFazaProiectCmtListGlobal = new List<int>();
        public List<int> marjaFazaListGlobal = new List<int>();
        public List<string> dcParalelCmdListGlobal = new List<string>();
        public List<int> rataCrestereCostDcCmdListGlobal = new List<int>();
        public List<int> durataProiectListGlobal = new List<int>();
        public List<int> projectCostListGlobal = new List<int>();
        public List<string> um_costGlobal = new List<string>();
        public List<string> um_durataGlobal = new List<string>();
        public List<int> scurtarePosibilaProiect = new List<int>();


        public List<int> durataFazaTimpPreaMareGlobal = new List<int>();
        public List<int> scurtarePosibilaFazaTimpPreaMareGLobal = new List<int>();
        public List<int> costFazaTimpPreaMareGlobal = new List<int>();
        public List<int> durataProiectListTimpPreaMareGlobal = new List<int>();
        public List<int> projectCostListTimpPreaMareGlobal = new List<int>();
        public List<int> scurtarePosibilaProiectTimpPreaMare = new List<int>();

        public int userInputTimeLimit = 0;
        public int userInputProdusID = 0;
        public int count = 0;
        public int sumOfTotalDuration = 0;
        public int totalCostProject = 0;
        public int stopOptimization = 0;

        public int posibleOptimizationCount;
        public IActionResult OnPost()
        {
            userInputTimeLimit = Convert.ToInt32(storeUserInputProductID().Item2);
            userInputProdusID = Convert.ToInt32(storeUserInputProductID().Item1);

            Counter++;

            string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";

            // Creați un obiect de tip Database
            Database database = new Database(connectionString);

            // Apelați metoda GetProjectInformations pentru a obține informațiile despre proiect
            Project project = database.GetProjectInformations(userInputProdusID, userInputTimeLimit, Counter);
            count = project.ProjectActivitiesId.Count;
            posibleOptimizationCount = database.rezultat.Count;           

            if (project.projectDuration.Count > 0)
            {
                sumOfTotalDuration = project.projectDuration[1];
                totalCostProject = project.ProjectCost[1];
            }

            if(Counter > posibleOptimizationCount)
            {
                Counter = 0;
            }           

            for (int i = 0;i < count; i++)
            {
                idFazaProiectListGlobal.Add(project.ProjectActivitiesId[i]);
                durataStandardFazaListGlobal.Add(project.StandardTime[i]);
                durataMinimaFazaListGlobal.Add(project.MinimTime[i]);
                durataFazaListGlobal.Add(project.CurrentDuration[i]);
                scurtarePosibilaListGLobal.Add(project.PosibleTimeBonus[i]);
                costStandardFazaListGlobal.Add(project.NormalCost[i]);
                rataCrestereCostFazaListGlobal.Add(project.PriceModifier[i]);
                costFazaListGlobal.Add(project.StepCost[i]);
                npFazaProiectCmdListGlobal.Add(project.PriorityForProjectCMD[i]);
                npFazaProiectCmtListGlobal.Add(project.PriorityForProjectCMT[i]);
                marjaFazaListGlobal.Add(project.marginStep[i]);
                dcParalelCmdListGlobal.Add(project.DC_paralel_CMD[i]);
                rataCrestereCostDcCmdListGlobal.Add(project.PriceModifierRateCMD[i]);
                durataProiectListGlobal.Add(project.projectDuration[i]);
                projectCostListGlobal.Add(project.ProjectCost[i]);
                um_costGlobal.Add(project.UM_cost[i]);
                um_durataGlobal.Add(project.UM_durata[i]);
                scurtarePosibilaProiect.Add(project.projectDuration[i] - userInputTimeLimit);

                //timp prea mare
                durataFazaTimpPreaMareGlobal.Add(project.Durata_Faza_Timp_Crescut[i]);
                scurtarePosibilaFazaTimpPreaMareGLobal.Add(project.Scurtare_posibila_faza_timp_crescut[i]);
                costFazaTimpPreaMareGlobal.Add(project.Cost_faza_timp_crescut[i]);
                durataProiectListTimpPreaMareGlobal.Add(project.Durata_proiect_timp_crescut[i]);
                projectCostListTimpPreaMareGlobal.Add(project.Cost_proiect_timp_crescut[i]);
                scurtarePosibilaProiectTimpPreaMare.Add(project.Durata_proiect_timp_crescut[i] - userInputTimeLimit);
            }

            if (durataProiectListGlobal.Count == 0)
            {
                stopOptimization = 2;
                
            }
            else if (userInputTimeLimit == durataProiectListGlobal[0])
            {
                stopOptimization = 1;
            }
            else if (userInputTimeLimit < durataProiectListGlobal[0])
            {
                //do nothing
            }
            

            if(userInputTimeLimit >= sumOfTotalDuration)
            {
                Counter = 0;
            }

            return Page();
        }

        public class Project
        {
            public int Id { get; set; }
            public int posibleResult { get; set; }
            public List<List<int>> Dependecies { get; set; }
            //CRITICAL
            public List<int> ParalelActivities { get; set; }
            //NON CRITICAL
            public List<int> NonParalelActivities { get; set; }
            public List<int> StandardTime { get; set; }
            public List<int> ProjectActivitiesId { get; set; }
            public List<int> MinimTime { get; set; }
            public List<int> CurrentDuration { get; set; }
            public List<int> PosibleTimeBonus { get; set; }
            public List<int> NormalCost { get; set; }
            public List<int> PriceModifier { get; set; }
            public List<int> StepCost { get; set; }
            public List<int> PriorityForProjectCMD { get; set; }
            public List<int> PriorityForProjectCMT { get; set; }
            public List<int> marginStep { get; set; }
            public List<int> NumberCriticalPath { get; set; }
            public List<int> PriceModifierRateCMD { get; set; }
            public List<int> PriceModifierRateCMT { get; set; }
            public List<int> projectDuration { get; set; }
            public List<int> DC_paralel_CMT { get; set; }
            public List<string> DC_paralel_CMD { get; set; }
            public List<int> ProjectCost { get; set; }
            public List<int> NeceasaryShortage { get; set; }
            public List<string> Nr_drum_critic_paralel { get; set; }
            public List<int> Scurtate_posibila { get; set; }
            public List<string> UM_durata { get; set; }
            public List<string> UM_cost { get; set; }

            //Time too big
            public List<int> Durata_Faza_Timp_Crescut { get; set; }
            public List<int> Scurtare_posibila_faza_timp_crescut { get; set; }            
            public List<int> Rata_crestere_cost_faza_timp_crescut { get; set; }
            public List<int> Cost_faza_timp_crescut { get; set; }
            public List<int> Durata_proiect_timp_crescut { get; set; }
            public List<int> Cost_proiect_timp_crescut { get; set; }
        }

        public class Database
        {
            public string connectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=D:\\Projects\\BD_Valasciuc_Bogdan_06.04.23.accdb;";

            //first query
            public List<int> idActivitateProiectList = new List<int>();
            public List<int> idActivitatePredecesoareList = new List<int>();
            public List<int> durataStandardActivitateList = new List<int>();
            public List<int> durataMinimaActivitateList = new List<int>();
            public List<int> costStandardActivitateList = new List<int>();
            public List<int> costMaximActivitateList = new List<int>();

            public List<List<int>> isParalelIndexDictionary = new List<List<int>>();
            public List<int> workDaysCorespondenceToEveryIndexFromCycleStandardList = new List<int>();
            public List<int> workDaysCorespondenceToEveryIndexFromCycleMinimList = new List<int>();
            public List<int> costCorespondenceToEveryIndexFromCycleStandardList = new List<int>();
            public List<int> costCorespondenceToEveryIndexFromCycleMinimList = new List<int>();
            public List<int> isParalelLineVect = new List<int>();
            public List<int> bestSolution = new List<int>();
            public List<int> daysForEachActivityFromActivityCycleList = new List<int>();
            public List<int> durataPlanificataProiectList = new List<int>();

            public List<List<int>> rezultat = new List<List<int>>();
            public List<int> currentLine = new List<int>();
            public List<int> counterActivitiesCyclePerProject = new List<int>();
            public List<List<int>> callBackActivities = new List<List<int>>();
            public List<List<int>> durataMinimaFormatListaDeListe = new List<List<int>>();
            public List<int> stabilesteDurataPlanificareProiect = new List<int>();
            public List<List<int>> rezultatActualListOfList = new List<List<int>>();
            public List<int> currentDurationPerActivity = new List<int>();
            public List<int> minimumDurationPerActivity = new List<int>();
            public List<int> minCostPerProject = new List<int>();
            public List<int> maxCostPerProject = new List<int>();
            public List<List<int>> durataMinimaCicluCompletListOfList = new List<List<int>>();
            public List<List<int>> costStandardCicluCompletListOfList = new List<List<int>>();
            public List<List<int>> costMaximCicluCompletListOfList = new List<List<int>>();
            public List<int> costFazaCurenta = new List<int>();
            public List<int> aparitiiInFazelePoriectului = new List<int>();
            public List<int> nivelPrioritateFazaProiectList = new List<int>();
            public List<int> rataCrestereDrumCritic = new List<int>();
            public List<int> scurtarePosibilaProiect =  new List<int>();


            //Display to user lists
            public List<string> um_durata_toUser = new List<string>();
            public List<int> cost_planificat_activitate_toUser = new List<int>();
            public List<int> durata_planificata_activitate_toUser = new List<int>();
            public List<string> um_cost_toUser = new List<string>();

            //second query
            public List<int> idProiectList = new List<int>();
            public List<int> planificareIdActivitateProiectList = new List<int>();
            public List<int> durataPlanificataActivitateList = new List<int>();
            public List<int> costPlanificatActivitateList = new List<int>();

            //Auxiliar lists
            List<int> cicluCompletActivitatiList = new List<int>();

            public Database(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public Project GetProjectInformations(int projectId, int finalizationTime, int btnCounter) 
            {
                Project project = new Project
                {
                    Id = projectId,
                    posibleResult = new int(),
                    Dependecies = new List<List<int>>(),
                    ParalelActivities = new List<int>(),
                    NonParalelActivities = new List<int>(),
                    ProjectActivitiesId = new List<int>(),
                    StandardTime = new List<int>(),
                    MinimTime = new List<int>(),
                    CurrentDuration = new List<int>(),
                    PosibleTimeBonus = new List<int>(),
                    NormalCost = new List<int>(),
                    PriceModifier = new List<int>(),
                    StepCost = new List<int>(),
                    PriorityForProjectCMD = new List<int>(),
                    PriorityForProjectCMT = new List<int>(),
                    marginStep = new List<int>(),
                    NumberCriticalPath = new List<int>(),
                    PriceModifierRateCMD = new List<int>(),
                    PriceModifierRateCMT = new List<int>(),
                    projectDuration = new List<int>(),
                    DC_paralel_CMT = new List<int>(),
                    DC_paralel_CMD = new List<string>(),
                    ProjectCost = new List<int>(),
                    NeceasaryShortage = new List<int>(),
                    Nr_drum_critic_paralel = new List<string>(),
                    Scurtate_posibila = new List<int>(),
                    UM_cost = new List<string>(),
                    UM_durata = new List<string>(),
                    //time too big

                    Durata_Faza_Timp_Crescut = new List<int>(),
                    Scurtare_posibila_faza_timp_crescut = new List<int>(),
                    Rata_crestere_cost_faza_timp_crescut = new List<int>(),
                    Cost_faza_timp_crescut = new List<int>(),
                    Durata_proiect_timp_crescut = new List<int>(),
                    Cost_proiect_timp_crescut = new List<int>(),
                };

                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();

                    //Extract data for dependecies
                    string dependeciesQuery = "SELECT ID_activitate_proiect, ID_activitati_predecesoare, Durata_standard_activitate, Durata_minima_activitate, Cost_standard_activitate, Cost_maxim_activitate FROM Activitati_Proiect WHERE ID_produs =" + projectId;

                    OdbcCommand command = new OdbcCommand(dependeciesQuery, connection);

                    OdbcDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int idActivitateProiect = reader.GetInt32(0);
                        int idActivitatiPredecesoare = reader.GetInt32(1);
                        int durataStandardActivitate = reader.GetInt32(2);
                        int durataMinimaActivitate = reader.GetInt32(3);
                        int costStandardActivitate = reader.GetInt32(4);
                        int costMaximActivitate = reader.GetInt32(5);

                        idActivitateProiectList.Add(idActivitateProiect);
                        idActivitatePredecesoareList.Add(idActivitatiPredecesoare);
                        durataStandardActivitateList.Add(durataStandardActivitate);
                        durataMinimaActivitateList.Add(durataMinimaActivitate);
                        costStandardActivitateList.Add(costStandardActivitate);
                        costMaximActivitateList.Add(costMaximActivitate);

                    }

                    //Extract informations about cost and time for activities
                    string timeCostQueryForActivities = "SELECT ID_proiect, ID_activitate_proiect, Durata_planificata_activitate, Cost_planificat_activitate, Durata_planificata_proiect FROM Planificare_Proiect WHERE ID_produs =" + projectId;

                    OdbcCommand command1 = new OdbcCommand(timeCostQueryForActivities, connection);

                    OdbcDataReader reader1 = command1.ExecuteReader();

                    while (reader1.Read())
                    {
                        int idProiect = reader1.GetInt32(0);
                        int idActivitateProiect = reader1.GetInt32(1);
                        int durataPlanificataActivitate = reader1.GetInt32(2);
                        int costPlanificatActivitate = reader1.GetInt32(3);
                        int durataPlanificataProiect = reader1.GetInt32(4);


                        idProiectList.Add(idProiect);
                        planificareIdActivitateProiectList.Add(idActivitateProiect);
                        durataPlanificataActivitateList.Add(durataPlanificataActivitate);
                        costPlanificatActivitateList.Add(costPlanificatActivitate);
                        durataPlanificataProiectList.Add(durataPlanificataProiect);
                    }

                    for (int i = 0; i < idActivitateProiectList.Count; i++)
                    {
                        List<int> secventaActivitati = new List<int>();

                        int activitateCurenta = idActivitateProiectList[i];
                        secventaActivitati.Add(activitateCurenta);

                        int predecesor = idActivitatePredecesoareList[i];
                        while (predecesor != 0)
                        {
                            secventaActivitati.Insert(0, predecesor);
                            predecesor = idActivitatePredecesoareList[predecesor - 1];
                        }
                        for (int j = 0; j < secventaActivitati.Count; j++)
                        {
                            cicluCompletActivitatiList.Add(secventaActivitati[j]);
                        }
                        project.Dependecies.Add(secventaActivitati);
                        callBackActivities.Add(secventaActivitati);
                    }

                    // Construirea grafului de activități
                    Dictionary<int, List<int>> graf = new Dictionary<int, List<int>>();
                    for (int i = 0; i < idActivitateProiectList.Count; i++)
                    {
                        int activitate = idActivitateProiectList[i];
                        int predecesor = idActivitatePredecesoareList[i];

                        if (!graf.ContainsKey(activitate))
                            graf[activitate] = new List<int>();

                        graf[activitate].Add(predecesor);
                    }

                    // Determinarea activităților care pot fi desfășurate în paralel
                    List<int> activitatiParalele = new List<int>();
                    List<int> activitatiNeparalele = new List<int>();

                    // Verificarea fiecărei activități
                    foreach (int activitate in idActivitateProiectList)
                    {
                        bool poateFiParalela = true;

                        // Verificarea dacă există activități predecesoare care nu au fost finalizate
                        if (graf.ContainsKey(activitate))
                        {
                            foreach (int predecesor in graf[activitate])
                            {
                                if (activitatiParalele.Contains(predecesor))
                                {
                                    poateFiParalela = false;
                                    break;
                                }
                            }
                        }

                        // Adăugarea activității în lista corespunzătoare
                        if (poateFiParalela)
                        {
                            activitatiParalele.Add(activitate);
                        }
                        else
                        {
                            activitatiNeparalele.Add(activitate);
                        }

                    }

                    List<int> changeAux = new List<int>(activitatiParalele);

                    activitatiParalele = activitatiNeparalele;
                    activitatiNeparalele = changeAux;

                    for (int i = 0; i < activitatiParalele.Count; i++)
                    {
                        project.NonParalelActivities.Add(activitatiParalele[i]);
                    }
                    for (int i = 0; i < activitatiNeparalele.Count; i++)
                    {
                        project.ParalelActivities.Add(activitatiNeparalele[i]);
                    }

                    List<List<int>> listTempParalel = new List<List<int>>(callBackActivities);

                    // Initializează un dicționar de adiacență
                    Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();

                    List<List<int>> doarDependeteleListList = new List<List<int>>();

                    foreach (List<int> row in listTempParalel)
                    {
                        List<int> modifiedRow = row.GetRange(0, row.Count - 1);
                        doarDependeteleListList.Add(modifiedRow);
                    }                   

                    for (int i = 0; i < idActivitateProiectList.Count; i++)
                    {
                        int counter = 0;
                        for (int j = 0; j < cicluCompletActivitatiList.Count; j++)
                        {
                            if (idActivitateProiectList[i] == cicluCompletActivitatiList[j])
                            {
                                counter++;
                            }
                        }
                        aparitiiInFazelePoriectului.Add(counter);
                    }

                    nivelPrioritateFazaProiectList = AssignPriority(aparitiiInFazelePoriectului);

                    List<List<int>> nivelPrioritateEgal = new List<List<int>>();

                    nivelPrioritateEgal = FindActionsWithSamePriority(idActivitateProiectList, nivelPrioritateFazaProiectList);
                    
                    //Reda id_activitate cu aceleasi nivel prioritar
                    List<List<int>> FindActionsWithSamePriority(List<int> actions, List<int> priorities)
                    {
                        Dictionary<int, List<int>> actionsByPriority = new Dictionary<int, List<int>>();

                        // Gruparea acțiunilor în funcție de nivelul de prioritate
                        for (int i = 0; i < actions.Count; i++)
                        {
                            int priority = priorities[i];

                            if (!actionsByPriority.ContainsKey(priority))
                            {
                                actionsByPriority[priority] = new List<int>();
                            }

                            actionsByPriority[priority].Add(actions[i]);
                        }

                        // Filtrarea acțiunilor cu același nivel de prioritate
                        List<List<int>> actionsWithSamePriority = actionsByPriority.Values
                            .Where(list => list.Count > 1)
                            .ToList();

                        return actionsWithSamePriority;
                    }


                    //Corespondenta zile din ciclu lista totala cu valorile standard
                    for (int i = 0; i < cicluCompletActivitatiList.Count; i++)
                    {
                        for (int j = 0; j < idActivitateProiectList.Count; j++)
                        {
                            if (cicluCompletActivitatiList[i] == j + 1)
                            {
                                workDaysCorespondenceToEveryIndexFromCycleStandardList.Add(durataStandardActivitateList[j]);
                            }
                        }
                    }

                    //Corespondenta zile din ciclu lista totala cu valorile minime
                    for (int i = 0; i < cicluCompletActivitatiList.Count; i++)
                    {
                        for (int j = 0; j < idActivitateProiectList.Count; j++)
                        {
                            if (cicluCompletActivitatiList[i] == j + 1)
                            {
                                workDaysCorespondenceToEveryIndexFromCycleMinimList.Add(durataMinimaActivitateList[j]);
                            }
                        }
                    }

                    //Corespondenta costului din ciclu lista totala cu valorile standard
                    for (int i = 0; i < cicluCompletActivitatiList.Count; i++)
                    {
                        for (int j = 0; j < idActivitateProiectList.Count; j++)
                        {
                            if (cicluCompletActivitatiList[i] == j + 1)
                            {
                                costCorespondenceToEveryIndexFromCycleStandardList.Add(costStandardActivitateList[j]);
                            }
                        }
                    }

                    //Corespondenta costului din ciclu lista totala cu valorile maxim
                    for (int i = 0; i < cicluCompletActivitatiList.Count; i++)
                    {
                        for (int j = 0; j < idActivitateProiectList.Count; j++)
                        {
                            if (cicluCompletActivitatiList[i] == j + 1)
                            {
                                costCorespondenceToEveryIndexFromCycleMinimList.Add(costMaximActivitateList[j]);
                            }
                        }
                    }

                    //numar caractere unice in lista prioritate
                    int CountUniqueNumbers(List<int> numbers)
                    {
                        HashSet<int> uniqueNumbers = new HashSet<int>(numbers);
                        return uniqueNumbers.Count;
                    }

                    List<List<int>> GetDistinctLines(List<List<int>> listaDeListe)
                    {
                        List<List<int>> liniileUnice = new List<List<int>>();

                        foreach (List<int> linie in listaDeListe)
                        {
                            if (!liniileUnice.Any(l => l.SequenceEqual(linie)))
                            {
                                liniileUnice.Add(linie);
                            }
                        }
                        return liniileUnice;
                    }

                    //calcul cmd
                    List<int> earliestTimesStartBefore = CalculateEarliestTimesBefore(doarDependeteleListList, durataStandardActivitateList);
                    //calcul cmt
                    List<int> earliestTimesFinishBefore = new List<int>();
                    //calcul cmd - tarziu
                    List<int> latestTimesStartBefore = new List<int>();
                    //calcul cmt -tarziu 
                    List<int> latestTimesFinishBefore = new List<int>();
                    //cpm before 
                    List<string> drumCriticListBefore = new List<string>();

                    static List<int> CalculateEarliestTimesBefore(List<List<int>> dependencies, List<int> durations)
                    {
                        int numActivities = durations.Count;
                        List<int> earliestTimes = new List<int>();

                        // Initializează timpul cel mai devreme pentru fiecare activitate cu 0
                        for (int i = 0; i < numActivities; i++)
                        {
                            earliestTimes.Add(0);
                        }

                        // Calculează timpul cel mai devreme pentru fiecare activitate în ordine topologică
                        for (int i = 0; i < numActivities; i++)
                        {
                            int earliestTime = 0;

                            // Parcurge activitățile care depind de activitatea curentă și găsește timpul cel mai mare
                            foreach (int dependentActivity in dependencies[i])
                            {
                                int dependentTime = earliestTimes[dependentActivity - 1] + durations[dependentActivity - 1];
                                earliestTime = Math.Max(earliestTime, dependentTime);
                            }

                            if (i == numActivities - 1)
                            {
                                earliestTime = Math.Max(earliestTimes[i - 2] + durations[i - 2], earliestTimes[i - 1] + durations[i - 1]);
                            }

                            earliestTimes[i] = earliestTime;
                        }

                        return earliestTimes;
                    }

                    for (int i = 0; i < earliestTimesStartBefore.Count; i++)
                    {
                        earliestTimesFinishBefore.Add(earliestTimesStartBefore[i] + durataStandardActivitateList[i]);
                    }

                    //pentru ultima activitate calculam datele de inceput pentru cmt
                    latestTimesFinishBefore.Add(earliestTimesFinishBefore[earliestTimesFinishBefore.Count - 1]);

                    if (idActivitateProiectList.Count % 2 == 0)
                    {
                        for (int i = idActivitateProiectList.Count - 2; i >= 1; i--)
                        {
                            if (doarDependeteleListList[i + 1].Count > 1)
                            {
                                latestTimesFinishBefore.Add(earliestTimesStartBefore[i + 1]);
                                if (i > 1)
                                {
                                    latestTimesFinishBefore.Add(earliestTimesStartBefore[i + 1]);
                                }
                            }
                            else
                            {
                                latestTimesFinishBefore.Add(Math.Min(latestTimesFinishBefore[i] - durataStandardActivitateList[i], latestTimesFinishBefore[i + 1] - durataStandardActivitateList[i + 1]));
                            }
                        }
                    }
                    else
                    {
                        for (int i = idActivitateProiectList.Count - 1; i >= 1; i--)
                        {
                            if (doarDependeteleListList[i].Count > 1)
                            {
                                latestTimesFinishBefore.Add(earliestTimesStartBefore[i]);
                            }
                            else
                            {
                                latestTimesFinishBefore.Add(Math.Min(latestTimesFinishBefore[i - 1] - durataStandardActivitateList[i - 1], latestTimesFinishBefore[i] - durataStandardActivitateList[i]));
                            }
                        }
                    }

                    latestTimesFinishBefore.Reverse();

                    for (int i = 0; i < latestTimesFinishBefore.Count; i++)
                    {
                        latestTimesStartBefore.Add(latestTimesFinishBefore[i] - durataStandardActivitateList[i]);
                    }

                    for (int i = 0; i < earliestTimesFinishBefore.Count; i++)
                    {
                        if (earliestTimesFinishBefore[i] - latestTimesFinishBefore[i] == 0)
                        {
                            drumCriticListBefore.Add("DA");
                        }
                        else
                        {
                            drumCriticListBefore.Add("NU");
                        }
                    }


                    List<string> MapareActivitati(List<int> listaActivitati, List<string> listaCorespondenta, List<int> listaIndex)
                    {
                        List<string> listaRezultat = new List<string>();
                        foreach (int index in listaIndex)
                        {
                            int activitate = listaActivitati[index - 1]; // indexele din listaIndex încep de la 1, deci trebuie scăzut 1
                            string elementCorespondent = listaCorespondenta[activitate - 1]; // activitățile încep de la 1, deci trebuie scăzut 1
                            listaRezultat.Add(elementCorespondent);
                        }
                        return listaRezultat;
                    }

                    List<string> activitateCriticaDinCicluActivitati = new List<string>();

                    activitateCriticaDinCicluActivitati = MapareActivitati(idActivitateProiectList, drumCriticListBefore, cicluCompletActivitatiList);


                    int caractereUniceInListaDePrioritati = CountUniqueNumbers(nivelPrioritateFazaProiectList);

                    List<List<int>> liniiUnice = new List<List<int>>();

                    int indexRezultatLista = 0;

                    List<int> startList = new List<int>(workDaysCorespondenceToEveryIndexFromCycleStandardList);

                    rezultat.Add(startList);
                  

                    for (int i = 0; i < caractereUniceInListaDePrioritati - nivelPrioritateEgal.Count; i++)
                    {
                        List<int> tempListRezultate = new List<int>(rezultat[i]);

                        for (int index = 0; index < cicluCompletActivitatiList.Count; index++)
                        {                           
                            if (cicluCompletActivitatiList[index] == nivelPrioritateFazaProiectList[nivelPrioritateFazaProiectList.IndexOf(i + 1)]  && activitateCriticaDinCicluActivitati[index] != "NU")
                            {
                                tempListRezultate[index] = workDaysCorespondenceToEveryIndexFromCycleMinimList[index];
                            }

                        }
                        rezultat.Add(tempListRezultate);
                    }

                    List<int> tmp = new List<int>(workDaysCorespondenceToEveryIndexFromCycleStandardList);

                    for (int j = 0; j < cicluCompletActivitatiList.Count; j++)
                    {

                        for (int i = 0; i < activitatiNeparalele.Count; i++)
                        {
                            if (activitatiNeparalele[i] == cicluCompletActivitatiList[j])
                            {
                                tmp[j] = workDaysCorespondenceToEveryIndexFromCycleMinimList[j];
                            }
                        }
                    }

                    rezultat.Add(tmp);

                    for (int i = 0; i < nivelPrioritateEgal.Count; i++)
                    {
                        List<int> tempListRezultate = new List<int>(rezultat[rezultat.Count - 2]);

                        for (int index = 0; index < cicluCompletActivitatiList.Count; index++)
                        {
                            for (int j = 0; j < nivelPrioritateEgal.Count; j++)
                            {
                                for (int k = 0; k < nivelPrioritateEgal[j].Count; k++)
                                {
                                    if (cicluCompletActivitatiList[index] == nivelPrioritateEgal[j][k])
                                    {
                                        tempListRezultate[index] = workDaysCorespondenceToEveryIndexFromCycleMinimList[index];
                                    }
                                }
                            }
                        }
                        rezultat.Add(tempListRezultate);
                    }

                    rezultat = GetDistinctLines(rezultat);


                    //Calculte current CMD, CMT, CRITICAL PATH & other data to be displayed to user
                    if (btnCounter >= 0 && btnCounter <= rezultat.Count)
                    {

                        List<int> currentDurataionActivityForResultList = new List<int>();
                        int currentMaximActivities = 0;
                        int currentMaximTimeDuration = 0;

                        for(int i = 0; i < rezultat[btnCounter - 1].Count; i++)
                        {
                            if (cicluCompletActivitatiList[i] > currentMaximActivities)
                            {
                                currentMaximActivities = cicluCompletActivitatiList[i];
                                currentMaximTimeDuration = rezultat[btnCounter - 1][i];
                                currentDurataionActivityForResultList.Add(currentMaximTimeDuration);
                            }

                        }

                        static int SumaElemente(List<int> lista)
                        {
                            int suma = 0;
                            foreach (int numar in lista)
                            {
                                suma += numar;
                            }
                            return suma;
                        }

                        //calcul cmd
                        List<int> earliestTimesStart = CalculateEarliestTimes(doarDependeteleListList, currentDurataionActivityForResultList, currentDurataionActivityForResultList);
                        //calcul cmt
                        List<int> earliestTimesFinish = new List<int>();
                        //calcul cmd - tarziu
                        List<int> latestTimesStart = new List<int>();
                        //calcul cmt -tarziu 
                        List<int> latestTimesFinish = new List<int>();

                        //durata proiect timp crescut
                        List<int> earliestTimeFinishTooMuchTime = new List<int>();
                        List<int> earliestTimeStartTooMuchTime = CalculateEarliestTimes(doarDependeteleListList, durataStandardActivitateList, durataStandardActivitateList);

                        for (int i = 0; i < earliestTimeStartTooMuchTime.Count; i++)
                        {
                            earliestTimeFinishTooMuchTime.Add(earliestTimeStartTooMuchTime[i] + durataStandardActivitateList[i]);
                        }

                        int durataTimpProiectTimpCrescut = earliestTimeFinishTooMuchTime[earliestTimeFinishTooMuchTime.Count - 1];

                        static List<int> CalculateEarliestTimes(List<List<int>> dependencies, List<int> durations, List<int> currentDurations)
                        {
                            int numActivities = durations.Count;
                            List<int> earliestTimes = new List<int>();

                            // Initializează timpul cel mai devreme pentru fiecare activitate cu 0
                            for (int i = 0; i < numActivities; i++)
                            {
                                earliestTimes.Add(0);
                            }

                            // Calculează timpul cel mai devreme pentru fiecare activitate în ordine topologică
                            for (int i = 0; i < numActivities; i++)
                            {
                                int earliestTime = 0;

                                // Parcurge activitățile care depind de activitatea curentă și găsește timpul cel mai mare
                                foreach (int dependentActivity in dependencies[i])
                                {    
                                     int dependentTime = earliestTimes[dependentActivity - 1] + durations[dependentActivity - 1];
                                     earliestTime = Math.Max(earliestTime, dependentTime);
                                }

                                if(i == numActivities - 1)
                                {
                                    earliestTime = Math.Max(earliestTimes[i - 2] + durations[i - 2], earliestTimes[i - 1] + durations[i - 1]);
                                }

                                earliestTimes[i] = earliestTime;
                            }

                            return earliestTimes;
                        }

                        for (int i = 0; i < earliestTimesStart.Count; i++)
                        {
                            earliestTimesFinish.Add(earliestTimesStart[i] + currentDurataionActivityForResultList[i]);
                        }

                        //Durata proiect
                        int projectDurationTime = earliestTimesFinish[earliestTimesFinish.Count - 1];

                        //pentru ultima activitate calculam datele de inceput pentru cmt
                        latestTimesFinish.Add(projectDurationTime);                     

                        if(idActivitateProiectList.Count % 2 == 0)
                        {
                            for (int i = idActivitateProiectList.Count - 2; i >= 1; i--)
                            {
                                if (doarDependeteleListList[i + 1].Count > 1)
                                {
                                    latestTimesFinish.Add(earliestTimesStart[i + 1]);
                                    if (i > 1)
                                    {
                                        latestTimesFinish.Add(earliestTimesStart[i + 1]);
                                    }
                                }
                                else
                                {
                                    latestTimesFinish.Add(Math.Min(latestTimesFinish[i] - currentDurataionActivityForResultList[i], latestTimesFinish[i + 1] - currentDurataionActivityForResultList[i + 1]));
                                }
                            }
                        }
                        else
                        {
                            for (int i = idActivitateProiectList.Count - 1; i >= 1; i--)
                            {
                                if (doarDependeteleListList[i].Count > 1)
                                {
                                    latestTimesFinish.Add(earliestTimesStart[i]);
                                }
                                else
                                {
                                    latestTimesFinish.Add(Math.Min(latestTimesFinish[i - 1] - currentDurataionActivityForResultList[i - 1], latestTimesFinish[i] - currentDurataionActivityForResultList[i]));
                                }
                            }
                        }
                        
                        latestTimesFinish.Reverse();

                        for(int i = 0; i< latestTimesFinish.Count; i++)
                        {
                            latestTimesStart.Add(latestTimesFinish[i] - currentDurataionActivityForResultList[i]);
                        }

                        List<string> drumCriticList = new List<string>();

                        for (int i = 0; i < earliestTimesFinish.Count; i++)
                        {
                            if (earliestTimesFinish[i] - latestTimesFinish[i] == 0)
                            {
                                drumCriticList.Add("DA");
                            }
                            else
                            {
                                drumCriticList.Add("NU");
                            }
                        }

                        //final cmd, cmt , drum critic per activitate curenta

                        for (int i = 0; i < project.Dependecies.Count; i++)
                        {
                            int sum = 0;
                            List<int> tempCalculateCounterEveryActivitiesCycle = new List<int>(project.Dependecies[i]);
                            for (int j = 0; j < tempCalculateCounterEveryActivitiesCycle.Count; j++)
                            {
                                sum += 1;
                            }
                            counterActivitiesCyclePerProject.Add(sum);
                        }

                        //Creaza o lista de liste cu durata minima pentru fiecare activitate din ciclu activitati, de acelasi format ca si project.dependecy
                        int numarElementeTotal = workDaysCorespondenceToEveryIndexFromCycleMinimList.Count; // Numărul total de elemente din lista normală
                        int index = 0;
                        for (int i = 0; i < counterActivitiesCyclePerProject.Count; i++)
                        {
                            List<int> linie = new List<int>();

                            for (int j = 0; j < counterActivitiesCyclePerProject[i]; j++)
                            {
                                if (index < numarElementeTotal)
                                {
                                    linie.Add(workDaysCorespondenceToEveryIndexFromCycleMinimList[index]);
                                    index++;
                                }
                            }
                            durataMinimaFormatListaDeListe.Add(linie);
                        }

                        // transforma lista in lista de liste
                        int indexRezultat = 0;
                        for (int i = 0; i < counterActivitiesCyclePerProject.Count; i++)
                        {
                            List<int> linie = new List<int>();

                            for (int j = 0; j < counterActivitiesCyclePerProject[i]; j++)
                            {
                                if (indexRezultat < numarElementeTotal)
                                {
                                    linie.Add(rezultat[btnCounter - 1][indexRezultat]);
                                    indexRezultat++;
                                }
                            }
                            rezultatActualListOfList.Add(linie);
                        }
                        List<string> tipDurataActivitateCurenta = new List<string>();
                        List<string> tipCostActivitateCurenta = new List<string>();

                        for(int i  = 0; i< currentDurataionActivityForResultList.Count; i++)
                        {
                            if (currentDurataionActivityForResultList[i] == durataMinimaActivitateList[i])
                            {
                                tipDurataActivitateCurenta.Add("Durata_minima_activitate");
                                tipCostActivitateCurenta.Add("Cost_maxim_activitate");
                            }
                            else
                            {
                                tipDurataActivitateCurenta.Add("Durata_standard_activitate");
                                tipCostActivitateCurenta.Add("Cost_standard_activitate");
                            }
                        }

                        for (int j = 0; j < idActivitateProiectList.Count; j++)
                        {
                            int id_activitate_proiect = j + 1;
                            string queryDisplayUserData = "SELECT UM_durata, " + tipDurataActivitateCurenta[j] + ", " + tipCostActivitateCurenta[j] + ",UM_cost FROM Activitati_proiect WHERE ID_produs = " + projectId +
                                                          " AND ID_activitate_proiect = " + id_activitate_proiect;


                            OdbcCommand command2 = new OdbcCommand(queryDisplayUserData, connection);

                            OdbcDataReader reader2 = command2.ExecuteReader();

                            while (reader2.Read())
                            {
                                string um_durata = reader2.GetString(0);
                                int durata_planificat_activitate = reader2.GetInt32(1);
                                int cost_planificat_activitate = reader2.GetInt32(2);
                                string um_cost = reader2.GetString(3);


                                um_durata_toUser.Add(um_durata);
                                durata_planificata_activitate_toUser.Add(durata_planificat_activitate);
                                cost_planificat_activitate_toUser.Add(cost_planificat_activitate);
                                um_cost_toUser.Add(um_cost);
                            }
                        }

                        int numarElementeTotalMinim = workDaysCorespondenceToEveryIndexFromCycleMinimList.Count; // Numărul total de elemente din lista normală

                        // transforma linia in lista de liste durataMinima
                        int indexDurataMinima = 0;
                        for (int i = 0; i < counterActivitiesCyclePerProject.Count; i++)
                        {
                            List<int> linie = new List<int>();

                            for (int j = 0; j < counterActivitiesCyclePerProject[i]; j++)
                            {
                                if (indexDurataMinima < numarElementeTotalMinim)
                                {
                                    linie.Add(workDaysCorespondenceToEveryIndexFromCycleMinimList[indexDurataMinima]);
                                    indexDurataMinima++;
                                }
                            }
                            durataMinimaCicluCompletListOfList.Add(linie);
                        }

                        // transforma linia in lista de liste cost standard
                        int indexCostStandard = 0;
                        for (int i = 0; i < counterActivitiesCyclePerProject.Count; i++)
                        {
                            List<int> linie = new List<int>();

                            for (int j = 0; j < counterActivitiesCyclePerProject[i]; j++)
                            {
                                if (indexCostStandard < numarElementeTotalMinim)
                                {
                                    linie.Add(costCorespondenceToEveryIndexFromCycleStandardList[indexCostStandard]);
                                    indexCostStandard++;
                                }
                            }
                            costStandardCicluCompletListOfList.Add(linie);
                        }

                        // transforma linia in lista de liste cost maxim
                        int indexCostMaxim = 0;
                        for (int i = 0; i < counterActivitiesCyclePerProject.Count; i++)
                        {
                            List<int> linie = new List<int>();

                            for (int j = 0; j < counterActivitiesCyclePerProject[i]; j++)
                            {
                                if (indexCostMaxim < numarElementeTotalMinim)
                                {
                                    linie.Add(costCorespondenceToEveryIndexFromCycleMinimList[indexCostMaxim]);
                                    indexCostMaxim++;
                                }
                            }
                            costMaximCicluCompletListOfList.Add(linie);
                        }

                        //Costul cel mai mare posibil
                        for (int i = 0; i < costMaximCicluCompletListOfList.Count; i++)
                        {
                            int costMaxim = 0;
                            for (int j = 0; j < costMaximCicluCompletListOfList[i].Count; j++)
                            {
                                costMaxim += costMaximCicluCompletListOfList[i][j];
                            }
                            maxCostPerProject.Add(costMaxim);
                        }

                        //Costul standard
                        for (int i = 0; i < costStandardCicluCompletListOfList.Count; i++)
                        {
                            int costMinim = 0;
                            for (int j = 0; j < costStandardCicluCompletListOfList[i].Count; j++)
                            {
                                costMinim += costStandardCicluCompletListOfList[i][j];
                            }
                            minCostPerProject.Add(costMinim);
                        }

                        for (int i = 0; i < stabilesteDurataPlanificareProiect.Count; i++)
                        {
                            if (stabilesteDurataPlanificareProiect[i] == 1)
                            {
                                costFazaCurenta.Add(costStandardActivitateList[i]);
                            }
                            else
                            {
                                costFazaCurenta.Add(costMaximActivitateList[i]);
                            }
                        }
                       

                        for (int i = 0; i < idActivitateProiectList.Count; i++)
                        {
                            if (drumCriticList[i] == "DA")
                            {
                                rataCrestereDrumCritic.Add((costMaximActivitateList[i] - costStandardActivitateList[i]) / (durataStandardActivitateList[i] - durataMinimaActivitateList[i]));
                            }
                            else
                            {
                                rataCrestereDrumCritic.Add(0);
                            }
                        }

                        List<int> list1 = new List<int>();
                        List<int> list2 = new List<int>();

                        for (int i = 0; i < durataPlanificataProiectList.Count / 2; i++)
                        {
                            list1.Add(durataPlanificataProiectList[i]);
                        }

                        for (int j = (durataPlanificataProiectList.Count) / 2; j < durataPlanificataProiectList.Count; j++)
                        {
                            list2.Add(durataPlanificataProiectList[j]);
                        }


                        for (int i = 0; i < counterActivitiesCyclePerProject.Count; i++)
                        {
                            List<int> line = new List<int>();
                            for (int j = 0; j < counterActivitiesCyclePerProject[i]; j++)
                            {
                                line.Add(rezultatActualListOfList[i][j]);
                            }
                            line.Reverse();
                            currentDurationPerActivity.Add(line[0]);
                        }

                        int cost_planificat_proeict_toUser = 0;

                        for(int i = 0; i< cost_planificat_activitate_toUser.Count; i++)
                        {
                            cost_planificat_proeict_toUser += cost_planificat_activitate_toUser[i];
                        }

                        int counterRezultat = rezultat.Count;

                        int cost_planificat_proiect_too_much = 0;

                        for( int i=0; i< costStandardActivitateList.Count; i++)
                        {
                            cost_planificat_proiect_too_much += costStandardActivitateList[i];
                        }

                        for (int i = 0; i < idActivitateProiectList.Count; i++)
                        {
                            //ID_faza_proiect
                            project.ProjectActivitiesId.Add(idActivitateProiectList[i]);
                            //Durata_standard_faza
                            project.StandardTime.Add(durataStandardActivitateList[i]);
                            //Durata_minima_faza
                            project.MinimTime.Add(durataMinimaActivitateList[i]);
                            //Durata_faza_curenta
                            project.CurrentDuration.Add(currentDurataionActivityForResultList[i]);
                            //Scurtare_posibila
                            project.PosibleTimeBonus.Add(currentDurataionActivityForResultList[i] - durataMinimaActivitateList[i]);
                            //Cost_standard
                            project.NormalCost.Add(costStandardActivitateList[i]);
                            //Rata_crestere_cost_faza
                            project.PriceModifier.Add((costMaximActivitateList[i] - costStandardActivitateList[i]) / (durataStandardActivitateList[i] - durataMinimaActivitateList[i]));
                            //Cost_faza
                            project.StepCost.Add(cost_planificat_activitate_toUser[i]);
                            //Nivel prioritate CMD
                            project.PriorityForProjectCMD.Add(nivelPrioritateFazaProiectList[i]);
                            //Nivel prioritate CMT
                            project.PriorityForProjectCMT.Add(nivelPrioritateFazaProiectList[i]);
                            //Marja faza
                            project.marginStep.Add(latestTimesFinish[i] - earliestTimesFinish[i]);
                            //Drum critic proiect
                            project.DC_paralel_CMD.Add(drumCriticList[i]);
                            //Rata crestere cost drum critic
                            project.PriceModifierRateCMD.Add(rataCrestereDrumCritic[i]);
                            //Durata actuala proiect
                            project.projectDuration.Add(projectDurationTime);
                            ////Cost actual proiect
                            project.ProjectCost.Add(cost_planificat_proeict_toUser);
                            //um_durata
                            project.UM_durata.Add(um_durata_toUser[i]);
                            //um_cost
                            project.UM_cost.Add(um_cost_toUser[i]);


                            //Timp prea mare
                            //Durata activitate
                            project.Durata_Faza_Timp_Crescut.Add(durataStandardActivitateList[i]);
                            //scurtare posibila
                            project.Scurtare_posibila_faza_timp_crescut.Add(durataStandardActivitateList[i] - durataMinimaActivitateList[i]);
                            //cost faza
                            project.Cost_faza_timp_crescut.Add(costStandardActivitateList[i]);
                            //durata proiect
                            project.Durata_proiect_timp_crescut.Add(durataTimpProiectTimpCrescut);
                            //project cost
                            project.Cost_proiect_timp_crescut.Add(cost_planificat_proiect_too_much);
                        }
                    }
                    
                    return project;
                }
            }
        }
        
        static List<int> AssignPriority(List<int> numbers)
        {
            List<int> sortedNumbers = numbers.OrderByDescending(x => x).ToList();
            Dictionary<int, int> priorityDict = new Dictionary<int, int>();

            int priority = 1;
            for (int i = 0; i < sortedNumbers.Count; i++)
            {
                if (!priorityDict.ContainsKey(sortedNumbers[i]))
                {
                    priorityDict[sortedNumbers[i]] = priority;
                    priority++;
                }
            }

            List<int> priorities = new List<int>();
            foreach (int num in numbers)
            {
                priorities.Add(priorityDict[num]);
            }

            return priorities;
        }

        Tuple<string, string> storeUserInputProductID()
        {
            return new Tuple<string, string>(Request.Form["productID"], Request.Form["finalizationTime"]);
        }
    }
}

