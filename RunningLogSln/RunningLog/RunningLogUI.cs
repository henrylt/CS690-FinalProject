namespace RunningLog;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class RunningLogUI
{
    private RunningLogs _runningLogs;
    // private SaveFile _saveFile;
    private LogInUI logUI;
    private CreateAccountUI accoutUI;

    private LogProcessUI processUI;

    public RunningLogUI(){
        _runningLogs = new RunningLogs();
        logUI = new LogInUI();
        accoutUI = new CreateAccountUI();
        processUI = new LogProcessUI();
    }
    
     
    public void Start(){
        // start menu 
        while (true){
        // get input option, loop unitl a valid input is entered.
        // option 1 is to log in, option 2 is to create an account, and option 3 is to exit
            StartPrompt();
            string opiton =Console.ReadLine().Trim();
            if (opiton == "1"){
               _runningLogs = logUI.Show();
              
                break;
            } else if(opiton == "2"){
                accoutUI.Show();
                Console.Clear();
                System.Console.WriteLine("Please Log in");
                _runningLogs = logUI.Show();
                break;
            } else if(opiton == "3"){
                System.Console.WriteLine("Goodbye!");
                // Console.Read();
                Environment.Exit(0);
            } else{
                System.Console.WriteLine("Illegal input, please try again");
            }

        }



        processUI.Show(_runningLogs);
    
    }

        
    private static void StartPrompt(){
        Console.WriteLine("Please select option (1/2/3): \n1. Log In \n2. Create Account \n3. Exit");

    }



}