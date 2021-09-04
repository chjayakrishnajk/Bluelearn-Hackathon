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
                Title = ") Expected",
                Created = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"),
                Description = "I am getting weird error related to some bracket missing in the Code",
                LastUpdated = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss"),
                Tag = "Beginner",
                Solution = "Format the Code with ctrl + F and ctrl + d then you can easily find the missing bracket",
                Status = "Solved!"
            };
            await Issue.Initiliaze();
            var result = await Issue.GetIssues("chjayakrishnajk@gmail.com");
        }
    }
}
