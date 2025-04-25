namespace RunningLogs.Tests;
using RunningLog;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RunningLogsTest
{


    // user exists test
    [Fact]
    public void UserExistTest()
    {
        string fileName = "usertest-runninglogs.txt";
     
      
        Assert.True(User.IsUsernameExist(fileName));
        Assert.False(User.IsUsernameExist("somefile.txt"));
    }

    //user name verify test
    [Fact]
    public void UserNameVerifyTest()
    {
        // Given
        User testUser =  new User();
        testUser.Username = "testUser";
        testUser.Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3";
        testUser.Age = 23;
        testUser.Weight = 160;
        testUser.Height=72;
        testUser.Gender = "M";

    
        // When
    
        // Then
        Assert.True(testUser.LoginUsernameVerify("testUser"));
        Assert.False(testUser.LoginUsernameVerify("someUser"));
    }


    //user password verify test
    [Fact]
    public void UserPasswordVerifyTest()
    {
        // Given
        User testUser =  new User();
        testUser.Username = "testUser";
        testUser.Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3";
        testUser.Age = 23;
        testUser.Weight = 160;
        testUser.Height=72;
        testUser.Gender = "M";
    
        // When

        string testCode1 = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3";
        string testCode2 = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae4";

        Assert.True(testUser.LoginPasswordVerify(testCode1));
        Assert.False(testUser.LoginPasswordVerify(testCode2));
    
        // Then
    }


    //calc and set Pace, caloreisBurned, Bmi test
    [Fact]
    public void RunningPaceTest()
    {
        Running testRunning = new Running();
        testRunning.RunningDate = DateTime.Now;
        testRunning.Duration = 45;
        testRunning.Distance =4.5;
        testRunning.Weight = 150;
        testRunning.SetHeight(69);
        testRunning.calcPace();
        testRunning.calcCaloriesBurned();
        testRunning.calcBMI();

        double pace = 10;
        double pace2 = 9.2;

        double calories = 519;
        double calories2 = 500;

        double bmi = 22.15;
        double bmi2 = 21.34;


        Assert.Equal(pace, testRunning.GetPace());
        Assert.NotEqual(pace2, testRunning.GetPace());

        Assert.Equal(calories, testRunning.GetCaloriesBurned());
        Assert.NotEqual(calories2, testRunning.GetCaloriesBurned());

        Assert.Equal(bmi, testRunning.GetBMI());
        Assert.NotEqual(bmi2, testRunning.GetBMI()); 
    }


    //load data from persistent file test
    [Fact]
    public void LoadFileTest()
    {
        // Given

 

        string fileName = "test2-runninglogs.txt";
        string fileName2 = "somelogs.txt";
        RunningLogs runningLogs = new RunningLogs();
        RunningLogs runningLogs2 = new RunningLogs();
        RunningLogs modifiedRunningLogs = new RunningLogs();
        List<Running> sampleRunningList = new List<Running>();
        List<Running> modifiedRunningList = new List<Running>();

        User user1 = new User();
        user1.Username = "test2";
        user1.Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"; 
        user1.Gender = "M";
        user1.Height = 68;
        user1.Weight = 161;
        user1.Age = 34;


        User user2 = new User();
        user2.Username = "test3";
        user2.Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"; 
        user2.Gender = "M";
        user2.Height = 65;
        user2.Weight = 161;
        user2.Age = 35;

        Running r1 = new Running();
        r1.SetHeight(user1.Height);
        r1.Duration = 45;
        r1.Distance = 4.1;
        r1.Weight =161;
        r1.RunningDate = DateTime.ParseExact("02/01/2025", "d", CultureInfo.InvariantCulture);
        r1.calcPace();
        r1.calcCaloriesBurned();
        r1.calcBMI();

        Running r2 = new Running();
        r2.SetHeight(user1.Height);
        r2.Duration = 46;
        r2.Distance = 4.2;
        r2.Weight =160;
        r2.RunningDate = DateTime.ParseExact("02/02/2025", "d", CultureInfo.InvariantCulture);
        r2.calcPace();
        r2.calcCaloriesBurned();
        r2.calcBMI();

        RunningLogs sampleLogs = new RunningLogs();
        sampleLogs.SetUser(user1);
        sampleLogs.GetRunningLogList().Add(r1);
        sampleLogs.GetRunningLogList().Add(r2);
        sampleLogs.GetRunningLogList().Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        sampleRunningList.Add(r1);
        sampleRunningList.Add(r2);
        sampleRunningList.Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        // sampleLogs.SaveLogs();

        Running modifiedLog= new Running();
        modifiedLog.SetHeight(user1.Height);
         modifiedLog.Duration = 50;
         modifiedLog.Distance = 4.8;
         modifiedLog.Weight =155;
         modifiedLog.RunningDate = DateTime.ParseExact("02/02/2025", "d", CultureInfo.InvariantCulture);
         modifiedLog.calcPace();
         modifiedLog.calcCaloriesBurned();
         modifiedLog.calcBMI();

         modifiedRunningLogs.SetUser(user1);
         modifiedRunningLogs.GetRunningLogList().Add(r1);
         modifiedRunningLogs.GetRunningLogList().Add(modifiedLog);
        modifiedRunningLogs.GetRunningLogList().Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));


        modifiedRunningList.Add(r1);
        modifiedRunningList.Add(modifiedLog);
        modifiedRunningList.Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        
        bool result = RunningLogs.LoadDataFromFile(fileName, out runningLogs);
        bool result2 = RunningLogs.LoadDataFromFile(fileName2, out runningLogs2);
        

        var samleJson = JsonSerializer.Serialize(sampleLogs);
        var loadedJson = JsonSerializer.Serialize(runningLogs);
        var modifiedJson = JsonSerializer.Serialize(modifiedRunningLogs);

        // data load from file test
        Assert.True(result);
        Assert.False(result2);
        Assert.Equal(runningLogs2, null);
        Assert.Equal(samleJson, loadedJson);
        Assert.NotEqual(loadedJson, modifiedJson);

        var samleUserJson = JsonSerializer.Serialize(user1);
        var modifiedUserJson = JsonSerializer.Serialize(user2);
        var loadedUserJson = JsonSerializer.Serialize(runningLogs.GetUser());
        // GetUser test
        Assert.Equal(samleUserJson, loadedUserJson);
        Assert.NotEqual(modifiedUserJson, loadedUserJson);

        //GetrunningLogList test

        var sampleRunningListJson = JsonSerializer.Serialize(sampleRunningList);
        var loadedRunningListJson = JsonSerializer.Serialize(runningLogs.GetRunningLogList());
        var modifiedRunningListJson = JsonSerializer.Serialize(modifiedRunningList);

        Assert.Equal(sampleRunningListJson, loadedRunningListJson);
        Assert.NotEqual(modifiedRunningListJson, loadedRunningListJson);

    }
    
    
    //get latest 7 days log list and summarie result test
    [Fact]
    public void GetReportLogLatestDaysSummaryTest()
    {
        // Given

        RunningLogs samplerunningLogs = new RunningLogs();
        User user1 = new User();
        user1.Username = "test2";
        user1.Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92"; 
        user1.Gender = "M";
        user1.Height = 68;
        user1.Weight = 161;
        user1.Age = 34;
        samplerunningLogs.SetUser(user1);

        Running r1 = new Running();
        r1.SetHeight(user1.Height);
        r1.Duration = 45;
        r1.Distance = 4.1;
        r1.Weight =161;
        r1.RunningDate = DateTime.Today.AddDays(-2);
        r1.calcPace();
        r1.calcCaloriesBurned();
        r1.calcBMI();

        Running r2 = new Running();
        r2.SetHeight(user1.Height);
        r2.Duration = 36;
        r2.Distance = 3.2;
        r2.Weight =160;
        r2.RunningDate =  DateTime.Today.AddDays(-1);
        r2.calcPace();
        r2.calcCaloriesBurned();
        r2.calcBMI();


        // When
        
        Running r3 = new Running();
        r3.SetHeight(user1.Height);
        r3.Duration = 50;
        r3.Distance = 4.4;
        r3.Weight =160;
        r3.RunningDate =  DateTime.Today;
        r3.calcPace();
        r3.calcCaloriesBurned();
        r3.calcBMI();
        // a day not in lastest 7 days
        Running mr3 = new Running();
        mr3.SetHeight(user1.Height);
        mr3.Duration = 60;
        mr3.Distance = 5.4;
        mr3.Weight =160;
        mr3.RunningDate =  DateTime.Today.AddDays(-10);
        mr3.calcPace();
        mr3.calcCaloriesBurned();
        mr3.calcBMI();

        List<Running> sampleRunningList = new List<Running>();
        sampleRunningList.Add(r1);
        sampleRunningList.Add(r2);
        sampleRunningList.Add(r3);
        sampleRunningList.Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));


        samplerunningLogs.GetRunningLogList().Add(r1);
        samplerunningLogs.GetRunningLogList().Add(r2);
        samplerunningLogs.GetRunningLogList().Add(r3);
        // add a day not in the last 7 days
        samplerunningLogs.GetRunningLogList().Add(mr3);
        samplerunningLogs.GetRunningLogList().Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        
        List<Running> mSample = new List<Running>();
        mSample.Add(r1);
        mSample.Add(r2);
        mSample.Add(mr3);
        mSample.Sort((log1, log2) => DateTime.Compare(log1.RunningDate, log2.RunningDate));
        // Then
        var sampleRunningListJson = JsonSerializer.Serialize(sampleRunningList);
        var gotRunningListJson = JsonSerializer.Serialize(samplerunningLogs.GetLatestSevendays());
        var mSampleJson = JsonSerializer.Serialize(mSample);
        Assert.Equal(sampleRunningListJson, gotRunningListJson);
        Assert.NotEqual(mSampleJson, gotRunningListJson);

        string sampleSumSting = "[4.4,3.2,11.7,3.9,50,36,131,43.67,10.98,11.36,11.2,535,389,1426,475.33,-0.15]";
       
        string mSampleSumSting = "[4.8,3.2,11.7,3.9,50,36,131,43.67,10.98,11.36,11.2,535,389,1426,475.33,-0.15]";
        List<double> gotSummary =samplerunningLogs.GetSummary(samplerunningLogs.GetLatestSevendays());
        var gotSummayJson = JsonSerializer.Serialize(gotSummary);

        Assert.Equal(sampleSumSting, gotSummayJson);
        Assert.NotEqual(mSampleSumSting, gotSummayJson);

    }


    // get weekly and monthly log list test
    [Fact]
    public void GetWeeklyMonthlyTest()
    {
        // Given

        string weeklyJson = """[{"week 6/2025":[{"Duration":48,"Distance":4.7,"Weight":161,"RunningDate":"2025-02-03T00:00:00","_pace":10.21,"_caloriesBurned":580,"_BMI":24.48},{"Duration":66,"Distance":5.9,"Weight":160,"RunningDate":"2025-02-04T00:00:00","_pace":11.19,"_caloriesBurned":718,"_BMI":24.33}]}]""" ;
        
        string monthlyJson = """[{"2/2025":[{"Duration":48,"Distance":4.7,"Weight":161,"RunningDate":"2025-02-03T00:00:00","_pace":10.21,"_caloriesBurned":580,"_BMI":24.48},{"Duration":66,"Distance":5.9,"Weight":160,"RunningDate":"2025-02-04T00:00:00","_pace":11.19,"_caloriesBurned":718,"_BMI":24.33}]}]""";

        string mWeeklyJson = """[{"week 6/2025":[{"Duration":23,"Distance":4.7,"Weight":161,"RunningDate":"2025-02-03T00:00:00","_pace":10.21,"_caloriesBurned":580,"_BMI":24.48},{"Duration":66,"Distance":5.9,"Weight":160,"RunningDate":"2025-02-04T00:00:00","_pace":11.19,"_caloriesBurned":718,"_BMI":24.33}]}]""" ;
        
        string mMonthlyJson = """[{"2/2025":[{"Duration":68,"Distance":4.7,"Weight":161,"RunningDate":"2025-02-03T00:00:00","_pace":10.21,"_caloriesBurned":580,"_BMI":24.48},{"Duration":66,"Distance":5.9,"Weight":160,"RunningDate":"2025-02-04T00:00:00","_pace":11.19,"_caloriesBurned":718,"_BMI":24.33}]}]""";


        string fileName = "test3-runninglogs.txt";

        RunningLogs runningLogs = new RunningLogs();

        bool result = RunningLogs.LoadDataFromFile(fileName, out runningLogs);

        var monthlyLogsJson = JsonSerializer.Serialize(runningLogs.GetMonthlyLogs());
        var weeklyLogsJson = JsonSerializer.Serialize(runningLogs.GetWeeklyLogs());        

        // Then

        Assert.Equal(weeklyJson, weeklyLogsJson);
        Assert.NotEqual(mWeeklyJson, weeklyLogsJson);
        Assert.Equal(monthlyJson, monthlyLogsJson);
        Assert.NotEqual(mMonthlyJson, monthlyLogsJson);
    }


// add new log test
[Fact]
public void InputTest()
{
    // Given


    string originList = """[{"Duration":36,"Distance":3.2,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":11.25,"_caloriesBurned":389,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string addNewList = """[{"Duration":63,"Distance":5.4,"Weight":161,"RunningDate":"2025-04-19T00:00:00","_pace":11.67,"_caloriesBurned":661,"_BMI":24.48},{"Duration":36,"Distance":3.2,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":11.25,"_caloriesBurned":389,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";

    string fileName = "test4-runninglogs.txt";

    RunningLogs runningLogs = new RunningLogs();
    // When

    bool result = RunningLogs.LoadDataFromFile(fileName, out runningLogs);

    DateTime inputDate = DateTime.ParseExact("04/19/2025", "d", CultureInfo.InvariantCulture);
    double duration = 63;
    double distance = 5.4;
    double weight =161;
    runningLogs.AddNewLog(inputDate, duration, distance, weight);

    List<Running> addedList = runningLogs.GetRunningLogList();   
    var addedListJson = JsonSerializer.Serialize(addedList);

    // Then
    Assert.Equal(addNewList, addedListJson);
    Assert.NotEqual(originList, addedListJson);
}


// edit distance of selected log test
[Fact]
public void DistanceEditTest()
{
    // Given

    string originList = """[{"Duration":36,"Distance":3.2,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":11.25,"_caloriesBurned":389,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string editDistanceList = """[{"Duration":36,"Distance":3.5,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":10.29,"_caloriesBurned":429,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string fileName = "test4-runninglogs.txt";

    RunningLogs runningLogs = new RunningLogs();

    // When
    bool result = RunningLogs.LoadDataFromFile(fileName, out runningLogs);
    runningLogs.EditLogDistance(0, 3.5);

    List<Running> editList = runningLogs.GetRunningLogList();   
    var editListJson = JsonSerializer.Serialize(editList);

    // Then
    Assert.Equal(editDistanceList, editListJson);
    Assert.NotEqual(originList, editListJson);

}

//edit duration of selected log test
[Fact]
public void DurationEditTest()
{
    // Given

    string originList = """[{"Duration":36,"Distance":3.2,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":11.25,"_caloriesBurned":389,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string editDurationList = """[{"Duration":40,"Distance":3.2,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":12.5,"_caloriesBurned":392,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string fileName = "test4-runninglogs.txt";

    RunningLogs runningLogs = new RunningLogs();

    // When
    bool result = RunningLogs.LoadDataFromFile(fileName, out runningLogs);
    runningLogs.EditLogDuration(0, 40);

    List<Running> editList = runningLogs.GetRunningLogList();   
    var editListJson = JsonSerializer.Serialize(editList);

    // Then
    Assert.Equal(editDurationList, editListJson);
    Assert.NotEqual(originList, editListJson);

}


// delete selected log test
[Fact]
public void DeleteTest()
{
    // Given

    string originList = """[{"Duration":36,"Distance":3.2,"Weight":160,"RunningDate":"2025-04-23T00:00:00","_pace":11.25,"_caloriesBurned":389,"_BMI":24.33},{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string deletedList = """[{"Duration":50,"Distance":4.4,"Weight":160,"RunningDate":"2025-04-24T00:00:00","_pace":11.36,"_caloriesBurned":535,"_BMI":24.33}]""";
    string fileName = "test4-runninglogs.txt";

    RunningLogs runningLogs = new RunningLogs();
    // When
    bool result = RunningLogs.LoadDataFromFile(fileName, out runningLogs);
    runningLogs.RemoveLog(0);

    List<Running> editList = runningLogs.GetRunningLogList();   
    var editListJson = JsonSerializer.Serialize(editList);

    // Then
    Assert.Equal(deletedList, editListJson);
    Assert.NotEqual(originList, editListJson);
}

}
