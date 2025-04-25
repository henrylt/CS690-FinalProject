namespace RunningLog;

using System.IO;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Security.Cryptography;

public class RunningLogs{
    
     [JsonInclude]
    private User _user; 
     [JsonInclude]
    private List<Running> _runninglogList;  

    public RunningLogs(){
        _user = new User();
        _runninglogList = new List<Running>();
    }

    public void SetUser(User user){
        _user = user;
    }

    // public void AddRunninglogList(Running running){
    //     _runninglogList.Add(running);
    // }

    public User GetUser(){
        return _user;
    }

    public List<Running> GetRunningLogList(){
        return _runninglogList;
    }

    public List<Running> GetLatestSevendays(){
        List<Running> LatestLogs = new List<Running>();
        int length = _runninglogList.Count;
        if(length <= 0){
            return LatestLogs;
        }
        // get log of latest 7 days from today
        DateTime today = DateTime.Now;
        foreach(Running logRecord in _runninglogList){
            int days = (today.Date - logRecord.RunningDate.Date).Days;
            if(days < 7){
                LatestLogs.Add(logRecord);
            }
        } 
        return LatestLogs;
    }

    public List<Dictionary<string, List<Running>>> GetMonthlyLogs(){
        List<Dictionary<string, List<Running>>> monthLogsList = new List<Dictionary<string, List<Running>>>();
        if(_runninglogList.Count <= 0 ){
            return monthLogsList;
        }

        int month = _runninglogList[0].RunningDate.Month;
        int year =  _runninglogList[0].RunningDate.Year;
        
        int count = 0;
        monthLogsList.Add(new Dictionary<string, List<Running>>());
        monthLogsList[count].Add(month + "/" + year, new List<Running>());

        foreach(var log in _runninglogList){          

            if(log.RunningDate.Month != month || log.RunningDate.Year != year){     
                 count +=1 ;
                 month = log.RunningDate.Month;
                 year = log.RunningDate.Year;
                 string keystring = month + "/" + year;
              
                 monthLogsList.Add(new Dictionary<string, List<Running>>(){
                    {keystring, new List<Running>()}
                 });
           
                  monthLogsList[count][keystring].Add(log);
           
            } else{

                if (! monthLogsList[count].ContainsKey(month + "/" + year))
                {
                     monthLogsList[count].Add(month + "/" + year, new List<Running>());
                     monthLogsList[count][month + "/" + year].Add(log);
                }
                else{
                     monthLogsList[count][month + "/" + year].Add(log);
                }

            }
        }


        return monthLogsList;
     
    }
    public List<Dictionary<string, List<Running>>> GetWeeklyLogs(){
        CultureInfo myCI = new CultureInfo("en-US");
        Calendar myCal = myCI.Calendar;

        CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
        DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
        List<Dictionary<string, List<Running>>> weekLogsList = new List<Dictionary<string, List<Running>>>();
        if(_runninglogList.Count <=0){
            return weekLogsList;
        }
        
        int weeknumber = myCal.GetWeekOfYear(_runninglogList[0].RunningDate, myCWR, myFirstDOW );
        int year =  _runninglogList[0].RunningDate.Year;
        
        int count = 0;
        weekLogsList.Add(new Dictionary<string, List<Running>>());
        weekLogsList[count].Add("week " + weeknumber + "/" + year, new List<Running>());


        foreach(var log in _runninglogList){          

            if(myCal.GetWeekOfYear(log.RunningDate, myCWR, myFirstDOW )!= weeknumber || log.RunningDate.Year != year){     
                 count +=1 ;
                 weeknumber = myCal.GetWeekOfYear(log.RunningDate, myCWR, myFirstDOW );
                 year = log.RunningDate.Year;
                 string keystring = "week " + weeknumber + "/" + year;
              
                 weekLogsList.Add(new Dictionary<string, List<Running>>(){
                    {keystring, new List<Running>()}
                 });
           
                  weekLogsList[count][keystring].Add(log);
           
            } else{

                if (! weekLogsList[count].ContainsKey("week " + weeknumber + "/" + year))
                {
                     weekLogsList[count].Add("week " + weeknumber + "/" + year, new List<Running>());
                     weekLogsList[count]["week " + weeknumber + "/" + year].Add(log);
                }
                else{
                     weekLogsList[count]["week " + weeknumber + "/" + year].Add(log);
                }

            }
        }


       return weekLogsList;
     
    }


    


    
    public List<double> GetSummary(List<Running> runningsList){
        List<double> summaries = new List<double>();
        var maxDistanceLog = runningsList.MaxBy(x => x.Distance);
        var minDistanceLog = runningsList.MinBy(x=> x.Distance);
        double sumDistance = runningsList.Sum(x=>x. Distance);
        var minDurationLog = runningsList.MinBy(x => x.Duration);
        var maxDurationLog = runningsList.MaxBy(x => x.Duration);
        double sumDuration = runningsList.Sum(x => x.Duration);

        var lowerestPaceLog = runningsList.MaxBy(x => x.GetPace());
        var highestPaceLog = runningsList.MinBy(x => x.GetPace());
        double averagePace = Math.Round(sumDuration/sumDistance, 2);
        var maxCaloriesLog = runningsList.MaxBy(x=>x.GetCaloriesBurned());
        var minCaloriesLog = runningsList.MinBy(x=>x.GetCaloriesBurned());
        double sumCalories = runningsList.Sum(x=>x.GetCaloriesBurned());

        summaries.Add(maxDistanceLog.Distance);
        summaries.Add(minDistanceLog.Distance);
        summaries.Add(Math.Round(sumDistance, 2));
        summaries.Add(Math.Round(sumDistance/runningsList.Count, 2));

        summaries.Add(maxDurationLog.Duration);
        summaries.Add(minDurationLog.Duration);
        summaries.Add(Math.Round(sumDuration, 2));
        summaries.Add(Math.Round(sumDuration/runningsList.Count, 2));

        summaries.Add(highestPaceLog.GetPace());
        summaries.Add(lowerestPaceLog.GetPace());
        summaries.Add(averagePace);

        summaries.Add(maxCaloriesLog.GetCaloriesBurned());
        summaries.Add(minCaloriesLog.GetCaloriesBurned());
        summaries.Add(Math.Round(sumCalories, 2));
        summaries.Add(Math.Round(sumCalories/runningsList.Count, 2));

        double BMIchanged = Math.Round( runningsList[runningsList.Count - 1].GetBMI() -  runningsList[0].GetBMI(), 2);
        summaries.Add(BMIchanged);
        return summaries;


    }

    public void AddNewLog(DateTime runningDate, double duration, double distance, double weight){
        Running newLog = new Running();
        newLog.SetHeight(_user.Height);
        newLog.RunningDate = runningDate;
        newLog.Duration = duration;
        newLog.Distance = distance;
        newLog.Weight = weight;
        newLog.calcPace();
        newLog.calcCaloriesBurned();
        newLog.calcBMI();        
        _runninglogList.Add(newLog);
        _runninglogList.Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));

    }

    public void EditLogDuration(int logItem, double duration){     
        _runninglogList[logItem].Duration = duration;
        _runninglogList[logItem].calcPace();
        _runninglogList[logItem].calcCaloriesBurned();

    }

    public void EditLogDistance(int logItem, double distance){
        _runninglogList[logItem].Distance = distance;
        _runninglogList[logItem].calcPace();
        _runninglogList[logItem].calcCaloriesBurned();

    }

    public void RemoveLog(int logItem){
        _runninglogList.RemoveAt(logItem);
    }

    public void SaveLogs(){
        if(_runninglogList.Count > 0){
            _runninglogList.Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        }
        string fileName = _user.Username + "-runninglogs.txt";
        var options = new JsonSerializerOptions { WriteIndented = true};
        var logJson = JsonSerializer.Serialize(this, options);
        File.WriteAllText(fileName, logJson);   
    }
    public static bool LoadDataFromFile(string fileName, out RunningLogs loadedrunningLogs){

        try {
            string jsonUser = File.ReadAllText(fileName);
            if(jsonUser != null){    
                loadedrunningLogs = JsonSerializer.Deserialize<RunningLogs>(jsonUser);       
                return true;
            }  
            loadedrunningLogs = null;               
            return false;
        } catch{
            loadedrunningLogs = null;   
            return false;           
        }
    }
}
