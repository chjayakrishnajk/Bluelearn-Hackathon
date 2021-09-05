using Core.Issues;
using Firebase.Auth;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core
{
    public class IssueCore
    {
        public FirestoreDb database { get; set; }
        public async Task Initiliaze()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "firebase.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            database = await FirestoreDb.CreateAsync("bluelearn-hackathon");
        }
        public async Task<DocumentReference> CreateIssue(Issue issue , string userId)
        {
            issue.Email = userId;
            CollectionReference col = database.Collection("users").Document(userId).Collection("Issues");
            var data = issue.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(issue, null));
            var result = await col.AddAsync(data);
            issue.Id = result.Id; 
            await UpdateIssue(userId , issue);
            return result;
        }
        public async Task Signin()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyC17liZERnPsPKzQ_2LF0G6u3KA8RTMZO8"));
        }
        public async Task<List<Issue>> GetIssues(string userId)
        {
            var docRef = database.Collection("users").Document(userId).Collection("Issues");
            var snap = await docRef.GetSnapshotAsync();
            var issues = new List<Issue>();
            foreach (var item in snap.Documents)
            {
                var issueItem = item.ConvertTo<Issue>();
                issueItem.Id = item.Id;
                issues.Add(issueItem);
            }
            issues = issues.OrderBy(o => o.Created).ToList();
            return issues;
        }
        public async Task<WriteResult> UpdateIssue(string userId , Issue issue)
        {

            var data = issue.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(issue, null));
            if(issue.IsPublic)
            {
                var col = database.Collection("PublicIssues").Document(issue.Id);
                var dataa = issue.GetType()
         .GetProperties(BindingFlags.Instance | BindingFlags.Public)
              .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(issue, null));
                return (await col.UpdateAsync(dataa));
            }
            return(await database.Collection("users").Document(userId).Collection("Issues").Document(issue.Id).UpdateAsync(data));
        }
        public async Task<Google.Cloud.Firestore.WriteResult> DeleteIssue(string userId, string docId)
        {
            return(await database.Collection("users").Document(userId).Collection("Issues").Document(docId).DeleteAsync());
        }
        public async Task<WriteResult> UpdatePublicIssue(Issue Issue)
        {
            var col = database.Collection("PublicIssues").Document(Issue.Id);
            var data = Issue.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(Issue, null));
            await UpdateIssue(Issue.Email, Issue);
            return (await col.UpdateAsync(data));
        }
        public async Task<WriteResult> AskEveryone(Issue Issue)
        {
            Issue.IsPublic = true;
            var data = Issue.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                     .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(Issue, null));
            await database.Collection("users").Document(Issue.Email).Collection("Issues").Document(Issue.Id).UpdateAsync(data);
            var col = database.Collection("PublicIssues").Document(Issue.Id);
            var dataa = Issue.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(Issue, null));
            return (await col.CreateAsync(dataa));
        }
        public async Task<List<Issue>> GetPublicIssues()
        {
            var docRef = database.Collection("PublicIssues");
            var snap = await docRef.GetSnapshotAsync();
            var issues = new List<Issue>();
            foreach (var item in snap.Documents)
            {
                var issueItem = item.ConvertTo<Issue>();
                issueItem.Id = item.Id;
                issues.Add(issueItem);
            }
            issues = issues.OrderBy(o => o.Created).ToList();
            return issues;
        }
        public async Task<Issue> GetPublicIssue(string Id)
        {
            var issue = new Issue();
            try
            {
                var docRef = database.Collection("PublicIssues").Document(Id);
                var snap = await docRef.GetSnapshotAsync();
                var issueItem = snap.ConvertTo<Issue>();
                return issueItem;
            }
            catch
            {
                return issue;
            }
        }
    }
}
