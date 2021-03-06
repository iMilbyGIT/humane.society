﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }
        

        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            throw new NotImplementedException();
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            throw new NotImplementedException();
        }

        internal static Animal GetAnimalByID(int id)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {            
            throw new NotImplementedException();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            throw new NotImplementedException();
        }
      
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            IQueryable<Animal> results = db.Animals;
            
            foreach (KeyValuePair<int, string> traits in updates)
            {               
                switch(traits.Key)
                {
                    
                    case 1:

                        results = results.Where(a => a.Category.Name == traits.Value);
                        break;
                    case 2:
                        results = results.Where(a => a.Name == traits.Value);

                        break;
                    case 3:
                        results = results.Where(a => a.Age == Int32.Parse(traits.Value));

                        break;
                    case 4:
                        results = results.Where(a => a.Demeanor == traits.Value);

                        break;
                    case 5:
                        results = results.Where(a => a.KidFriendly == bool.Parse(traits.Value));

                        break;
                    case 6:
                        results = results.Where(a => a.PetFriendly == bool.Parse(traits.Value));

                        break;
                    case 7:
                        results = results.Where(a => a.Weight == Int32.Parse(traits.Value));

                        break;
                    case 8:
                        results = results.Where(a => a.AnimalId == Int32.Parse(traits.Value));

                        break;
                    default:
                        
                        break;
                }
            }
            return results;
        }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            Category category = db.Categories.Where( c => c.Name == categoryName).FirstOrDefault() ;
            return category.CategoryId;
        }
        
        internal static Room GetRoom(int animalId)
        {
            throw new NotImplementedException();
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            throw new NotImplementedException();
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            UserInterface.DisplayUserOptions("Who will be adopting today?");
            DisplayAllCustomers();
            UserInterface.DisplayUserOptions("Enter client Id ");
            int id = Int32.Parse(Console.ReadLine());
            Client personAdopting = db.Clients.Where(c => c.ClientId == id).FirstOrDefault();
            // add clientId to the Adoptions table
            Console.Clear();

            UserInterface.DisplayUserOptions("What Animal is being requested?");
            DisplayAnimals();
            UserInterface.DisplayUserOptions("Enter animal Id ");
            int animalId = Int32.Parse(Console.ReadLine());
            Animal animalBingAdopted = GetAnimalByID(animalId);
            //Add animalId to same table  
            Console.Clear();

            UserInterface.DisplayUserOptions("What is the adoption status (Approved, UnApproved or Pending)");
            string status = Console.ReadLine();
            //add status to table 

            UserInterface.DisplayUserOptions("What is the adoption fee?");
            int fee = Int32.Parse(Console.ReadLine());
            //add fee

            UserInterface.DisplayUserOptions("Has this adoption breen paid for? (yes or no)");
            string yesorno = Console.ReadLine();
            bool payStatus = UserInterface.GetBitData(yesorno);          
            //add status to table




        }

        internal static void DisplayAllCustomers()
        {
            var animals = db.Animals;
            foreach (Animal animal in animals)
            {
                Console.WriteLine(animal.Name + " " + animal.Category + " " + animal.AnimalId);
            }
        }

        internal static void DisplayAnimals()
        {
            var animals = db.Clients;
            foreach (Client animal in animals)
            {
                Console.WriteLine(animal.FirstName + " " + animal.LastName + " " + animal.ClientId);
            }
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            IQueryable<Adoption> adoptions =  db.Adoptions.Where(a => a.ApprovalStatus == "Pending");
            return adoptions;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            //db.Adoptions.DeleteOnSubmit(a);
            //db.SubmitChanges();
        }
        internal static void DisplayAllCustomers()
        {
            var animals = db.Animals;
            foreach (Animal animal in animals)
            {
                Console.WriteLine(animal.Name + " " + animal.Category + " " + animal.AnimalId);
            }
        }
        internal static void DisplayAnimals()
        {
            var animals = db.Clients;
            foreach(Client animal in animals)
            {
                Console.WriteLine(animal.FirstName + " " + animal.LastName + " " + animal.ClientId);
            }
        }


        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            IQueryable<AnimalShot> shots = db.AnimalShots;
            return shots;
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            var updates = UserInterface.GetAnimalSearchCriteria();
            var animals = Query.SearchForAnimalsByMultipleTraits(updates).ToList();

            UserInterface.DisplayUserOptions("What animal got a shot?");
            while (animals.Count != 1)
            {
                Console.Clear();
                Console.WriteLine("You have to return a single animal");
                updates = UserInterface.GetAnimalSearchCriteria();
                animals = Query.SearchForAnimalsByMultipleTraits(updates).ToList();
            }
            Animal pet = animals[0];
            List<Shot> Shots = db.Shots.ToList();
            UserInterface.DisplayUserOptions("what shot do you need for this animal (Enter its name)");
            foreach(Shot shot in Shots)
            {                
                UserInterface.DisplayUserOptions(shot.Name);
            }
            string answer = Console.ReadLine();
            Shot shotToUse = db.Shots.Where(s => s.Name == answer).FirstOrDefault();
            Console.Clear();
            UserInterface.DisplayUserOptions("Do you want to give " + pet.Name + " the " + shotToUse.Name + " shot ");
            switch (answer)
            {
                case "yes":
                    //add the shot and animal to junction tabel
                    break;
                case "no":
                    //send them back to employee opstions
                    break;

            }
        }
    }
}