namespace RunningLog;

using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class RunningLogs{
    
     [JsonInclude]
    private User _user; 
     [JsonInclude]
    private List<Running> _runninglogList ;

    

    public RunningLogs(){
        _user = new User();
        _runninglogList = new List<Running>();
    }

    public void SetUser(User user){
        _user = user;
    }

    public void AddRunninglogList(Running running){
        _runninglogList.Add(running);
    }

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
        else if(length <= 7){
            LatestLogs =  _runninglogList;
        }
        else{
        for(int i=length - 7; i<length; i++){
            LatestLogs.Add(_runninglogList[i]);

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


        // Dictionary<string, List<Running>> monthLogs = new Dictionary<string, List<Running>>();

        
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


        // Dictionary<string, List<Running>> monthLogs = new Dictionary<string, List<Running>>();

        
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

        // foreach(var weeklog in weekLogsList){
        //     foreach(string week in weeklog.Keys){
        //         System.Console.WriteLine(week);
        //     }
        // }

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
}
