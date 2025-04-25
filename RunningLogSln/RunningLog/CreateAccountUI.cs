namespace RunningLog;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class CreateAccountUI
{
    private RunningLogs _runningLogs;

    public CreateAccountUI(){
        _runningLogs = new RunningLogs();
    }

    public void Show(){
        
        // User tempUser = new User();
        // username
        while(true){
        System.Console.WriteLine("Please input username: ");
        string username = Console.ReadLine().Trim();
        if(username.Length <=0){
            System.Console.WriteLine("Input is empty, please try again.");
            continue;
        }
        if (!User.IsUsernameExist(username + "-runninglogs.txt")){
            _runningLogs.GetUser().Username = username;
            break;

        }
        System.Console.WriteLine("Username exists. Please try again");
           
        }

    //   password
        while(true){
            System.Console.WriteLine("Please enter password: ");
            string pswd = PswdProcess();
         
            System.Console.WriteLine("Please confirm the password: ");
            string pswd2 = PswdProcess();
          
            if(pswd.Equals(pswd2)){
            _runningLogs.GetUser().Password = pswd2;
            break;
            }
            System.Console.WriteLine("Password didn't match. Please try again");
            continue;

        }
// age input
        while(true){
            System.Console.WriteLine("Please input your age: ");
            string ageStr = Console.ReadLine().Trim();
            try{
              int age = Int32.Parse(ageStr);
              if(age < 10 || age > 130){
                System.Console.WriteLine("Age entered is too small or too large, please try again");
                continue;
              }
              _runningLogs.GetUser().Age = age;
              break;
            } catch{
                System.Console.WriteLine("Illegal input, please try again.");
                
            }
        }

        // weight input 
        while(true){
            System.Console.WriteLine("Please input your weight in pounds: ");
            string weightStr = Console.ReadLine().Trim();
            try{
              double weight = double.Parse(weightStr);
              if(weight < 50){
                System.Console.WriteLine("Weight entered is too small, please try again");
                continue;
              }
              _runningLogs.GetUser().Weight = weight;
              break;
            } catch{
                System.Console.WriteLine("Illegal input, please try again.");
                
            }
        }

//  height input
        while(true){
            System.Console.WriteLine("Please input your height in inches: ");
            string heightStr = Console.ReadLine().Trim();
            try{
              double height = double.Parse(heightStr);
              if(height < 20){
                System.Console.WriteLine("Height entered is too small, please try again");
                continue;
              }
              _runningLogs.GetUser().Height = height;
              break;
            } catch{
                System.Console.WriteLine("Illegal input, please try again.");
                
            }
        }

        // input gender
        while(true){
            System.Console.WriteLine("Please enter your gender (F/M): ");
            string gender = Console.ReadLine().Trim();
            string[] genders = {"F", "f", "M", "m"};
              
              if(genders.Contains(gender)){
                _runningLogs.GetUser().Gender = gender.ToUpper();
                break;

              }
            System.Console.WriteLine("Illegal input, please try again");
        
        }
       
        _runningLogs.SaveLogs(); 
    }
    

public static string PswdProcess(){
        // get password, * show to replace enterd chars. 
        // encrypt password by SHA256
        List<char> password = new List<char>();
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter ){
                if(password.Count > 0)
                {
                    Console.WriteLine();
                    break;
                }
                else{
                    System.Console.WriteLine("Empty input. Please try again.");
                    continue;
                }
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Count > 0)
                {
                    password.RemoveAt(password.Count - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                string pattern = @"[\w!@#$&,.?;:]";
                Regex regex = new Regex(pattern);
                // a-zA-Z0-9_!@#$&,.?;: are allowed for password
                if (regex.IsMatch(key.KeyChar.ToString())){

                password.Add(key.KeyChar);
                Console.Write("*");
                }
            }
        }

        return HashCalc(string.Join("", password));        
        
    }


private static string HashCalc(string password)
{
    using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
    
}
    

}