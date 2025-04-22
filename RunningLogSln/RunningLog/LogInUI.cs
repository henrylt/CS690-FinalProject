namespace RunningLog;
using System;
using System.IO;
using System.Text.Json;

public class LogInUI
{

    private RunningLogs _runningLogs;

    public LogInUI(){
        _runningLogs = new RunningLogs();
    }

    public RunningLogs Show(){
        int count = 3;
        while(count > 0){
            System.Console.WriteLine("Please enter user name: ");
            string username = Console.ReadLine().Trim();
            if(!File.Exists(username+"-runninglogs.txt")){
                if(count <= 1){
                    System.Console.WriteLine("You have entered wrong username or password 3 times. Application will exit.");
                    System.Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Environment.Exit(0);                  
                    
                }
                System.Console.WriteLine("User doesn't exist. Please try again.");
                count--;
                continue;
            }
            System.Console.WriteLine("Please eneter password: ");
            // input password, 
            string pswd =CreateAccountUI.PswdProcess();
            LoadDataFromFile(username+"-runninglogs.txt");
            if(pswd.Equals(_runningLogs.GetUser().Password) && username.Equals(_runningLogs.GetUser().Username)){
               
                
                // System.Console.WriteLine("Welcome " + username);
                // LogsMenu();
                // return true;
                break;
            }
            // compare user name and password with stored data
            count--;       
            if(count <= 0){
                    System.Console.WriteLine("You have entered wrong username or password 3 times. Application will exit.");
                    System.Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Environment.Exit(0);   
            }
            System.Console.WriteLine("Wrong password. You will have " + count + " more chances to try again."); 
        }
       
        return _runningLogs;


    }
    private void LoadDataFromFile(string fileName){

        string jsonUser = File.ReadAllText(fileName);
        if(jsonUser != null){
  
        _runningLogs = JsonSerializer.Deserialize<RunningLogs>(jsonUser);
 
        } else{
            System.Console.WriteLine("Something wrong happens to the application.");
            System.Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }

}
