using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backend.Models;
using ContextSwitcher.Database;
using ContextSwitcher.Plugins;
using NUnit.Framework;

namespace BackendTests;

public class Tests
{

    private static Random random = new Random();
    private string _title, _description;
    private string _type, _path;
    
    private string[] _correctTypes = new[]
    {
        "chrome",
        "word",
        "excel",
        "outlook",
        "notepad++",
        "powerpoint"
    };

    
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void AuthenticationTest()
    {
        Assert.NotNull(Database.Instance.Authenticate(new UserLoginRequestModel()
        {
            username = "string",
            passw = "string"
        }).Result);
        
        Assert.Null(Database.Instance.Authenticate(new UserLoginRequestModel()
        {
            username = "string1",
            passw = "string1"
        }).Result);
    }
    
    
    [Test]
    public void ReadUserTest()
    {
        Assert.NotNull(Database.Instance.ReadUser(2).Result);
        
        Assert.Null(Database.Instance.ReadUser(-1).Result);
    }


    [Test]
    public void CreateUserTest()
    {
        Database.Instance.CreateUser(new UserModel()
        {
            IsAdmin = false,
            Password = "test",
            Username = "test",
        });
        
        Assert.NotNull(Database.Instance.ReadUser("test"));
        
        Database.Instance.DeleteUser("test");
    }


    [Test]
    public void CreateTaskTest()
    {
        _title = RandomString(50);
        _description = RandomString(100);
        
        Assert.DoesNotThrow(() => Database.Instance.CreateTask(_title, _description, 2));
        Assert.NotNull(Database.Instance.ReadTask(_title));
    }


    [Test]
    public void ReadTaskTest()
    {
        string title = RandomString(50);
        string description = RandomString(100);
        
        Assert.DoesNotThrow(() => Database.Instance.CreateTask(title, description, 2));
        Assert.NotNull(Database.Instance.ReadTask(title));
        Assert.NotNull(Database.Instance.ReadTask(title).Result);
        
        int taskId = Database.Instance.ReadTask(title).Result.TaskId;
        Database.Instance.DeleteTask(taskId, 2);
        Assert.Null(Database.Instance.ReadTask(title).Result);
    }
    

    [Test]
    public void DeleteTaskTest()
    {
        Assert.NotNull(Database.Instance.ReadTask(_title).Result);
        int taskId = Database.Instance.ReadTask(_title).Result.TaskId;
        Database.Instance.DeleteTask(taskId, 2);
        Assert.Null(Database.Instance.ReadTask(_title).Result);
    }


    [Test]
    public void CorrectPluginTypeTest()
    {
        Assert.DoesNotThrow(() => Database.Instance.ReadAllPlugins());
        List<PluginModel> plugins = Database.Instance.ReadAllPlugins().Result;

        foreach (PluginModel pluginModel in plugins)
        {
            Assert.Contains(pluginModel.PluginType, _correctTypes);
        }
        
    }


    [Test]
    public void CorrectPluginFilePathTest()
    {
        char[] invalidChars = Path.GetInvalidPathChars();
        Assert.DoesNotThrow(() => Database.Instance.ReadAllPlugins());
        List<PluginModel> plugins = Database.Instance.ReadAllPlugins().Result;

        foreach (PluginModel pluginModel in plugins)
        {
            foreach (char c in pluginModel.FilePath)
            {
                Assert.AreEqual(invalidChars.Contains(c), false);
            }
            
        }
    }


    [Test]
    public void DeletePluginTest()
    {
        string title = RandomString(50);
        string description = RandomString(100);
        _type = _correctTypes[random.Next() % 5];
        _path = RandomString(50);
        
        Assert.DoesNotThrow(() => Database.Instance.CreateTask(title, description, 2));
        Assert.NotNull(Database.Instance.ReadTask(title));
        
        Assert.NotNull(Database.Instance.ReadTask(title).Result);
        int taskId = Database.Instance.ReadTask(title).Result.TaskId;
        
        Assert.DoesNotThrow(() => Database.Instance.CreatePlugin(_type, _path, taskId));

        int pluginId = Database.Instance.ReadPlugin(taskId, _path).Result.PluginId;

        Database.Instance.DeleteTask(taskId, 2);
        Assert.Null(Database.Instance.ReadTask(title).Result);
        
        Assert.DoesNotThrow(() => Database.Instance.DeletePlugin(pluginId));
    }


    [Test]
    public void CreatePluginTest()
    {
        string title = RandomString(50);
        string description = RandomString(100);
        _type = _correctTypes[random.Next() % 5];
        _path = RandomString(50);
        
        Assert.DoesNotThrow(() => Database.Instance.CreateTask(title, description, 2));
        Assert.NotNull(Database.Instance.ReadTask(title));
        
        Assert.NotNull(Database.Instance.ReadTask(title).Result);
        int taskId = Database.Instance.ReadTask(title).Result.TaskId;
        
        Assert.DoesNotThrow(() => Database.Instance.CreatePlugin(_type, _path, taskId));

        Database.Instance.DeleteTask(taskId, 2);
        Assert.Null(Database.Instance.ReadTask(title).Result);
    }
}