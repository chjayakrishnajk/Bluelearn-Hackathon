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
            CollectionReference col = database.Collection("users").Document(userId).Collection("Issues");
            var data = issue.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(issue, null));
            return(await col.AddAsync(data));
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
            return issues;
        }
        public async Task<WriteResult> UpdateIssue(string userId , Issue issue)
        {
            var data = issue.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(issue, null));
            return(await database.Collection("users").Document(userId).Collection("Issues").Document(issue.Id).UpdateAsync(data));
        }
        public async Task<Google.Cloud.Firestore.WriteResult> DeleteIssue(string userId, string docId)
        {
            return(await database.Collection("users").Document(userId).Collection("Issues").Document(docId).DeleteAsync());
        }
    }
}
