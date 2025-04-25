using System.Text.Json.Serialization;

namespace RunningLog;

public class Running{


    [JsonInclude]
    private double _pace;
    [JsonInclude]
    private double _caloriesBurned;

    
    private double _height;
   
    [JsonInclude]
    private double _BMI;

    public double Duration {set; get;}
    public double Distance {set; get;}
    public double Weight {set; get;}
 

    

    public DateTime RunningDate {get; set;}



    public void SetHeight(double height){
        _height = height;
    }

    public void calcPace(){

        // pace in min/mile, to calculate met, min pace is 14.482
        _pace = Math.Round(Duration/Distance, 2);
    }

    public void calcCaloriesBurned(){
        //Estimate met for running based on pace, 5 mph (12 min/mile ) --> met 8.0; 7 mph (8.571 min/mile) --> 11.5; 10 mph ( 6min /mile) --> 16.0
        double met = 32.755 - 3.522 * _pace + 0.1216 * _pace * _pace; 
        //estimation calories burned. Duration in minute, weight in kg
        _caloriesBurned =Math.Floor (3.5 * met * Duration * Weight * 0.4536  / 200);
    }

    public void calcBMI(){
        // weight in pounds, height in inches
        _BMI =Math.Round(703 * Weight / (_height * _height), 2);
    }

    public double GetPace(){
        return _pace;

    }

    public double GetBMI(){
        return _BMI;

    }

    public double GetCaloriesBurned(){
        return _caloriesBurned;
    }

}