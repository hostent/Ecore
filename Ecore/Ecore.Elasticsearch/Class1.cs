using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ecore.Elasticsearch
{
    public class Class1
    {
        public Class1()
        {


        }


        public void Test()
        {
            var client = ElasticHelp.GetClient();


            List<Person> p = new List<Person>(new List<Person>()
            {
                new  Person(){ Id="11111" , Chains=new string[] { "a1","a1"} , Content="哪里人？,我是中国人,福建人", Firstname="张", Lastname="三"},
                new  Person(){ Id="22222" , Chains=new string[] { "b1","b1"} , Content="哪里人？,我是美国人", Firstname="李", Lastname="四"},
                new  Person(){ Id="33333" , Chains=new string[] { "c1","c1"} , Content="どこの人？,我是日本人", Firstname="王", Lastname="五"},
                new  Person(){ Id="44444" , Chains=new string[] { "c1","c1"} , Content="你们是谁？,我们是中国人，而且是超人", Firstname="王", Lastname="五"}
            });



            if (!client.IndexExists("person").Exists)
            {
                client.CreateIndex("person");
            }

            client.IndexMany<Person>(p, "person");



            // like
            var searchResults = client.Search<Person>(q =>
                q.Index("person").
                  Query(a =>
                    a.QueryString(b => b.Query("你们是谁？,我们是中国人，而且是超人").DefaultOperator(Operator.Or))
                    )
            );


            client.Search<Person>(q =>
                q.Index("person").Aggregations(
                    a=>a.s
                    )
                  
            );


            //精确



            var searchResults1 = client.Search<Person>(a =>
                a.Index("person").Query(q => q.Term(t => t.Field(f => f.Id).Value("44444")))
            );

            //



            var doc = searchResults.Documents;

            var doc1 = searchResults1.Documents;
        }


    }

    public class ElasticHelp
    {
        public static IElasticClient GetClient()
        {

            var node = new Uri("http://127.0.0.1:9200");

            var settings = new ConnectionSettings(
                node

            );

            settings.DefaultIndex("test");
            return new ElasticClient(settings);
        }
    }

    public class Person
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string[] Chains { get; set; }
        public string Content { get; set; }
    }
}
