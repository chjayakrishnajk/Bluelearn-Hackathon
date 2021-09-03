using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Core.Issues
{
    [FirestoreData]
    public class Issue
    {
        [FirestoreProperty]
        public string Title { get; set; }
        [FirestoreProperty]
        public string Created { get; set; }
        [FirestoreProperty]
        public string LastUpdated { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string Tag { get; set; }
        [FirestoreProperty]
        public List<string> Keywords { get; set; }
        [FirestoreProperty]
        public string Solution { get; set; }
        [FirestoreProperty]
        public List<string> Base64Images { get; set; }
    }
}