using MySql.Data.MySqlClient;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using Mysqlx.Prepare;
using System.Net;
using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Relational;
using MySqlX.XDevAPI;
using System.Data;
using System.Runtime.Remoting.Messaging;


namespace TD11_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Bien vérifier, via Workbench par exemple, que ces paramètres de connexion sont valides !!!
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE= fleurs;UID=root;PASSWORD=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // Get user input for new customer information
            Console.WriteLine("Prénom:");
            string prenom_client = Console.ReadLine();

            Console.WriteLine("Nom:");
            string nom_client = Console.ReadLine();

            Console.WriteLine("Numéro de téléphone:");
            string ligne = Console.ReadLine();
            int tel_client = Convert.ToInt32(ligne);

            Console.WriteLine("Adresse mail:");
            string mail_client = Console.ReadLine();

            Console.WriteLine("Mot de passe:");
            string mdp_client = Console.ReadLine();

            Console.WriteLine("Numéro de facturation:");
            string ligne2 = Console.ReadLine();
            int Num_facturation = Convert.ToInt32(ligne2);

            Console.WriteLine("Rue de facturation:");
            string rue_facturation = Console.ReadLine();

            Console.WriteLine("Ville de facturation:");
            string ville_facturation = Console.ReadLine();


            Console.WriteLine("Carte de crédit:");
            string ligne3 = Console.ReadLine();
            BigInteger CB_client = BigInteger.Parse(ligne3);

            // Check if the email address already exists in the database
            bool emailExists = CheckEmailExists(mail_client);

            // If the email address already exists, prompt the user to choose a different email address
            if (emailExists)
            {
                Console.WriteLine("An account with this email address already exists. Please choose a different email address.");
                Console.ReadKey();
            }
            // If the email address does not exist, create a new customer record in the database
            else
            {
                CreateNewCustomer(mail_client, nom_client, prenom_client, tel_client, mdp_client, CB_client, Num_facturation, rue_facturation, ville_facturation, connection);
                Console.WriteLine("New customer account created successfully.");
            }

            connection.Close();
            Console.ReadKey();
        }

        static bool CheckEmailExists(string email)
        {
            // Database connection string
            string connectionString = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // SQL query to check if the email address already exists in the Customers table
                string query = "SELECT COUNT(*) FROM Customers WHERE courriel_c = @Email";

                // Create a command object with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add a parameter for the email address
                    command.Parameters.AddWithValue("@Email", email);

                    // Open the database connection
                    connection.Open();

                    // Execute the query and get the number of matching records
                    int count = (int)command.ExecuteScalar();

                    // If the count is greater than zero, the email address already exists
                    return count > 0;
                }
            }
        }

        static void CreateNewCustomer(string mail_client, string nom_client, string prenom_client, int tel_client, string mdp_client, BigInteger creditCardNumber, int Num_facturation,string rue_facturation, string ville_facturation, MySqlConnection connection)
        {
            // Database connection string
            string connectionString = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True";

            connection.Close();
            Console.ReadKey();
        }

        /*
        static bool ClientExists(MySqlConnection connection, string mail)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM client WHERE courriel_c = @Mail";
            command.Parameters.AddWithValue("@Mail", mail);

            int count = Convert.ToInt32(command.ExecuteScalar());

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */
        static void Exo1(MySqlConnection connection)
        {

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM commande"; // exemple de requete bien-sur !

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            /* exemple de manipulation du resultat */
            while (reader.Read())                           // parcours ligne par ligne
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)    // parcours cellule par cellule
                {
                    string valueAsString = reader.GetValue(i).ToString();  // recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
                    currentRowAsString += valueAsString + ";";
                }
                Console.WriteLine(currentRowAsString);    // affichage de la ligne (sous forme d'une "grosse" string) sur la sortie standard
            }
        }

        //1 : La liste des marques des véhicules proposées en location
        static void Exo1_1(MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT DISTINCT marque FROM location,voiture"; 

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            /* exemple de manipulation du resultat */
            while (reader.Read())                           // parcours ligne par ligne
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)    // parcours cellule par cellule
                {
                    string valueAsString = reader.GetValue(i).ToString();  // recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
                    currentRowAsString += valueAsString + ";";
                }
                Console.WriteLine(currentRowAsString);    // affichage de la ligne (sous forme d'une "grosse" string) sur la sortie standard
            }
        }

        //2. Afficher la liste des propriétaires (leur pseudo) et de leurs véhicules (marque, modèle, immat)
        static void Exo1_2(MySqlConnection connection)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT pseudo,marque,modele,immat from proprietaire natural join voiture"; 

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            /* exemple de manipulation du resultat */
            while (reader.Read())                           // parcours ligne par ligne
            {
                string currentRowAsString = "";
                for (int i = 0; i < reader.FieldCount; i++)    // parcours cellule par cellule
                {
                    string valueAsString = reader.GetValue(i).ToString();  // recuperation de la valeur de chaque cellule sous forme d'une string (voir cependant les differentes methodes disponibles !!)
                    currentRowAsString += valueAsString + ";";
                }
                Console.WriteLine(currentRowAsString);    // affichage de la ligne (sous forme d'une "grosse" string) sur la sortie standard
            }
        }







 
        static MySqlCommand CreateRequest(MySqlConnection mySqlConnection, string request)
        {
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = request;
            return mySqlCommand;
        }

        static string GetSingleValue(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader;
            reader = mySqlCommand.ExecuteReader();
            reader.Read();
            string value = reader[0].ToString();
            reader.Close();
            return value;
        }


        //11. Mise à jour de la BD 
        // Lire le fichier clients.csv
        // Enregistrer chacun de ces nouveaux clients dans la table CLIENT de la BD
        // Pour les codes client (codeC) vous utiliserez : 
        // - des codes client séquentiels à partir de C700 
        // - des codes clients séquentiels à partir de la valeur max de codeC valeur
        // qu'il faudra lire préalablement dans la BD. 
        // Attention : codeC est un string , C672 par exemple.Le code suivant sera alors le string C673.
        static void Exo1_11(MySqlConnection connection, string filePath)
        {
            string maxCodeC = GetSingleValue(CreateRequest(connection, "SELECT max(codeC) FROM client;"));
            Func<string, string> incrementCode = x => $"C{Convert.ToInt32(x.Substring(1)) + 1}";

            StreamReader reader = new StreamReader(filePath);
            string ligne = reader.ReadLine();
            MySqlCommand command = new MySqlCommand(null, connection);

            // Créer et préparer la commande SQL
            command.CommandText = "INSERT INTO loueur.client (codeC, nom, prenom, age, permis, adresse, ville) VALUES (@codeC, @nom, @prenom, @age, @permis, @adresse, @ville);";
            MySqlParameter codeCParam = new MySqlParameter("@codeC", MySqlDbType.VarChar, 4);
            MySqlParameter nomParam = new MySqlParameter("@nom", MySqlDbType.VarChar, 20);
            MySqlParameter prenomParam = new MySqlParameter("@prenom", MySqlDbType.VarChar, 20);
            MySqlParameter ageParam = new MySqlParameter("@age", MySqlDbType.Int32, 0);
            MySqlParameter permisParam = new MySqlParameter("@permis", MySqlDbType.VarChar, 10);
            MySqlParameter adresseParam = new MySqlParameter("@adresse", MySqlDbType.VarChar, 50);
            MySqlParameter villeParam = new MySqlParameter("@ville", MySqlDbType.VarChar, 20);

            /*codeCParam.Value = "C700";
            nomParam.Value = "Rodriguez";
            prenomParam.Value = "Christophe";
            ageParam.Value = "40";
            permisParam.Value = "2348653";
            adresseParam.Value = "champs de mars";
            villeParam.Value = "Paris";*/

            command.Parameters.Add(codeCParam);
            command.Parameters.Add(nomParam);
            command.Parameters.Add(prenomParam);
            command.Parameters.Add(ageParam);
            command.Parameters.Add(permisParam);
            command.Parameters.Add(adresseParam);
            command.Parameters.Add(villeParam);

            command.Prepare();
            string codeCIncrement = incrementCode(maxCodeC);
            while (ligne != null)
            {
                string[] liste = ligne.Split(';');
                codeCIncrement = incrementCode(codeCIncrement);
                command.Parameters[0].Value = codeCIncrement;
                command.Parameters[1].Value = liste[0];
                command.Parameters[2].Value = liste[1];
                command.Parameters[3].Value = liste[2];
                command.Parameters[4].Value = liste[3];
                command.Parameters[5].Value = liste[4];
                command.Parameters[6].Value = liste[5];
                command.ExecuteNonQuery();
                //Console.WriteLine(string.Join(" ", liste.Select(x => x)));
                ligne = reader.ReadLine();
            }
            reader.Close();
        }
    }
}
