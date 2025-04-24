namespace RunningLog;
using System;
using System.IO;
using System.Text.Json;

public class User{
    public string Username {get; set;}
    public string Password{get; set;}
    public string Gender {set; get;}
    public int Age {get; set;}
    public double Height {get; set;}
    public double Weight {get; set;}
    public static bool IsUsernameExist(string username){
        return File.Exists(username+"-runninglogs.txt");
    }

    public bool LoginPasswordVerify(string password){
        return password == Password;
    }

    public bool LoginUsernameVerify(string username){
        return username.Equals(Username);
    }

           
}