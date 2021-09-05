using Core.Issues;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorCookieAuth.Data
{
    public class TFIDF
    {
        public async Task<Dictionary<Issue , float>> Group(string query , List<Issue> products)
        {
            var newProducts = products.Select(p => new Issue()
            {
                Id = p.Id,
                Description = p.Description,
                LastUpdated = p.LastUpdated,
                Solution = p.Solution,
                Solved = p.Solved,
                Tag = p.Tag,
                Status = p.Status,
                Title = p.Title
            }).ToList();
            var docs = newProducts.Select(x => x.Title).ToList();
            var tf_idf_matrix = GenerateVectors(query, docs);
            var query_vector = BuildQueryVector(query, docs);
            var similarities = new Dictionary<Issue ,float>();
            for (int i = 0; i < tf_idf_matrix[0].Length; ++i)
            {
                var matrix = new float[1][];
                matrix[0] = new float[tf_idf_matrix.Length];
                for (int j = 0; j < tf_idf_matrix.Length; ++j)
                {
                    matrix[0][j] = tf_idf_matrix[j][i];
                }
                var similarity = ConsineSimilarity(matrix, query_vector);
                var product = newProducts[i];
                product.Title = product.Title; 
                similarities.Add( product,  similarity);
            }
            return similarities;
        }
        public float InverseDocumentFrequency(string term , List<string> documents)
        {
            int count = 0;
            foreach (var doc in documents)
            {
                if(doc.ToLower().Contains(term.ToLower()))
                {
                    ++count;
                }
            }
            if (count > 0)
            {
                var divvalue = (float) ((float) documents.Count) / ((float) count);
                var result =  Math.Log(divvalue);
                return (float)(1.0 + result);
            }
            else
            {
                return 1;
            }
        }
        public float TermFrequency(string term, string document)
        {
            string[] normalizeDocument = document.ToLower().Split(" ");
            int count = 0;
            foreach(var item in normalizeDocument)
            {
                if (item == term)
                    ++count;
            }
            return (((float) count) / (float) (normalizeDocument.Count()));   
        }
        public float tf_idf(string term , string document , List<string> documents)
        {
            var tf = TermFrequency(term , document);
            var idf = InverseDocumentFrequency(term, documents);
            return tf * idf;
        }
        public float[][] GenerateVectors (string query , List<string> documents)
        {
            var qsplit = WordCount(query);
            float[][] tf_idf_matrix = new float[qsplit.Count][];
            var docLength = documents.Count;
            for (int i = 0; i < qsplit.Count; ++i)
            {
                tf_idf_matrix[i] = new float[docLength];               
            }
            for ( int i = 0; i < qsplit.Count; ++i)
            {
                var s = qsplit.Keys.ElementAt(i);
                var idf = InverseDocumentFrequency(s, documents);
                for (int j = 0; j < documents.Count; ++j)
                {
                    var doc = documents[j];
                    tf_idf_matrix[i][j] = idf * TermFrequency(s, doc);
                }
            }
            return tf_idf_matrix;
        }
       public Dictionary<string , int> WordCount(string s)
        {
            var counts = new Dictionary<string, int>();
            var words = s.ToLower().Split(" ");
            foreach(var word in words)
            {
                if(counts.ContainsKey(word))
                    counts[word] += 1;
                else
                    counts.Add(word, 1);
            }
            return counts;
        }
        public float[][] BuildQueryVector(string query , List<string> documents)
        {
            var count = WordCount(query);
            float[][] vector = new float[count.Count][];
            for (int i = 0;i < count.Count; ++i)
            {
                vector[i] = new float[1];
            }
            var qsplit = query.ToLower().Split(" ");
            for (int i = 0; i < count.Count; ++i)
            {
                var word = qsplit[i];
                var wcount = (float)(count[word]) / count.Count;
                var inversevalue = InverseDocumentFrequency(word, documents);
                var denom =  inversevalue;
                vector[i][0] = ((float) (wcount)) * ((float) (denom));
            }
            return vector;
        }
        public float ConsineSimilarity(float[][] v1 , float[][] v2)
        {
            float dot = 0;
            for(int i = 0; i < v1[0].Length; ++i)
            {
                dot += v1[0][i] * v2[i][0];
            }
            float v1norm = 0;
            float v2norm = 0;
            float v1sum = 0;
            float v2sum = 0;
            for (int i = 0; i < v2.Length; ++i)
            {
                v1sum += v1[0][i] * v1[0][i];
            }
            for (int i = 0; i < v2.Length; ++i)
            {
                v2sum += v2[i][0] * v2[i][0];
            }
            v1norm = (float)Math.Sqrt(v1sum);
            v2norm = (float)Math.Sqrt(v2sum);
            return (dot) / (v1norm * v2norm);
        }
        /*
       def consine_similarity(v1, v2):
    return np.dot(v1,v2)/float(LA.norm(v1)*LA.norm(v2))
         */


    }
}
