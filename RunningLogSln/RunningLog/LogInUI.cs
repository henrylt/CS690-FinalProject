namespace RunningLog;
using System;
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
            if(!User.IsUsernameExist(username + "-runninglogs.txt")){
                if(count <= 1){
                    System.Console.WriteLine("You have entered wrong username or password 3 times. Application will exit.");
                    System.Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Environment.Exit(0);                  
                    
                }
                
                count--;
                System.Console.WriteLine("User doesn't exist. Please try again. " + count + " attempts left");
                continue;
            }
            System.Console.WriteLine("Please eneter password: ");
            // input password, 
            string pswd =CreateAccountUI.PswdProcess();
           
            if(!RunningLogs.LoadDataFromFile(username + "-runninglogs.txt",  out _runningLogs)){
                System.Console.WriteLine("Fail loading running log file. Press any to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
             // compare user name and password with stored data
            if(_runningLogs.GetUser().LoginPasswordVerify(pswd) && _runningLogs.GetUser().LoginUsernameVerify(username)){
                break;
            }

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

}
