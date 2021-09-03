using Core;
using System;
using Core.Issues;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var Issue = new IssueCore();
            var IssueData = new Issue()
            {
                Title = "Null Reference Point Exception Updated",
                Created = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"),
                Description = "I am getting weird error related to Null Reference Point Exception",
                LastUpdated = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"),
                Tag = "Beginner",
                Keywords = new List<string>() { "Null Reference", "Null Point" },
                Solution = "Insert Breakpoints and check the value that is null",
                Base64Images = new List<string> { "gsdfjsakuyfijiajisjdfiashjfihafisafhsaihfj", "opksaidhgisdhghusdoghuosdhgusdjhgisdgkju", "gofpdsighsdiuhjgdshjgjh" }
            };
            await Issue.Initiliaze();
            var result = await Issue.DeleteIssue("jk" , "LFkX4o0dWLyFQuqI9HkH");
        }
    }
}
