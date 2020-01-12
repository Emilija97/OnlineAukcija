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

        public static User GetUser(string email, string password, bool role)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();
         
            if (graphClient == null)
                return null;

            try
            {
                CypherQuery query;
                if (role)
                {
                    query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n:Organizer) where exists(n.email) " +
                                                                   "and n.email = '" + email + "' " +
                                                                   "and n.password = '" + password + "' return n",
                                                                     new Dictionary<string, object>(), CypherResultMode.Set);
                }
                else
                {
                    query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n:User) where exists(n.email) " +
                                                                  "and n.email = '" + email + "' " +
                                                                  "and n.password = '" + password + "' return n",
                                                                    new Dictionary<string, object>(), CypherResultMode.Set);
                }
                    List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(query).ToList();

                    User user = new User();
                    if (users != null)
                    {
                        user.name = users[0].name;
                        user.email = users[0].email;
                        user.password = users[0].password;
                        return user;
                    }
                    else
                    {
                        return null;
                    }
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
                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Organizer {name:'" + tmp
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
                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:User {name:'" + tmp
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

        #region Auction

        public static void CreateAuction(Auction auction)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            string d = "";
            d += auction.datePublished.Year + "-" + auction.datePublished.Month + "-" + auction.datePublished.Day;

            string duration = "";
            duration += auction.duration.Year + "-" + auction.duration.Month + "-" + auction.duration.Day;

            queryDict.Add("title", auction.title);
            queryDict.Add("type", auction.type);
            queryDict.Add("datePublished", d);
            queryDict.Add("duration", duration);

            
            User user = NeoDataLayer.Store.GetInstance().loggedUser;

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Auction {title:'" + auction.title
                                                            + "', type:'" + auction.type
                                                            + "', datePublished:date('" + d
                                                            + "'), duration:date('" + duration
                                                            + "')}) return n",
                                                            queryDict, CypherResultMode.Set);

                List<Auction> auctions = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Auction>(query).ToList();
          

                var queryAddRel = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Organizer),(a:Auction) WHERE " +
                                                                    "n.name = '"+user.name+ "' AND " +
                                                                    "n.email = '"+user.email+"' AND " +
                                                                    "a.title = '"+auction.title+"' " +
                                                                    "CREATE(n) -[r: ORGANIZE]->(a) RETURN n",
                                                                    new Dictionary<string, object>(), CypherResultMode.Set);

                List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(queryAddRel).ToList();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public static void CreateRelation(string title)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return;

            User user = NeoDataLayer.Store.GetInstance().loggedUser;

            try
            {
                var queryAddRel = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Organizer),(a:Auction) WHERE " +
                                                                    "n.name = '" + user.name + "' AND " +
                                                                    "n.email = '" + user.email + "' AND " +
                                                                    "a.title = '" + title + "' " +
                                                                    "CREATE(n) -[r: ORGANIZE]->(a) RETURN n",
                                                                    new Dictionary<string, object>(), CypherResultMode.Set);

                List<User> users = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<User>(queryAddRel).ToList();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public static List<Auction> GetAllAuctions()
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return null;

            User user = NeoDataLayer.Store.GetInstance().loggedUser;

            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Auction) WHERE " +
                                                               "n.duration > date('" +currentDate +"') RETURN n",
                                                                new Dictionary<string, object>(), CypherResultMode.Set);

                List<Auction> auctions = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Auction>(query).ToList();
                if(auctions != null)
                {
                    return auctions;
                }
                return null;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Subject

        public static List<Subject> GetAllSubjects(string title)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return null;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            queryDict.Add("title", title);

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n)<-[r:ON_AUCTION]-(a) " +
                                                               "WHERE EXISTS(n.title) and n.title ='"+title+"' RETURN a",
                                                                queryDict, CypherResultMode.Set);

                List<Subject> subjects = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Subject>(query).ToList();
                if (subjects != null)
                {
                    return subjects;
                }
                return null;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public static bool OfferPrice(string title, string subjectName, int price)
        {
            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return false;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            queryDict.Add("title", title);

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n)<-[r:ON_AUCTION]-(a) " +
                                                               "WHERE EXISTS(n.title) and n.title ='" + title + "' and a.name ='"+subjectName+"' RETURN a",
                                                                queryDict, CypherResultMode.Set);

                List<Subject> subjects = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Subject>(query).ToList();
                Subject subject = new Subject();
                if (subjects != null)
                {
                    subject.sellingPrice = subjects[0].sellingPrice;
                    subject.startingPrice = subjects[0].startingPrice;

                    if(price>subject.startingPrice && price > subject.sellingPrice)
                    {
                        var queryUpdate = new Neo4jClient.Cypher.CypherQuery("start n=node(*) MATCH (n)<-[r:ON_AUCTION]-(a) " +
                                                                             "WHERE EXISTS(n.title) and n.title ='" + title + "' " +
                                                                             "and a.name ='" + subjectName + "' " +
                                                                             "SET a.sellingPrice="+price+" RETURN a",
                                                                            new Dictionary<string, object>(), CypherResultMode.Set);
                        List<Subject> subjectsData = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Subject>(queryUpdate).ToList();
                        if(subjectsData != null)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public static bool AddSubject(string title, Subject s, User user)
        {

            IGraphClient graphClient = GraphClientManager.GetGraphClient();

            if (graphClient == null)
                return false;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();

            queryDict.Add("title", title);
            queryDict.Add("name", s.name);
            queryDict.Add("startingPrice", s.startingPrice);

            try
            {
                var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Subject {name:'" + s.name
                                                           + "', sellingPrice: 0"
                                                           + ", startingPrice:" + s.startingPrice
                                                           + "}) RETURN n",
                                                           queryDict, CypherResultMode.Set);

                List<Auction> auctions = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Auction>(query).ToList();


                var queryAddRel = new Neo4jClient.Cypher.CypherQuery("MATCH (n:Auction),(s:Subject),(o:Organizer) WHERE " +
                                                                    "n.title = '" + title + "' AND " +
                                                                    "s.name = '" + s.name + "' AND " +
                                                                    "o.email= '" + user.email +"' " +
                                                                    "CREATE (o)-[r:POST_ON_AUCTION]->(s)" +
                                                                    "CREATE (s)-[k: ON_AUCTION]->(n) RETURN n",
                                                                    new Dictionary<string, object>(), CypherResultMode.Set);

                List<Auction> auctionsData = ((IRawGraphClient)graphClient).ExecuteGetCypherResults<Auction>(queryAddRel).ToList();
                return true;
            }
            catch (Exception)
            {
                //throw new NotImplementedException();
                return false;
            }
        }

        #endregion
    }
}
