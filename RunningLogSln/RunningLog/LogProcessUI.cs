namespace RunningLog;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

public class LogProcessUI
{
    // public RunningLogs Logs {get; set;}

    public void Show(RunningLogs Logs){
        Console.Clear();
        System.Console.WriteLine("Welcome " + Logs.GetUser().Username);
        while(true){
            Console.WriteLine("Please select option (1/2/3/4): \n1. Input Running Log \n2. Edit Running Log \n3. Review Running Log\n4. Exit");
            string opitonInput =Console.ReadLine().Trim();
            if (opitonInput == "1"){
                InputLog(Logs);
                // break;
            } else if(opitonInput == "2"){
                if(Logs.GetRunningLogList().Count <= 0){
                    System.Console.WriteLine("No data available");
                    System.Console.WriteLine();
                    continue;
                }
                System.Console.WriteLine("The following are the running logs: ");
                for(int i = 0; i<Logs.GetRunningLogList().Count; i++){
                    Running log = Logs.GetRunningLogList()[i];
                    System.Console.WriteLine((i+1).ToString() + ". " + log.RunningDate.ToString("MM/dd/yyy") + "; " + log.Duration.ToString() + " mins; " + log.Distance.ToString() + " miles");
                }
                EditLogs(Logs);

                // break;

            } else if(opitonInput == "3"){

                ReviewLog(Logs);
                
                
            } else if(opitonInput == "4"){
                System.Console.WriteLine("Goodbye!");
                // Console.Read();
                Environment.Exit(0);
            }
            else{
                System.Console.WriteLine("Illegal option. Please try again.");
                System.Console.WriteLine();
            }

        }

    }

    private static void LogProcessMenu(RunningLogs Logs){
        
        System.Console.WriteLine("Welcome " + Logs.GetUser().Username);
        Console.WriteLine("Please select option (1/2/3/4): \n1. Input Running Log \n2. Edit Running Log \n3. Review Running Log\n4. Exit");
    }

    private void InputLog(RunningLogs Logs){

        Running newRunningLog = new Running();
        User tempUser = Logs.GetUser();

  
        // System.Console.WriteLine(tempUser.Height);
        newRunningLog.SetHeight(tempUser.Height);

        while(true){

            System.Console.WriteLine("Please enter date in MM/dd/yyyy:");
            try {string inputDate = Console.ReadLine();
            newRunningLog.RunningDate = DateTime.ParseExact(inputDate, "d", CultureInfo.InvariantCulture);
            break;
            } catch {
                System.Console.WriteLine("Wrong date format. Please try again considering leading zeros");
            }
        }

        //input duration
        EnterDuration(newRunningLog);

        //input distance
        EnterDistance(newRunningLog);
        // input weight
        while(true){
        System.Console.WriteLine("Please enter weight in pounds with max 2 decimals: ");
        try{
        newRunningLog.Weight = Math.Round(double.Parse(Console.ReadLine()), 2);
        break;
        } catch {
            System.Console.WriteLine("Numbers are needed. Please try again");
        }
        }
        newRunningLog.calcPace();
        newRunningLog.calcCaloriesBurned();
        newRunningLog.calcBMI();
        Logs.AddRunninglogList(newRunningLog);
        SaveLog(Logs);
    }

    private void EditLogs(RunningLogs Logs){
        System.Console.WriteLine("Please enter the log number you want to edit, Q to return: ");
        int logItem = 0;
        while(true){
            string command = Console.ReadLine();
   
            if (command == "Q" || command == "q"){
                break;
            }
            
            try{
             logItem = Int32.Parse(command);
             if (logItem <1 || logItem > Logs.GetRunningLogList().Count()){
                System.Console.WriteLine("Item number entered is out of range. Please try again");
                continue;
             }
            EditItem(logItem, Logs);
            SaveLog(Logs);
             break;
            } catch {
                System.Console.WriteLine("Please enter a number.");
            }                     
          
        }



    }

    private void EditItem(int logItem, RunningLogs Logs){
            while (true){
                System.Console.WriteLine("Please select item to be edited\n1. Duration\n2. Distance\n3. Delete\n4. Exit");
                string inputNumber = Console.ReadLine();
                if(inputNumber == "1"){
                EnterDuration(logItem, Logs);
                Logs.GetRunningLogList()[logItem-1].calcPace();
                Logs.GetRunningLogList()[logItem-1].calcCaloriesBurned();
                // Logs.GetRunningLogList()[logItem-1].calcBMI();
                // SaveLog(Logs);
                // break;
                }
                else if(inputNumber == "2"){
                EnterDistance(logItem, Logs);
                Logs.GetRunningLogList()[logItem-1].calcPace();
                Logs.GetRunningLogList()[logItem-1].calcCaloriesBurned();
                // Logs.GetRunningLogList()[logItem-1].calcBMI();
                // SaveLog(Logs);                  
                // continue;
                }
                else if(inputNumber =="3"){
                    Logs.GetRunningLogList().RemoveAt(logItem - 1);
                    // SaveLog(Logs);
                    System.Console.WriteLine("Deleted selected log. Press any key to continue.");
                    Console.ReadKey();
                    break;
                }
                else if(inputNumber == "4"){
                    // Show(Logs);
                    break;
                }
                else {
                    System.Console.WriteLine("Illegal input. Please try again.");
                }
                
            }        
    }

    private void EnterDistance(Running newRunningLog){
        while(true){
        System.Console.WriteLine("Please enter distance in miles with max 2 decimals: ");
        try{
        newRunningLog.Distance = Math.Round(double.Parse(Console.ReadLine()), 2);
        break;
        } catch {
            System.Console.WriteLine("Numbers are needed. Please try again");
        }

        }
    }

        private void EnterDistance(int logItem, RunningLogs Logs){
        while(true){
        System.Console.WriteLine("Please enter distance in miles with max 2 decimals: ");
        try{
        Logs.GetRunningLogList()[logItem -1].Distance = Math.Round(double.Parse(Console.ReadLine()), 2);

        break;
        } catch {
            System.Console.WriteLine("Numbers are needed. Please try again");
        }

        }

    }

    private void EnterDuration(Running newRunningLog){
        while(true){
        System.Console.WriteLine("Please enter duration in decimal minutes: ");
        try{
        double inputDuration = double.Parse(Console.ReadLine());
        // double min =double.Parse( inputDuration.Split("/")[0]);
        // double sec = double.Parse(inputDuration.Split("/")[1]);
        newRunningLog.Duration =Math.Round(inputDuration, 2);
        break;
        } catch{
            System.Console.WriteLine("Numbers are needed. Please try again");
        }

        }
    }

        private void EnterDuration( int logItem , RunningLogs Logs){
        while(true){
        System.Console.WriteLine("Please enter duration in decimal minutes: ");
        try{
        double inputDuration = double.Parse(Console.ReadLine());
        // double min =double.Parse( inputDuration.Split("/")[0]);
        // double sec = double.Parse(inputDuration.Split("/")[1]);
        Logs.GetRunningLogList()[logItem -1 ].Duration =Math.Round(inputDuration, 2);

        break;
        } catch{
            System.Console.WriteLine("Numbers are needed. Please try again");
        }

        }

    }





    private void SaveLog(RunningLogs Logs){
        Logs.GetRunningLogList().Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        string fileName = Logs.GetUser().Username + "-runninglogs.txt";
        var options = new JsonSerializerOptions { WriteIndented = true };
        var logJson = JsonSerializer.Serialize(Logs, options);
        File.WriteAllText(fileName, logJson);
    }

    private void ReviewLog(RunningLogs Logs){     
            while(true){
            Console.WriteLine();
            System.Console.WriteLine("Please select review option\n1. View all logs\n2. Review last 7 days\n3. Weekly report\n4. Monthly report\n5. Exit");
            string inputOption = Console.ReadLine();

            if (inputOption == "5"){
                break;
            }
            Console.WriteLine();
            if(inputOption =="1"){
                // ReviewLog
                if(Logs.GetRunningLogList().Count <=0){
                    System.Console.WriteLine("No data available");
                    continue;
                }
                for(int i = 0; i<Logs.GetRunningLogList().Count; i++){
                    Running log = Logs.GetRunningLogList()[i];
                    System.Console.WriteLine((i+1).ToString() + ". " + log.RunningDate.ToString("MM/dd/yyy") + ": " 
                    + log.Duration.ToString() + " mins; " + log.Distance.ToString() + " miles; " + log.GetPace() + " min/mile; " + log.GetCaloriesBurned() + " calories burned");
                }
                Console.WriteLine();
                System.Console.WriteLine("Press any key back");
                Console.ReadKey();
                

            }else if(inputOption == "2")
            {

                if(Logs.GetLatestSevendays().Count <=0){
                    System.Console.WriteLine("No data available");
                    continue;
                }
                List<Running> latestLogs = Logs.GetLatestSevendays();
                
                List<double> summaries = Logs.GetSummary(latestLogs);
                System.Console.WriteLine("Your reports for latest 7 days:");
                System.Console.WriteLine("You have run " + latestLogs.Count + " days in the past 7 days");
                PrintReport(summaries);
                System.Console.WriteLine("Beginning and end BMI during the time interval is " 
                + latestLogs[0].GetBMI() + "/" + latestLogs[^1].GetBMI() + ", BMI change is " + summaries[15]);
                Console.WriteLine();
                System.Console.WriteLine("Press any key back");
                
                Console.ReadKey();
             
                // continue;
                
            }else if(inputOption == "3"){
                if( Logs.GetWeeklyLogs().Count <=0){
                    System.Console.WriteLine("No data available");
                    continue;
                }   
                List<Dictionary<string, List<Running>>> weekLogsList = Logs.GetWeeklyLogs();
             
                System.Console.WriteLine("Your weekly report:");

                PeriodReport(weekLogsList, Logs);
                System.Console.WriteLine("Press any key back");                
                Console.ReadKey();
            }
            else if(inputOption == "4")
            {
                if( Logs.GetMonthlyLogs().Count <=0){
                    System.Console.WriteLine("No data available");
                    continue;
                }  
                List<Dictionary<string, List<Running>>> monthLogsList = Logs.GetMonthlyLogs();
                System.Console.WriteLine("Your monthly report:");
                PeriodReport(monthLogsList, Logs);


                System.Console.WriteLine("Press any key back");                
                Console.ReadKey();            
                
            } else{
                System.Console.WriteLine("Illegal input. Please try again.");
            }
        }

    }

    private void PrintReport(List<double> summaries){
                System.Console.WriteLine("Max distance is " + summaries[0] + " miles, min distance is " + summaries[1] + 
                " miles, total distance is " + summaries[2] + " miles, average distance is " + summaries[3] + " miles");
                // System.Console.WriteLine("Min distance is " + summaries[1] + " miles");
                System.Console.WriteLine("Max duration is " + summaries[4] + " minutes, min duration is " + summaries[5] + 
                " minutes, total duration is " + summaries[6] + " minutes, average duration is " + summaries[7] + " minutes");
                System.Console.WriteLine("Fastest pace is " + summaries[8] + " minutes/mile, slowest pace is " + summaries[9] + 
                " minutes/mile, average pace is " + summaries[10] + " minutes/mile");
                System.Console.WriteLine("Max calories burned is " + summaries[11] + " calories, min calories burned is " + summaries[12] + 
                " calories, total calories burned is " + summaries[13] + " calories, average calories burned is " + summaries[14] + " calories");
                // System.Console.WriteLine("BMI change is " + summaries[15]);
    }

    private void PeriodReport( List<Dictionary<string, List<Running>>> LogsList, RunningLogs Logs)
    {
        foreach(var item in LogsList){
        
            foreach(string key in item.Keys){
                System.Console.WriteLine(key);
            
                List<double> summaries = Logs.GetSummary(item[key]);
                
                System.Console.WriteLine("You have run " + item[key].Count + " days in " + key);
                PrintReport(summaries);
                System.Console.WriteLine("Beginning and end BMI during the time interval is " 
                + item[key][0].GetBMI() + "/" + item[key][^1].GetBMI() + ", BMI change is " + summaries[15]);

                Console.WriteLine();
                
            }

       }
        
    }
}