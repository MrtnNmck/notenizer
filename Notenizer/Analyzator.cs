using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using MongoDB.Bson;
using nsConstants;
using nsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizer
{
    public static class Analyzator
    {

        public static void Analyze()
        {
            long notesCount;
            long rulesCount;
            long articlesCount;
            long andRulesCount;
            long sentencesCount;
            long structuresCount;
            StanfordCoreNLP pipe;
            Annotation annotation;
            long articlesSentencesCount;
            List<BsonDocument> articles;

            pipe = new Notenizer(true).Pipeline;

            notesCount = GetCount(DBConstants.NotesCollectionName);
            rulesCount = GetCount(DBConstants.RulesCollectionName);
            andRulesCount = GetCount(DBConstants.AndRulesCollectionName);
            sentencesCount = GetCount(DBConstants.SentencesCollectionName);
            structuresCount = GetCount(DBConstants.StructuresCollectionName);
            articles = GetAll(DBConstants.ArticlesCollectionName);
            articlesCount = articles.Count;
            articlesSentencesCount = 0;

            Console.WriteLine(String.Format("Getting number of sentences in {0} articles...", articlesCount));
            for (int i = 0; i < articlesCount; i++)
            {
                Console.Write(String.Format("Parsing article no.{0} ... ", i + 1));
                annotation = new Annotation(articles[i][DBConstants.TextFieldName].AsString);
                pipe.annotate(annotation);
                articlesSentencesCount += (annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as java.util.ArrayList).size();
                Console.WriteLine("Done.");
            }

            Console.WriteLine(String.Format("{0}{0}Analysis:", Environment.NewLine));
            Notify(DBConstants.NotesCollectionName, notesCount);
            Notify(DBConstants.SentencesCollectionName, sentencesCount);
            Notify(DBConstants.RulesCollectionName, rulesCount);
            Notify(DBConstants.AndRulesCollectionName, andRulesCount);
            Notify(DBConstants.StructuresCollectionName, structuresCount);
            Notify(DBConstants.ArticlesCollectionName, articlesCount);
            Console.WriteLine(String.Format("Number of sentences in articles: {0}", articlesSentencesCount));
        }

        private static long GetCount(String collectionName)
        {
            long count;

            Console.Write(String.Format("Getting number of documents from collection {0}... ", collectionName));
            count = DB.GetCount(collectionName).Result;
            Console.WriteLine("Done.");

            return count;
        }

        private static List<BsonDocument> GetAll(String collectionName)
        {
            List<BsonDocument> docs;

            Console.Write(String.Format("Getting all documents from collection {0}... ", collectionName));
            docs = DB.GetAll(collectionName).Result;
            Console.WriteLine("Done.");

            return docs;
        }

        private static void Notify(String collectionName, long count)
        {
            Console.WriteLine(String.Format("Number of documents in collection {0}: {1}", collectionName, count));
        }
    }
}
