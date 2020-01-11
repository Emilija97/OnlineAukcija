using NeoDataLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace NeoDataLayer
{
    public static class DataProvider
    {
        #region User

        public static User GetUser(string email, string password)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();
         
            if (graphClient == null)
                return null;

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n) where exists(n.email) " +
                                                               "and n.email = '" + email + "' " +
                                                               "and n.password = '" + password + "' return n",
                                                                 new Dictionary<string, object>(), CypherResultMode.Set);

                List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(query).ToList();

                User user = new User();
                if (users != null)
                {
                    user.name = users[0].name;
                    user.email = users[0].email;
                    user.password = users[0].password;
                }
            
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void RegisterOrganizer(User u)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            string tmp = "";
            tmp += u.name + " " + u.surname;
            queryDict.Add("name", tmp);
            queryDict.Add("email", u.email);
            queryDict.Add("password", u.password);

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Organizer {name:'" + u.name
                                                                + "', email:'" + u.email
                                                                + "', password:'" + u.password
                                                                + "'}) return n",
                                                                queryDict, CypherResultMode.Set);

                List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(query).ToList();
            }
            catch(Exception e)
            {
                return;
            }
        }

        public static void RegisterUser(User u)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            string tmp = "";
            tmp += u.name + " " + u.surname;
            queryDict.Add("name", tmp);
            queryDict.Add("email", u.email);
            queryDict.Add("password", u.password);

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:User {name:'" + u.name
                                                            + "', email:'" + u.email
                                                            + "', password:'" + u.password
                                                            + "'}) return n",
                                                            queryDict, CypherResultMode.Set);

            List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(query).ToList();
            }
            catch (Exception e)
            {
                return;
            }
        }

        public static bool ValidateUser(string email, string password)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return false;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("email", email);
            queryDict.Add("password", password);

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n) where " +
                                                               "n.email = '" + email + "' " +
                                                               "and n.password = '" + password + "' return n",
                                                                 queryDict, CypherResultMode.Set);


                List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(query).ToList();

                if (users != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
