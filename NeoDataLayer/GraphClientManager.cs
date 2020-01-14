using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace NeoDataLayer
{
    public static class GraphClientManager
    {
        //private static GraphClient client;
        public static IGraphClient client;
        public static IGraphClient GetGraphClient()
        {
            if (client == null)
            {
                client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "admin");
                try
                {
                    client.Connect();
                }
                catch (Exception exc)
                {

                }
            }
            return client;
        }
    }
}
